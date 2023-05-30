using FleetSharp.Builder;
using FleetSharp.Types;
using FleetSharp;
using static FleetSharp.Sigma.ConstantSerializer;
using static FleetSharp.Sigma.ISigmaCollection;
using static FleetSharp.Sigma.IPrimitiveSigmaType;


namespace HodlCoin.Client.HodlCoinImpl
{
    public static class HodlErgoReceiptBox
    {
        public static TokenAmount<long> NewReserveCoinToken(long amount)
        {
            return new TokenAmount<long> { tokenId = Parameters.HODLCOIN_TOKEN_ID, amount = amount };
        }

        public static OutputBuilder CreateMintReserveCoinCandidate(Box<long> bankBox, long amountToMint, ErgoAddress userAddress, long reservecoinValueInBase)
        {
            var rbReservecoinToken = NewReserveCoinToken(amountToMint);
            var rbTokens = new List<TokenAmount<long>> { rbReservecoinToken };

            var registers = new NonMandatoryRegisters { R4 = SConstant(SLong(amountToMint)), R5 = SConstant(SLong(reservecoinValueInBase)) };

            //Create the receipt box candidate
            var candidate = new OutputBuilder(Parameters.MIN_BOX_VALUE, userAddress)
                .AddTokens(rbTokens)
                .SetAdditionalRegisters(registers);

            return candidate;
        }

        public static OutputBuilder CreateRedeemReserveCoinCandidate(Box<long> bankBox, List<Box<long>> rcBoxes, long amountToRedeem, ErgoAddress userAddress, long txFee, long reservecoinValueInbase, long devFee)
        {
            // Find how many ReserveCoin are inside of the ReserveCoin boxes
            var rcBoxesTotalRC = rcBoxes.Sum(x => x.assets.Where(y => y.tokenId == Parameters.HODLCOIN_TOKEN_ID).Sum(y => y.amount));

            // The amount of nanoErgs in the rc_boxes + the value of the
            // ReserveCoins being redeemed - the transaction fee
            var rbValue = reservecoinValueInbase - txFee - devFee;
            if (rbValue < Parameters.MIN_BOX_VALUE) rbValue = Parameters.MIN_BOX_VALUE;

            // Specify the registers in the Receipt box
            var rbRegisters = new NonMandatoryRegisters { R4 = SConstant(SLong(0 - amountToRedeem)), R5 = SConstant(SLong(0 - reservecoinValueInbase)) };

            //Define the tokens
            var rbTokens = new List<TokenAmount<long>>();

            // Check if there are any extra tokens that aren't being redeemed
            // and include them in the output
            if (rcBoxesTotalRC > amountToRedeem)
            {
                var amount = rcBoxesTotalRC - amountToRedeem;
                var newRCToken = NewReserveCoinToken(amount);
                rbTokens.Add(newRCToken);
            }

            //Create the candidate
            var candidate = new OutputBuilder(rbValue, userAddress)
                .AddTokens(rbTokens)
                .SetAdditionalRegisters(rbRegisters);

            return candidate;
        }
    }
}
