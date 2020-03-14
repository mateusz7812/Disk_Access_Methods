using System;
using System.Collections.Generic;
using System.Text;

namespace DiskAccessMethods
{
    public abstract class AbstractDiscState: IDiscState
    {
        protected readonly IDisc Disc;

        protected AbstractDiscState(IDisc disc)
        {
            Disc = disc;
        }

        public abstract void Update(int nowInMilliseconds);
    }
}
