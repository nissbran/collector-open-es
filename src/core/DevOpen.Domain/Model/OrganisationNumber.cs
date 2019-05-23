using System;

namespace DevOpen.Domain.Model
{
    public class OrganisationNumber : IEquatable<OrganisationNumber>
    {
        public string Number { get; }
        
        public Country Country { get; }
        
        public OrganisationNumber(string number, Country country)
        {
            Number = number;
            Country = country;
        }

        public override string ToString()
        {
            return $"{Country.CodeSymbol}-{Number}";
        }

        public bool Equals(OrganisationNumber other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Number, other.Number) && Equals(Country, other.Country);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OrganisationNumber) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Number != null ? Number.GetHashCode() : 0) * 397) ^ (Country != null ? Country.GetHashCode() : 0);
            }
        }

        public static bool operator ==(OrganisationNumber left, OrganisationNumber right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(OrganisationNumber left, OrganisationNumber right)
        {
            return !Equals(left, right);
        }
    }
}