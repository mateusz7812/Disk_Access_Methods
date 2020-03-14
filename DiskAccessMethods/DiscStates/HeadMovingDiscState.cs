using System;

namespace DiskAccessMethods.DiscStates
{
    public class HeadMovingDiscState : AbstractDiscState
    {
        public HeadMovingDiscState(IDisc disc) : base(disc) => _direction = Disc.NextDataBlockAddress > Disc.CurrentAddress ? 1 : -1;

        private readonly int _direction;

        public override void Update(int nowInMilliseconds)
        {
            while (!DiscIsPositionedOnNextBlock() && IsEnoughTimeOnNextMove(nowInMilliseconds)) MakeDiscHeadMove();
            if (DiscIsPositionedOnNextBlock()) Disc.SetState<RequestHandlingDiscState>();
        }

        private bool IsEnoughTimeOnNextMove(int nowInMilliseconds) => Disc.LastTimeInMilliseconds + Disc.MoveToNextBlockTimeInMilliseconds <= nowInMilliseconds;
        private bool DiscIsPositionedOnNextBlock() => Disc.CurrentAddress == Disc.NextDataBlockAddress;

        private void MakeDiscHeadMove()
        {
            Disc.LastTimeInMilliseconds += Disc.MoveToNextBlockTimeInMilliseconds;
            Disc.CurrentAddress += _direction;
        }
    }
}