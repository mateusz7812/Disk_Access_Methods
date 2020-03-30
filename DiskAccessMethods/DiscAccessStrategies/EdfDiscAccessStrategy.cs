using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskAccessMethods.DiscAccessStrategies
{
    public class EdfDiscAccessStrategy: AbstractDiscAccessStrategy
    {
        public EdfDiscAccessStrategy(IHandler nextHandler) : base(nextHandler) { }
        protected new int? NextDataAddress;
        protected override int? SelectNextMove(int currentAddress, List<IAccessRequest> requests)
        {
            if (NextDataAddress != currentAddress)
            {
                var realTimeRequests = requests.Where(r => r is IRealTime);
                var count = realTimeRequests.Count(r => r.DataBlockAddress.Equals(NextDataAddress));
                if (count == 0) NextDataAddress = null;
            }
            if (NextDataAddress == currentAddress || NextDataAddress == null)
            {
                var request = SelectDeathNearestRealTime(requests);
                if (request == null) return null;
                NextDataAddress = ((IAccessRequest)request).DataBlockAddress;
            }

            if (NextDataAddress == null) return null;
            var move = (int)NextDataAddress - currentAddress;
            return move == 0 ? 0 : move / Math.Abs(move);
        }

        protected override bool? IsDiscReadyToReading(int currentAddress, List<IAccessRequest> requests)
        {
            var realTimeRequests = requests.Where(r => r is IRealTime);
            if (!realTimeRequests.Any()) return null;
            return realTimeRequests.Any(x => x.DataBlockAddress == currentAddress);
        }

        private static IRealTime SelectDeathNearestRealTime(IEnumerable<IAccessRequest> requests)
        {
            var realTimes = requests.OfType<IRealTime>();
            var orderedRealTimes = realTimes.OrderBy(r=>r.Deadline);
            return orderedRealTimes.FirstOrDefault();
        }
    }
}
