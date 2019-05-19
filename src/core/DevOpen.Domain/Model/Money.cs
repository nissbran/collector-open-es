using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DevOpen.Domain.Model
{
    public class Money : IEquatable<Money>
    {
        public decimal Value { get; }

        public Currency Currency { get; }
        
        private Money(decimal value, Currency currency)
        {
            Value = value;
            Currency = currency;
        }

        public bool IsEmpty => Value == 0;

        public static Money Create(decimal value, Currency currency)
        {
            return new Money(value, currency);
        }
        
        private static readonly Regex ParseRegex = new Regex(@"^(\-?[0-9\.,]+) ([a-zA-Z]{3})$", RegexOptions.Compiled);

        public static Money Parse(string value)
        {
            var match = ParseRegex.Match(value);
            if (!match.Success) throw new Exception($"Could not parse the string value '{value}' into a Money object.");

            var amountString = match.Groups[1].Value;
            var amount = decimal.Parse(amountString, CultureInfo.InvariantCulture);

            var currencySymbol = match.Groups[2].Value;
            var currency = Currency.Parse(currencySymbol);

            return new Money(amount, currency);
        }

        public override string ToString()
        {
            return $"{Value:N2} {Currency.Code}";
        }

        public string ToFullPrecisionString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", Value, Currency.Code);
        }

        public Money Add(Money other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (IsEmpty) return other;
            if (other.IsEmpty) return this;
            if (!Currency.Equals(other.Currency))
                throw new InvalidOperationException("Cannot add different currencies.");

            return new Money(Value + other.Value, Currency);
        }

        public Money Subtract(Money other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (IsEmpty) return new Money(-other.Value, other.Currency);
            if (other.IsEmpty) return this;
            if (!Currency.Equals(other.Currency))
                throw new InvalidOperationException("Cannot subtract different currencies.");

            return new Money(Value - other.Value, Currency);
        }

        public Money Multiply(Money other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (IsEmpty) return this;
            if (!Currency.Equals(other.Currency))
                throw new InvalidOperationException("Cannot multiply different currencies.");

            return new Money(Value * other.Value, Currency);
        }

        public Money DivideBy(Money other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (!Currency.Equals(other.Currency))
                throw new InvalidOperationException("Cannot divide different currencies.");

            return new Money(Value / other.Value, Currency);
        }

        public static Money operator +(Money left, Money right) => left.Add(right);
        public static Money operator -(Money left, Money right) => left.Subtract(right);
        public static Money operator *(Money left, Money right) => left.Multiply(right);
        public static Money operator /(Money left, Money right) => left.DivideBy(right);
        public static Money operator -(Money money) => new Money(-money.Value, money.Currency);

        public static implicit operator decimal(Money money) => money.Value;
        public static implicit operator Currency(Money money) => money.Currency;
        public static implicit operator string(Money money) => money.Value.ToString(CultureInfo.InvariantCulture);
        public static implicit operator Money(string value) => Parse(value);

        public bool Equals(Money other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value && Equals(Currency, other.Currency);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Money) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Value.GetHashCode() * 397) ^ (Currency != null ? Currency.GetHashCode() : 0);
            }
        }
    }
}