using Caliburn.Core;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace CodeCovered.GeoShop.Infrastructure.NHibernate.Conventions
{
    public class PluralClassConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            var tableName = Inflector.Pluralize(instance.EntityType.Name);
            instance.Table(tableName);
        }
    }
}