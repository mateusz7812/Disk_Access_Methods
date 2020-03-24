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
            if (accessRequests.Count == 0) return null;
            NextDataAddress = GetOldestAccessRequest(accessRequests).DataBlockAddress;
            var move = (int)NextDataAddress - currentAddress;
            return move == 0 ? 0 : move/Math.Abs(move);
        }

        private static IAccessRequest GetOldestAccessRequest(List<IAccessRequest> accessRequests)
        {
            accessRequests.Sort((a, b) => a.CreateTime - b.CreateTime);
            return accessRequests[0];
        }
        
    }
}
