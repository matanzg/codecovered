using CodeCovered.GeoShop.Server.Entities;
using CodeCovered.GeoShop.Server.Mapping.Fluent.Behaviors;

namespace CodeCovered.GeoShop.Server.Mapping.Fluent
{
    public class CategoryMap : ClassMapWithBehaviors<Category>
    {
        public CategoryMap()
        {
            Map(c => c.Description);
        }

        protected override IFluentBehavior<Category>[] Behaviors
        {
            get
            {
                return new IFluentBehavior<Category>[]
                           {
                               new IntEntityFluentBehavior<Category>()
                           };
            }
        }
    }
}