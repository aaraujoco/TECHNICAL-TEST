using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyManager.Domain.Common;

namespace PropertyManager.Application.UnitTest.Common
{
    [TestFixture]
    public class ErrorUnitTest
    {
        private Error _error = new Error();

        [Test]
        public void AzureContainersResponse_AzureWebJobStorage_Error()
        {
            //Arrange
            var error = new Error()
            {
                Field = "Property",
                Message = "Error"
            };

            _error = error;

            //Assert
            Assert.NotNull(_error);
        }
    }
}
