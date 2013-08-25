Feature: Merge
	Merging users from one repository to my local database.

Scenario: Merge one user
	Given database is empty
	Given database does not have a user with Id 1 
    When Merge user with Id 1 
    Then database has user with Id 1

Scenario: Merge one users aliases. Deactivate alias that was deleted.
	Given database is empty
	Given database has User with Id 1 
    Given database User with Id1 has addresses [foo1@gr.com|Active, foo2@gr.com|Active]
    When Merge user with Id 1 and addresses [foo2@gr.com|Active, foo3@gr.com|Active]
	Then database has user Id 1 and addresses [foo1@gr.com|Inactive, foo2@gr.com|Active, foo3@gr.com|Active]

Scenario: Merge a second user. The user is not merged because someone has already claimed their aliases.
	Given database has User with Id 1 
    Given database User with Id1 has addresses [foo1@gr.com|Active, foo2@gr.com|Active]
    When Merge user with Id 2 and addresses [foo2@gr.com|Active, foo3@gr.com|Active]
    Then database has user Id 1 and addresses [foo1@gr.com|Active, foo2@gr.com|Active]
    Then database does not have User with Id 2