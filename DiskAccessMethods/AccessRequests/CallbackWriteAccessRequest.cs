using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods.AccessRequests
{
    public class CallbackWriteAccessRequest: IAccessRequest
    {

        public CallbackWriteAccessRequest(int dataBlockPosition, int createTime, string data, Action<bool> callback)
        {
            _data = data;
            _callback = callback;
            CreateTime = createTime;
            DataBlockAddress = dataBlockPosition;
        }

        private readonly string _data;
        private readonly Action<bool> _callback;
        public int CreateTime { get; }
        public int DataBlockAddress { get; }

        public void Visit(IDataBlock dataBlock)
        {
            dataBlock.Data = _data;
            _callback(true);
        }
    }
}
