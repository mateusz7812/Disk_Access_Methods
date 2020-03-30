using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods.AccessRequests
{
    public class CallbackRealTimeReadAccessRequest: CallbackReadAccessRequest, IRealTime
    {
        public CallbackRealTimeReadAccessRequest(int dataBlockAddress, int createTime, int deadline,
            Action<string> callback) : base(dataBlockAddress, createTime, callback)
        {
            Deadline = deadline;
        }

        public int Deadline { get; }
    }
}
