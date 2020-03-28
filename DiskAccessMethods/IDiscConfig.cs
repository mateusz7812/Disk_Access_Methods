using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods
{
    public interface IDiscConfig
    {
        int MoveToNextBlockTimeInMilliseconds { get; }
        int IoOperationTimeInMilliseconds { get; }
    }
}
