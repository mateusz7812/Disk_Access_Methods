using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiskAccessMethods.DiscAccessStrategies
{
    public class C_ScanDiscAccessStrategy: AbstractDiscAccessStrategy
    {
        private readonly int _discSize;
        private bool _readingMode = true;

        public C_ScanDiscAccessStrategy(int discSize, IHandler nextHandler) : base(nextHandler)
        {
            _discSize = discSize;
        }

        protected override int? SelectNextMove(int currentAddress, List<IAccessRequest> requests)
        {
            if (currentAddress == _discSize - 1) _readingMode = false;
            else if (currentAddress == 0) _readingMode = true;
            return _readingMode ? 1 : -1;
        }

        protected override bool? IsDiscReadyToReading(int currentAddress, List<IAccessRequest> requests)
        {
            return _readingMode && requests.Any(x=>x.DataBlockAddress == currentAddress);
        }
    }
}
