using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Remotion.Linq.Parsing.Structure;

namespace ArtaInfra.Utils.Extensions
{
    public static class QueryableExtensions
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

        private static readonly PropertyInfo NodeTypeProviderField = QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == "NodeTypeProvider");

        private static readonly MethodInfo CreateQueryParserMethod = QueryCompilerTypeInfo.DeclaredMethods.First(x => x.Name == "CreateQueryParser");

        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        public static string ToSql<T>(this IQueryable<T> query) where T : class
        {
            if (!(query is EntityQueryable<T>) && !(query is InternalDbSet<T>))
            {
                throw new ArgumentException("Invalid query");
            }

            var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
            var nodeTypeProvider = (INodeTypeProvider)NodeTypeProviderField.GetValue(queryCompiler);
            var parser = (IQueryParser)CreateQueryParserMethod.Invoke(queryCompiler, new object[] { nodeTypeProvider });
            var queryModel = parser.GetParsedQuery(query.Expression);
            var database = DataBaseField.GetValue(queryCompiler);
            var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor) queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<T>(queryModel);
            var sql = modelVisitor.Queries.First().ToString();

            return sql;
        }

        /// <summary>
        /// Automates the standard sorting builder
        /// </summary>
        /// <typeparam name="T">Type of the Queryable</typeparam>
        /// <typeparam name="U">Type of the sorting field</typeparam>
        /// <param name="query">the IQueryable to perform sorting on</param>
        /// <param name="func">Lambda containing the sorting field</param>
        /// <param name="isFirstSortKey">Distinction to know when to use OrderBy (first sorting key) and ThenBy (all next sorting keys)</param>
        /// <param name="sortAscending">Sort order</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> Sort<T, U>(this IQueryable<T> query, Expression<Func<T, U>> func, bool isFirstSortKey, bool sortAscending)
        {
            return (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(Expression.Call(typeof(Queryable),
                (isFirstSortKey ? "OrderBy" : "ThenBy") + (sortAscending ? string.Empty : "Descending"),
                new[] { typeof(T), typeof(U) }, query.Expression, Expression.Quote(func)));
        }
    }
}
