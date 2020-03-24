using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiskAccessMethods.DiscAccessStrategies
{
    public class SstfDiscAccessStrategy: AbstractDiscAccessStrategy
    {
        public SstfDiscAccessStrategy(IHandler nextHandler) : base(nextHandler) {}

        protected override int? SelectNextMove(int currentAddress, List<IAccessRequest> requests)
        {
            if (requests.Count == 0) return null;
            var nearestRequest = NearestRequest(currentAddress, requests);
            NextDataAddress = nearestRequest.DataBlockAddress;
            var move = NextDataAddress - currentAddress;
            return move == 0 ? 0 : move / Math.Abs(move);
        }

        private static IAccessRequest NearestRequest(int currentAddress, List<IAccessRequest> requests)
        {
            return (from x in requests orderby Math.Abs(currentAddress - x.DataBlockAddress) select x).First();
        }
    }
}
