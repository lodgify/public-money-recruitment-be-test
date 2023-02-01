using System.Collections.Generic;
using AutoFixture;
using VacationRental.Api.Models;
using VacationRental.Api.Providers;
using Xunit;

namespace VacationRental.Api.Tests.Providers;

public class IdGeneratorTests
{
    private readonly IdGenerator _sut = new ();
    private readonly Fixture _fixture = new ();
    
    [Fact]
    public void IdGenerate_ReturnsExpectedId_WhenNoElementsinHashSet()
    {
        // arrange
        var set = new HashSet<BookingViewModel>();

        // act
        var actual = _sut.Generate(set);

        // assert
        Assert.Equal(1, actual);
    }
    
    [Fact]
    public void IdGenerate_ReturnsExpectedId_WhenOneElementinHashSet()
    {
        // arrange
        var set = new HashSet<BookingViewModel>
        {
            _fixture.Build<BookingViewModel>().With(x => x.Id, 1).Create()
        };

        // act
        var actual = _sut.Generate(set);

        // assert
        Assert.Equal(2, actual);
    }
}