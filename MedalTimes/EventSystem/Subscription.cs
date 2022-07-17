using System;

namespace TNRD.Zeepkist.MedalTimes.EventSystem
{
    public class Subscription : IEquatable<Subscription>
    {
        private readonly Guid guid;
        private readonly Type type;

        public Guid Guid => guid;
        public Type Type => type;

        public Subscription(Type type)
        {
            this.type = type;
            guid = Guid.NewGuid();
        }

        /// <inheritdoc />
        public bool Equals(Subscription other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return guid.Equals(other.guid);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Subscription)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return guid.GetHashCode();
        }
    }
}
