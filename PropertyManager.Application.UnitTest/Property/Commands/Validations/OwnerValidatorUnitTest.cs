using System.Globalization;
using AutoFixture;
using FluentValidation.TestHelper;
using PropertyManager.Application.Common.Models;
using PropertyManager.Application.Property.Commands.Validations;
using PropertyManager.Application.Utilities;

namespace PropertyManager.Application.UnitTest.Property.Commands.Validations
{
    [TestFixture]
    public class OwnerValidatorUnitTest
    {
        private OwnerValidator _sut = null!;
        private readonly Fixture _fixture = new Fixture();
    

     [SetUp]
        public void SetUp()
        {
            _sut = new OwnerValidator();
        }

        [Test]
        public void Validate_ValidationException_WhenFieldNameNullOrEmpty()
        {
            //Arrange

            var owner = new OwnerModel
            {
                Name = string.Empty
            };

            //Act
            var validation = _sut.TestValidate(owner);

            //Arrange
            validation?.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Test]
        public void Validate_ValidationException_WhenFieldAddressNullOrEmpty()
        {
            //Arrange

            var owner = new OwnerModel
            {
                Address = string.Empty
            };

            //Act
            var validation = _sut.TestValidate(owner);

            //Arrange
            validation?.ShouldHaveValidationErrorFor(x => x.Address);
        }

        [Test]
        public void Validate_ValidationException_WhenBirthday()
        {
            //Arrange

            var birthday = DateTime.Now.ToString(CultureInfo.InvariantCulture);

            //Act
            var validation = UtilDate.BeValidDate(birthday);

            //Arrange
            Assert.IsFalse(validation);
        }
    }
}
