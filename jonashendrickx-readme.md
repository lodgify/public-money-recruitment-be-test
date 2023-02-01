I enjoyed the coding exercise. A nice start was the migration from .NET Core 2.2 to .NET 6.

Remarks:
- Use of Hashets with GetHashCode override was perfect for replacing the dictionaries while keeping the primary key in the view model.
- Demonstrated knowledge of reference and value type parameters.
- I could probably go overboard with testing, actually pass a json instead of an object, and cover every possible thing. I've also written tests for middleware before, including replacing some injected services in the DI container for integration testing. This is particularly useful when testing code related to third parties.