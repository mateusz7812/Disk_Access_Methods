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
            SetState<WaitDiscState>();
        }

        private readonly AbstractDiscAccessStrategy _chainedBaseStrategy;
        private readonly Dictionary<int, List<IAccessRequest>> _accessRequests = new Dictionary<int, List<IAccessRequest>>();
        private readonly List<IDataBlock> _dataBlocks = new List<IDataBlock>();
        private void AddAccessRequest(IAccessRequest accessRequest)
        {
            var blockPosition = accessRequest.DataBlockAddress;
            if (!_accessRequests.ContainsKey(blockPosition)) _accessRequests.Add(blockPosition, new List<IAccessRequest>());
            _accessRequests[blockPosition].Add(accessRequest);
        }

        public int LastTimeInMilliseconds { set; get; } = 0;
        public int CurrentAddress { get; set; } = 0;
        public int MoveToNextBlockTimeInMilliseconds { get; set; } = 100;
        public int IoOperationTimeInMilliseconds { get; set; } = 100;
        public int NextDataBlockAddress { get; set; }
        public IDiscState State { get; private set; }

        public void Update(int milliseconds) => State.Update(milliseconds);
        public void AddDataBlocks(List<IDataBlock> dataBlocks) => _dataBlocks.AddRange(dataBlocks);
        public void AddAccessRequests(List<IAccessRequest> accessRequests) => accessRequests.ForEach(AddAccessRequest);
        public void RemoveRequest(IAccessRequest accessRequest) => _accessRequests[accessRequest.DataBlockAddress].Remove(accessRequest);
        public void SetState<T>() where T : AbstractDiscState => State = (T)Activator.CreateInstance(typeof(T), this);
        public int GetAddressOfBlock(IDataBlock dataBlock) => _dataBlocks.IndexOf(dataBlock);
        public IDataBlock GetDataBlockOfCurrentAddress() => _dataBlocks[CurrentAddress];
        public IAccessRequest HandleSelectionRequest(List<IAccessRequest> accessRequests) => _chainedBaseStrategy.HandleSelectionRequest(accessRequests);
        public List<IAccessRequest> GetRequestsByBlockNumber(int dataBlockNumber) => _accessRequests[dataBlockNumber];
        public List<IAccessRequest> GetAllAccessRequests() => _accessRequests.Values.SelectMany(x => x).ToList();
    }
}
