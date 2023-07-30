using FleetSharp.Builder;
using FleetSharp.Types;
using FleetSharp;
using static FleetSharp.Sigma.ConstantSerializer;
using static FleetSharp.Sigma.ISigmaCollection;
using static FleetSharp.Sigma.IPrimitiveSigmaType;


namespace HodlCoin.Client.HodlCoinImpl
{
    public static class HodlErgoFeeBox
    {
        public static OutputBuilder CreateFeeBoxCandidate(long amount)
        {
            var candidate = new OutputBuilder(amount, Parameters.DEV_FEE_ADDRESS);
            return candidate;
        }
    }
}
