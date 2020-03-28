using System;
using System.Collections.Generic;
using System.Text;
using DiskAccessMethods;

namespace XUnitTestProject1
{
    public class DumbStrategy : AbstractDiscAccessStrategy
    {
        public DumbStrategy(IHandler nextHandler) : base(nextHandler) { }

        protected override int? SelectNextMove(int currentAddress, List<IAccessRequest> accessRequests)
        {
            if (currentAddress == NextDataAddress)
            {
                if (accessRequests.Count == 0) return null;
                NextDataAddress = accessRequests[0].DataBlockAddress;
            }
            var move = NextDataAddress - currentAddress;
            return move % 2;
        }

    }
}
