using FleetSharp.Builder;
using FleetSharp;
using FleetSharp.Types;
using System.Xml.Linq;
using System;
using static FleetSharp.Sigma.ConstantSerializer;
using static FleetSharp.Sigma.ISigmaCollection;
using static FleetSharp.Sigma.IPrimitiveSigmaType;

namespace HodlCoin.Client.HodlCoinImpl
{/*
	public static class Initialize
	{
		public static TransactionBuilder BuildMintBankNFT(List<Box<long>> unspentWalletBoxes, long currentHeight, ErgoAddress changeAddress, string name, string description)
		{
			return BuildTokenMintingTX(unspentWalletBoxes, currentHeight, changeAddress, 1, name, 0, description);
		}

		public static TransactionBuilder BuildMintHodlToken(List<Box<long>> unspentWalletBoxes, long currentHeight, ErgoAddress changeAddress, long amount, string name, int decimals, string description)
		{
			return BuildTokenMintingTX(unspentWalletBoxes, currentHeight, changeAddress, amount, name, decimals, description);
		}

        //This also burns 1 full hodlcoin to make sure circ supply never goes to 0. Also requires 1 erg.
        //for erg boxValue = amtToBurn
        public static TransactionBuilder BuildInitialBankBox(List<Box<long>> unspentWalletBoxes, long currentHeight, ErgoAddress changeAddress, string contractAddress, string bankId, string tokenId, long maxSupply, long amtToBurn, long boxValue)
        {
			//var amtToBurn = 1000000000L;//this assumes 1 hodlcoin = 1 erg for box value

            return new TransactionBuilder(currentHeight)
                .from(unspentWalletBoxes)
                .to(new List<OutputBuilder>
                {
                    new OutputBuilder(boxValue, ErgoAddress.fromErgoTree(ErgoAddress.fromBase58(contractAddress).GetErgoTreeHex(), Parameters.NETWORK))
                        .AddTokens(new List<TokenAmount<long>> {
                            new TokenAmount<long> { tokenId = tokenId, amount = maxSupply - amtToBurn },
                            new TokenAmount<long> { tokenId = bankId, amount = 1 }
                        })
						.SetAdditionalRegisters(new NonMandatoryRegisters {
							R4 = SConstant(SLong(0))//dev fee starts at 0
                        })
                })
				.burnTokens(new List<TokenAmount<long>> { new TokenAmount<long> { tokenId = tokenId, amount = amtToBurn } })
                .sendChangeTo(changeAddress)
                .payMinFee();
        }

        public static TransactionBuilder BuildTokenMintingTX(List<Box<long>> unspentWalletBoxes, long currentHeight, ErgoAddress changeAddress, long amount, string name, int decimals, string description)
		{
			return new TransactionBuilder(currentHeight)
				.from(unspentWalletBoxes)
				.to(new List<OutputBuilder>
				{
					new OutputBuilder(OutputBuilder.SAFE_MIN_BOX_VALUE, changeAddress)
						.mintToken(new NewToken<long>
						{
							amount = amount,
							name = name,
							decimals = decimals,
							description = description
						})
				})
				.sendChangeTo(changeAddress)
				.payMinFee();
		}
	}*/
}
