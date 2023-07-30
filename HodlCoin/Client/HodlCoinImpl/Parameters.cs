using FleetSharp;
using FleetSharp.Builder;
using FleetSharp.Types;
using System.Numerics;

namespace HodlCoin.Client.HodlCoinImpl
{
	public class HodlTokenInfo
	{
		public string name { get; set; }
        public string description { get; set; }
        public string bankNFTTokenId { get; set; }
        public string tokenId { get; set; }
        public int decimals { get; set; }
        public long feeDenom { get; set; }
        public long precisionFactor { get; set; }//Purely for UI
        public decimal protocolFeePercentage { get; set; }//Purely for UI
        public decimal devFeePercentage { get; set; }//Purely for UI

        //Base token info
        public string baseTokenName { get; set; }
        public string baseTokenId { get; set; }
        public int baseTokenDecimals { get; set; }

    }
	public class Parameters
	{
		public const Network NETWORK = Network.Testnet;

        public const long MIN_BOX_VALUE = 1000000L;//0.001 ERG

		//ToDo: Change to mainnet!
        public static ErgoAddress DEV_FEE_ADDRESS = ErgoAddress.fromBase58("3m1wJ88xGVWx79p9oenKvCTwXp4o4J6n7KnbF9N3uRcmvtH1MAegmuBxsmB7kfjYTie2Rpp6He9kh3XJBNwwqr5HUiPsD1jLQcJ15kYneii6JN8UfSPcHb6mZA8k3PB7pcQUFox416Tv6AmrReVtVHaP1VGxfvM7WRNEbJDbu8wU7fwkRyqxPBbJXTcUMogpg9XVstiQHPmndzqbdREUSNVY5iuV68GYaYc7YuBqhax8oVLxWPCWGoaZEevPtsEnxXTKvR94H6KfPW1xwRFztZMpwzyv9PNVSqmXXCqhe8ryb3RkSEsk59CBcEMoNxkEA2zi3BMWV65U3h5PHa1d1X1ePS7fBaPTHRa5roSs6AhEdUK1tPjun7N1Jqo88SbGeWrY4hbNjCVGjAcx9ZC9vfE4sZdUtyCQyguzEwYdfdb6Xgn9YaKwC3LeR9Q2jDPt3wEtLrPdpAhCpn8b9MzaH79fdM7pnxxMj7NqdrFJUZ8Aqq3eb4CrBj4y8eUztPuuBMA7kdJ4GHLy8t4aPSUhkSPVdFcTGTbvv4e83UMZdEAL1GeqA3CJjTYRiiK3kKsphdXuxwmZHPPBAqSnm3H2nnAbb");

		public static List<HodlTokenInfo> tokens = new List<HodlTokenInfo>()
		{
			{
				new HodlTokenInfo {
					name = "hodlERG",
					description = "hodlERG 3%",
					bankNFTTokenId = "142a10ec641cfa67828f93dff63291d51236458263de22f136f3e786871d97df",
					tokenId = "3035f1ffd385adc4a8869755b20bde61d1031398e7780145437c9eb68a1469e9",
					decimals = 9,
					feeDenom = 1000L,
					baseTokenName = "ERG",
					baseTokenId = "0000000000000000000000000000000000000000000000000000000000000000",
					baseTokenDecimals = 9,

					//Purely for UI
					precisionFactor = 1000000L,
                    protocolFeePercentage = 3M,
                    devFeePercentage = 0.3M,
                }
			}

        };
	}
}
