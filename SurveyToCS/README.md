To make sure the survey json works properly with SurveyToCS:

1) No spaces when wrapping a property in brackets... {property} is good but { property } is no good.

2) When setting a visibleIf condition, make sure the value condition is inside single quotes.
	{TypeOfBreach} = 'other' is good but {TypeOfBreach} = other is no good

3) In a list of values such as for anyof, make sure that there is no blank spaces between the items.
	... anyof ['item1','item2','item3','item4'] is good but anyof ['item1', 'item2', 'item3', 'item4'] is not good.


4) CAPITALIZE the first letter of all variable names in JSON.

5)
