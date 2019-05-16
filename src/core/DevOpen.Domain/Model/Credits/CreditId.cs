using System;

namespace DevOpen.Domain.Model.Credits
{
    public struct CreditId : IEquatable<CreditId>
    {
        private readonly Guid _value;

        private CreditId(Guid value) 
        {
            _value = value;
        }

        public static CreditId Empty => new CreditId(Guid.Empty);
        public static CreditId NewId() => new CreditId(Guid.NewGuid());

        public static implicit operator Guid(CreditId id) => id._value;
        public static implicit operator string(CreditId id) => id._value.ToString();

        public static CreditId Parse(Guid id) => new CreditId(id);
        public static CreditId Parse(string id) => new CreditId(Guid.Parse(id));
        
        public override string ToString() => _value.ToString();
        
        public bool Equals(CreditId other) 
        {
            return _value.Equals(other._value);
        }

        public override bool Equals(object obj) 
        {
            if (obj is null) return false;
            return obj is CreditId id && Equals(id);
        }

        public override int GetHashCode() 
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(CreditId left, CreditId right) 
        {
            return left.Equals(right);
        }

        public static bool operator !=(CreditId left, CreditId right) 
        {
            return !left.Equals(right);
        }
    }
}