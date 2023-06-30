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

        public const long MIN_BOX_VALUE = 1000000L;//0.001 ERG

		public const double DEV_FEE_PERCENT = 0.003;
		
        public const string DEV_FEE_ADDRESS1 = "3WymZ3DL8m7Uw67nUr7uAFUPB5V4PdidrhG9ZdhXdiL2E7mhvjtn";
        public const string DEV_FEE_ADDRESS2 = "3WzHee44uHF82VmoGZyZQEb97c2E7ZoNDjmAcZx81weReUc9GA86";
        public const string DEV_FEE_ADDRESS3 = "3WwVHEVXgq7gKtbpenLCovacwSTdBRB8d4AThGAHTfUELM5QuEx6";

		public static List<HodlTokenInfo> tokens = new List<HodlTokenInfo>()
		{
			{
				new HodlTokenInfo {
					name = "hodlERG",
					description = "hodlERG 3%",
					bankNFTTokenId = "bfc9dd7af3f9b9b107c03b62f41bfe3b335e474f11b065bcf5126295144e8a4f",
					tokenId = "cab60a6fb68f111b294a4d77d8bb23ab733a608888301a2cc08d941f21ad7c2d",
					decimals = 9,
					maxSupply = 97739924000000000L,
					preciseFactor = 1000L,
					feePercent = 3,
					contractAddress = "32EYw1mCAKwSg8FV4oHEnYwnUv4GVkEb6DypH4eW9ye1Y4rfuDi3e4dLd3nPkPjVvYMDGgb3vmGiwtZqno6CFYd5JBPbnR2Gx1zEkpRaN6ZW5QFbS3kg44hKViPf48DgbiLZGjy7rkv8HqmpVAX9qBbn42nZDuarsjk37sMqjcSwcUJJBAKeZ2ZmsZSqG6x9gq1rSoGrEWdLrdGjio3tguSSJUWEvwBJ3fXPQ2pPrKSxZF7uzuyCNxde6tUAsQQc26W8Ytoh9nxiEdxYMWieMYULaCbF5WC6TMTYYbAiR1vQuvg7raKvX1rz62i7aKWHssJFmcMYsCL8WnnQh9MTenHHoSndchGG87KE9Aj2ewPyWQzMMsNKKryo2CNxJNkzyoad2F3E69KaGuddE3q2Sbr4V6jjuVUAAHtQV8UU4BwGPqhfxkVLXDqLcQP522es41ehTqcbb9siY9YRWuCnJyujPDzV5XCUdHCowLchLBfvifMvLvybH1syFGiyUxRMpZVriVTTpbu3m215AunRAQhiUhwGAw8Q9W",
					baseTokenName = "ERG",
					baseTokenId = "0000000000000000000000000000000000000000000000000000000000000000",
					baseTokenDecimals = 9,
                }
			}

        };
	}
}
