using FleetSharp;
using FleetSharp.Types;

namespace HodlCoin.Client
{
    public class Config
    {
        public static NodeInterface node = new NodeInterface("https://ergo-node-2.sigmaexplorer.org");
        public static Explorer explorer = new Explorer("https://api.ergoplatform.com/api/v1");

        public static string ExplorerURL = "https://explorer.ergoplatform.com";

        public const long DEFAULT_TX_FEE = 1_500_000L;//0.0015 ERG
    }
}
