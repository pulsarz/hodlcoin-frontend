using FleetSharp;
using FleetSharp.Types;

namespace HodlCoin.Client
{
    public class Config
    {
        public static NodeInterface node = new NodeInterface("https://tn-ergo-node-api.anetabtc.io");
        public static Explorer explorer = new Explorer("https://api-testnet.ergoplatform.com/api/v1");

        public static string ExplorerURL = "https://explorer.ergoplatform.com";

        public const long DEFAULT_TX_FEE = 5000000L;//0.005 ERG
    }
}
