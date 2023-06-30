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
        public static TokenAmount<long> NewReserveCoinToken(HodlTokenInfo info, long amount)
        {
            return new TokenAmount<long> { tokenId = info.tokenId, amount = amount };
        }

        public static OutputBuilder CreateMintReserveCoinCandidate(HodlTokenInfo info, Box<long> bankBox, long amountToMint, ErgoAddress userAddress, long reservecoinValueInBase)
        {
            var rbReservecoinToken = NewReserveCoinToken(info, amountToMint);
            var rbTokens = new List<TokenAmount<long>> { rbReservecoinToken };

            //Create the receipt box candidate
            var candidate = new OutputBuilder(Parameters.MIN_BOX_VALUE, userAddress)
                .AddTokens(rbTokens);

            return candidate;
        }

        public static OutputBuilder CreateRedeemReserveCoinCandidate(HodlTokenInfo info, Box<long> bankBox, List<Box<long>> rcBoxes, long amountToRedeem, ErgoAddress userAddress, long txFee, long reservecoinValueInbase, long devFee)
        {
            // Find how many ReserveCoin are inside of the ReserveCoin boxes
            var rcBoxesTotalRC = rcBoxes.Sum(x => x.assets.Where(y => y.tokenId == info.tokenId).Sum(y => y.amount));

            // The amount of nanoErgs in the rc_boxes + the value of the
            // ReserveCoins being redeemed - the transaction fee
            var rbValue = reservecoinValueInbase - txFee - devFee;
            if (rbValue < Parameters.MIN_BOX_VALUE) rbValue = Parameters.MIN_BOX_VALUE;
            //Define the tokens
            var rbTokens = new List<TokenAmount<long>>();

            // Check if there are any extra tokens that aren't being redeemed
            // and include them in the output
            if (rcBoxesTotalRC > amountToRedeem)
            {
                var amount = rcBoxesTotalRC - amountToRedeem;
                var newRCToken = NewReserveCoinToken(info, amount);
                rbTokens.Add(newRCToken);
            }

            //Create the candidate
            var candidate = new OutputBuilder(rbValue, userAddress)
                .AddTokens(rbTokens);

            return candidate;
        }
    }
}
