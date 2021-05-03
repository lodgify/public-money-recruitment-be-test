# Business features
- When registering new `rentals`, you can set `PreparationTimeInDays`. By default, this value is 0
- Extended answer from `/api/v1/rentals/{rentalId}`. The answer now contains `PreparationTimeInDays`
## Method PUT has added into Controller `api/v1/rentals`. Added the ability to edit the properties of the `rental`
- You can freely increase the number of `units`
- You can freely reduce the number of preparatory days
- You can reduce the number of units, but on condition that the current booking schedule allows you to do this
- You can increase the number of days of preparation, also if the schedule allows

## Expanded response from `/api/v1/calendar`
- Added `preparationTimeInDays` property
- In response, added `freeUnits` properties for each dates object. `freeUnits` shows how many free `units` are left today
- `Nights` property has been added to each element of the `booking` array. `nights` shows the duration of the booking

# Restrictions
-Unable to book a past date
-Unable to register rentals if its Units 0 or less
-It is impossible to register a booking if all rooms for the current period are occupied

# Refactoring
- Contract models have been moved to a separate assembly
- Added assembly with message for all possible errors
- Validation of input data has been moved to separated Filters. Controllers code only works with fully valid data
- Exception handler added, it returns special ExceptionViewModel
- Test Helpers added. This allows you to reduce the size of the test without losing logic
- Added tests for checking PUT endpoint
- TestServer recreated before each test method
- Added logging