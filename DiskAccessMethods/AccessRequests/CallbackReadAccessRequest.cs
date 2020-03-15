using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods.AccessRequests
{
    public class CallbackReadAccessRequest: IAccessRequest
    {
        public CallbackReadAccessRequest(int dataBlockAddress, int createTime, Action<string> callback)
        {
            CreateTime = createTime;
            DataBlockAddress = dataBlockAddress;
            _callback = callback;
        }

        private readonly Action<string> _callback;
        public int CreateTime { get; }
        public int DataBlockAddress { get; }

        public void Visit(IDataBlock dataBlock)
        {
            var data = dataBlock.Data;
            _callback(data);
        }
    }
}
