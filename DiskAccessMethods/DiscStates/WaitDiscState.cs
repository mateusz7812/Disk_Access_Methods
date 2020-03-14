namespace DiskAccessMethods.DiscStates
{
    public class WaitDiscState: AbstractDiscState
    {
        public WaitDiscState(IDisc disc) : base(disc) { }
        
        public override void Update(int nowInMilliseconds)
        {
            var nextRequest = Disc.HandleSelectionRequest(Disc.GetAllAccessRequests());
            if (nextRequest == null) return;
            Disc.NextDataBlockAddress = nextRequest.DataBlockAddress;
            Disc.SetState<HeadMovingDiscState>();
        }
    }
}
