using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods
{
    public interface IHandler
    {
        IAccessRequest HandleSelectionRequest(List<IAccessRequest> accessRequests);
    }
}
