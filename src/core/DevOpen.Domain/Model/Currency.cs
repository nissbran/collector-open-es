using System;
using System.Collections.Generic;

namespace DevOpen.Domain.Model
{
    public sealed class Currency : IEquatable<Currency>
    {
        public string Code { get; }
        
        public string Name { get; }
        
        private Currency(string code, string name)
        {
            Code = code;
            Name = name;
        }
        
        public static readonly Currency Empty = new Currency("Empty", "");
        
        public static readonly Currency SEK = new Currency("SEK", "Swedish Krona"); 
        public static readonly Currency EUR = new Currency("EUR", "Euro");
        public static readonly Currency NOK = new Currency("NOK", "Norwegian Krone");
        public static readonly Currency DKK = new Currency("DKK", "Danske Krone");

        private static readonly IReadOnlyDictionary<string, Currency> ValidCurrencies = new Dictionary<string, Currency>
        {
            {SEK.Code, SEK},
            {EUR.Code, EUR},
            {NOK.Code, NOK},
            {DKK.Code, DKK},
        };
        
        public static Currency Parse(string code)
        {
            if (ValidCurrencies.TryGetValue(code.ToUpperInvariant(), out var currency))
                return currency;
            
            throw new FormatException($"Currency code {code}, is not a valid currency");
        }

        public bool Equals(Currency other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Code, other.Code);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Currency other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Code != null ? Code.GetHashCode() : 0);
        }

        public static bool operator ==(Currency left, Currency right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Currency left, Currency right)
        {
            return !Equals(left, right);
        }
    }
}