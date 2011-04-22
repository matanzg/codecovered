using System;

namespace CodeCovered.GeoShop.Infrastructure.Entities
{
    public abstract class EntityBase<TId>
    {
        public virtual TId Id { get; protected set; }
        public virtual int Version { get; protected set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as EntityBase<TId>);
        }

        private static bool IsTransient(EntityBase<TId> obj)
        {
            return obj != null &&
                   Equals(obj.Id, default(TId));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(EntityBase<TId> other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                       otherType.IsAssignableFrom(thisType);
            }
            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(TId)))
                return base.GetHashCode();

            return Id.GetHashCode();
        }
    }

    public abstract class GuidEntity : EntityBase<Guid>
    {

    }

    public abstract class IntEntity : EntityBase<int>
    {

    }
}