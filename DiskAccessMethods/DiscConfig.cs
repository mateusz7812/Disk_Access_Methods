using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods
{
    public class DiscConfig: IDiscConfig
    {
        public int MoveToNextBlockTimeInMilliseconds { get; set; } = 100;
        public int IoOperationTimeInMilliseconds { get; set; } = 100;
    }
}
