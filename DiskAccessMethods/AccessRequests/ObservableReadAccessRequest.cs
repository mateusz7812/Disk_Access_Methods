using System;
using System.Collections.Generic;

namespace DiskAccessMethods.AccessRequests
{
    public class ObservableReadAccessRequest: IAccessRequest, IObservable<string>
    {
        private readonly List<IObserver<string>> _observers = new List<IObserver<string>>();

        public ObservableReadAccessRequest(int dataBlockAddress, int createTime)
        {
            DataBlockAddress = dataBlockAddress;
            CreateTime = createTime;
        }

        public int CreateTime { get; }
        public int DataBlockAddress { get; }
        public void Visit(IDataBlock dataBlock)
        {
            var data = dataBlock.Data;
            _observers.ForEach(o=>o.OnNext(data));
        }

        public class Disposable : IDisposable
        {
            private readonly ObservableReadAccessRequest _accessRequest;
            private readonly IObserver<string> _observer;

            public Disposable(ObservableReadAccessRequest accessRequest, IObserver<string> observer)
            {
                _accessRequest = accessRequest;
                _observer = observer;
            }

            public void Dispose()
            {
                _accessRequest.UnSubscribe(_observer);
            }
        }

        private void UnSubscribe(IObserver<string> observer)
        {
            _observers.Remove(observer);
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            _observers.Add(observer);
            return new Disposable(this, observer);
        }
    }
}
