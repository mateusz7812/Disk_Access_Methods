using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods.DiscAccessStrategies
{
    public class FcfsDiscAccessStrategy: AbstractDiscAccessStrategy
    {
        public FcfsDiscAccessStrategy(IHandler nextHandler) : base(nextHandler) { }

        protected override int? SelectNextMove(int currentAddress, List<IAccessRequest> accessRequests)
        {
            if (currentAddress == NextDataAddress || NextDataAddress == null)
            {
                if (accessRequests.Count == 0) return null;
                accessRequests.Sort((a, b) => a.CreateTime - b.CreateTime);
                NextDataAddress = accessRequests[0].DataBlockAddress;
            }
            var move = NextDataAddress - currentAddress;
            return move % 2;
        }
        
    }
}
