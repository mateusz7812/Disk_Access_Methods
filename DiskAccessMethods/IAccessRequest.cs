using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods
{
    public interface IAccessRequest
    {
        int CreateTime { get; }
        int DataBlockAddress { get; }
        void Visit(IDataBlock dataBlock);
    }
}
