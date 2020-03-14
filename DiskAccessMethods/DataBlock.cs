using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods
{
    public class DataBlock: IDataBlock
    {
        public string Data { get; set; }
        public void Accept(IAccessRequest accessRequest)
        {
            accessRequest.Visit(this);
        }
    }
}
