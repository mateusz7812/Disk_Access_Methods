using System;
using System.Collections.Generic;
using System.Linq;
using DiskAccessMethods.DiscStates;

namespace DiskAccessMethods
{
    public class Disc : IDisc
    {
        public Disc(AbstractDiscAccessStrategy chainedBaseStrategy)
        {
            _chainedBaseStrategy = chainedBaseStrategy;
            SetState<HeadMovingDiscState>();
        }

        private readonly AbstractDiscAccessStrategy _chainedBaseStrategy;
        private readonly Dictionary<int, List<IAccessRequest>> _accessRequests = new Dictionary<int, List<IAccessRequest>>();
        private readonly List<IDataBlock> _dataBlocks = new List<IDataBlock>();
        private int _currentAddress = 0;

        private void AddAccessRequest(IAccessRequest accessRequest)
        {
            var blockPosition = accessRequest.DataBlockAddress;
            if (!_accessRequests.ContainsKey(blockPosition)) _accessRequests.Add(blockPosition, new List<IAccessRequest>());
            _accessRequests[blockPosition].Add(accessRequest);
        }

        public int LastTimeInMilliseconds { set; get; } = 0;

        public int CurrentAddress
        {
            get => _currentAddress;
            set
            {
                var difference = value - _currentAddress;
                if (difference == 1 || difference == -1)
                {
                    _currentAddress = value;
                }
            }
        }

        public int MoveToNextBlockTimeInMilliseconds { get; set; } = 100;
        public int IoOperationTimeInMilliseconds { get; set; } = 100;
        public IDiscState State { get; private set; }

        public void Update(int milliseconds) => State.Update(milliseconds);
        public void AddDataBlocks(List<IDataBlock> dataBlocks) => _dataBlocks.AddRange(dataBlocks);
        public void AddAccessRequests(List<IAccessRequest> accessRequests) => accessRequests.ForEach(AddAccessRequest);
        public void RemoveRequest(IAccessRequest accessRequest) => _accessRequests[accessRequest.DataBlockAddress].Remove(accessRequest);
        public void HandleNextMove()
        {
            var nextMove = _chainedBaseStrategy.HandleNextMoveSelection(CurrentAddress, GetAllAccessRequests());
            if (nextMove != null) CurrentAddress += (int) nextMove;
            LastTimeInMilliseconds += MoveToNextBlockTimeInMilliseconds;
        }

        public void HandleNextRequest()
        {
            var dataBlock = GetDataBlockOfCurrentAddress();

            var requests = GetRequestsByBlockNumber(CurrentAddress);
            requests.Sort((r1, r2) => r1.CreateTime - r2.CreateTime);
            var r = requests.First();
            r.Visit(dataBlock);
            RemoveRequest(r);
            LastTimeInMilliseconds += IoOperationTimeInMilliseconds;
        }

        public bool IsEnoughTimeOnNextMove(int nowInMilliseconds) => LastTimeInMilliseconds + MoveToNextBlockTimeInMilliseconds <= nowInMilliseconds;
        public bool IsEnoughTimeOnOperation(int nowInMilliseconds) => IoOperationTimeInMilliseconds + LastTimeInMilliseconds <= nowInMilliseconds;
        public bool CurrentAddressHaveNotDoneRequests() => GetRequestsByBlockNumber(CurrentAddress).Count != 0;

        public void SetState<T>() where T : AbstractDiscState => State = (T)Activator.CreateInstance(typeof(T), this);
        public IDataBlock GetDataBlockOfCurrentAddress() => _dataBlocks[CurrentAddress];
        public List<IAccessRequest> GetRequestsByBlockNumber(int dataBlockNumber)
        {
            if(!_accessRequests.ContainsKey(dataBlockNumber)) _accessRequests.Add(dataBlockNumber, new List<IAccessRequest>());
            return _accessRequests[dataBlockNumber];
        }

        public List<IAccessRequest> GetAllAccessRequests() => _accessRequests.Values.SelectMany(x => x).ToList();
        public bool DiscReadyToReading()
        {
            return _chainedBaseStrategy.HandleDiscReadingReadiness(CurrentAddress, GetAllAccessRequests()) ?? false;
        }
    }
}
