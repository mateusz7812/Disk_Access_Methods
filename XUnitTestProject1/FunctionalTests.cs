
using System;
using System.Collections.Generic;
using DiskAccessMethods;
using DiskAccessMethods.AccessRequests;
using DiskAccessMethods.DiscAccessStrategies;
using DiskAccessMethods.DiscStates;
using Xunit;

namespace XUnitTestProject1
{
    public class FunctionalTests
    {
        [Fact]
        public void MinimalTest()
        {
            var dataBlocks = new List<IDataBlock>(){ new DataBlock(), new DataBlock(), new DataBlock()};
            var requests = new List<IAccessRequest>()
            {
                new CallbackWriteAccessRequest(0, 0, "Hello", Assert.True),
                new CallbackWriteAccessRequest(1, 0, "World", Assert.True),
                new CallbackWriteAccessRequest(2, 0, "!!!", Assert.True),
                new CallbackReadAccessRequest(0, 100, s=> Assert.Equal("Hello", s)),
                new CallbackReadAccessRequest(1, 100, s=> Assert.Equal("World", s)),
                new CallbackReadAccessRequest(2, 100, s=> Assert.Equal("!!!", s))
            };
           
            IDisc disc = new Disc(new DumbStrategy(null), new DiscConfig());
            disc.AddDataBlocks(dataBlocks);
            disc.AddAccessRequests(requests);
            disc.Update(1000);
            Assert.Empty(disc.GetAllWaitingAccessRequests());
        }

        [Fact]
        public void TestRealTimeRequests()
        {
            IDisc disc = new Disc(new EdfDiscAccessStrategy(new FcfsDiscAccessStrategy(null)), 
                new DiscConfig(){IoOperationTimeInMilliseconds = 10, MoveToNextBlockTimeInMilliseconds = 5});
            int i = 0;
            var dataBlocks = new List<IDataBlock>() {new DataBlock(), new DataBlock(), new DataBlock(), new DataBlock(), new DataBlock(), new DataBlock()};
            var requests = new List<IAccessRequest>()
            {
                new CallbackWriteAccessRequest(0, 0, "Hello", b => { Assert.True(b);Assert.Equal(2, i++);}),
                new CallbackRealTimeWriteAccessRequest(1, 0, 1000, "data1", b => { Assert.True(b);Assert.Equal(0, i++);}),
                new CallbackWriteAccessRequest(2, 0, "Hello", b => { Assert.True(b);Assert.Equal(4, i++);}),
                new CallbackRealTimeWriteAccessRequest(3, 75, 1000, "data1", b => { Assert.True(b);Assert.Equal(3, i++);}),
                new CallbackWriteAccessRequest(4, 0, "Hello", b => { Assert.True(b);Assert.Equal(5, i++);}),
                new CallbackRealTimeWriteAccessRequest(5, 0, 1000, "data1", b => { Assert.True(b);Assert.Equal(1, i++);})
            };

            disc.AddDataBlocks(dataBlocks);
            disc.AddAccessRequests(requests);
            disc.Update(100);
        }

        [Fact]
        public void TestAsynchronous()
        {
            var dataBlocks = new List<IDataBlock>() { new DataBlock(), new DataBlock(), new DataBlock() };
            var requests = new List<IAccessRequest>()
            {
                new CallbackWriteAccessRequest(0, 1000, "Hello", (_)=>throw new Exception())
            };

            IDisc disc = new Disc(new DumbStrategy(null), new DiscConfig());
            disc.AddDataBlocks(dataBlocks);
            disc.AddAccessRequests(requests);
            disc.Update(100);
        }
    }
}
