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
            var dataBlock = Disc.GetDataBlockOfCurrentAddress();
            var requests = Disc.GetRequestsByBlockNumber(Disc.NextDataBlockAddress);

            requests.Sort((r1, r2)=>r1.CreateTime - r2.CreateTime);
            while (requests.Count != 0 && IsEnoughTimeOnOperation(nowInMilliseconds))
            {
                var r = requests.First();
                r.Visit(dataBlock);
                Disc.RemoveRequest(r);
                Disc.LastTimeInMilliseconds += Disc.IoOperationTimeInMilliseconds;
            }

            if (requests.Count != 0) return;
            Disc.SetState<WaitDiscState>();
            Disc.Update(nowInMilliseconds);
        }

        private bool IsEnoughTimeOnOperation(int nowInMilliseconds) => Disc.IoOperationTimeInMilliseconds + Disc.LastTimeInMilliseconds <= nowInMilliseconds;
    }
}
