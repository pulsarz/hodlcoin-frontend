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
		public long maxSupply { get; set; }
        public long preciseFactor { get; set; }
        public long feePercent { get; set; }
        public string contractAddress { get; set; }

        //Base token info
        public string baseTokenName { get; set; }
        public string baseTokenId { get; set; }
        public int baseTokenDecimals { get; set; }

    }
	public class Parameters
	{
		public const Network NETWORK = Network.Testnet;

        public const long MIN_BOX_VALUE = OutputBuilder.SAFE_MIN_BOX_VALUE;

		public const double DEV_FEE_PERCENT = 0.003;
		public const long MINIMUM_DEV_FEE_ERG = 1000000L;//0.001 ERG, per box
                                                     //ToDo: Change me to mainnet address AND in the eroscript enable this!!!!
        public const string DEV_FEE_ADDRESS1 = "3WyfzsRppTdUgJBEdi75HyNz3e2PfBeZJx53YoZoR7KtKJZrDUDV";
        public const string DEV_FEE_ADDRESS2 = "3WyfzsRppTdUgJBEdi75HyNz3e2PfBeZJx53YoZoR7KtKJZrDUDV";
        public const string DEV_FEE_ADDRESS3 = "3WyfzsRppTdUgJBEdi75HyNz3e2PfBeZJx53YoZoR7KtKJZrDUDV";

        public const string CONTRACT_ADDRESS_ERG = "Y2S1gAniW8TBEqubZBoa4UFH9MjMzqH8FTSi2JhQdmW6QJKHNweqVZhfdso5xdAHzhaa8Myd8LgEpn6BTryv2VReMGdmtuBR4wv8bFN7VWTbcTBdVDKAPzJJjsP9tNuZyjvkVPBGTEAvytSeRVyG29FcV6kQbCyF4gwy4i1GwCrHFX5Rmci9Duy6RM7fqQxMCBEtNG8YYSXJ46ao4cCmD8Xki45TnubWimXQQiAjgTdqVvz5tVg66pTjb9GN6si38keSxUtP5aGdJ4G1QJnQMc7QQhnXPyb61mAQyBF2qnyCgubyyiV7Qb7ifEFsd6SorA2ekBs6FWSsckkNYhrkj5ubvZdu1cm4tQZVyN6QAjEeJ43LqcagJtpn4ZtsVg6SqPVqNSWDKyHSavGk8P2dBUzZYL9Q5tTwbXzdAsqhWgzVGsbqgxBo6WB7xdRUnNq";
		public const string CONTRACT_ADDRESS_TOKEN = "";//ToDo: create contract for tokens

		public static List<HodlTokenInfo> tokens = new List<HodlTokenInfo>()
		{
			{
				new HodlTokenInfo {
					name = "hodlERG",
					description = "hodlERG 3%",
					bankNFTTokenId = "fb645d484218923dee9adfa42d90faf4c29ae862f0e5130f873e5b0bfece37c0",
					tokenId = "2679615d28e1d0289eff28b71b7c93dcd7210a6419fa56a7df5f5f8ea1740d83",
					decimals = 9,
					maxSupply = 97739924000000000L,
					preciseFactor = 1000L,
					feePercent = 3,
					contractAddress = CONTRACT_ADDRESS_ERG,
					baseTokenName = "ERG",
					baseTokenId = "0000000000000000000000000000000000000000000000000000000000000000",
					baseTokenDecimals = 9,
                }
			}

        };
	}
}
