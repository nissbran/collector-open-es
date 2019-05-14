using System;
using System.Collections.Generic;
using System.Linq;

namespace DevOpen.Domain.Model
{
    public sealed class Country : IEquatable<Country>
    {
        public CountryCode Code { get; }
        public string CodeSymbol { get; }
        public Currency DefaultCurrency { get; }
        public string Name { get; }

        private Country(CountryCode code, string name, Currency defaultCurrency)
        {
            Code = code;
            CodeSymbol = code == CountryCode.None ? string.Empty : code.ToString().ToUpperInvariant();
            DefaultCurrency = defaultCurrency;
            Name = name;
        }

        public static readonly Country Empty = new Country(CountryCode.None, "Empty", Currency.Empty);

        public static readonly Country Sweden = new Country(CountryCode.SE, "Sweden", Currency.SEK);
        public static readonly Country Norway = new Country(CountryCode.NO, "Norway", Currency.NOK);
        public static readonly Country Finland = new Country(CountryCode.FI, "Finland", Currency.EUR);
        public static readonly Country Denmark = new Country(CountryCode.DK, "Denmark", Currency.DKK);

        private static readonly IReadOnlyList<Country> ValidCountries = new List<Country>
        {
            Sweden, Norway, Finland, Denmark
        };

        public static bool TryParse(string countryCode, out Country country)
        {
            if (string.IsNullOrEmpty(countryCode))
            {
                country = null;
                return false;
            }
            
            country = ValidCountries.FirstOrDefault(c => c.CodeSymbol == countryCode.ToUpperInvariant());

            return country != null;
        }

        public static Country Parse(string code)
        {
            if (TryParse(code, out var country))
                return country;

            throw new FormatException($"Country code {code}, is not a valid country");
        }

        public bool Equals(Country other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Code, other.Code);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Country other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int) Code;
        }

        public static bool operator ==(Country left, Country right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Country left, Country right)
        {
            return !Equals(left, right);
        }
    }
    public enum CountryCode
    {
        None,
        SE,
        NO,
        DK,
        FI
    }
}