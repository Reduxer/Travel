using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Travel.Domain.Entities;
using Travel.Application.Dtos.Tour;
using Travel.Application.Common.Mappings;
using Xunit;

namespace Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configurationProvider;

        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configurationProvider = new MapperConfiguration(setup =>
            {
                setup.AddProfile<MappingProfile>();
            });

            _mapper = _configurationProvider.CreateMapper();
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _configurationProvider.AssertConfigurationIsValid();
        }

        [Theory]
        [InlineData(typeof(TourList), typeof(TourListDto))]
        [InlineData(typeof(TourPackage), typeof(TourPackageDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);
            _mapper.Map(instance, source, destination);
        }
    }
}
