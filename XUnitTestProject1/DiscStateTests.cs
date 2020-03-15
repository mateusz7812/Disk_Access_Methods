using System.Collections.Generic;
using DiskAccessMethods;
using DiskAccessMethods.DiscStates;
using Moq;
using Xunit;

namespace XUnitTestProject1
{
    public class DiscStateTests
    {
        [Fact]
        public void TestHeadMovingDiscState()
        {
            var time = 0;
            const int timeForMove = 11;
            var currentPosition = 0;
            const int destinationPosition = 4;

            var discMock = new Mock<IDisc>();
            discMock.Setup(disc => disc.HandleNextMove()).Callback((() =>
            {
                currentPosition++;
                time += timeForMove;
            }));
            discMock.Setup(disc => disc.DiscReadyToReading()).Returns(() => currentPosition == destinationPosition);
            discMock.Setup(disc => disc.IsEnoughTimeOnNextMove(It.IsAny<int>()))
                .Returns((int now) => time + timeForMove <= now);
            discMock.Setup(disc => disc.SetState<RequestHandlingDiscState>());
            var discState = new HeadMovingDiscState(discMock.Object);

            discState.Update(30);
            Assert.Equal(2, currentPosition);
            discState.Update(60);
            Assert.Equal(4, currentPosition);

            discMock.VerifyAll();
        }

        [Fact]
        public void TestRequestHandlingDiscState()
        {
            var time = 0;
            const int timeForOperation = 11;

            var requests = new List<string>() { "request1", "request2"};

            var discMock = new Mock<IDisc>();
            discMock.Setup(disc => disc.CurrentAddressHaveNotDoneRequests()).Returns(() => requests.Count != 0);
            discMock.Setup(disc => disc.IsEnoughTimeOnOperation(It.IsAny<int>()))
                .Returns((int now) => time + timeForOperation <= now);
            discMock.Setup(disc => disc.HandleNextRequest()).Callback(() =>
            {
                requests.RemoveAt(0);
                time += timeForOperation;
            });
            discMock.Setup(disc => disc.SetState<HeadMovingDiscState>());
            var discState = new RequestHandlingDiscState(discMock.Object);

            discState.Update(20);
            Assert.Single(requests);

            discState.Update(30);
            Assert.Empty(requests);

            discMock.VerifyAll();
        }
    }
}
