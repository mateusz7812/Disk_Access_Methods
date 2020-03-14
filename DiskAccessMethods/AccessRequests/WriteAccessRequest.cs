using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods.AccessRequests
{
    public class WriteAccessRequest: IAccessRequest
    {
        public WriteAccessRequest(int createTime, int dataBlockPosition)
        {
            CreateTime = createTime;
            DataBlockAddress = dataBlockPosition;
        }

        public int CreateTime { get; }
        public int DataBlockAddress { get; }
        public void Visit(IDataBlock dataBlock)
        {
            throw new NotImplementedException();
        }
    }
}
