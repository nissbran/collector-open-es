using System;

namespace DevOpen.Domain.Model
{
    public class Address : IEquatable<Address>
    {
        public string Street { get; }
        public string Street2 { get; }
        public string PostalCode { get; }
        public string City { get; }
        public string Country { get; }
        public string CareOf { get; }

        public Address(string street, string street2, string postalCode, string city, string country, string careOf)
        {
            Street = street;
            Street2 = street2;
            PostalCode = postalCode;
            City = city;
            Country = country;
            CareOf = careOf;
        }
        
        public static Address Empty => new Address(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

        public bool IsComplete => !string.IsNullOrEmpty(Street) && !string.IsNullOrEmpty(PostalCode) && !string.IsNullOrEmpty(City) && !string.IsNullOrEmpty(Country);

        public bool IsEmpty => string.IsNullOrEmpty(Street) && string.IsNullOrEmpty(Street2) && string.IsNullOrEmpty(CareOf) && 
                               string.IsNullOrEmpty(PostalCode) && string.IsNullOrEmpty(City) && string.IsNullOrEmpty(Country);

        public bool Equals(Address other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Street, other.Street) && 
                   string.Equals(Street2, other.Street2) && 
                   string.Equals(PostalCode, other.PostalCode) &&
                   string.Equals(City, other.City) &&
                   string.Equals(Country, other.Country) && 
                   string.Equals(CareOf, other.CareOf);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Address) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Street != null ? Street.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Street2 != null ? Street2.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PostalCode != null ? PostalCode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (City != null ? City.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Country != null ? Country.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CareOf != null ? CareOf.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}