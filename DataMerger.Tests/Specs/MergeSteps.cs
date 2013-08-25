using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace DataMerger.Tests.Specs
{
    [Binding]
    public class MergeSteps
    {
        private Merger merger= new Merger();

        [Given(@"database does not have a user with Id (.*)")]
        public void GivenDatabaseDoesNotHaveAUserWithId(int userId)
        {
            var existingUser = merger.FindUser(userId);
            
            Assert.IsNull(existingUser);
        }
        
        [Given(@"database has User with Id (.*)")]
        public void GivenDatabaseHasUserWithId(int userId)
        {
            merger.Merge(new User() {UniqueId = userId});
        }

        [Given(@"database User with Id(.*) has addresses \[(.*)]")]
        public void GivenDatabaseUserWithIdHasAddresses(int userId, string aliases)
        {
            var userAliases = ParseAliases(aliases);

            var incomingUser = new User {UniqueId = userId, EmailAddresses = new List<UserAlias>(userAliases)};
            merger.Merge(incomingUser);
        }

        [When(@"Merge user with Id (.*)")]
        public void WhenMergeUserWithId(int userId)
        {
            merger.Merge(new User {UniqueId = userId});
        }

        [When(@"Merge user with Id (.*) and addresses \[(.*)]")]
        public void WhenMergeUserWithIdAndAddresses(int userId, string aliases)
        {
            var userAliases = ParseAliases(aliases);

            var incomingUser = new User { UniqueId = userId, EmailAddresses = new List<UserAlias>(userAliases) };
            merger.Merge(incomingUser);
        }

        [Then(@"database has user with Id (.*)")]
        public void ThenDatabaseHasUserWithId(int userId)
        {
            var existingUser = merger.FindUser(userId);
            Assert.IsNotNull(existingUser);
        }

        [Then(@"database has user Id (.*) and addresses \[(.*)]")]
        public void ThenDatabaseHasUserIdAndAddresses(int userId, string addresses)
        {
            List<UserAlias> userAliases = ParseAliases(addresses).ToList();

            var existingUser = merger.FindUser(userId);
            
            Assert.IsNotNull(existingUser);
            CollectionAssert.AreEquivalent(userAliases, existingUser.EmailAddresses);

            Assert.AreEqual(userAliases.Count(x => x.Active),
               existingUser.EmailAddresses.Count(x => x.Active));
        }

        [Then(@"database does not have User with Id (.*)")]
        public void ThenDatabaseDoesNotHaveUserWithId(int userId)
        {
            var existingUser = merger.FindUser(userId);

            Assert.IsNull(existingUser);
        }


        [Given(@"database is empty")]
        public void GivenDatabaseIsEmpty()
        {
            merger.Clear();
        }


        private static IEnumerable<UserAlias> ParseAliases(string aliases)
        {
            var results = aliases.Split(new[] { ',' });
            foreach (var result in results)
            {
                var parts = result.Split(new[] { '|' });
                yield return new UserAlias
                {
                    Address = parts[0].Trim(),
                    Active = parts[1] == "Active"
                };
            }
        }
    }
}
