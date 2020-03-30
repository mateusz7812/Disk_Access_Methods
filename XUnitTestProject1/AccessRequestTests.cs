using System;
using System.Collections.Generic;
using System.Text;
using DiskAccessMethods;
using DiskAccessMethods.AccessRequests;
using Moq;
using Xunit;

namespace XUnitTestProject1
{
    public class AccessRequestTests
    {
        [Fact]
        public void TestReadRequest()
        {
            var readObserverMock = new Mock<IObserver<string>>();
            readObserverMock.Setup(m => m.OnNext("data"));
            var dataBlock = new DataBlock {Data = "data"};
            var readRequest = new ObservableReadAccessRequest(1, 0);
            readRequest.Subscribe(readObserverMock.Object);
            dataBlock.Accept(readRequest);

            readObserverMock.VerifyAll();
        }

        [Fact]
        public void TestWriteRequest()
        {
            var writeObserverMock = new Mock<IObserver<bool>>();
            writeObserverMock.Setup(m => m.OnNext(true));
            var dataBlock = new DataBlock { Data = "data1" };
            var writeRequest = new ObservableWriteAccessRequest(0, 1, "data2");
            writeRequest.Subscribe(writeObserverMock.Object);

            dataBlock.Accept(writeRequest);

            Assert.Equal("data2", dataBlock.Data);
            writeObserverMock.VerifyAll();
        }

        [Fact]
        public void TestRealTimeReadRequest()
        {
            var data = "";
            var dataBlock = new DataBlock { Data = "data" };
            var readRequest = new CallbackRealTimeReadAccessRequest(0, 0, 100, (s => data = s));

            dataBlock.Accept(readRequest);

            Assert.Equal("data", data);
        }

    }
}
