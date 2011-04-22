using System;
using CodeCovered.GeoShop.Infrastructure.Entities;
using FluentNHibernate.Mapping;

namespace CodeCovered.GeoShop.Mapping.Fluent.Behaviors
{
    public class GuidEntityFluentBehavior<T> : IFluentBehavior<T>
        where T : GuidEntity
    {
        public void ApplyMap(ClassMap<T> mapBase)
        {
            mapBase.Id(p => p.Id).GeneratedBy.GuidComb();
            mapBase.Version(p => p.Version);
        }
    }
}