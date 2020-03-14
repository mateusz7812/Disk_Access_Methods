using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods
{
    public abstract class AbstractDiscAccessStrategy : IHandler
    {
        private readonly IHandler _nextHandler;

        protected AbstractDiscAccessStrategy(IHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public virtual IAccessRequest HandleSelectionRequest(List<IAccessRequest> accessRequests)
        {
            var nextRequest = SelectNextRequest(accessRequests);
            return nextRequest ?? _nextHandler?.HandleSelectionRequest(accessRequests);
        }

        protected abstract IAccessRequest SelectNextRequest(List<IAccessRequest> accessRequests);
    }
}
