using Blazored.LocalStorage;
using FleetSharp;
using FleetSharp.Types;
using Microsoft.JSInterop;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HodlCoin.Client
{
	public class SignedTransactionString
	{
		public string id { get; set; }
		public List<SignedInput> inputs { get; set; }
		public List<DataInput> dataInputs { get; set; }
		public List<Box<string>> outputs { get; set; }
		public long size { get; set; }
	}

	public class Wallet
    {
        public static async Task<bool> IsWalletConnected(IJSRuntime JS, ILocalStorageService localStorage)
        {
            var connectedWallet = await localStorage.GetItemAsync<string>("connectedWallet");
            if (connectedWallet != null && connectedWallet != "")
            {
                await ConnectWallet(JS, localStorage);
            }
            return await JS.InvokeAsync<bool>("isWalletConnected");
        }

        public static async Task<bool> ConnectWallet(IJSRuntime JS, ILocalStorageService localStorage)
        {
            var ret = await JS.InvokeAsync<bool>("connectWallet");
            if (ret)
            {
                await localStorage.SetItemAsync("connectedWallet", "nautilus");
            }
            return ret;
        }

        public static async Task<bool> DisconnectWallet(IJSRuntime JS, ILocalStorageService localStorage)
        {
            var ret = await JS.InvokeAsync<bool>("disconnectWallet");
            if (ret) await localStorage.RemoveItemAsync("connectedWallet");
            return ret;
        }

        public static async Task<bool> IsValidWalletAddress(IJSRuntime JS, string address)
        {
            return await JS.InvokeAsync<bool>("isValidWalletAddress", address);
        }

        public static async Task<List<string>> GetWalletAddressList(IJSRuntime JS)
        {
            return await JS.InvokeAsync<List<string>>("getWalletAddressList");
        }

        public static async Task<long> GetBalance(IJSRuntime JS, string tokenId = "ERG")
        {
            return long.Parse(await JS.InvokeAsync<string>("getBalance", tokenId));
        }

		public static async Task<string> GetChangeAddress(IJSRuntime JS)
		{
			return await JS.InvokeAsync<string>("getChangeAddress");
		}

		public static async Task<int> GetCurrentHeight(IJSRuntime JS)
		{
			return await JS.InvokeAsync<int>("getCurrentHeight");
		}

		public static async Task<List<Box<long>>?> GetUtxos(IJSRuntime JS, long? amount = null, string? tokenId = null)
		{
            string? amountStr = null;
            if (amount != null) amountStr = amount.ToString();

			var ret = await JS.InvokeAsync<List<Box<string>>?>("getUtxos", amount, tokenId);
            if (ret == null) return null;
            return ret.Select(x => new Box<long>
            {
                boxId = x.boxId,
                additionalRegisters = x.additionalRegisters,
                creationHeight = x.creationHeight,
                ergoTree = x.ergoTree,
                index = x.index,
                transactionId = x.transactionId,
                value = long.Parse(x.value),
                assets = x.assets.Select(y => new TokenAmount<long> { tokenId = y.tokenId, amount = long.Parse(y.amount) }).ToList()
			}).ToList();
		}

		public static async Task<SignedTransactionString> SignTX(IJSRuntime JS, EIP12UnsignedTransaction unsignedTX)
        {
            //serialize object ourselves otherwise the casing is changed.. lmao good design choice MS.
            return await JS.InvokeAsync<SignedTransactionString>("signTx", JsonSerializer.Serialize(unsignedTX, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }));
        }

        public static async Task<string?> SubmitTX(IJSRuntime JS, SignedTransactionString signedTX)
        {
			//serialize object ourselves otherwise the casing is changed.. lmao good design choice MS.
			return await JS.InvokeAsync<string?>("submitTx", JsonSerializer.Serialize(signedTX, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }));
        }

        public static async Task<string?> ProcessTX(IJSRuntime JS, EIP12UnsignedTransaction unsignedTX)
        {
			//serialize object ourselves otherwise the casing is changed.. lmao good design choice MS.
			return await JS.InvokeAsync<string?>("processTx", JsonSerializer.Serialize(unsignedTX, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }));
        }
    }
}
