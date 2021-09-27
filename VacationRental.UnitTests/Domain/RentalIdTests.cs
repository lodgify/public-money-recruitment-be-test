using VacationRental.Domain.Values;
using Xunit;

namespace VacationRental.UnitTests.Domain
{
    [Collection("Domain")]
    public class RentalIdTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void Equals_ValuesAreTheSame_ReturnsTrue(int rawValue)
        {
            var id = new RentalId(rawValue);
            var anotherId = new RentalId(rawValue);

            var equal = id.Equals(anotherId);

            Assert.True(equal);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void EqualOperator_ValuesAreTheSame_ReturnsTrue(int rawValue)
        {
            var id = new RentalId(rawValue);
            var anotherId = new RentalId(rawValue);

            var equal = id == anotherId;

            Assert.True(equal);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void Equals_ValuesAreNotTheSame_ReturnsFalse(int rawValue)
        {
            var id = new RentalId(rawValue);
            var anotherId = new RentalId(rawValue + 1);

            var equal = id.Equals(anotherId);

            Assert.False(equal);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void NotEqualOperator_ValuesAreNotTheSame_ReturnsTrue(int rawValue)
        {
            var id = new RentalId(rawValue);
            var anotherId = new RentalId(rawValue + 1);

            var equal = id != anotherId;

            Assert.True(equal);
        }
    }
}
