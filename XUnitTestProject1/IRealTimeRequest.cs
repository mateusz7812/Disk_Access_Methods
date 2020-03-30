using System;
using System.Collections.Generic;
using System.Text;
using DiskAccessMethods;

namespace XUnitTestProject1
{
    public interface IRealTimeRequest: IAccessRequest, IRealTime
    {
    }
}
