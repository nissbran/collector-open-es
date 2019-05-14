using System;

namespace DevOpen.Domain.Model
{
    public struct CreditApplicationId : IEquatable<CreditApplicationId>
    {
        private readonly Guid _value;

        private CreditApplicationId(Guid value) 
        {
            _value = value;
        }

        public static CreditApplicationId Empty => new CreditApplicationId(Guid.Empty);
        public static CreditApplicationId NewId() => new CreditApplicationId(Guid.NewGuid());

        public static implicit operator Guid(CreditApplicationId id) => id._value;
        public static implicit operator string(CreditApplicationId id) => id._value.ToString();

        public static CreditApplicationId Parse(Guid id) => new CreditApplicationId(id);
        public static CreditApplicationId Parse(string id) => new CreditApplicationId(Guid.Parse(id));
        
        public override string ToString() => _value.ToString();
        
        public bool Equals(CreditApplicationId other) 
        {
            return _value.Equals(other._value);
        }

        public override bool Equals(object obj) 
        {
            if (obj is null) return false;
            return obj is CreditApplicationId id && Equals(id);
        }

        public override int GetHashCode() 
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(CreditApplicationId left, CreditApplicationId right) 
        {
            return left.Equals(right);
        }

        public static bool operator !=(CreditApplicationId left, CreditApplicationId right) 
        {
            return !left.Equals(right);
        }
    }
}