using System;
using System.Collections.Generic;
using DiskAccessMethods.AccessRequests;
using DiskAccessMethods.DiscAccessStrategies;

namespace DiskAccessMethods
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var dataBlocks = new List<IDataBlock>(){new DataBlock(), new DataBlock(), new DataBlock(), new DataBlock()};
            var discConfig = new DiscConfig();
            var disc = new Disc(discConfig);
            disc.AddDataBlocks(dataBlocks);
            disc.ChainAccessStrategy(new FdScanDiscAccessStrategy(discConfig.MoveToNextBlockTimeInMilliseconds, () => disc.CurrentTime));
            disc.ChainAccessStrategy(new CScanDiscAccessStrategy(dataBlocks.Count));

            var accessRequests = new List<IAccessRequest>()
            {
                new CallbackWriteAccessRequest(0, 0, "data1", Console.WriteLine),
                new CallbackWriteAccessRequest(2, 50, "data2", Console.WriteLine),
                new CallbackWriteAccessRequest(1, 150, "data3", Console.WriteLine),
                new CallbackRealTimeWriteAccessRequest(3, 100, 200, "data4", Console.WriteLine),
                new CallbackReadAccessRequest(0, 200, Console.WriteLine),
                new CallbackRealTimeReadAccessRequest(2, 400, 500, Console.WriteLine),
                new CallbackReadAccessRequest(3, 500, Console.WriteLine),
                new CallbackReadAccessRequest(1, 300, Console.WriteLine)
            };
            
            disc.AddAccessRequests(accessRequests);
            disc.Update(3000);
            Console.ReadLine();
        }
    }
}
