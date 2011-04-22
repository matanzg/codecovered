using System;
using Caliburn.Core;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace CodeCovered.GeoShop.Infrastructure.NHibernate.Conventions
{
    public class UnderscorePropertyConvention : IPropertyConvention, IIdConvention, IVersionConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            var propName = Inflector.Underscore(instance.Name.ToLower());
            instance.Column(propName);
        }

        public void Apply(IIdentityInstance instance)
        {
            var propName = Inflector.Underscore(instance.Name.ToLower());
            instance.Column(propName);
        }

        public void Apply(IVersionInstance instance)
        {
            var propName = Inflector.Underscore(instance.Name.ToLower());
            instance.Column(propName);
        }
    }
}