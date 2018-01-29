using AutoMapper;

namespace Arta.Infrastructure.Profiles
{
    //https://stackoverflow.com/questions/4367591/how-to-ignore-all-destination-members-except-the-ones-that-are-mapped
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }
    }
}