using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiskAccessMethods.DiscAccessStrategies
{
    public class FdScanDiscAccessStrategy: AbstractDiscAccessStrategy
    {
        private readonly int _timeForMove;
        private readonly Func<int> _freeTime;
        protected new int? NextDataAddress = null;

        public FdScanDiscAccessStrategy(IHandler nextHandler, int timeForMove, Func<int> freeTime) : base(nextHandler)
        {
            _timeForMove = timeForMove;
            _freeTime = freeTime;
        }

        protected override int? SelectNextMove(int currentAddress, List<IAccessRequest> requests)
        {
            RemoveUnreachableRequests(currentAddress, ref requests);
            if (NextDataAddress != currentAddress)
            {
                var realTimeRequests = requests.Where(r => r is IRealTime);
                var count = realTimeRequests.Count(r => r.DataBlockAddress.Equals(NextDataAddress));
                if (count == 0) NextDataAddress = null;
            }
            if (NextDataAddress == currentAddress || NextDataAddress == null)
            {
                var request = SelectNearestRealTime(currentAddress, requests);
                if (request == null) return null;
                NextDataAddress = ((IAccessRequest)request).DataBlockAddress;
            }
            
            if (IsDiscReadyToReading(currentAddress, requests) == true) return 0;
            if (NextDataAddress == null) return null;
            var move = (int)NextDataAddress - currentAddress;
            return move == 0 ? 0 : move / Math.Abs(move);
        }

        private static IAccessRequest SelectNearestRealTime(int currentAddress, List<IAccessRequest> requests)
        {
            var realTimes = requests.Where(r=>r is IRealTime);
            var orderedRealTimes = realTimes.OrderBy(r => Math.Abs(r.DataBlockAddress-currentAddress));
            return orderedRealTimes.FirstOrDefault();
        }

        private void RemoveUnreachableRequests(int currentAddress, ref List<IAccessRequest> requests)
        {
            requests.RemoveAll(r => r is IRealTime time &&
                                    (time.Lifetime + r.CreateTime +
                                    Math.Abs(r.DataBlockAddress - currentAddress) * _timeForMove < _freeTime()));
        }

        protected override bool? IsDiscReadyToReading(int currentAddress, List<IAccessRequest> requests)
        {
            if (requests.Any(x => x.DataBlockAddress.Equals(currentAddress))) return true;
            RemoveUnreachableRequests(currentAddress, ref requests);
            if (requests.Count(r => r is IRealTime) != 0) return false;
            return null;
        }
    }
}
