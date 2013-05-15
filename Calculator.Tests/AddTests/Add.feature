Feature: Add
	This is text I can add.
	It can be anything I think is useful.

Scenario: Add two numbers
	Given I have entered 50 and 70 into the calculator
	When I press add
	Then the result should be 120 on the screen
