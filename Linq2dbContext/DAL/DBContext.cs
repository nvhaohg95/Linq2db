using Linq2dbContext.Common;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.Linq;
using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace Linq2dbContext.DAL
{
	public class DBContext : IDisposable
	{
		string _cnnString = "";
		string _providerName = "";
		public MappingSchema _mappingSchema
		{
			get
			{
				var mappingSchema = new MappingSchema();
				mappingSchema.AddMetadataReader(new SystemComponentModelDataAnnotationsSchemaAttributeReader());
				if (_providerName.StartsWith("Postgre"))
				{
					mappingSchema.SetConverter<DateTime, DataParameter>(x =>
					new DataParameter(null,
						x.Kind == DateTimeKind.Utc ? x : x.ToUniversalTime(),
						DataType.DateTime));
					mappingSchema.SetConverter<DateTime?, DataParameter>(x =>
						new DataParameter(null,
							x == null ? null :
							x.Value.Kind == DateTimeKind.Utc ? x : x.Value.ToUniversalTime(),
							DataType.DateTime));

					AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
					AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

					var ddType = new LinqToDB.Common.DbDataType(typeof(string), "citext");
					var sqlDatype = new LinqToDB.SqlQuery.SqlDataType(ddType);
					mappingSchema.AddScalarType(sqlDatype.SystemType, sqlDatype);
					mappingSchema.SetConvertExpression<string, DataParameter>(value => new DataParameter(null, value, "citext"), addNullCheck: false);
				}

				return mappingSchema;
			}
		}

		public DBContext(string cnnStr = "", string providerName = "")
		{
			_cnnString = cnnStr;
			_providerName = providerName;
		}

		public T GetValue<T>(string tableName, string selectField, string predicate, params object[] parameters)
		{
			var sql = $"SELECT Top 1 {selectField} FROM {tableName} WHERE {predicate}";
			DataParameter[] paras = parameters?.Select((x, i) => new DataParameter
			{
				Name = $"@{i}",
				Value = x
			}).ToArray();

			using (var db = GetDBConnection())
				return db.Execute<T>(sql, paras);
		}

		public string GetValue(string tableName, string selectField, string predicate, params object[] parameters)
			=> GetValue<string>(tableName, selectField, predicate, parameters);

		public T GetOne<T>(Expression<Func<T, bool>> predicate = null) where T : class
		{
			var qry = Query<T>();

			if (predicate != null)
				qry = qry.Where(predicate);

			return qry.FirstOrDefault();
		}

		public List<T> Get<T>(Expression<Func<T, bool>> predicate) where T : class
		{
			var qry = Query<T>();
			if (predicate == null)
				return qry.ToList();

			return qry.Where(predicate).ToList();
		}

		public T GetOneSorted<T, key>(Expression<Func<T, key>> sortSelector, Expression<Func<T, bool>> predicate = null) where T : class
		{
			var qry = Query<T>();

			qry = qry.OrderBy(sortSelector);

			if (predicate != null)
				qry = qry.Where(predicate);

			return qry.FirstOrDefault();
		}

		public bool Exist<T>(Expression<Func<T, bool>> predicate) where T : class
		{
			var qry = Query<T>();

			return qry.Any(predicate);
		}

		public IQueryable<T> Query<T>() where T : class => GetDBContext().GetTable<T>();


		public int Add<T>(T data) where T : class
		{
			try
			{
				using (var db = GetDBConnection())
					return db.Insert(data);
			}
			catch (Exception ex)
			{
				Log.Instance.Error(ex);
				return 0;
			}
		}

		public int Update<T>(Expression<Func<T, bool>> predicate, object updates) where T : class
		{
			if (updates == null) return 0;

			try
			{
				using (var db = GetDBConnection())
				{
					var qry = db.GetTable<T>()
						.Where(predicate);

					IUpdatable<T> qryUpdate = null;
					var pros = updates.GetType().GetProperties();
					foreach (var up in pros)
					{
						var value = up.GetValue(updates);

						if (qryUpdate == null)
							qryUpdate = qry.Set(t => Sql.Property<T>(t, up.Name), value);
						else
							qryUpdate = qryUpdate.Set(t => Sql.Property<T>(t, up.Name), value);
					}

					if (qryUpdate != null)
						return qryUpdate.Update();
				}
			}
			catch (Exception ex)
			{
				Log.Instance.Error(ex);
				return 0;
			}

			return 0;
		}

		public int Update<T>(T data) where T : class
		{
			try
			{
				using (var db = GetDBConnection())
				{
					return db.Update(data);
				}
			}
			catch (Exception ex)
			{
				Log.Instance.Error(ex);
				return 0;
			}
		}

		public Dictionary<string, object> GetDic(string tableName, string predicate, object[] parameters, string selectFields = "*")
		{
			var lstDatas = GetData(tableName, selectFields, predicate, parameters, 1);
			return lstDatas?.FirstOrDefault();
		}

		public IEnumerable<Dictionary<string, object>> GetData(string tableName, string selectFields = "*", string predicate = "", object[] parameters = null, int top = 0)
		{
			List<Dictionary<string, object>> lstReturns = new List<Dictionary<string, object>>();
			if (string.IsNullOrEmpty(selectFields)) selectFields = "*";

			var sTOP = "";
			if (top > 0)
				sTOP = $"TOP {top} ";

			var sWHERE = "";
			DataParameter[] paras = null;

			if (!string.IsNullOrEmpty(predicate))
			{
				sWHERE = $" WHERE {predicate}";
				paras = parameters?.Select((x, i) => new DataParameter
				{
					Name = $"@{i}",
					Value = x
				}).ToArray();
			}

			if (selectFields.Contains(';'))
				selectFields = selectFields.Replace(";", ",");

			var sql = $"SELECT {sTOP}{selectFields} FROM {tableName}{sWHERE}";
			using (var db = GetDBConnection())
			using (var dbReader = db.ExecuteReader(sql, paras))
			using (var dbRdr = dbReader.Reader)
				while (dbRdr.Read())
				{
					var dic = Enumerable.Range(0, dbRdr.FieldCount).ToDictionary(dbRdr.GetName, dbRdr.GetVal);
					lstReturns.Add(dic);
				}

			return lstReturns;
		}

		public DataConnection GetDBConnection()
		{
			var DBConnection = new DataConnection(_providerName, _cnnString, _mappingSchema);
			DBConnection.IsMarsEnabled = true;

			return DBConnection;
		}

		public DataContext GetDBContext()
		{
			var dbCtx = new PreprocessorDataContext(_providerName, _cnnString, _mappingSchema)
			{
				IsMarsEnabled = true
			};
			var provider = (DataProviderBase)dbCtx.DataProvider;
			ProviderExp.SetProviderField(provider.DataReaderType, (r, i) => ProviderExp.GetDateTime(r, i), provider);
			return dbCtx;
		}

		public void Dispose()
		{
			Dispose();
		}
	}

	public static class Extend
	{
		public static object GetVal(this IDataReader reader, int i)
		{
			var v = reader.GetValue(i);
			if (v == DBNull.Value)
			{
				v = null;
			}
			return v;
		}
	}

	class ProviderExp
	{
		internal static DateTime GetDateTime(DbDataReader r, int i)
		{
			var dt = r.GetDateTime(i);
			return dt.Kind == DateTimeKind.Local ? dt : dt.ToLocalTime();
		}

		internal static void SetProviderField<T>(Type DataReaderType, Expression<Func<DbDataReader, int, T>> expr, DataProviderBase provider)
		{
			provider.ReaderExpressions[new ReaderInfo { DataReaderType = DataReaderType, ToType = typeof(T), FieldType = typeof(T) }] = expr;
		}
	}

}