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
            readObserverMock.Setup(m => m.OnNext(It.IsAny<string>()));
            var dataBlock = new DataBlock();
            var readRequest = new ObservableReadAccessRequest(1, 0);
            readRequest.Subscribe(readObserverMock.Object);
            dataBlock.Accept(readRequest);

            readObserverMock.VerifyAll();
        }
    }
}
