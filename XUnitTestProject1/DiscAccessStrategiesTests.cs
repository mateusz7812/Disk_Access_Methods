using System;
using System.Collections.Generic;
using System.Text;
using DiskAccessMethods;
using DiskAccessMethods.DiscAccessStrategies;
using Moq;
using Xunit;

namespace XUnitTestProject1
{
    public class DiscAccessStrategiesTests
    {
        [Fact]
        public void TestFcfs()
        {
            var currentAddress = 0;

            var request1Mock = new Mock<IAccessRequest>();
            request1Mock.Setup(request => request.CreateTime).Returns(5);
            var request2Mock = new Mock<IAccessRequest>();
            request2Mock.Setup(request => request.CreateTime).Returns(2);
            request2Mock.Setup(request => request.DataBlockAddress).Returns(3);
            var accessRequests = new List<IAccessRequest>() {request1Mock.Object, request2Mock.Object};
            var strategy = new FcfsDiscAccessStrategy(null);

            Assert.False(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));
            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));

            currentAddress = 2;

            Assert.False(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));
            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));

            currentAddress = 3;
            Assert.True(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));
            Assert.Equal(0, strategy.HandleNextMoveSelection(currentAddress, accessRequests));

            currentAddress = 4;

            Assert.Equal(-1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.False(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            request1Mock.VerifyAll();
            request2Mock.VerifyAll();
        }
    }
}
