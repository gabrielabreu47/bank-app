using System.Linq.Expressions;

namespace Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Filter<T>(this IQueryable<T> query, string? filters)
    {
        if (filters == null) return query;

        var parameter = Expression.Parameter(typeof(T), "t");
        var expressions = new List<Expression>();
        foreach (var filterGroup in filters.Split("&"))
        {
            var orExpressions = new List<Expression>();
            foreach (var filter in filterGroup.Split("|"))
            {
                var parts = filter.Split("="); 
                if (parts.Length != 2) continue;

                var propertyName = typeof(T).GetProperties()
                    .FirstOrDefault(p => p.Name.Equals(parts[0], StringComparison.OrdinalIgnoreCase))?.Name;

                if (propertyName == null) continue;

                var property = Expression.Property(parameter, propertyName);
                var value = Expression.Constant(parts[1].ToLower());
                MethodCallExpression propertyToString;

                if (property.Type == typeof(DateTime) || property.Type == typeof(DateTimeOffset))
                {
                    var argument = Expression.Constant("yyyy-MM-dd");
                    var toString = property.Type.GetMethod("ToString", new Type[] { typeof(string) });
                    propertyToString = Expression.Call(property, toString!, argument);
                }
                else 
                { 
                    propertyToString = Expression.Call(property, "ToString", null);
                }

                var propertyLower = Expression.Call(propertyToString, "ToLower", null);
                var contains = Expression.Call(propertyLower, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, value);
                orExpressions.Add(contains);
            }

            if (orExpressions.Count > 0)
            {
                expressions.Add(orExpressions.Aggregate(Expression.OrElse));
            }
        }
        var body = expressions.Count > 0 ? expressions.Aggregate(Expression.AndAlso) : Expression.Constant(true);
        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
        var res = query.Where(lambda);
        return res;
    }
}