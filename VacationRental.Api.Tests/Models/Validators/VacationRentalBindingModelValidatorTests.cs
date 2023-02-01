using FluentValidation.TestHelper;
using VacationRental.Api.Models;
using VacationRental.Api.Validators;
using Xunit;

namespace VacationRental.Api.Tests.Models.Validators;

public class VacationRentalBindingModelValidatorTests
{
    private readonly VacationRentalBindingModelValidator _sut = new();

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(4)]
    public void PreparationTimeInDays_Is_Invalid_WhenEqualToValue(int value)
    {
        // arrange
        var model = new VacationRentalBindingModel
        {
            PreparationTimeInDays = value
        };

        // act
        var actual = _sut.TestValidate(model);

        // assert
        actual.ShouldHaveValidationErrorFor(x => x.PreparationTimeInDays);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void PreparationTimeInDays_Is_Valid_WhenEqualToValue(int value)
    {
        // arrange
        var model = new VacationRentalBindingModel
        {
            PreparationTimeInDays = value
        };

        // act
        var actual = _sut.TestValidate(model);

        // assert
        actual.ShouldNotHaveValidationErrorFor(x => x.PreparationTimeInDays);
    }
}