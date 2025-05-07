using AutoFixture;
using FluentValidation.TestHelper;
using PropertyManager.Application.Common.Models;
using PropertyManager.Application.Property.Commands.Validations;

namespace PropertyManager.Application.UnitTest.Property.Commands.Validations
{
    [TestFixture]
    public class PropertyValidatorUnitTest
    {
        private PropertyValidator _sut = null!;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _sut = new PropertyValidator();
        }

        [Test]
        public void Validate_ValidationException_WhenFieldNameNullOrEmpty()
        {
            //Arrange

            var property = new PropertyModel
            {
                Name = string.Empty
            };

            //Act
            var validation = _sut.TestValidate(property);

            //Arrange
            validation?.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Test]
        public void Validate_ValidationException_WhenFieldAddressNullOrEmpty()
        {
            //Arrange

            var property = new PropertyModel
            {
                Address = string.Empty
            };

            //Act
            var validation = _sut.TestValidate(property);

            //Arrange
            validation?.ShouldHaveValidationErrorFor(x => x.Address);
        }

        [Test]
        public void Validate_ValidationException_WhenFieldCodeInternalNullOrEmpty()
        {
            //Arrange

            var property = new PropertyModel
            {
                CodeInternal = string.Empty
            };

            //Act
            var validation = _sut.TestValidate(property);

            //Arrange
            validation?.ShouldHaveValidationErrorFor(x => x.CodeInternal);
        }
    }
}
