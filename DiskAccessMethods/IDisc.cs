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
        int NextDataBlockAddress { get; set; }
        IDiscState State { get; }

        void SetState<T>() where T: AbstractDiscState;
        void Update(int milliseconds);
        void AddDataBlocks(List<IDataBlock> dataBlocks);
        void AddAccessRequests(List<IAccessRequest> accessRequests);
        void RemoveRequest(IAccessRequest accessRequest);
        IDataBlock GetDataBlockOfCurrentAddress();
        IAccessRequest HandleSelectionRequest(List<IAccessRequest> accessRequests);
        List<IAccessRequest> GetRequestsByBlockNumber(int dataBlockNumber);
        List<IAccessRequest> GetAllAccessRequests();
    }
}
