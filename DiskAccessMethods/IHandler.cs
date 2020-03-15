using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods
{
    public interface IHandler
    {
        int? HandleNextMoveSelection(int currentAddress, List<IAccessRequest> requests);
        bool? HandleDiscReadingReadiness(int currentAddress, List<IAccessRequest> requests);
    }
}
