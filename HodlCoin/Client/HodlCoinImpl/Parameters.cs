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
                                                     //ToDo: Change me to mainnet address AND in the eroscript enable this!!!!
        public const string DEV_FEE_ADDRESS1 = "3WyfzsRppTdUgJBEdi75HyNz3e2PfBeZJx53YoZoR7KtKJZrDUDV";
        public const string DEV_FEE_ADDRESS2 = "3WyfzsRppTdUgJBEdi75HyNz3e2PfBeZJx53YoZoR7KtKJZrDUDV";
        public const string DEV_FEE_ADDRESS3 = "3WyfzsRppTdUgJBEdi75HyNz3e2PfBeZJx53YoZoR7KtKJZrDUDV";

        public const string CONTRACT_ADDRESS_ERG = "ATDbjU5VJUs87ckaw6WQjow4bZpDngic31uAN4fc5mMQWrwLmMwUeksSFPCDZ7iDmauQitG1YLM9qwQq9Yx7xo51JXARhSA3kDNW582euEAxueUUhQMXaN7BgNSusuqhKsJKAgYaY4gUWG85gF56PZ1oR9EfoWUzxxw944UKvpUbVSiHxQuxacnTvYAfnBUeYZvdRQDy6JxznNmzkrjm4XgcB19GgsyZDWfaxsf6hhRgHKDetJuwdm6M3LsNPiipdBEox7cANhZ2PogeH7eENBKzg6Zxis5C6ovSDMo14eMPrvC541DFr7vqqCkcyBxr8c1gBrBHDYKTjhTKeLykNCyiPet4PcvF1HHne1R1qCrChmYSwNgUKBPGCvEYkvu8vLiNeqhYGHR2asL1rQkvqFzUGoPkm7S5JWx3AycatUHD9iaCpxdjrXDeaVKmjiwNYWqbXxbg9LQsDjqadkKsdgZo62PBVqLeXW28vaqkDuXnFiKUUdS8eLZapAfCwBnZHGusHSxnFkmWcbjqF9oReS8Yk9BypHGJ8NUBLjXK2dz21TRKqRWFYkEWqAjjgqC8Lrygj8Cm3EQtcNMmt3Z94ArSSiCB8HsDW8icpZUjFa";
		public const string CONTRACT_ADDRESS_TOKEN = "";//ToDo: create contract for tokens

		public static List<HodlTokenInfo> tokens = new List<HodlTokenInfo>()
		{
			{
				new HodlTokenInfo {
					name = "hodlERG",
					description = "hodlERG 3%",
					bankNFTTokenId = "e3fea267b489291a13ff69be05f5231a6fefdeed54a66b431f826e09ba150e09",
					tokenId = "fffcba7b514f817d1a282f10356c23f398cbc34aa2dbf75e2a67128a7253c52e",
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
