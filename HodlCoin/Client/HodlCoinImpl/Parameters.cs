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
		public const Network NETWORK = Network.Mainnet;

        public const long MIN_BOX_VALUE = 1000000L;//0.001 ERG

		public const double DEV_FEE_PERCENT = 0.003;
		
        public const string DEV_FEE_ADDRESS1 = "9hHondX3uZMY2wQsXuCGjbgZUqunQyZCNNuwGu6rL7AJC8dhRGa";
        public const string DEV_FEE_ADDRESS2 = "9gnBtmSRBMaNTkLQUABoAqmU2wzn27hgqVvezAC9SU1VqFKZCp8";
        public const string DEV_FEE_ADDRESS3 = "9iE2MadGSrn1ivHmRZJWRxzHffuAk6bPmEv6uJmPHuadBY8td5u";

		public static List<HodlTokenInfo> tokens = new List<HodlTokenInfo>()
		{
			{
				new HodlTokenInfo {
					name = "hodlERG",
					description = "hodlERG 3%",
					bankNFTTokenId = "28d9082b4a9eb7497e58b028840273d5f528cf1b6beeff97cab10714a5d40725",
					tokenId = "28bd64421838751c96eddc09d7c990fde5c0d307b774463b448bf71d33dba324",
					decimals = 9,
					maxSupply = 97739924000000000L,
					preciseFactor = 1000L,
					feePercent = 3,
					contractAddress = "7YErXocwTgQbejCSkKGS3GBorYCcyqcExQS23ErypeC7QtgrkqnJT4TxAaKKRgB1Lg1RjTxCbUhWCmP5qzfHeFSGZHhVBzX49PJNJzWeEb2nkbW9LYjQjvU1SBNwZBajSYZwk2Y2JVUwxm5CXYWoNMjZGPnTXd3oFG5zCBBjEgLYtvNPiJZKuRoWNbkPiiahB4TLeVGZcn4HZhFEaANj9yMvmUeqHhS8PsaHeSNpXEUxDGmeoNYqkNM2sEMyZtLDxBCHKLiA99eYhZoqA7VjxG4UPr7zVTg6dXE9ctfqmvPQyugn37ryRPx7K3WkTRb4D9yNnR4YEAZeUnLZjWX8XfoB2JT5KCUgyqtDtf5wYFNsRuq4RSUxnG9Cj1M3tEcY5f174R6PHCKPUKhDwYMiPHo67erww6ZL25m5cJ5BkMhpoVgnbhjboAr86wN2XWfmMh5mVEBB776pkamhADnu4JyfuY28tbZ89PVj6hJMU8xM9kW5RN3AvuBdLSez347XzynVnraVEBsYFUNgLqfbHXM4VsCqrb6SsquM65H3b2n19tKQ3R87Sho3pUFWFYfGvbdoyu1tjctkFgMLDVKj85KXGU6v3opFajoYSY7EbvX1N4pa2CaPqYKpDUFZaVZ1EiUtjRPV62evMotexxD3fVgoGzYZtxi1APXRK261pBU5qBgU2rkCMtAcCuQXkAQaKCpohaX5tNW2QzQUhcUAfAwvzBdakEAGfiopa",
					baseTokenName = "ERG",
					baseTokenId = "0000000000000000000000000000000000000000000000000000000000000000",
					baseTokenDecimals = 9,
                }
			}

        };
	}
}
