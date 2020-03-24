using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiskAccessMethods;

namespace DiskAccessMethods.DiscAccessStrategies
{
    public class ScanDiscAccessStrategy: AbstractDiscAccessStrategy
    {
        private readonly int _discSize;
        private int _direction = 1;

        public ScanDiscAccessStrategy(int discSize, IHandler nextHandler) : base(nextHandler)
        {
            _discSize = discSize;
        }

        protected override int? SelectNextMove(int currentAddress, List<IAccessRequest> requests)
        {
            if (currentAddress == _discSize - 1) _direction = -1;
            else if (currentAddress == 0) _direction = 1;
            return _direction;
        }

        protected override bool? IsDiscReadyToReading(int currentAddress, List<IAccessRequest> requests)
        {
            return requests.Any(x => x.DataBlockAddress == currentAddress);
        }
    }
}
