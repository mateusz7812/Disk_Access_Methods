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
            request1Mock.Setup(request => request.DataBlockAddress).Returns(2);
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

        [Fact]
        public void SsftDiscAccessStrategy()
        {

            var currentAddress = 0;

            var request1Mock = new Mock<IAccessRequest>();
            request1Mock.Setup(request => request.DataBlockAddress).Returns(0);
            var request2Mock = new Mock<IAccessRequest>();
            request2Mock.Setup(request => request.DataBlockAddress).Returns(3);
            var request3Mock = new Mock<IAccessRequest>();
            request3Mock.Setup(request => request.DataBlockAddress).Returns(6);
            var accessRequests = new List<IAccessRequest>() { request1Mock.Object, request2Mock.Object , request3Mock.Object};

            var strategy = new SstfDiscAccessStrategy(null);

            Assert.Equal(0, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.True(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 2;

            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.False(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 3;
            Assert.Equal(0, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.True(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 4;

            Assert.Equal(-1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.False(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            request1Mock.VerifyAll();
            request2Mock.VerifyAll();
            request3Mock.VerifyAll();
        }

        [Fact]
        public void TestScan()
        {
            var currentAddress = 0;

            var request1Mock = new Mock<IAccessRequest>();
            request1Mock.Setup(request => request.DataBlockAddress).Returns(0);
            var request2Mock = new Mock<IAccessRequest>();
            request2Mock.Setup(request => request.DataBlockAddress).Returns(3);
            var request3Mock = new Mock<IAccessRequest>();
            request3Mock.Setup(request => request.DataBlockAddress).Returns(6);
            var accessRequests = new List<IAccessRequest>() { request1Mock.Object, request2Mock.Object, request3Mock.Object };

            var strategy = new ScanDiscAccessStrategy(7, null);

            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.True(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 2;

            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.False(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 3;
            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.True(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 4;
            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.False(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 6;
            Assert.Equal(-1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.True(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 3;
            Assert.Equal(-1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.True(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 0;
            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.True(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            request1Mock.VerifyAll();
            request2Mock.VerifyAll();
            request3Mock.VerifyAll();

        }

        [Fact]
        public void TestC_Scan()
        {
            var currentAddress = 0;

            var request1Mock = new Mock<IAccessRequest>();
            request1Mock.Setup(request => request.DataBlockAddress).Returns(0);
            var request2Mock = new Mock<IAccessRequest>();
            request2Mock.Setup(request => request.DataBlockAddress).Returns(3);
            var request3Mock = new Mock<IAccessRequest>();
            request3Mock.Setup(request => request.DataBlockAddress).Returns(6);
            var accessRequests = new List<IAccessRequest>() { request1Mock.Object, request2Mock.Object, request3Mock.Object };

            var strategy = new C_ScanDiscAccessStrategy(7, null);

            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.True(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 2;

            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.False(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 3;
            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.True(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 4;
            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.False(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 6;
            Assert.Equal(-1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.False(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 3;
            Assert.Equal(-1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.False(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 0;
            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.True(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            request1Mock.VerifyAll();
            request2Mock.VerifyAll();
            request3Mock.VerifyAll();

        }

        [Fact]
        public void Test_Edf()
        {
            var currentAddress = 0;

            var request1Mock = new Mock<IAccessRequest>();
            request1Mock.Setup(request => request.DataBlockAddress).Returns(0);
            var accessRequests = new List<IAccessRequest>() { request1Mock.Object };

            var strategy = new EdfDiscAccessStrategy(null);

            Assert.Null(strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.Null(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            var request2Mock = new Mock<IAccessRequest>();
            request2Mock.Setup(request => request.DataBlockAddress).Returns(1);
            request2Mock.Setup(request => request.DataBlockAddress).Returns(1);
            var realTimeRequestMock = request2Mock.As<IRealTime>();
            realTimeRequestMock.Setup(m => m.Lifetime).Returns(100);

            accessRequests.Add(realTimeRequestMock.Object as IAccessRequest);

            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.False(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 1;
            Assert.Equal(0, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.True(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            accessRequests.RemoveAt(1);
            Assert.Null(strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.Null(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            var request4Mock = new Mock<IAccessRequest>();
            request4Mock.Setup(request => request.DataBlockAddress).Returns(4);
            var realTimeRequestMock3 = request4Mock.As<IRealTime>();
            realTimeRequestMock3.Setup(m => m.Lifetime).Returns(100);
            accessRequests.Add(realTimeRequestMock3.Object as IAccessRequest);

            currentAddress = 3;
            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.False(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 4;
            Assert.Equal(0, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.True(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

        }

        [Fact]
        public void Test_FdScan()
        {
            var currentAddress = 0;

            var request1Mock = new Mock<IAccessRequest>();
            request1Mock.Setup(request => request.DataBlockAddress).Returns(5);
            var accessRequests = new List<IAccessRequest>() { request1Mock.Object };

            var strategy = new FdScanDiscAccessStrategy(null, 100, ()=>1000);

            Assert.Null(strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.Null(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            var request2Mock = new Mock<IAccessRequest>();
            request2Mock.Setup(request => request.DataBlockAddress).Returns(10);
            var realTimeRequestMock = request2Mock.As<IRealTime>();
            realTimeRequestMock.Setup(m => m.Lifetime).Returns(1000);

            accessRequests.Add(realTimeRequestMock.Object as IAccessRequest);

            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.False(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 5;
            Assert.Equal(0, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.True(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 9;
            Assert.Equal(1, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.False(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

            currentAddress = 10;
            Assert.Equal(0, strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.True(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));
            accessRequests.RemoveAt(1);

            Assert.Null(strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.Null(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));
            
            var request4Mock = new Mock<IAccessRequest>();
            request4Mock.Setup(request => request.DataBlockAddress).Returns(20);
            var realTimeRequestMock3 = request4Mock.As<IRealTime>();
            realTimeRequestMock3.Setup(m => m.Lifetime).Returns(100);

            accessRequests.Add(realTimeRequestMock3.Object as IAccessRequest);

            currentAddress = 25;
            Assert.Null(strategy.HandleNextMoveSelection(currentAddress, accessRequests));
            Assert.Null(strategy.HandleDiscReadingReadiness(currentAddress, accessRequests));

        }
    }
}
