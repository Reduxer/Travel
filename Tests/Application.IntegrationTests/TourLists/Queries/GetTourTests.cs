using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Travel.Application.TourLists.Queries.GetTours;
using FluentAssertions;
using Travel.Domain.Entities;

namespace Application.IntegrationTests.TourLists.Queries
{
    using static DatabaseFixture;

    [Collection("DatabaseCollection")]
    public class GetTourTests
    {
        public GetTourTests()
        {
            ResetState().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task ShouldReturnTourLists()
        {
            var query = new GetToursQuery()
            {
                NoCache = true
            };

            var result = await SendAsync(query);

            result.Lists.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldReturnAllTourListsAndPackages()
        {
            await AddAsync(new TourList
            {
                City = "Manila",
                Country = "Philippines",
                About = "Lorem Ipsum",
                TourPackages = new List<TourPackage>
                {
                    new()
                    {
                        Name = "Free Walking Tour Manila",
                        Duration = 2,
                        Price = 10,
                        InstantConfirmation = true,
                        MapLocation = "Lorem Ipsum",
                        WhatToExpect = "Lorem Ipsum"
                    }
                }
            });

            var query = new GetToursQuery()
            {
                NoCache = true
            };

            var result = await SendAsync(query);

            result.Lists.Should().HaveCount(1);
        }
    }
}
