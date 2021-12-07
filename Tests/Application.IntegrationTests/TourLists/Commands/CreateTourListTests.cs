using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Travel.Application.TourLists.Commands.CreateTourList;
using Travel.Application.Common.Exceptions;
using Travel.Domain.Entities;

namespace Application.IntegrationTests.TourLists.Commands
{
    using static DatabaseFixture;

    [Collection("DatabaseCollection")]
    public class CreateTourListTests
    {
        public CreateTourListTests()
        {
            ResetState().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task ShouldRequireMinimumFields()
        {
            var command = new CreateTourListCommand();
            await FluentActions.Invoking(async () => await SendAsync(command))
                .Should()
                .ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ShouldRequireAbout()
        {
            var command = new CreateTourListCommand()
            {
                City = "Angeles",
                Country = "Philippines",
                About = String.Empty,
            };

            await FluentActions.Invoking(async () => await SendAsync(command))
                .Should()
                .ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ShouldCreateTourList()
        {
            var command = new CreateTourListCommand()
            {
                City = "Angeles",
                Country = "Philippines",
                About = "I Lived Here",
            };

            var id = await SendAsync(command);
            var list = await FindAsync<TourList>(id);

            list.Should().NotBeNull();
            list.City.Should().Be(command.City);
            list.Country.Should().Be(command.Country);
            list.About.Should().Be(command.About);
        }
    }
}
