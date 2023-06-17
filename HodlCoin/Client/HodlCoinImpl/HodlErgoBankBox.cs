using FleetSharp.Builder;
using FleetSharp.Types;
using FleetSharp;
using static FleetSharp.Sigma.ConstantSerializer;
using static FleetSharp.Sigma.ISigmaCollection;
using static FleetSharp.Sigma.IPrimitiveSigmaType;

namespace HodlCoin.Client.HodlCoinImpl
{
	public class HodlErgoBankBox
	{
		private Box<long> _bankBox;
		private long _NumCirculatingReserveCoins;

		public HodlErgoBankBox(Box<long> bankBox)
		{
			_bankBox = bankBox;
			_NumCirculatingReserveCoins = SParse(_bankBox.additionalRegisters.R4);
		}

		public Box<long> GetBox()
		{
			return _bankBox;
		}

		/// The number of ReserveCoins currently minted. In other words the number
		/// currently in circulation. Held in R4 of Bank box.
		public long NumCirculatingReserveCoins()
		{
			return _NumCirculatingReserveCoins;
		}

		/// Provides the base(Erg) reserves of the Bank. This is the total amount
		/// of nanoErgs held inside, minus the minimum box value required for
		/// posting a box on-chain
		public long BaseReserves()
		{
			if (_bankBox.value < Parameters.MIN_BOX_VALUE)
			{
                return 0;
			}
            return _bankBox.value;
		}

        /// Current ReserveCoin nominal price with precise factor of PRECISE_FACTOR
        public long ReserveCoinNominalPrice()
		{
			if (NumCirculatingReserveCoins() <= 0) return Parameters.PRECISE_FACTOR;
            return (BaseReserves() * Parameters.PRECISE_FACTOR) / NumCirculatingReserveCoins();
		}

		/// The total amount of nanoErgs which is needed to cover minting
		/// the provided number of ReserveCoins, cover tx fees, implementor
		/// fee, etc.
		public long BaseCostToMintReserveCoin(long amountToMint)
		{
			long feelessCost = (long)((ReserveCoinNominalPrice() * amountToMint) / Parameters.PRECISE_FACTOR);
			var protocolFee = 0;
			return feelessCost + protocolFee;
		}

		/// The total amount of nanoErgs which is needed to cover minting
		/// the provided number of ReserveCoins, cover tx fees, implementor
		/// fee, etc.
		public long TotalCostToMintReserveCoin(long amountToMint, long txFee)
		{
			var baseCost = BaseCostToMintReserveCoin(amountToMint);
            return (long)(baseCost + txFee);
		}

		public long BaseAmountFromRedeemingReserveCoin(long amountToRedeem)
		{
			var feelessAmount = (long)((ReserveCoinNominalPrice() * amountToRedeem) / Parameters.PRECISE_FACTOR);
			var protocolFee = feelessAmount * Parameters.FEE_PERCENT / 100;

			return (long)(feelessAmount - protocolFee);
		}

        public long TotalAmountFromRedeemingReserveCoin(long amountToMint, long txFee)
        {
            var baseCost = BaseAmountFromRedeemingReserveCoin(amountToMint);
			var devFee = (long)((double)baseCost * Parameters.DEV_FEE_PERCENT);
			if (devFee < Parameters.MINIMUM_DEV_FEE) devFee = Parameters.MINIMUM_DEV_FEE;

            return (long)(baseCost - txFee - devFee);
        }

        //Binary search to find out how much hodlcoin we would receive for sending x erg (no fees except protocol fee)
        public long BaseAmountToReserveCoinAmount(long maxCost)
        {
            long minAmount = 0;
            long maxAmount = maxCost;

            while (minAmount <= maxAmount)
            {
                var midAmount = (minAmount + maxAmount) / 2;
                var cost = BaseCostToMintReserveCoin(midAmount);

                if (cost == maxCost)
                {
                    return midAmount;
                }
                else if (cost > maxCost)
                {
                    maxAmount = midAmount - 1;
                }
                else
                {
                    minAmount = midAmount + 1;
                }
            }

            return maxAmount;
        }

		public long FeesFromRedeemingReserveCoin(long amountToRedeem, long txFee)
		{
			var feelessAmount = (long)((ReserveCoinNominalPrice() * amountToRedeem) / Parameters.PRECISE_FACTOR);
			var protocolFee = feelessAmount * Parameters.FEE_PERCENT / 100;
			var devFee = (long)((double)BaseAmountFromRedeemingReserveCoin(amountToRedeem) * Parameters.DEV_FEE_PERCENT);
			if (devFee < Parameters.MINIMUM_DEV_FEE) devFee = Parameters.MINIMUM_DEV_FEE;

			return (protocolFee + txFee + devFee);
		}

		//Methods for creating the candicate
		/// Create an `OutputBuilder` for the output Bank box for the
		/// `Mint ReserveCoin` action
		public OutputBuilder CreateMintReserveCoinCandidate(Box<long> inputBankBox, long amountToMint, long circulatingReservecoinsOut, long reservecoinValueInBase)
		{
			var bankReservecoinTokenIn = inputBankBox.assets[0];

			var reservecoinTokens = new TokenAmount<long> { tokenId = Parameters.HODLCOIN_TOKEN_ID, amount = (long)(bankReservecoinTokenIn.amount - amountToMint) };
			var nftToken = inputBankBox.assets[1];
			var obbTokens = new List<TokenAmount<long>> { reservecoinTokens, nftToken };

			var registers = new NonMandatoryRegisters { R4 = SConstant(SLong((long)circulatingReservecoinsOut)) };

			var outputBankCandidate = new OutputBuilder((long)(inputBankBox.value + reservecoinValueInBase), ErgoAddress.fromErgoTree(ErgoAddress.fromBase58(Parameters.CONTRACT_ADDRESS).GetErgoTreeHex(), Parameters.NETWORK))
				.AddTokens(obbTokens)
				.SetAdditionalRegisters(registers);

			return outputBankCandidate;
		}

		public OutputBuilder CreateRedeemReserveCoinCandidate(Box<long> inputBankBox, long amountToRedeem, long circulatingReservecoinsOut, long reservecoinValueInBase)
		{
			var bankReservecoinTokenIn = inputBankBox.assets[0];

			var reservecoinTokens = new TokenAmount<long> { tokenId = Parameters.HODLCOIN_TOKEN_ID, amount = (long)(bankReservecoinTokenIn.amount + amountToRedeem) };
			var nftToken = inputBankBox.assets[1];
			var obbTokens = new List<TokenAmount<long>> { reservecoinTokens, nftToken };

			var registers = new NonMandatoryRegisters { R4 = SConstant(SLong((long)circulatingReservecoinsOut)) };

            var outputBankCandidate = new OutputBuilder((long)(inputBankBox.value - reservecoinValueInBase), ErgoAddress.fromErgoTree(ErgoAddress.fromBase58(Parameters.CONTRACT_ADDRESS).GetErgoTreeHex(), Parameters.NETWORK))
				.AddTokens(obbTokens)
				.SetAdditionalRegisters(registers);

			return outputBankCandidate;
		}
	}
}
