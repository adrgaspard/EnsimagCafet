using APITools.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace APITools.Domain
{
    public static class EntityHelper
    {
        public static bool? CheckEquality(this IEntity source, IEntity? other)
        {
            if (source is null || other is null)
            {
                return false;
            }
            if (ReferenceEquals(source, other))
            {
                return true;
            }
            if (source.GetType() != other.GetType())
            {
                return false;
            }
            return null;
        }

        public static bool CheckEquality<TKey>(this IEntity<TKey> source, IEntity<TKey>? other) where TKey : IEquatable<TKey>
        {
            if (source is null || other is null)
            {
                return false;
            }
            if (ReferenceEquals(source, other))
            {
                return true;
            }
            if (source.GetType() != other.GetType())
            {
                return false;
            }
            return source.Id.Equals(other.Id);
        }
    }
}
