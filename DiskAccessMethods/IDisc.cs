using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods
{
    public interface IDisc
    {
        int LastTimeInMilliseconds { get; set; }
        int CurrentAddress { get; set; }
        int MoveToNextBlockTimeInMilliseconds { get; }
        int IoOperationTimeInMilliseconds { get; }
        IDiscState State { get; }

        void SetState<T>() where T: AbstractDiscState;
        void Update(int milliseconds);
        void AddDataBlocks(List<IDataBlock> dataBlocks);
        void AddAccessRequests(List<IAccessRequest> accessRequests);
        void RemoveRequest(IAccessRequest accessRequest);
        void HandleNextMove();
        void HandleNextRequest();
        bool IsEnoughTimeOnNextMove(int nowInMilliseconds);
        IDataBlock GetDataBlockOfCurrentAddress();
        List<IAccessRequest> GetRequestsByBlockNumber(int dataBlockNumber);
        List<IAccessRequest> GetAllAccessRequests();
        bool DiscReadyToReading();
        bool IsEnoughTimeOnOperation(int nowInMilliseconds);
        bool CurrentAddressHaveNotDoneRequests();
    }
}
