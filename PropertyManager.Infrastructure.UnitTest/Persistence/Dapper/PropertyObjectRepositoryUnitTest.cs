using System.Data;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Application.Property.Queries;
using PropertyManager.Domain.Entities;
using PropertyManager.Infrastructure.Persistence.Dapper;

namespace PropertyManager.Infrastructure.UnitTest.Persistence.Dapper
{
    [TestFixture]
    public class PropertyObjectRepositoryUnitTest
    {
        private Mock<IUnitOfWork> _unitOfWorkMock = null!;
        private Mock<IReadRepository<Property>> _readRepoMock = null!;
        private PropertyObjectRepository _service = null!;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _readRepoMock = new Mock<IReadRepository<Property>>();

            _unitOfWorkMock
                .Setup(u => u.GetReadRepository<Property>())
                .Returns(_readRepoMock.Object);

            _service = new PropertyObjectRepository(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task GetFilteredPropertiesAsync_ShouldReturnFilteredProperties()
        {
            // Arrange
            var filters = new GetPropertiesPaginatedQuery
            {
                Name = "Casa",
                Address = "Calle 123",
                Price = 100000,
                CodeInternal = "XYZ123",
                Year = 2020,
                Page = 1,
                Size = 10
            };

            var expectedProperties = new List<Property>
        {
            new Property { IdProperty = 1, Name = "Casa", Address = "Calle 123" },
            new Property { IdProperty = 2, Name = "Casa 2", Address = "Calle 124" }
        };

            _readRepoMock
                .Setup(r => r.ExecuteSpListAsync("GetProperties_With_Filters", It.IsAny<Dictionary<string, object?>>()))
                .ReturnsAsync(expectedProperties);

            // Act
            var result = await _service.GetFilteredPropertiesAsync(filters);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("Casa"));
        }
    }
}
