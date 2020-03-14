using System.Collections.Generic;
using DiskAccessMethods;
using DiskAccessMethods.DiscStates;
using Moq;
using Xunit;

namespace XUnitTestProject1
{
    public class DiscStateTests
    {
        private Mock<IAccessRequest> _requestMock;

        [Fact]
        public void TestWaitDiscState()
        {
            var dataBlockMock1 = new Mock<IDataBlock>();
            var dataBlockMock2 = new Mock<IDataBlock>();
            var accessRequestMock = new Mock<IAccessRequest>();
            accessRequestMock.Setup(m => m.DataBlockAddress).Returns(1);
            var discAccessStrategy = new Mock<AbstractDiscAccessStrategy>(new []{ (object) null });
            discAccessStrategy.Setup(m 
                    => m.HandleSelectionRequest(It.IsAny<List<IAccessRequest>>()))
                .Returns(accessRequestMock.Object);
            IDisc disc = new Disc(discAccessStrategy.Object);
            disc.AddDataBlocks(new List<IDataBlock>() { dataBlockMock1.Object, dataBlockMock2.Object });
            disc.AddAccessRequests(new List<IAccessRequest>(){ accessRequestMock.Object });

            Assert.IsType<WaitDiscState>(disc.State);

            disc.Update(0);

            discAccessStrategy.VerifyAll();
            accessRequestMock.VerifyAll();
            Assert.Equal(1 , disc.NextDataBlockAddress);
            Assert.IsType<HeadMovingDiscState>(disc.State);
        }

        [Fact]
        public void TestHeadMovingDiscState()
        {
            var dataBlockMock1 = new Mock<IDataBlock>();
            var dataBlockMock2 = new Mock<IDataBlock>();
            var disc = new Disc(null);
            disc.AddDataBlocks(new List<IDataBlock>(){ dataBlockMock1.Object, dataBlockMock2.Object });
            _requestMock = new Mock<IAccessRequest>();
            _requestMock.Setup(m=>m.DataBlockAddress).Returns(1);
            disc.AddAccessRequests(new List<IAccessRequest>(){_requestMock.Object});
            disc.NextDataBlockAddress = 1;
            disc.SetState<HeadMovingDiscState>();
            disc.MoveToNextBlockTimeInMilliseconds = 100;
            Assert.Equal(0, disc.CurrentAddress);

            disc.Update(90);
            Assert.Equal(0, disc.CurrentAddress);

            disc.Update(100);
            Assert.Equal(1, disc.GetAddressOfBlock(dataBlockMock2.Object));
            Assert.Equal(1, disc.CurrentAddress);
            Assert.IsType<RequestHandlingDiscState>(disc.State);
        }

        [Fact]
        public void TestRequestHandlingDiscState()
        {
            var dataBlockMock1 = new Mock<IDataBlock>();
            var dataBlockMock2 = new Mock<IDataBlock>();
            var disc = new Disc(null);
            disc.AddDataBlocks(new List<IDataBlock>() { dataBlockMock1.Object, dataBlockMock2.Object });
            disc.NextDataBlockAddress = 1;
            disc.SetState<RequestHandlingDiscState>();

            var accessRequestMock1 = new Mock<IAccessRequest>();
            accessRequestMock1.SetupAllProperties();
            accessRequestMock1.Setup(m=>m.DataBlockAddress).Returns(1);
            accessRequestMock1.Setup(m=>m.CreateTime).Returns(0);
            accessRequestMock1.Setup(m => m.Visit(It.IsAny<IDataBlock>()));
            disc.AddAccessRequests(new List<IAccessRequest> {accessRequestMock1.Object});

            var accessRequestMock2 = new Mock<IAccessRequest>();
            accessRequestMock2.SetupAllProperties();
            accessRequestMock2.Setup(m=>m.DataBlockAddress).Returns(1);
            accessRequestMock2.Setup(m=>m.CreateTime).Returns(1);
            accessRequestMock2.Setup(m => m.Visit(It.IsAny<IDataBlock>()));
            disc.AddAccessRequests(new List<IAccessRequest> {accessRequestMock2.Object});

            disc.IoOperationTimeInMilliseconds = 100;

            disc.Update(150);
            accessRequestMock1.VerifyAll();
            Assert.Single(disc.GetRequestsByBlockNumber(1));

            disc.Update(300);
            accessRequestMock2.VerifyAll();
            Assert.Empty(disc.GetRequestsByBlockNumber(1));

            Assert.IsType<WaitDiscState>(disc.State);
        }
    }
}
