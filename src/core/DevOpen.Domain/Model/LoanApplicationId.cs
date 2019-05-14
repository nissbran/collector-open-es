using System;

namespace DevOpen.Domain.Model
{
    public struct LoanApplicationId : IEquatable<LoanApplicationId>
    {
        private readonly Guid _value;

        private LoanApplicationId(Guid value) 
        {
            _value = value;
        }

        public static LoanApplicationId Empty => new LoanApplicationId(Guid.Empty);
        public static LoanApplicationId NewId() => new LoanApplicationId(Guid.NewGuid());

        public static implicit operator Guid(LoanApplicationId id) => id._value;
        public static implicit operator string(LoanApplicationId id) => id._value.ToString();

        public static LoanApplicationId Parse(Guid id) => new LoanApplicationId(id);
        public static LoanApplicationId Parse(string id) => new LoanApplicationId(Guid.Parse(id));
        
        public override string ToString() => _value.ToString();
        
        public bool Equals(LoanApplicationId other) 
        {
            return _value.Equals(other._value);
        }

        public override bool Equals(object obj) 
        {
            if (obj is null) return false;
            return obj is LoanApplicationId id && Equals(id);
        }

        public override int GetHashCode() 
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(LoanApplicationId left, LoanApplicationId right) 
        {
            return left.Equals(right);
        }

        public static bool operator !=(LoanApplicationId left, LoanApplicationId right) 
        {
            return !left.Equals(right);
        }
    }
}