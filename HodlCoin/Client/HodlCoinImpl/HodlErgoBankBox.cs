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
            if (_info.baseTokenId == "0000000000000000000000000000000000000000000000000000000000000000")
            {
                var value = _bankBox.value;
                if (value < Parameters.MIN_BOX_VALUE) return 0;
                return value;
            }
            else
            {
                return _bankBox.assets.Where(x => x.tokenId == _info.baseTokenId).First().amount;
            }
		}

        /// Current ReserveCoin nominal price with precise factor of PRECISE_FACTOR
        public BigInteger ReserveCoinNominalPrice()
		{
			if (NumCirculatingReserveCoins() <= 0) return _precisionFactor;
            return ((BigInteger)BaseReserves() * _precisionFactor / NumCirculatingReserveCoins());
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

        public BigInteger DivUp(BigInteger dividend, BigInteger divisor)
        {
            if (divisor == 0)
            {
                return -1;
            }
            else
            {
                return (dividend + (divisor - 1)) / divisor;
            }
        }

        public (long bankFee, long devFee) CalculateDevAndBankFee(long amountToRedeem)
        {
            var feelessAmount = BaseAmountFromRedeemingReserveCoinWithoutProtocolFee(amountToRedeem);

            if (_info.baseTokenId == "0000000000000000000000000000000000000000000000000000000000000000")
            {
                var bankFee = feelessAmount * _bankFeeNum / _info.feeDenom;
                var devFee = (feelessAmount * _devFeeNum / _info.feeDenom);

                return (bankFee, devFee);
            }
            else
            {
                var dividend_1 = (BigInteger)feelessAmount * ((BigInteger)_bankFeeNum + (BigInteger)_devFeeNum);
                var divisor_1 = (BigInteger)_info.feeDenom;

                var bankFeeAndDevFeeAmount = DivUp(dividend_1, divisor_1);

                var dividend_2 = bankFeeAndDevFeeAmount * (BigInteger)_devFeeNum;
                var divisor_2 = (BigInteger)_bankFeeNum + (BigInteger)_devFeeNum;

                var devFee = DivUp(dividend_2, divisor_2);
                var bankFee = bankFeeAndDevFeeAmount - devFee;

                var devFeeAmountAdjusted = (bankFee == 0) ? 0L : devFee;
                var bankFeeAmountAdjusted = (bankFee == 0) ? devFee : bankFee;

                return ((long)bankFeeAmountAdjusted, (long)devFeeAmountAdjusted);
            }
        }

		/// The total amount of nanoErgs which is needed to cover minting
		/// the provided number of ReserveCoins, cover tx fees, implementor
		/// fee, etc.
		public long TotalCostToMintReserveCoin(long amountToMint, long txFee)
		{
			var baseCost = BaseCostToMintReserveCoin(amountToMint);
            return (baseCost + txFee);
		}
        
        public long BaseAmountFromRedeemingReserveCoinWithoutProtocolFee(long amountToRedeem)
        {
            var feelessAmount = (long)((ReserveCoinNominalPrice() * amountToRedeem) / _precisionFactor);

            return feelessAmount;
        }
        
        public long BaseAmountFromRedeemingReserveCoin(long amountToRedeem)
		{
			var feelessAmount = BaseAmountFromRedeemingReserveCoinWithoutProtocolFee(amountToRedeem);
            var fees = CalculateDevAndBankFee(amountToRedeem);

			return (feelessAmount - fees.bankFee);
		}

        public long TotalAmountFromRedeemingReserveCoin(long amountToRedeem, long txFee)
        {
            var feelessAmount = BaseAmountFromRedeemingReserveCoinWithoutProtocolFee(amountToRedeem);
            var fees = CalculateDevAndBankFee(amountToRedeem);

            return (feelessAmount - txFee - fees.bankFee - fees.devFee);
        }

		//Methods for creating the candicate
		/// Create an `OutputBuilder` for the output Bank box for the
		/// `Mint ReserveCoin` action
		public OutputBuilder CreateMintReserveCoinCandidate(Box<long> inputBankBox, long amountToMint, long circulatingReservecoinsOut, long reservecoinValueInBase, long totalDevFee)
		{
            var nftToken = inputBankBox.assets[0];
            var bankReservecoinTokenIn = inputBankBox.assets[1];
            
            var reservecoinTokens = new TokenAmount<long> { tokenId = _info.tokenId, amount = (bankReservecoinTokenIn.amount - amountToMint) };
			var obbTokens = new List<TokenAmount<long>> { nftToken, reservecoinTokens };

            var outputBoxValue = 0L;

            if (_info.baseTokenId != "0000000000000000000000000000000000000000000000000000000000000000")
            {
                outputBoxValue = inputBankBox.value;

                var reserveIn = inputBankBox.assets[2];
                var reserveTokens = new TokenAmount<long> { tokenId = _info.baseTokenId, amount = (reserveIn.amount + reservecoinValueInBase) };
                obbTokens.Add(reserveTokens);
            }
            else
            {
                outputBoxValue = inputBankBox.value + reservecoinValueInBase;
            }

			var outputBankCandidate = new OutputBuilder(outputBoxValue, ErgoAddress.fromErgoTree(_ergoTree, Parameters.NETWORK))
				.AddTokens(obbTokens)
				.SetAdditionalRegisters(_bankBox.additionalRegisters);//Preserve registers

            return outputBankCandidate;
        }

		public OutputBuilder CreateRedeemReserveCoinCandidate(Box<long> inputBankBox, long amountToRedeem, long circulatingReservecoinsOut, long reservecoinValueInBase)
		{
            var nftToken = inputBankBox.assets[0];
            var bankReservecoinTokenIn = inputBankBox.assets[1];

			var reservecoinTokens = new TokenAmount<long> { tokenId = _info.tokenId, amount = (bankReservecoinTokenIn.amount + amountToRedeem) };
			var obbTokens = new List<TokenAmount<long>> { nftToken, reservecoinTokens };

            var outputBoxValue = 0L;

            if (_info.baseTokenId != "0000000000000000000000000000000000000000000000000000000000000000")
            {
                outputBoxValue = inputBankBox.value;

                var reserveIn = inputBankBox.assets[2];
                var reserveTokens = new TokenAmount<long> { tokenId = _info.baseTokenId, amount = (reserveIn.amount - reservecoinValueInBase) };
                obbTokens.Add(reserveTokens);
            }
            else
            {
                outputBoxValue = inputBankBox.value - reservecoinValueInBase;
            }

            var outputBankCandidate = new OutputBuilder(outputBoxValue, ErgoAddress.fromErgoTree(_ergoTree, Parameters.NETWORK))
				.AddTokens(obbTokens)
				.SetAdditionalRegisters(_bankBox.additionalRegisters);//Preserve registers

			return outputBankCandidate;
		}
    }
}
