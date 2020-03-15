
using System;
using System.Collections.Generic;
using DiskAccessMethods;
using DiskAccessMethods.AccessRequests;
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
           
            IDisc disc = new Disc(new DumbStrategy(null));
            disc.AddDataBlocks(dataBlocks);
            disc.AddAccessRequests(requests);
            disc.Update(1000);
            Assert.Empty(disc.GetAllAccessRequests());
        }

    }
}
