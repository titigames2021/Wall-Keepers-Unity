using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LibraryShopManager : MonoBehaviour
{
    /*
     * Library Shop Manager Control Main Player interactions with this scene.
     * 1 - Stores Pulls windows & pull down bar posters
     * 2 - Manages changes between pulls
     * 3 - Manages back to main scene button
     */

    #region Singleton
    public static LibraryShopManager instance { get; private set; } // Can be read by all scripts but only modified by this
    #endregion

    #region Main Scene Variables
    [Header("Player Currencies")]
    [SerializeField] private TextMeshProUGUI manaKeys_TMP;

    [Header("Pulls Windows and Pulls downbar posters")]
    [SerializeField] private List<Animator> pullsWindows_list = new List<Animator>();
    [SerializeField] private List<Animator> downBarPoster_list = new List<Animator>();
    private int i_pullWindowAnimatorControlVariable = Animator.StringToHash("Change");

    [Header("Scene General Buttons")]
    [SerializeField] private Button backToMainMenu_Button;
    [SerializeField] private Button previousPull_Button, nextPullButton;

    #endregion

    #region Control Variables
    internal int i_currentPull = 0; // Used to know which pull is showing
    private int i_maxPulls;
    #endregion

    #region Unity Life Cycle
    private void Awake()
    {
        // Implement Singleton Pattern
        if (instance != null) // Error check to avoid more than one Instance of this script to be created
        {
            Debug.LogError("Tere is more than one LibraryShopManager! " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this; // Equal the instance to this script

        // Show First Pull - Index 4 (Show from Right)
        PlayAnimationOFPullWindow(4);
        PlayDownBarPosterAnimation(4);
        i_maxPulls = pullsWindows_list.Count;
        CheckIfMorePullsToTheRight();
        CheckIfMorePullsToTheLeft();
    }
    private void Start()
    {
        // For Testing - REMOVE
        //PlayerData.i_keys = 100;

        UpdatePlayerManaKeys(0);
    }
    #endregion

    #region Private Local Methods

    #region Call animations of Pulls Windows and Posters
    private void PlayAnimationOFPullWindow(int i_animationIndex)
    {
        pullsWindows_list[i_currentPull].SetInteger(i_pullWindowAnimatorControlVariable, i_animationIndex);
    }
    private void PlayDownBarPosterAnimation(int i_animationIndex)
    {
        downBarPoster_list[i_currentPull].SetInteger(i_pullWindowAnimatorControlVariable, i_animationIndex);
    }
    #endregion

    #region Checks
    private void CheckIfMorePullsToTheLeft()
    {
        if (i_currentPull >= i_maxPulls -1)
        {
            // Disactivate Next pull Arrow buttons
            previousPull_Button.interactable = false;
        }
        else
        {
            previousPull_Button.interactable = true;
        }
    }
    private void CheckIfMorePullsToTheRight()
    {
        if (i_currentPull <= 0)
        {
            // Disactivate Next pull Arrow buttons
            nextPullButton.interactable = false;
        }
        else
        {
            nextPullButton.interactable = true;
        }
    }
    #endregion

    #endregion

    public void UpdatePlayerManaKeys(int i_keysReduced)
    {
        PlayerData.i_keys -= i_keysReduced;
        manaKeys_TMP.text = PlayerData.i_keys.ToString();
    }

    #region Public Methods for buttons
    public void ShowNextPullWindow()
    {
        if(i_currentPull >= i_maxPulls) // Chek if no more Pull to the right (Next)
        {
            return;
        }

        // Play Hide Current window Hide Animations
        int nextPullWindow = i_currentPull+1;
        PlayAnimationOFPullWindow(3); // Hide to the left current Pull window
        PlayDownBarPosterAnimation(3); // Hide Down bar poster of current window to the left

        // Play Next Pull Window Appear animations
        PlayAnimationOFPullWindow(2); // Show from the right next Pull window
        PlayDownBarPosterAnimation(2); // Show Down bar poster of next window from right

        // Last check
        CheckIfMorePullsToTheLeft();
    }
    public void ShowPreviousPullWindow()
    {
        if (i_currentPull <= 0) // Chek if no more Pull to the right (Next)
        {
            return;
        }

        // Play Hide Current window Hide Animations
        int nextPullWindow = i_currentPull + 1;
        PlayAnimationOFPullWindow(1); // Hide to the right current Pull window
        PlayDownBarPosterAnimation(1); // Hide Down bar poster of current window to the right

        // Play Next Pull Window Appear animations
        PlayAnimationOFPullWindow(4); // Show from the left next Pull window
        PlayDownBarPosterAnimation(4); // Show Down bar poster of next window from left

        // Last check
        CheckIfMorePullsToTheRight();
    }

    public void ShowScenesTransitionCanvas(Animator _animator)
    {
        _animator.SetTrigger("start");
    }

    public void ShowPullDropRates(GameObject go_pullDropRates)
    {
        go_pullDropRates.SetActive(true);
    }
    #endregion
}
