using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using TMPro;
using UnityEngine;
using Unity.Services.Authentication.PlayerAccounts.Samples;
using System.Collections;

public class AuthenticationManager : MonoBehaviour
{
    [SerializeField ] PlayerAccountsDemo script_playerAccountDemo;
    [SerializeField ] SaveData script_saveData;
    [SerializeField] GameObject go_loginPanel;
    [SerializeField] GameObject go_startButton;
    public TextMeshProUGUI tmp_playerID;
    public static bool b_dataAlreadyLoadedFromCloud = false;

    [SerializeField] GameObject go_gameTermsPanel;

    bool b_dataFromCloudLoaded;
    private void Start()
    {
        if (PlayerAccountService.Instance.IsSignedIn)
        {
            print("Player is Sign in");
            // Hide Log in Objects
            go_loginPanel.SetActive(false);
            //go_startButton.SetActive(true);
            //script_saveData.AssignDataAndSave();
        }
        else
        {
            if (AuthenticationService.Instance.PlayerId != null)
            {
                // Hide Log in Objects
                print("Player is Logged In as Guest");
                script_playerAccountDemo.Guest_SignIn();
                go_loginPanel.SetActive(false);
                //go_startButton.SetActive(true);
            }
            else
            {
                // Show Log in objects
                print("Player is NOT sign in");
                go_startButton.SetActive(false);
                go_loginPanel.SetActive(true);
            }
        }
        // Update player ID
        tmp_playerID.text = AuthenticationService.Instance.PlayerId;
    }
    private void LateUpdate()
    {
        if(AuthenticationService.Instance.AccessToken != null && !b_dataFromCloudLoaded)
        {
            // Assign Data saved in Cloud from player stored data
            print("Load data from cloud");
            script_saveData.LoadDataFromCloud();
            //script_saveData.AssignDataAndSave(); // To pass cloud data to Save Data container data
            print("Session Token is " + AuthenticationService.Instance.AccessToken);
            b_dataFromCloudLoaded = true;
        }
        if (AuthenticationService.Instance.PlayerId != null)
        {
            if (tmp_playerID.text != AuthenticationService.Instance.PlayerId)
            {
                tmp_playerID.text = AuthenticationService.Instance.PlayerId;
            }
        }

        if (b_dataAlreadyLoadedFromCloud)
        {
            // Check if Privacy Policies and Refund Policies has been Accepted, if not show panel
            if (!PlayerData.b_gameTermsAccepted)
            {
                StartCoroutine(ShowAcceptGameTermsPanel());
            }
            go_startButton.SetActive(true);
        }

    }
    IEnumerator ShowAcceptGameTermsPanel()
    {
        yield return new WaitForSeconds(1);

        ShowGameTermsPanel();
    }
    public void AcceptTerms()
    {
        PlayerData.b_gameTermsAccepted = true;
        HideGameTermsPanel();
        SaveData.instance.AssignDataAndSave();
    }
    private void ShowGameTermsPanel()
    {
        go_gameTermsPanel.SetActive(true);
    }
    private void HideGameTermsPanel()
    {
        go_gameTermsPanel.SetActive(false);
    }
    public void LoadGameTermsPrivacyPolicy()
    {
        Application.OpenURL("https://lroar8.wixsite.com/lions-roar-games/about-5");
    }
    public void LoadGameTermsOfUse()
    {
        Application.OpenURL("https://lroar8.wixsite.com/lions-roar-games/about-5-1");
    }
    public void LoadGameRefundPolicy()
    {
        Application.OpenURL("https://lroar8.wixsite.com/lions-roar-games/copia-de-terms-of-use");
    }
    public void HideLogInPanel()
    {
        go_loginPanel.SetActive(false);
        tmp_playerID.text = AuthenticationService.Instance.PlayerId;
    }
    public void ShowLogInPanel()
    {
        go_loginPanel.SetActive(true);
    }
    public void ShowPlayButton()
    {
        go_startButton.SetActive(true);
    }
}