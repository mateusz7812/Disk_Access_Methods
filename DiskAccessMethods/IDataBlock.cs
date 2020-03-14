using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods
{
    public interface IDataBlock
    {
        string Data { get; set; }
        void Accept(IAccessRequest accessRequest);
    }
}
