﻿using FleetSharp.Builder;
using FleetSharp.Types;
using FleetSharp;
using static FleetSharp.Sigma.ConstantSerializer;
using static FleetSharp.Sigma.ISigmaCollection;
using static FleetSharp.Sigma.IPrimitiveSigmaType;


namespace HodlCoin.Client.HodlCoinImpl
{
    public static class HodlErgoFeeBox
    {
        public static OutputBuilder CreateFeeBoxCandidate(HodlTokenInfo info, long amount)
        {
            if (info.baseTokenId == "0000000000000000000000000000000000000000000000000000000000000000")
            {
                return new OutputBuilder(amount, Parameters.DEV_FEE_ADDRESS);
            }
            else
            {
                return new OutputBuilder(Parameters.MIN_BOX_VALUE, Parameters.DEV_FEE_ADDRESS)
                    .AddToken(new TokenAmount<long> { tokenId = info.baseTokenId, amount = amount });
            }
        }
    }
}
