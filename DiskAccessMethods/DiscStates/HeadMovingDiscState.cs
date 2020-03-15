using System;

namespace DiskAccessMethods.DiscStates
{
    public class HeadMovingDiscState : AbstractDiscState
    {
        public HeadMovingDiscState(IDisc disc) : base(disc) { }

        public override void Update(int nowInMilliseconds)
        {
            while (!Disc.DiscReadyToReading() && Disc.IsEnoughTimeOnNextMove(nowInMilliseconds))
                Disc.HandleNextMove();

            if (!Disc.DiscReadyToReading()) return;
            Disc.SetState<RequestHandlingDiscState>();
            Disc.Update(nowInMilliseconds);
        }
    }
}
