using System;
using System.Text.RegularExpressions;
using Inflow.Services.Customers.Core.Exceptions;

namespace Inflow.Services.Customers.Core.Domain.ValueObjects
{
    public class Email : IEquatable<Email>
    {
        private static readonly Regex Regex = new(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.Compiled);
        
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidEmailException(value);
            }

            if (value.Length > 100)
            {
                throw new InvalidEmailException(value);
            }

            value = value.ToLowerInvariant();
            if (!Regex.IsMatch(value))
            {
                throw new InvalidEmailException(value);
            }

            Value = value;
        }

        public static implicit operator string(Email email) => email.Value;

        public static implicit operator Email(string email) => new(email);

        public bool Equals(Email other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Email) obj);
        }

        public override int GetHashCode() => Value is not null ? Value.GetHashCode() : 0;
        
        public override string ToString() => Value;
    }
}