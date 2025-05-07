using System.Reflection;
using System.Runtime.Serialization;
using FluentAssertions;
using Moq;
using PropertyManager.Application.Exceptions;

namespace PropertyManager.Application.UnitTest.Exceptions
{
    [TestFixture]
    public class DbContextExceptionTests
    {
        [Test]
        public void DbContextException_WithMessage_ShouldMatchExpected()
        {
            // Arrange
            const string expectedMessage = "Test message";
            // Act
            var actual = new DbContextException(expectedMessage);
            // Assert
            actual.Message.Should().Be(expectedMessage);
        }

        [Test]
        public void DbContextException_WithMessageAndInnerException_ReturnsExpected()
        {
            // Arrange
            const string expectedMessage = "Test message";
            var innerException = new Mock<Exception>();
            // Act
            var actual = new DbContextException(expectedMessage, innerException.Object);
            // Assert
            actual.Message.Should().Be(expectedMessage);
            actual.InnerException.Should().Be(innerException.Object);
        }

        [Test]
        public void DbContextException_WithNoParameters_ThrowsDbContextExceptionType()
        {
            // Arrange
            // Act
            var actual = new DbContextException();
            // Assert
            actual.Message.Should().Contain("DbContextException");
        }

    }
}
