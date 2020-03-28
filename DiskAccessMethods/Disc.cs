using System;
using System.Collections.Generic;
using System.Linq;
using DiskAccessMethods.DiscStates;

namespace DiskAccessMethods
{
    public class Disc : IDisc
    {
        public Disc(AbstractDiscAccessStrategy chainedBaseStrategy, IDiscConfig config)
        {
            _chainedBaseStrategy = chainedBaseStrategy;
            this.Config = config;
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

        private int LastTimeInMilliseconds { set; get; } = 0;

        private int CurrentAddress
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

        public readonly IDiscConfig Config;
        private IDiscState State { get; set; }

        public void Update(int milliseconds) => State.Update(milliseconds);
        public void AddDataBlocks(List<IDataBlock> dataBlocks) => _dataBlocks.AddRange(dataBlocks);
        public void AddAccessRequests(List<IAccessRequest> accessRequests) => accessRequests.ForEach(AddAccessRequest);
        private void RemoveRequest(IAccessRequest accessRequest) => _accessRequests[accessRequest.DataBlockAddress].Remove(accessRequest);
        public void HandleNextMove()
        {
            var nextMove = _chainedBaseStrategy.HandleNextMoveSelection(CurrentAddress, GetAllWaitingAccessRequests());
            if (nextMove != null) CurrentAddress += (int) nextMove;
            LastTimeInMilliseconds += Config.MoveToNextBlockTimeInMilliseconds;
        }

        public void HandleNextRequest()
        {
            var dataBlock = GetDataBlockOfCurrentAddress();

            var requests = GetRequestsByBlockNumber(CurrentAddress);
            requests.Sort((r1, r2) => r1.CreateTime - r2.CreateTime);
            var r = requests.First();
            r.Visit(dataBlock);
            RemoveRequest(r);
            LastTimeInMilliseconds += Config.IoOperationTimeInMilliseconds;
        }

        public bool IsEnoughTimeOnNextMove(int nowInMilliseconds) => LastTimeInMilliseconds + Config.MoveToNextBlockTimeInMilliseconds <= nowInMilliseconds;
        public bool IsEnoughTimeOnOperation(int nowInMilliseconds) => Config.IoOperationTimeInMilliseconds + LastTimeInMilliseconds <= nowInMilliseconds;
        public bool CurrentAddressHaveNotDoneRequests() => GetRequestsByBlockNumber(CurrentAddress).Count != 0;

        public void SetState<T>() where T : AbstractDiscState => State = (T)Activator.CreateInstance(typeof(T), this);
        private IDataBlock GetDataBlockOfCurrentAddress() => _dataBlocks[CurrentAddress];
        private List<IAccessRequest> GetRequestsByBlockNumber(int dataBlockNumber)
        {
            if(!_accessRequests.ContainsKey(dataBlockNumber)) _accessRequests.Add(dataBlockNumber, new List<IAccessRequest>());
            return _accessRequests[dataBlockNumber];
        }

        public List<IAccessRequest> GetAllWaitingAccessRequests() => _accessRequests.Values.SelectMany(x => x).Where(r=>r.CreateTime <= LastTimeInMilliseconds).ToList();
        public bool DiscReadyToReading()
        {
            return _chainedBaseStrategy.HandleDiscReadingReadiness(CurrentAddress, GetAllWaitingAccessRequests()) ?? false;
        }
    }
}
