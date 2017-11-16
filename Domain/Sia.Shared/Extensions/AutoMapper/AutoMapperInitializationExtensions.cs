using AutoMapper.EquivalencyExpression;
using Sia.Shared.Data;

namespace AutoMapper
{
    public static class AutoMapperInitializationExtensions
    {
        public static IMappingExpression<TSource, TDestination> UseResolveJsonToString<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
            where TSource : IJsonDataObject
            where TDestination : IJsonDataString
            => mapping.ForMember((ev) => ev.Data, (config) => config.ResolveUsing<ResolveJsonToString<TSource, TDestination>>());


        public static IMappingExpression<TSource, TDestination> UseResolveStringToJson<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
            where TSource : IJsonDataString
            where TDestination : IJsonDataObject
            => mapping.ForMember((ev) => ev.Data, (config) => config.ResolveUsing<ResolveStringToJson<TSource, TDestination>>());


        public static IMappingExpression<T1, T2> EqualityInsertOnly<T1, T2>(this IMappingExpression<T1, T2> mappingExpression)
            where T1 : class
            where T2 : class
            => mappingExpression.EqualityComparison((one, two) => false);

        public static IMappingExpression<T1, T2> EqualityById<T1, T2>(this IMappingExpression<T1, T2> mappingExpression)
            where T1 : class, IEntity
            where T2 : class, IEntity
            => mappingExpression.EqualityComparison((one, two) => one.Id == two.Id);
    }
}
