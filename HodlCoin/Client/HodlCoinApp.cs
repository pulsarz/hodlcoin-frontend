using FleetSharp.Builder;
using FleetSharp.Types;
using FleetSharp;
using HodlCoin.Client.HodlCoinImpl;
using System.Text.Json;

namespace HodlCoin.Client
{
	public static class HodlCoinApp
	{
        //Currently node is needed for mempool.
        public static async Task<Box<long>?> GetLastHodlCoinBankBox(NodeInterface node, Explorer explorer, HodlTokenInfo info)
        {
            //Check if any boxes exist in mempool.
            var boxesInMempool = await node.GetBoxesFromMempoolByTokenId(info.bankNFTTokenId);

            if (boxesInMempool != null && boxesInMempool.Count > 0)
            {
                Console.WriteLine($"Found {boxesInMempool.Count} bank boxes in mempool");
                return boxesInMempool.Last();
            }

            var lastBox = (await explorer.GetUnspentBoxesByTokenId(info.bankNFTTokenId))?.FirstOrDefault();

            return lastBox;
        }

        public static TransactionBuilder ActionMintHodlCoin(HodlTokenInfo info, List<Box<long>> ergsBoxes, HodlErgoBankBox bankBox, long amountToMint, ErgoAddress userAddress, long txFee, long currentHeight, ErgoAddress implementorAddress)
		{
			// Total ergs inside of `ergs_boxes`
			var inputErgsTotal = ergsBoxes.Sum(x => x.value);

			var circulatingReserveCoinsIn = bankBox.NumCirculatingReserveCoins();
			var reservecoinValueInBase = (long)bankBox.BaseCostToMintReserveCoin(amountToMint);

			var circulatingReservecoinsOut = circulatingReserveCoinsIn + amountToMint;

			if (amountToMint == 0)
			{
				throw new Exception("You must mint at least 0.000000001 hodlCoin.");
			}

			if (ergsBoxes.Count == 0)
			{
				throw new Exception("Insufficient number of boxes!");
			}

			// Verify that the provided ergs_boxes hold sufficient nanoErgs to
			// cover the minting, the tx fee, and to have MIN_BOX_VALUE in the
			// Receipt box.
			if (inputErgsTotal < (reservecoinValueInBase + txFee + Parameters.MIN_BOX_VALUE))
			{
				throw new Exception($"Insufficient nanoErgs input! Need > {reservecoinValueInBase} nanoErgs!");
			}

			//Setting up the output boxes
			var outputBankCandidate = bankBox.CreateMintReserveCoinCandidate(bankBox.GetBox(), amountToMint, circulatingReservecoinsOut, reservecoinValueInBase, 0L);

			// Create the Receipt box candidate
			var receiptBoxCandidate = HodlErgoReceiptBox.CreateMintReserveCoinCandidate(info, bankBox.GetBox(), amountToMint, userAddress, reservecoinValueInBase);

			var forceInclusionInput = new List<Box<long>> { bankBox.GetBox() };

			var outputs = new List<OutputBuilder> {
				outputBankCandidate,
				receiptBoxCandidate
			};

			var txBuilder = new TransactionBuilder(currentHeight)
			.from(ergsBoxes)
			.fromForcedInclusion(forceInclusionInput)
			.to(outputs)
			.sendChangeTo(userAddress)
			.payFee(txFee);

			return txBuilder;
		}

        public static TransactionBuilder ActionRedeemHodlCoin(HodlTokenInfo info, List<Box<long>> ergsBoxes, HodlErgoBankBox bankBox, long amountToRedeem, ErgoAddress userAddress, long txFee, long currentHeight)
        {
            var rcBoxes = ergsBoxes.Where(x => x.assets.Exists(y => y.tokenId == info.tokenId)).ToList();

            var inputReservecoinsTotal = rcBoxes.SelectMany(x => x.assets).Where(x => x.tokenId == info.tokenId).Sum(x => x.amount);

            var circulatingReservecoinsIn = bankBox.NumCirculatingReserveCoins();

            var reservecoinValueInBaseWithoutProtocolFee = bankBox.BaseAmountFromRedeemingReserveCoinWithoutProtocolFee(amountToRedeem);
            var reservecoinValueInBase = bankBox.BaseAmountFromRedeemingReserveCoin(amountToRedeem);

            //in new contract revision, it is calculated on total amount before protocol fee
            var devFee = bankBox.CalculateDevFee(reservecoinValueInBaseWithoutProtocolFee);

            if (circulatingReservecoinsIn < amountToRedeem)
            {
                throw new Exception("Insufficient reservecoins in the bank!");
            }

            var circulatingReservecoinsOut = circulatingReservecoinsIn - amountToRedeem;


            if (amountToRedeem == 0)
            {
                throw new Exception("You must redeem at least 0.000000001 ReserveCoin.");
            }

            if (rcBoxes.Count == 0)
            {
                throw new Exception("Insufficient number of boxes!");
            }

            if (inputReservecoinsTotal < amountToRedeem)
            {
                throw new Exception("Insufficient reservecoins in inputs!");
            }


            //Setting up the output boxes
            var outputBankCandidate = bankBox.CreateRedeemReserveCoinCandidate(bankBox.GetBox(), amountToRedeem, circulatingReservecoinsOut, reservecoinValueInBase);

            // Create the Receipt box candidate
            var receiptBoxCandidate = HodlErgoReceiptBox.CreateRedeemReserveCoinCandidate(info, bankBox.GetBox(), rcBoxes, amountToRedeem, userAddress, txFee, reservecoinValueInBase, devFee);

            var devFeeBoxCandidate = HodlErgoFeeBox.CreateFeeBoxCandidate(devFee);

            var outputs = new List<OutputBuilder> {
                outputBankCandidate,
                receiptBoxCandidate,
                devFeeBoxCandidate
            };

            var forceInclusionInput = new List<Box<long>> { bankBox.GetBox() };

            var txBuilder = new TransactionBuilder(currentHeight)
            .from(ergsBoxes)
            .fromForcedInclusion(forceInclusionInput)
            .to(outputs)
            .sendChangeTo(userAddress)
            .payFee(txFee);

            return txBuilder;
        }
    }
}
