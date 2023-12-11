using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Linq2dbContext.Helpers
{
	public static class ReadData
	{
		public static object ReadCsv<T>(string fileName) where T : class
		{
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				PrepareHeaderForMatch = args => args.Header.ToLower(),
			};
			using (var reader = new StreamReader(fileName))
			using (var csv = new CsvReader(reader, config))
			{
				var records = csv.GetRecords<T>().ToList();
				return records;
			}
		}
	}
}
