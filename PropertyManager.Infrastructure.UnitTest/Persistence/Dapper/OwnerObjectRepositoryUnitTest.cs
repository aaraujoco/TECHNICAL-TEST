using System.Data;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Domain.Entities;
using PropertyManager.Infrastructure.Persistence.Dapper;

namespace PropertyManager.Infrastructure.UnitTest.Persistence.Dapper
{
    [TestFixture]
    public class OwnerObjectRepositoryUnitTest
    {
        private IOwnerObjectRepository _sut = null!;
        private Mock<IDbConnectionProvider> _dbConnectionProviderMock = null!;
        private Mock<ILogger<DapperUnitOfWork>> _loggerDapper = null!;
        private Mock<IDbConnection> _iDbConnectionMock = null!;
        private Mock<IReadRepository<Owner>> _readRepositoryMock;
        private Mock<IDbTransaction> _iDbTransactionMock = null!;
        private Mock<IUnitOfWork> _iUnitWorkMock = null!;
        private DapperDbContext _sutDapper = null!;
        private DapperUnitOfWork _unitWorkDapper = null!;
        private Mock<ILogger<DapperDbContext>> _loggerMock = null!;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _readRepositoryMock = new Mock<IReadRepository<Owner>>();
            _loggerDapper = new Mock<ILogger<DapperUnitOfWork>>();
            _iUnitWorkMock = new Mock<IUnitOfWork>();
            _iDbConnectionMock = new Mock<IDbConnection>();
            _dbConnectionProviderMock = new Mock<IDbConnectionProvider>();
            _loggerMock = new Mock<ILogger<DapperDbContext>>();
            _iDbTransactionMock = new Mock<IDbTransaction>();
            _iDbConnectionMock.Setup(c => c.Dispose());
            _iDbTransactionMock.Setup(t => t.Dispose());
            _dbConnectionProviderMock.Setup(p => p.GetConnection()).Returns(_iDbConnectionMock.Object);
            _iDbConnectionMock.Setup(x => x.BeginTransaction()).Returns(_iDbTransactionMock.Object);
            _sutDapper = new DapperDbContext(_dbConnectionProviderMock.Object, _loggerMock.Object);
            _unitWorkDapper = new DapperUnitOfWork(_sutDapper, _loggerDapper.Object);
            _sut = new OwnerObjectRepository(_iUnitWorkMock.Object);
        }

        [Test]
        public async Task
        GetListOwnerAsync_ShouldReturnOwnerList_WhenNotThrowException()
        {
            _readRepositoryMock.Setup(x =>
                    x.CustomSingleQueryListAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object?>>()))
                .ReturnsAsync(_fixture.CreateMany<Owner>(5));
            _iUnitWorkMock.Setup(c => c.GetReadRepository<Owner>())
                .Returns(_readRepositoryMock.Object);
            var result = await _sut.GetAllOwnersAsync();

            _readRepositoryMock.Verify();
            result.Should().NotBeNull();
        }
        [TearDown]
        public void TearDown()
        {
            _unitWorkDapper?.Dispose();
            _sutDapper?.Dispose();
        }

    }
}
