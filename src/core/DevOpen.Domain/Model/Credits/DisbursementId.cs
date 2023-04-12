using System;

namespace DevOpen.Domain.Model.Credits
{
    public readonly struct DisbursementId : IEquatable<DisbursementId>
    {
        private readonly Guid _value;

        private DisbursementId(Guid value) 
        {
            _value = value;
        }

        public static DisbursementId Empty => new DisbursementId(Guid.Empty);
        public static DisbursementId NewId() => new DisbursementId(Guid.NewGuid());

        public static implicit operator Guid(DisbursementId id) => id._value;
        public static implicit operator string(DisbursementId id) => id._value.ToString();

        public static DisbursementId Parse(Guid id) => new DisbursementId(id);
        public static DisbursementId Parse(string id) => new DisbursementId(Guid.Parse(id));
        
        public override string ToString() => _value.ToString();
        
        public bool Equals(DisbursementId other) 
        {
            return _value.Equals(other._value);
        }

        public override bool Equals(object obj) 
        {
            if (obj is null) return false;
            return obj is DisbursementId id && Equals(id);
        }

        public override int GetHashCode() 
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(DisbursementId left, DisbursementId right) 
        {
            return left.Equals(right);
        }

        public static bool operator !=(DisbursementId left, DisbursementId right) 
        {
            return !left.Equals(right);
        }
    }
}