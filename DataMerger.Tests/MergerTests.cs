using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataMerger.Tests
{
    [TestClass]
    public class MergerTests
    {
        [TestMethod]
        [TestCase("Merge a new user", "mark.lamley")]
        public void Merge_NewUser_UserExists()
        {
            var merger = new Merger();

            const int uniqueId = 1;
            merger.Merge(new User {UniqueId = uniqueId});

            var exists = merger.FindUser(uniqueId);
            Assert.IsNotNull(exists);
        }

        [TestMethod]
        [TestCase("There are no users found.", "mark.lamley")]
        public void FindUser_NoUsersExist_ReturnsNull()
        {
            var merger = new Merger();
            
            var existingUser = merger.FindUser(1);

            Assert.IsNull(existingUser);
        }

        // Given database does not have a user with Id 1 
        // When Merge user with Id 1 
        // Then database has user with Id 1
        [TestMethod]
        [TestCase("After merging one user is added.", "mark.lamley")]
        public void Merge_MergeUserInDatabaseWithNoUsers_OneUserIsAdded()
        {
            var merger = new Merger();

            const int id1 = 1;
            var existingUser = merger.FindUser(id1);
            Assert.IsNull(existingUser);
            
            var user1 = new User{UniqueId = id1};
            merger.Merge(user1);

            existingUser = merger.FindUser(id1);
            Assert.IsNotNull(existingUser);
        }

        // Given database has User with Id 1 
        // Given User with Id1 has addresses [foo1@gr.com|Active, foo2@gr.com|Active]
        // When Merge user with Id 1 and addresses [foo2@gr.com|Active, foo3@gr.com|Active]
        // Then database has user Id 1 and addresses [foo1@gr.com|Inactive, foo2@gr.com|Active, foo3@gr.com|Active]
        [TestMethod]
        [TestCase("After merging the user has 1 address deactivated and a new address added.", "mark.lamley")]
        public void Merge_DatabaseHasOneUserWithTwoAddressesAndMergeUserWithTwoDifferentAddresses_ResultingUserHasThreeAddressesOneIsDisabled()
        {
            var merger = new Merger();

            const int user1Id = 1;
            var user1 = new User
                            {
                                UniqueId = user1Id,
                                EmailAddresses = new List<UserAlias>
                                                     {
                                                         new UserAlias {Active = true, Address = "foo1@gr.com"},
                                                         new UserAlias {Active = true, Address = "foo2@gr.com"}
                                                     }
                            };
            merger.Merge(user1);
            Assert.AreEqual(1, merger.UserCount);

            user1 = new User
                        {
                            UniqueId = user1Id,
                            EmailAddresses = new List<UserAlias>
                                                 {
                                                     new UserAlias {Active = true, Address = "foo2@gr.com"},
                                                     new UserAlias {Active = true, Address = "foo3@gr.com"}
                                                 }
                        };
            merger.Merge(user1);

            var existingUser = merger.FindUser(user1Id);

            CollectionAssert.AreEquivalent(
                new List<UserAlias>
                    {
                        new UserAlias {Active = false, Address = "foo1@gr.com"},
                        new UserAlias {Active = true, Address = "foo2@gr.com"},
                        new UserAlias {Active = true, Address = "foo3@gr.com"}
                    }, 
                existingUser.EmailAddresses);
        }

        // Given database has user with Id 1 
        // Given User with Id 1 has addresses [foo1@gr.com|Active, foo2@gr.com|Active]
        // When Merge user with Id 2 and addresses [foo2@gr.com|Active, foo3@gr.com|Active]
        // Then database has user Id 1 and addresses [foo1@gr.com|Active, foo2@gr.com|Active]
        // Then database does not have User with Id 2
        [TestMethod]
        [TestCase("After merging the user is not changed and the conflicting user is not added.", "mark.lamley")]
        public void Merge_DatabaseHasOneUserWithTwoAddressesAndMergeADifferentUserWhereOneOfTheAddressesMatch_User1IsNotChangedAndUser2IsNotMerged()
        {
            var merger = new Merger();

            const int user1Id = 1;
            var user1 = new User
            {
                UniqueId = user1Id,
                EmailAddresses = new List<UserAlias>
                                                     {
                                                         new UserAlias {Active = true, Address = "foo1@gr.com"},
                                                         new UserAlias {Active = true, Address = "foo2@gr.com"}
                                                     }
            };
            merger.Merge(user1);

            const int user2Id = 2;
            var user2 = new User
            {
                UniqueId = user2Id,
                EmailAddresses = new List<UserAlias>
                                                 {
                                                     new UserAlias {Active = true, Address = "foo2@gr.com"},
                                                     new UserAlias {Active = true, Address = "foo3@gr.com"}
                                                 }
            };
            merger.Merge(user2);

            user1 = merger.FindUser(user1Id);
            CollectionAssert.AreEquivalent(
                new List<UserAlias>
                    {
                        new UserAlias {Active = true, Address = "foo1@gr.com"},
                        new UserAlias {Active = true, Address = "foo2@gr.com"},
                    },
                user1.EmailAddresses);

            user2 = merger.FindUser(user2Id);
            Assert.IsNull(user2);
        }
    }
}
