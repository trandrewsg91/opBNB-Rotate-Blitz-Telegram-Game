using System.Collections;
using UnityEngine;
using Thirdweb;
using Thirdweb.Unity;
using TMPro;
using System.Numerics;
using System;
using UnityEngine.SceneManagement;

public class ConnectWalletManager : MonoBehaviour
{
    public TMP_Text logText;

    public string Address { get; private set; }
    public static BigInteger ChainId = 204;

    public UnityEngine.UI.Button playButton;
    public UnityEngine.UI.Button getBalanceButton;
    public UnityEngine.UI.Button rateButton;

    string customSmartContractAddress = "0xEd9C24E0b04e5cf3d2415efDC0755cBb6c22376B";
    string abiString = "[{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"player\",\"type\":\"address\"}],\"name\":\"DashPointsReset\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"player\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"totalDashPoints\",\"type\":\"uint256\"}],\"name\":\"DashPointsUpdated\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"player\",\"type\":\"address\"}],\"name\":\"getPlayerDashPoints\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"player\",\"type\":\"address\"}],\"name\":\"incrementDashPoints\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"player\",\"type\":\"address\"}],\"name\":\"resetDashPoints\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";

    string notEnoughToken = " BNB";

    private void Start()
    {
        logText.text = "Rate Our Game!";
    }

    public void SwitchToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void HideAllButtons()
    {
        playButton.interactable = false;
        getBalanceButton.interactable = false;
        rateButton.interactable = false;
    }

    private void ShowAllButtons()
    {
        playButton.interactable = true;
        getBalanceButton.interactable = true;
        rateButton.interactable = true;
    }

    private void UpdateStatus(string messageShow)
    {
        logText.text = messageShow;
    }

    private void BoughtSuccessFully()
    {
        UpdateStatus("Thanks for Your Rate");
    }
    IEnumerator WaitAndExecute()
    {
        Debug.Log("Coroutine started, waiting for 3 seconds...");
        yield return new WaitForSeconds(3f);
        Debug.Log("3 seconds have passed!");
        BoughtSuccessFully();
        ShowAllButtons();
    }

    private async void Claim1Token()
    {
        var wallet = ThirdwebManager.Instance.GetActiveWallet();
        var contract = await ThirdwebManager.Instance.GetContract(
           customSmartContractAddress,
           ChainId,
           abiString
       );
        var address = await wallet.GetAddress();
        await ThirdwebContract.Write(wallet, contract, "incrementDashPoints", 0, address);

        var result = ThirdwebContract.Read<int>(contract, "getPlayerDashPoints", address);
        Debug.Log("result: " + result);
    }

    public async void GetTokens()
    {
        HideAllButtons();
        UpdateStatus("Rating...");
        var wallet = ThirdwebManager.Instance.GetActiveWallet();
        var balance = await wallet.GetBalance(chainId: ChainId);
        var balanceEth = Utils.ToEth(wei: balance.ToString(), decimalsToDisplay: 4, addCommas: true);
        Debug.Log("balanceEth1: " + balanceEth);
        if (float.Parse(balanceEth) <= 0f)
        {
            UpdateStatus("Not Enough" + notEnoughToken);
            ShowAllButtons();
            return;
        }
        StartCoroutine(WaitAndExecute());
        try
        {
            Claim1Token();
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred during the transfer: {ex.Message}");
        }
    }
}
