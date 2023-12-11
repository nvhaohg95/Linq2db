using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Extensions;
using LinqToDB.Mapping;
using LinqToDB.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using static LinqToDB.Reflection.Methods.LinqToDB.Insert;

namespace Linq2dbContext.DAL
{
    public class SystemComponentModelDataAnnotationsSchemaAttributeReader : IMetadataReader
    {
        /// <inheritdoc cref="IMetadataReader.GetDynamicColumns"/>
        public MemberInfo[] GetDynamicColumns(Type type)
            => Array<MemberInfo>.Empty;

        static bool CompareProperty(MemberInfo property, MemberInfo memberInfo)
        {
            if (property == memberInfo)
                return true;

            if (property == null)
                return false;

            if (memberInfo.DeclaringType?.IsAssignableFrom(property.DeclaringType) == true
                && memberInfo.Name == property.Name
                && memberInfo.MemberType == property.MemberType
                && memberInfo.GetMemberType() == property.GetMemberType())
            {
                return true;
            }

            return false;
        }

        public MappingAttribute[] GetAttributes(Type type)
        {
            if (!type.IsClass)
                return Array<MappingAttribute>.Empty;
            var tableAtt = type.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>(true);
            if (tableAtt == null)
                return Array<MappingAttribute>.Empty;
            var attr = new TableAttribute { TableOptions = TableOptions.CreateIfNotExists };

            var name = tableAtt.Name;
            if (name != null)
            {
                var names = name.Replace("[", "").Replace("]", "").Split('.');

                switch (names.Length)
                {
                    case 0: break;
                    case 1: attr.Name = names[0]; break;
                    case 2:
                        attr.Name = names[0];
                        attr.Schema = names[1];
                        break;
                    default:
                        throw new MetadataException(string.Format("Invalid table name '{0}' of type '{1}'", name, type.FullName));
                }
            }

            return new[] { attr };
        }

        public MappingAttribute[] GetAttributes(Type type, MemberInfo memberInfo)
        {
            var colAtt = memberInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>(true);
            string dbType = null, clmName = null;
            int colOrder = 0;
            if (colAtt != null)
            {
                clmName = colAtt.Name;
                dbType = colAtt.TypeName;
                colOrder = colAtt.Order;
            }
            else
            {
                var props = type.GetProperties();
                colOrder = (props.Select((p, i) => new { p, index = i })
                                .FirstOrDefault(v => CompareProperty(v.p, memberInfo))?.index ?? 0) + 1;
            }

            var notMappedAtt = memberInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>();
            var foreignKeyAtt = memberInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute>();
            var keyAtt = memberInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>();
            var maxLengthAtt = memberInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.MaxLengthAttribute>();
            var requiredAtt = memberInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.RequiredAttribute>();

            var attr = new ColumnAttribute
            {
                Name = clmName ?? memberInfo.Name,
                DbType = dbType,
                IsPrimaryKey = keyAtt != null,
                IsColumn = notMappedAtt == null && foreignKeyAtt == null,
                Order = colOrder,
                Length = maxLengthAtt == null ? 0 : maxLengthAtt.Length,
                CanBeNull = requiredAtt == null
            };

            if (attr.IsPrimaryKey)
            {
                attr.PrimaryKeyOrder = colOrder;
            }

            return new[] { attr };
        }

        public string GetObjectID()
        {
            return $".{nameof(SystemComponentModelDataAnnotationsSchemaAttributeReader)}.";
        }
    }
}