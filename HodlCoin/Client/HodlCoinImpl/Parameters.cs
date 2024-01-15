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
        public ErgoAddress devFeeAddress { get; set; }
        public long precisionFactor { get; set; }//Purely for UI
        public decimal protocolFeePercentage { get; set; }//Purely for UI
        public decimal devFeePercentage { get; set; }//Purely for UI

        //Base token info
        public string baseTokenName { get; set; }
        public string baseTokenId { get; set; }
        public int baseTokenDecimals { get; set; }
		public DateTime launchedOnUTC { get; set; }

    }
	public class Parameters
	{
		public const Network NETWORK = Network.Mainnet;

        public const long MIN_BOX_VALUE = 1_000_000L;//0.001 ERG

		public static List<HodlTokenInfo> tokens = new List<HodlTokenInfo>()
		{
			{
				new HodlTokenInfo {
					name = "hodlERG3",
					description = "hodlERG 3%",
					bankNFTTokenId = "6e9c85c4be018b1308ddf034baf1406490e2a9dd406c01591bd6df41e98b6057",
					tokenId = "cbd75cfe1a4f37f9a22eaee516300e36ea82017073036f07a09c1d2e10277cda",
					decimals = 9,
					feeDenom = 1000L,
					baseTokenName = "ERG",
					baseTokenId = "0000000000000000000000000000000000000000000000000000000000000000",
					baseTokenDecimals = 9,
                    devFeeAddress = ErgoAddress.fromBase58("mCbJgVdTUoKiLn8a8Xucgvj45icZtV2uC85276wHVJQGtRhWzVAFiRDjJohyFDV81nShLn5Afc33mnV9mZUTkVty1zRPPRcXWRjhouRpxor7kMycsv8WSrbbP4p9oxsrsdoc6GQaoddfGexyzyPoQmgxNV1B9WQm4Ec5DTvEceHeV69mvqHGxB7cgps7eCvp2wLfhLm4DDuoteC6igiiHTtVhcG6SesqycQfnH7HACfjZLDsqAHtxCG3XzoGBpz6TjHTCXEmYEN3FiTd4AVskFaV31z5Re69ArUTxMoWPjP6dXZb3LBXtti3RUBeXLSAyTCbJktxU6irjjobTgz4jrjFr9QPnBnFENUesLpF4RYyXsR4t3awPZgXLbHFvCT8t8bcpnrP92Nue1wVCthaWRmCaJyRZddZDZLwAzSVutgaANNzr795EzVhv1kRTPgrae25VFZfdnVwvAiL1g67pJFc74etaEFGUQ26aotehPH6Y9hMreHiUDFRepGZiWRgsdpgoGnRdv4yR6GvePRWEymWCfsy1cNRhmqz4XbJPAaNuEoPkGbBTfvCFdjhfh9tczf4tkuRdhNLBePSbq9vntwFrek2Sy62D7MUfwK4GqwDkFDyYUNsgVKjC7z3nLQAdAU72ChhAXMqKLrxhvp2VaUmZj8jYdbXLUaH3Q7uJC65EHcMau"),

					//Purely for UI
					precisionFactor = 1000000L,
                    protocolFeePercentage = 3M,
                    devFeePercentage = 0.3M,
                    launchedOnUTC = new DateTime(2023,8,3,23,48,59)//https://explorer.ergoplatform.com/en/transactions/d5314f5fd09aa57a476dfe765aa725816c4437166498dea7c82c6c5b44eabbc5
                }
			}
        };
	}
}
