using Blazored.LocalStorage;
using FleetSharp;
using FleetSharp.Types;
using HodlCoin.Client.HodlCoinImpl;
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

    public class SafewTokenAmount<AmountType>
    {
        public string tokenId { get; set; }
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public AmountType amount { get; set; }
    }

    public class SafewBox<AmountType>
    {
        public string boxId { get; set; }
        public string transactionId { get; set; }
        public int index { get; set; }
        public string ergoTree { get; set; }
        public long creationHeight { get; set; }
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public AmountType value { get; set; }
        public List<SafewTokenAmount<AmountType>> assets { get; set; }
        public NonMandatoryRegisters additionalRegisters { get; set; }
    }

    public class Wallet
    {
        public static async Task<bool> IsWalletConnected(IJSRuntime JS, ILocalStorageService localStorage)
        {
            var connectedWallet = await localStorage.GetItemAsync<string>("connectedWallet");
            if (connectedWallet != null && connectedWallet != "")
            {
                await ConnectWallet(JS, localStorage, connectedWallet);
                return await JS.InvokeAsync<bool>("isWalletConnected", connectedWallet);
            }
            return false;
        }

        public static async Task<bool> ConnectWallet(IJSRuntime JS, ILocalStorageService localStorage, string wallet)
        {
            var ret = await JS.InvokeAsync<bool>("connectWallet", wallet);
            if (ret)
            {
                await localStorage.SetItemAsync("connectedWallet", wallet);
            }
            return ret;
        }

        public static async Task<bool> DisconnectWallet(IJSRuntime JS, ILocalStorageService localStorage)
        {
            var connectedWallet = await localStorage.GetItemAsync<string>("connectedWallet");

            var ret = await JS.InvokeAsync<bool>("disconnectWallet", connectedWallet);
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

        public static async Task<List<Box<long>>?> GetUtxos(IJSRuntime JS, ILocalStorageService localStorage, long? amount = null, string? tokenId = null)
		{
            string? amountStr = null;
            if (amount != null) amountStr = amount.ToString();
            var connectedWallet = await localStorage.GetItemAsync<string>("connectedWallet");

            if (connectedWallet == "safew")
            {
                try
                {
                    var ret = await JS.InvokeAsync<List<SafewBox<long>>?>("getUtxos", amount, tokenId);
                    Console.WriteLine(JsonSerializer.Serialize(ret));

                    return ret.Select(x => new Box<long>
                    {
                        boxId = x.boxId,
                        additionalRegisters = x.additionalRegisters,
                        creationHeight = x.creationHeight,
                        ergoTree = x.ergoTree,
                        index = x.index,
                        transactionId = x.transactionId,
                        value = x.value,
                        assets = x.assets.Select(y => new TokenAmount<long> { tokenId = y.tokenId, amount = y.amount }).ToList()
                    }).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return null;
                }
            }
            else
            {
                try
                {
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
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return null;
                }
            }
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

        //Uses the address list from nautilus but uses explorer directly to get the balance. Since nautilus only updates balances on opening the wallet.
        public static async Task<long> GetExplorerBalance(IJSRuntime JS, string tokenId = "ERG")
        {
            List<string> addresses = await Wallet.GetWalletAddressList(JS);
            Console.WriteLine($"All wallet addresses: {JsonSerializer.Serialize(addresses)}");

            //use explorer directly instead of wallet since wallet balance does not update until you reopen nautilus for some reason (well, use node interface).
            var balances = await Config.explorer.GetAddressesBalances(addresses);
            Console.WriteLine($"All wallet addresses balances: {JsonSerializer.Serialize(balances)}");

            if (tokenId == "ERG")
            {
                return balances.Sum(x => x.confirmed?.nanoErgs ?? 0);
            }
            else
            {
                var flatTokens = balances.Where(x => x.confirmed != null && x.confirmed.tokens != null).SelectMany(x => x.confirmed.tokens).ToList();
                return flatTokens.Where(x => x.tokenId == tokenId).Sum(x => x.amount);
            }
        }
    }
}
