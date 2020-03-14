using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods.AccessRequests
{
    public class ObservableWriteAccessRequest: IAccessRequest, IObservable<bool>
    {
        private readonly string _data;
        private readonly List<IObserver<bool>> _observers = new List<IObserver<bool>>();

        public ObservableWriteAccessRequest(int createTime, int dataBlockPosition, string data)
        {
            _data = data;
            CreateTime = createTime;
            DataBlockAddress = dataBlockPosition;
        }

        public int CreateTime { get; }
        public int DataBlockAddress { get; }

        public void Visit(IDataBlock dataBlock)
        {
            dataBlock.Data = _data;
            _observers.ForEach(o=>o.OnNext(true));
        }

        public class Disposable : IDisposable
        {
            private readonly ObservableWriteAccessRequest _accessRequest;
            private readonly IObserver<bool> _observer;

            public Disposable(ObservableWriteAccessRequest accessRequest, IObserver<bool> observer)
            {
                _accessRequest = accessRequest;
                _observer = observer;
            }

            public void Dispose()
            {
                _accessRequest.UnSubscribe(_observer);
            }
        }

        private void UnSubscribe(IObserver<bool> observer)
        {
            _observers.Remove(observer);
        }

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            _observers.Add(observer);
            return new Disposable(this, observer);
        }
    }
}
