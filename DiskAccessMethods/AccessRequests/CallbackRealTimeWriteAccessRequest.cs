using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods.AccessRequests
{
    public class CallbackRealTimeWriteAccessRequest : CallbackWriteAccessRequest, IRealTime
    {
        public CallbackRealTimeWriteAccessRequest(int dataBlockPosition, int createTime, int deadline, string data, Action<bool> callback) : base(dataBlockPosition, createTime, data, callback)
        {
            Deadline = deadline;
        }

        public int Deadline { get; }
    }
}
