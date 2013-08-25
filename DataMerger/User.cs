using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMerger
{
    public class UserAlias
    {
        public string Address { get; set; }
        public bool Active { get; set; }

        // Only the address is relevant
        protected bool Equals(UserAlias other)
        {
            return string.Equals(Address, other.Address);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserAlias) obj);
        }

        public override int GetHashCode()
        {
            return (Address != null ? Address.GetHashCode() : 0);
        }
    }

    public class User
    {
        public User()
        {
            EmailAddresses = new List<UserAlias>();
        }

        public int UniqueId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<UserAlias> EmailAddresses { get; set; }

        public bool DoesNotHaveEmailAddress(UserAlias a)
        {
            return !EmailAddresses.Contains(a);
        }


        public void Merge(User incomingUser)
        {
            FirstName = incomingUser.FirstName;
            LastName = incomingUser.LastName;

            var deletedAliases = EmailAddresses.Where(incomingUser.DoesNotHaveEmailAddress).ToList();
            foreach (var alias in deletedAliases)
            {
                alias.Active = false;
            }

            var newAliases = incomingUser.EmailAddresses.Where(DoesNotHaveEmailAddress);
            EmailAddresses.AddRange(newAliases);
        }

        // They are considered equal if they have the same unique id or
        // if they have one address that matches.
        protected bool Equals(User other)
        {
            return UniqueId.Equals(other.UniqueId)
                   || (EmailAddresses != null && EmailAddresses.Any(ea => other.EmailAddresses.Contains(ea)));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            return UniqueId.GetHashCode();
        }
    }
}