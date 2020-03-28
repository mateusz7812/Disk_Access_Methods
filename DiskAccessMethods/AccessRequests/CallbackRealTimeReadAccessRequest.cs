using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods.AccessRequests
{
    public class CallbackRealTimeReadAccessRequest: CallbackReadAccessRequest, IRealTime
    {
        public CallbackRealTimeReadAccessRequest(int dataBlockAddress, int createTime, Action<string> callback, int lifetime) : base(dataBlockAddress, createTime, callback)
        {
            Lifetime = lifetime;
        }

        public int Lifetime { get; }
    }
}
