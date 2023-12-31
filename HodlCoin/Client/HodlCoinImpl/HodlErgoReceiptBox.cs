﻿using FleetSharp.Builder;
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

            //Define the tokens
            var outputTokens = new List<TokenAmount<long>>();
            long receiptBoxValue = 0;

            if (info.baseTokenId == "0000000000000000000000000000000000000000000000000000000000000000")
            {
                receiptBoxValue = reservecoinValueInbase - txFee - devFee;
                if (receiptBoxValue < Parameters.MIN_BOX_VALUE) receiptBoxValue = Parameters.MIN_BOX_VALUE;
            }
            else
            {
                receiptBoxValue = Parameters.MIN_BOX_VALUE;
                outputTokens.Add(new TokenAmount<long> { tokenId = info.baseTokenId, amount = reservecoinValueInbase - devFee });
            }

            //Create the candidate
            var candidate = new OutputBuilder(receiptBoxValue, userAddress)
                .AddTokens(outputTokens);

            return candidate;
        }
    }
}
