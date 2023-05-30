using FleetSharp.Builder;
using FleetSharp.Types;
using System.Numerics;

namespace HodlCoin.Client.HodlCoinImpl
{
	public class Parameters
	{
		public const Network NETWORK = Network.Testnet;
		public const long MAX_SUPPLY = 97739924000000000L;


        public const long MIN_BOX_VALUE = OutputBuilder.SAFE_MIN_BOX_VALUE;

		public const long FEE_PERCENT = 3;

		public const double DEV_FEE_PERCENT = 0.003;
		public const long MINIMUM_DEV_FEE = 1000000L;//0.001 ERG
		//ToDo: Change me to mainnet address AND in the eroscript enable this!!!!
		public const string DEV_FEE_ADDRESS = "3WyfzsRppTdUgJBEdi75HyNz3e2PfBeZJx53YoZoR7KtKJZrDUDV";


        public const string HODLCOIN_TOKEN_ID = "d842bfc3723a17144861ed3aa7cac4af23a159f1ea8adcdc5fdefd6e3d4aca7a";
		public const string BANK_NFT_TOKEN_ID = "cecf2ede5dff47230f031556ddb173ca76f506dad2131b844b79fa1f64fe150c";

		public static long PRECISE_FACTOR = 1000L;

        public const string CONTRACT_ADDRESS = "axXSXt1Kgwe67ndmBtujKtjRBG7MFiREBfYwXTMAhn4W4p5WzZ2mPrmrGZSqpSERBjAzLtjw9E8k6TxzssqVvFkHZj14hPvzuikkmNwk7rqEYnQacmdTH4DVU74y9rRLebyV3kQScFSLtgzQteZykhMHY73yxf5okC2gUrbiEbzhsPMo9Mj4UrneBVkNLSxVR4q9kepNKExbVHnqzWuqFh2nrFdD8cZCUSxnpQ7pfJZXFXg6AWGiEXW5kfSwS14LMLYNDFWNkQir93PcZamJnq92eaz7DenjRvxQK1ecoLLZznK8VkMcdaUsh5KceVS1ehikjcNa4EahYwEmiP67DQrUk67j7tFLnjdDCmWtGjRGgjwpuxkmqZM5e6N6A8kyLgk2gaGGABUyaioCXCUbtG56D4yEuHBaRJj7c294GNRmt4jyRTwvwjRJni8jPYxsSD2TxnSkpdHpqR";
	}
}
