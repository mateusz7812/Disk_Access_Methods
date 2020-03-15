using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiskAccessMethods.DiscStates
{
    public class RequestHandlingDiscState: AbstractDiscState
    {
        public RequestHandlingDiscState(IDisc disc) : base(disc) { }

        public override void Update(int nowInMilliseconds)
        {
            while (Disc.CurrentAddressHaveNotDoneRequests() && Disc.IsEnoughTimeOnOperation(nowInMilliseconds))
                Disc.HandleNextRequest();

            if (Disc.CurrentAddressHaveNotDoneRequests()) return;
            Disc.SetState<HeadMovingDiscState>();
            Disc.Update(nowInMilliseconds);
        }
    }
}
