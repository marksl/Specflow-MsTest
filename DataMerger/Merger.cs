using System.Collections.Generic;
using System.Linq;

namespace DataMerger
{
    public class Merger : IMerge
    {
        private readonly List<User> _users = new List<User>();

        public void Merge(User incomingUser)
        {
            var existingUser = _users.Find(u => u.Equals(incomingUser));
            if (existingUser == null)
            {
                _users.Add(incomingUser);
            }
            else
            {
                // Don't add if the found user has a different Id. That means it's actually a different from the incoming user.
                if (existingUser.UniqueId != incomingUser.UniqueId)
                    return;

                existingUser.Merge(incomingUser);
            }
        }

        public User FindUser(int uniqueId)
        {
            return _users.SingleOrDefault(u => u.UniqueId == uniqueId);
        }

        public int UserCount
        {
            get { return _users.Count; }
        }

        public void Clear()
        {
            _users.Clear();
        }
    }
}