using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.Linq;
using LinqToDB.Mapping;
using LinqToDB;
using Linq2dbContext.Common;

namespace Linq2dbContext.DAL
{
    class PreprocessorDataConnection : DataConnection, IExpressionPreprocessor
    {
        public PreprocessorDataConnection(string providerName, string connectionString, MappingSchema mappingSchema) : base(providerName, connectionString, mappingSchema)
        {
        }
        public Expression ProcessExpression(Expression expression) => DataProvider.Name.StartsWith("Postgre") ? Extensions.ProcessExpression(expression) : expression;
    }

    class PreprocessorDataContext : DataContext, IExpressionPreprocessor
    {
        public PreprocessorDataContext(string providerName, string connectionString, MappingSchema mappingSchema) : base(new DataOptions()
                .UseConnectionString(
                    providerName,
                    connectionString)
                .UseMappingSchema(mappingSchema))
        {
        }

        public Expression ProcessExpression(Expression expression) => DataProvider.Name.StartsWith("Postgre") ? Extensions.ProcessExpression(expression) : expression;
    }
}
