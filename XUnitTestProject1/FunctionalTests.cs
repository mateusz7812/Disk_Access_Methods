using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiskAccessMethods;
using DiskAccessMethods.AccessRequests;
using Xunit;

namespace XUnitTestProject1
{
    public class FunctionalTests
    {
        private class Observer<T> : IObserver<T>
        {
            private readonly List<T> _dataField;

            public Observer(List<T> dataField)
            {
                _dataField = dataField;
            }

            public void OnCompleted() => throw new NotImplementedException();
            public void OnError(Exception error) => throw new NotImplementedException();

            public void OnNext(T value)
            {
                _dataField.Add(value);
            }
        }

        private class DumbStrategy : AbstractDiscAccessStrategy
        {
            public DumbStrategy(IHandler nextHandler) : base(nextHandler) { }
            protected override IAccessRequest SelectNextRequest(List<IAccessRequest> accessRequests) => accessRequests.Count != 0 ? accessRequests[0] : null;
        }

        [Fact]
        public void MinimalTest()
        {
            var writeData = new List<bool>();
            var readData = new List<string>();
            var dataBlocks = new List<IDataBlock>(){ new DataBlock(), new DataBlock(), new DataBlock()};
            var requests = new List<IAccessRequest>()
            {
                new ObservableWriteAccessRequest(0, 0, "Hello"),
                new ObservableWriteAccessRequest(0, 1, "World"),
                new ObservableWriteAccessRequest(0, 2, "!!!"),
                new ObservableReadAccessRequest(0, 100),
                new ObservableReadAccessRequest(1, 100),
                new ObservableReadAccessRequest(2, 100)
            };
            ((ObservableWriteAccessRequest) requests[0]).Subscribe(new Observer<bool>(writeData));
            ((ObservableWriteAccessRequest) requests[1]).Subscribe(new Observer<bool>(writeData));
            ((ObservableWriteAccessRequest) requests[2]).Subscribe(new Observer<bool>(writeData));
            ((ObservableReadAccessRequest) requests[3]).Subscribe(new Observer<string>(readData));
            ((ObservableReadAccessRequest) requests[4]).Subscribe(new Observer<string>(readData));
            ((ObservableReadAccessRequest) requests[5]).Subscribe(new Observer<string>(readData));

            var disc = new Disc(new DumbStrategy(null));
            disc.AddDataBlocks(dataBlocks);
            disc.AddAccessRequests(requests);
            disc.Update(1000);
            Assert.NotEmpty(writeData);
            Assert.All(writeData, Assert.True);
            Assert.Equal("Hello World !!!", string.Join(" ", readData));
        }

    }
}
