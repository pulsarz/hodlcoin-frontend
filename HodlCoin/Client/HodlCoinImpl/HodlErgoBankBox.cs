using FleetSharp.Builder;
using FleetSharp.Types;
using FleetSharp;
using static FleetSharp.Sigma.ConstantSerializer;
using static FleetSharp.Sigma.ISigmaCollection;
using static FleetSharp.Sigma.IPrimitiveSigmaType;
using static MudBlazor.Colors;
using System.Numerics;

namespace HodlCoin.Client.HodlCoinImpl
{
	public class HodlErgoBankBox
	{
		private Box<long> _bankBox;
		private long _NumCirculatingReserveCoins;

        //New ones
        private long _totalTokenSupply;
        private long _precisionFactor;
        private long _minBankValue;
        private long _devFeeNum;
        private long _bankFeeNum;
        private string _ergoTree;

        private HodlTokenInfo _info;

        public HodlErgoBankBox(Box<long> bankBox, HodlTokenInfo info)
		{
			_bankBox = bankBox;
            _totalTokenSupply = SParse(_bankBox.additionalRegisters.R4);
            _precisionFactor = SParse(_bankBox.additionalRegisters.R5);
            _minBankValue = SParse(_bankBox.additionalRegisters.R6);
            _devFeeNum = SParse(_bankBox.additionalRegisters.R7);
            _bankFeeNum = SParse(_bankBox.additionalRegisters.R8);
            _ergoTree = _bankBox.ergoTree;
            _info = info;

			_NumCirculatingReserveCoins = _totalTokenSupply - _bankBox.assets.Where(x => x.tokenId == info.tokenId).First().amount;
        }

		public Box<long> GetBox()
		{
			return _bankBox;
		}

		public long GetAmountBaseInBox(Box<long> box)
		{
            return box.value;
        }

		/// The number of ReserveCoins currently minted. In other words the number
		/// currently in circulation. Held in R4 of Bank box.
		public long NumCirculatingReserveCoins()
		{
			return _NumCirculatingReserveCoins;
		}


        /// Provides the base(Erg) reserves of the Bank. This is the total amount
        /// of nanoErgs held inside, minus the minimum box value required for
        /// posting a box on-chain <summary>
        /// Provides the base(Erg) reserves of the Bank. This is the total amount
		/// 
        /// this already exclused the accumulated dev fee!
        public long BaseReserves()
		{
			var value = _bankBox.value;
			if (value < Parameters.MIN_BOX_VALUE) return 0;

            return value;
		}

        /// Current ReserveCoin nominal price with precise factor of PRECISE_FACTOR
        public long ReserveCoinNominalPrice()
		{
			if (NumCirculatingReserveCoins() <= 0) return _precisionFactor;
            return (long)((BigInteger)BaseReserves() * _precisionFactor) / NumCirculatingReserveCoins();
		}

		/// The total amount of nanoErgs which is needed to cover minting
		/// the provided number of ReserveCoins, cover tx fees, implementor
		/// fee, etc.
		public long BaseCostToMintReserveCoin(long amountToMint)
		{
			long feelessCost = (long)((ReserveCoinNominalPrice() * amountToMint) / _precisionFactor);
			var protocolFee = 0;
			return feelessCost + protocolFee;
		}

		public long CalculateDevFee(long baseCost)
		{
            var devFee = (long)((double)baseCost * _devFeeNum / _info.feeDenom);
			return devFee;
        }

		/// The total amount of nanoErgs which is needed to cover minting
		/// the provided number of ReserveCoins, cover tx fees, implementor
		/// fee, etc.
		public long TotalCostToMintReserveCoin(long amountToMint, long txFee)
		{
			var baseCost = BaseCostToMintReserveCoin(amountToMint);
            return (long)(baseCost + txFee);
		}

        public long BaseAmountFromRedeemingReserveCoinWithoutProtocolFee(long amountToRedeem)
        {
            var feelessAmount = (long)((ReserveCoinNominalPrice() * amountToRedeem) / _precisionFactor);

            return feelessAmount;
        }

        public long BaseAmountFromRedeemingReserveCoin(long amountToRedeem)
		{
			var feelessAmount = BaseAmountFromRedeemingReserveCoinWithoutProtocolFee(amountToRedeem);
			var protocolFee = feelessAmount * _bankFeeNum / _info.feeDenom;

			return (long)(feelessAmount - protocolFee);
		}

        public long TotalAmountFromRedeemingReserveCoin(long amountToRedeem, long txFee)
        {
            var feelessAmount = BaseAmountFromRedeemingReserveCoinWithoutProtocolFee(amountToRedeem);
            var baseCost = BaseAmountFromRedeemingReserveCoin(amountToRedeem);

			var devFee = CalculateDevFee(feelessAmount);

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
			var feelessAmount = (long)((ReserveCoinNominalPrice() * amountToRedeem) / _precisionFactor);
			var protocolFee = feelessAmount * _bankFeeNum / _info.feeDenom;
            var devFee = CalculateDevFee(BaseAmountFromRedeemingReserveCoin(amountToRedeem));

            return (protocolFee + txFee + devFee);
		}

		//Methods for creating the candicate
		/// Create an `OutputBuilder` for the output Bank box for the
		/// `Mint ReserveCoin` action
		public OutputBuilder CreateMintReserveCoinCandidate(Box<long> inputBankBox, long amountToMint, long circulatingReservecoinsOut, long reservecoinValueInBase, long totalDevFee)
		{

            //ERG base
            var nftToken = inputBankBox.assets[0];
            var bankReservecoinTokenIn = inputBankBox.assets[1];

			var reservecoinTokens = new TokenAmount<long> { tokenId = _info.tokenId, amount = (long)(bankReservecoinTokenIn.amount - amountToMint) };
			var obbTokens = new List<TokenAmount<long>> { nftToken, reservecoinTokens };

            //Preserve registers
			var registers = new NonMandatoryRegisters {
                R4 = SConstant(SLong(_totalTokenSupply)),
                R5 = SConstant(SLong(_precisionFactor)),
                R6 = SConstant(SLong(_minBankValue)),
                R7 = SConstant(SLong(_devFeeNum)),
                R8 = SConstant(SLong(_bankFeeNum)),
            };

			var outputBankCandidate = new OutputBuilder((long)(inputBankBox.value + reservecoinValueInBase), ErgoAddress.fromErgoTree(_ergoTree, Parameters.NETWORK))
				.AddTokens(obbTokens)
				.SetAdditionalRegisters(registers);

			return outputBankCandidate;
        }

		public OutputBuilder CreateRedeemReserveCoinCandidate(Box<long> inputBankBox, long amountToRedeem, long circulatingReservecoinsOut, long reservecoinValueInBase)
		{
            var nftToken = inputBankBox.assets[0];
            var bankReservecoinTokenIn = inputBankBox.assets[1];

			var reservecoinTokens = new TokenAmount<long> { tokenId = _info.tokenId, amount = (long)(bankReservecoinTokenIn.amount + amountToRedeem) };
			var obbTokens = new List<TokenAmount<long>> { nftToken, reservecoinTokens };

            //Preserve registers
            var registers = new NonMandatoryRegisters
            {
                R4 = SConstant(SLong(_totalTokenSupply)),
                R5 = SConstant(SLong(_precisionFactor)),
                R6 = SConstant(SLong(_minBankValue)),
                R7 = SConstant(SLong(_devFeeNum)),
                R8 = SConstant(SLong(_bankFeeNum)),
            };

            var outputBankCandidate = new OutputBuilder((long)(inputBankBox.value - reservecoinValueInBase), ErgoAddress.fromErgoTree(_ergoTree, Parameters.NETWORK))
				.AddTokens(obbTokens)
				.SetAdditionalRegisters(registers);

			return outputBankCandidate;
		}
    }
}
