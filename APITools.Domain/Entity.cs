using APITools.Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITools.Domain
{
    public abstract class Entity : IEntity, IEquatable<Entity>
    {
        public abstract bool Equals(Entity? other);

        public override bool Equals(object? obj)
        {
            return Equals(obj as Entity);
        }

        public abstract override int GetHashCode();
    }

    public abstract class Entity<TKey> : Entity, IEntity<TKey>, IEquatable<Entity<TKey>> where TKey : IEquatable<TKey>
    {
#pragma warning disable CS8618
        protected Entity()
        {
        }
#pragma warning restore CS8618

        protected Entity(TKey id)
        {
            Id = id;
        }

        [PersonalData]
        public TKey Id { get; protected set; }

        public bool Equals(Entity<TKey>? other)
        {
            return this.CheckEquality(other);
        }

        public override bool Equals(Entity? other)
        {
            return Equals(other as Entity<TKey>);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Entity<TKey>);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Id);
            hash.Add(GetType());
            return hash.ToHashCode();
        }

        public override string ToString()
        {
            return $"[{GetType()}] Id: {Id}";
        }
    }
}
