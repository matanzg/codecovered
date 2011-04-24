using CodeCovered.GeoShop.Server.Entities;
using CodeCovered.GeoShop.Server.Mapping.Fluent.Behaviors;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class PersonMap : ClassMapWithBehaviors<Person>
    {
        public PersonMap()
        {
            Map(p => p.Name).Not.Nullable();
            Map(p => p.Gender).Not.Nullable();

            Component(p => p.HomeAddress).ColumnPrefix("Home");
        }

        protected override IFluentBehavior<Person>[] Behaviors
        {
            get
            {
                return new IFluentBehavior<Person>[]
                           {
                               new IntEntityFluentBehavior<Person>()
                           };
            }
        }
    }
}