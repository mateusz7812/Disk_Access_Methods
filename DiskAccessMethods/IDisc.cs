using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods
{
    public interface IDisc
    {
        void SetState<T>() where T: AbstractDiscState;
        void Update(int milliseconds);
        void AddDataBlocks(List<IDataBlock> dataBlocks);
        void AddAccessRequests(List<IAccessRequest> accessRequests);
        void HandleNextMove();
        void HandleNextRequest();
        List<IAccessRequest> GetAllWaitingAccessRequests();
        bool IsEnoughTimeOnNextMove(int nowInMilliseconds);
        bool DiscReadyToReading();
        bool IsEnoughTimeOnOperation(int nowInMilliseconds);
        bool CurrentAddressHaveNotDoneRequests();
    }
}
