using System;

namespace NetCoreStack.Hisar
{
    public sealed class CacheItem : IEquatable<CacheItem>
    {
        public DateTimeOffset? AbsoluteExpiration { get; set; }
        public string Key { get; }

        public CacheItem(string key)
        {
            Key = key;
        }

        public bool Equals(CacheItem other)
        {
            return other != null && other.Key.Equals(Key, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CacheItem);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public static bool operator ==(CacheItem left, CacheItem right)
        {
            if ((object)left == null)
            {
                return ((object)right == null);
            }
            else if ((object)right == null)
            {
                return ((object)left == null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(CacheItem left, CacheItem right)
        {
            return !(left == right);
        }
    }
}
