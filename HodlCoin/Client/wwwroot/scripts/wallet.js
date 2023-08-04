function promiseTimeout(ms, promise) {
    // Create a promise that rejects in <ms> milliseconds
    let timeout = new Promise((resolve, reject) => {
        let id = setTimeout(() => {
            clearTimeout(id);
            reject('Timed out in ' + ms + 'ms.')
        }, ms)
    })
    // Returns a race between our timeout and the passed in promise
    return Promise.race([
        promise,
        timeout
    ])
}

function hasExtensionConnector() {
    if ((typeof window.ergoConnector.nautilus !== 'undefined') && (typeof window.ergoConnector.nautilus.isConnected !== 'undefined')) {
        return true;
    } else if ((typeof window.ergoConnector.safew !== 'undefined') && (typeof window.ergoConnector.safew.isConnected !== 'undefined')) {
            return true;
    } else {
        return false;
    }
}

function hasConnectorInjected() {
    if (typeof ergo !== 'undefined') {
        return true;
    } else {
        return false;
    }
}

async function isWalletConnected(wallet) {
    if (hasExtensionConnector()) {
        try {
            if (wallet == "safew" && (typeof window.ergoConnector.safew !== 'undefined') && (typeof window.ergoConnector.safew.isConnected !== 'undefined')) {
                const res = await ergoConnector.safew.isConnected();
                return Promise.resolve(res);
            }
            else {
                const res = await ergoConnector.nautilus.isConnected();
                return Promise.resolve(res);
            }
            
        } catch (e) {
            console.error("isWalletConnected error", e);
            //errorAlert("dApp connector not found 1", "Install Nautilus or SAFEW wallet in your browser");
            return Promise.resolve(false);
        }
    } else {
        return Promise.resolve(true);
    }
}

// Connect to browser extension wallet, return True if success
async function connectWallet(wallet) {
    if (hasExtensionConnector()) {
        try {
            const alreadyConnected = await isWalletConnected();
            //console.log("connectWallet alreadyConnected", alreadyConnected);
            if (!alreadyConnected) {
                await sleep(100)
                var res = false;
                if (wallet == "safew" && (typeof window.ergoConnector.safew !== 'undefined') && (typeof window.ergoConnector.safew.isConnected !== 'undefined')) {
                    res = await window.ergoConnector.safew.connect();
                }
                else {
                    res = await window.ergoConnector.nautilus.connect();
                }

                await sleep(100); // need to fix SAFEW to remove this wait...
                if (res) {
                    const currentAddress = localStorage.getItem('address') ?? '';
                    const walletAddressList = await getWalletAddressList();
                    if (currentAddress === '' && walletAddressList && walletAddressList.length > 0) {
                        localStorage.setItem('address', walletAddressList[0])
                    }
                }
                return res
            } else {
                return true;
            }
        } catch (e) {
            console.error(e);
            alert("dApp connector not found 2", "Install Nautilus or SAFEW wallet in your browser");
            return false;
        }
    } else {
        return true;
    }

}

async function disconnectWallet(wallet) {
    //console.log("disconnectWallet");
    if (typeof window.ergoConnector !== 'undefined') {
        if (wallet == "safew" && typeof window.ergoConnector.safew !== 'undefined') {
            return await window.ergoConnector.safew.disconnect();
        }
        if (typeof window.ergoConnector.nautilus !== 'undefined') {
            return await window.ergoConnector.nautilus.disconnect();
        }
        return false;
    } else {
        return true;
    }
}

// Check the address is in the connected wallet
async function isValidWalletAddress(address) {
    //console.log("isValidWalletAddress", address);
    if (hasExtensionConnector()) {
        const walletConnected = await isWalletConnected();
        if (walletConnected && hasConnectorInjected()) {
            const address_list = await ergo.get_used_addresses();
            return address_list.includes(address);
        } else {
            return false;
        }
    } else {
        return true;
    }
}

async function getWalletAddressList() {
    //console.log("getWalletAddressList");
    if (hasExtensionConnector()) {
        const walletConnected = await isWalletConnected();
        if (walletConnected && hasConnectorInjected()) {
            const address_list = await ergo.get_used_addresses();
            return address_list;
        } else {
            return [];
        }
    } else {
        return [];
    }
}

async function getBalance(tokenId = 'ERG') {
    console.log('getBalance', tokenId);
    const walletConnected = await connectWallet();
    console.log('getBalance2', walletConnected, hasConnectorInjected());
    if (walletConnected && hasConnectorInjected()) {
        console.log('getBalance3', walletConnected, tokenId);
        const amount = await ergo.get_balance(tokenId);
        return amount;
    } else {
        console.log('getBalance4', walletConnected);
        const address = localStorage.getItem('address') ?? '';
        if (address !== '') {
            const balance = await getBalanceForAddress(address);
            if (balance.confirmed) {
                if (tokenId === 'ERG') {
                    return balance.confirmed.nanoErgs + balance.unconfirmed.nanoErgs;
                } else {
                    var tokenAmount = 0;
                    for (const tok of balance.confirmed.tokens) {
                        if (tok.tokenId === tokenId) {
                            tokenAmount = tok.amount;
                        }
                    }
                    for (const tok of balance.unconfirmed.tokens) {
                        if (tok.tokenId === tokenId) {
                            tokenAmount = tokenAmount + tok.amount;
                        }
                    }
                    return tokenAmount;
                }
            }
        }
        return 0;
    }
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function getUtxos(amount, tokenId) {
    const walletAccessGranted = await connectWallet();
    if (walletAccessGranted && hasExtensionConnector()) {
        const utxos = await ergo.get_utxos(amount, tokenId);
        return utxos;
    } else {
        return null;
    }
}

async function getChangeAddress() {
    try {
        console.log("getChangeAddress");
        return await ergo.get_change_address();
    } catch (e) {
        console.log(e);
        return null;
    }
}
/*Does not work with safew*/
async function getCurrentHeight() {
    try {
        console.log("getCurrentHeight");
        return await ergo.get_current_height();
    } catch (e) {
        console.log(e);
        return null;
    }
}

async function signTx(txToBeSignedSerialized) {
    var txToBeSigned = JSON.parse(txToBeSignedSerialized);
    try {
        console.log("signTx", txToBeSigned);
        return await ergo.sign_tx(txToBeSigned);
    } catch (e) {
        console.log(e);
        return null;
    }
}

async function submitTx(txToBeSubmittedSerialized) {
    var txToBeSubmitted = JSON.parse(txToBeSubmittedSerialized);
    try {
        console.log("submitTx");
        const res = await promiseTimeout(30000, ergo.submit_tx(txToBeSubmitted));
        return res;
    } catch (e) {
        console.log(e);
        return null;
    }
}

async function processTx(txToBeProcessedSerialized) {
    var txToBeProcessed = JSON.parse(txToBeProcessedSerialized);
    const walletAccessGranted = await connectWallet();
    if (walletAccessGranted && hasExtensionConnector()) {
        const msg = s => {
            console.log('[processTx]', s);
        };
        console.log("signTx", txToBeProcessed);
        const signedTx = await ergo.sign_tx(txToBeProcessed);
        if (!signedTx) {
            console.error(`No signed transaction found`);
            return null;
        }
        msg("Transaction signed - awaiting submission");
        console.log("submitTx");
        const txId = await promiseTimeout(30000, ergo.submit_tx(signedTx));
        //const txId = await postTxMempool(signedTx);
        if (!txId) {
            console.log(`No submitted tx ID`);
            return null;
        }
        msg("Transaction submitted ");
        return txId;
    } else {
        /*const [txId, ergoPayTx] = await getTxReducedB64Safe(txToBeProcessed, txToBeProcessed.inputs, txToBeProcessed.dataInputs);
        displayErgoPayTransaction(txId, ergoPayTx);
        return txId;*/
        return null;
    }
}