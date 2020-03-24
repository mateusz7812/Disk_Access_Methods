using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiskAccessMethods
{
    public abstract class AbstractDiscAccessStrategy : IHandler
    {
        private readonly IHandler _nextHandler;
        protected int NextDataAddress = 0;

        protected AbstractDiscAccessStrategy(IHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }
        
        public int? HandleNextMoveSelection(int currentAddress, List<IAccessRequest> requests)
        {
            var nextMove = SelectNextMove(currentAddress, requests);
            return nextMove ?? _nextHandler?.HandleNextMoveSelection(currentAddress, requests);
        }

        protected abstract int? SelectNextMove(int currentAddress, List<IAccessRequest> requests);

        public bool? HandleDiscReadingReadiness(int currentAddress, List<IAccessRequest> requests)
        {
            var isDiscReady = IsDiscReadyToReading(currentAddress, requests);
            return isDiscReady ?? _nextHandler?.HandleDiscReadingReadiness(currentAddress, requests);
        }
        
        protected virtual bool? IsDiscReadyToReading(int currentAddress, List<IAccessRequest> requests)
        {
            return NextDataAddress == currentAddress && requests.Any(x => x.DataBlockAddress.Equals(NextDataAddress));
        }
    }
}
