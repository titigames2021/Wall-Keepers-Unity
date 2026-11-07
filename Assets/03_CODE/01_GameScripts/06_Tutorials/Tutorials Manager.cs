using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialsManager : MonoBehaviour
{
    #region Tutorial Control variables
    public int i_tutorialIndex;
    private enum TutorialType { Welcome, HowToPlay, Deck};
    [Header("Control Variables")]
    [SerializeField] private TutorialType e_tutorialType;
    private int i_currentStep = 0;

    private bool b_checked = false;
    #endregion

    #region Tutorials Main Components
    [Header("Animator Components")]
    [SerializeField] Animator animator_Tutorial;
    private int animatorIntVariable = Animator.StringToHash("Step");
    [Header("Audio Components")]
    [SerializeField] AudioSource aS_tutorial;
    [SerializeField] List<AudioClip> listOf_aC_forTutorial = new List<AudioClip>();
    [SerializeField] AudioClip aC_tutorialButton;
    [Header("Text Components")]
    [SerializeField] TextMeshProUGUI tmp_displayedTextInChatBox;
    #endregion

    #region How to Play Tutorial Specifics
    [Header("Welcome Tutorial")]
    [SerializeField] private ScripteableCard[] arrayOf_initialSpellsIndeck = new ScripteableCard[8];
    [Header("How to Play")]
    [SerializeField] CanvasGroup canvasGroup_wavesManager; // Used to know when the level has started and step 3 shall start
    [Header("Deck Variables for tutorial checks")]
    [SerializeField] GameObject go_selectedSpellInDeckRemoveButton;
    [SerializeField] GameObject go_rockBallAddButton;
    [SerializeField] CanvasGroup canvasGroup_spellInfo;
    #endregion

    #region Unity Life Cycle
    private void Start()
    {
        switch (e_tutorialType)
        {
            case TutorialType.Welcome:
                if (PlayerData.b_tutorialComplete[i_tutorialIndex]) // Tutorial complete
                {
                    gameObject.SetActive(false);
                }
                else // Tutorial not complete
                {
                    AudioManager.instance.StopMusicPlaying();
                }
                break;

            case TutorialType.HowToPlay:
                AudioManager.instance.StopMusicPlaying();
                break;

            case TutorialType.Deck:
                if (PlayerData.b_tutorialComplete[i_tutorialIndex]) // Tutorial complete
                {
                    gameObject.SetActive(false);
                }
                else // Tutorial not complete
                {
                    AudioManager.instance.StopMusicPlaying();
                }
                break;
        }
    }
    private void Update()
    {
        switch (e_tutorialType)
        {
            case TutorialType.HowToPlay:
                switch (i_currentStep)
                {
                    case 3: // Step 2 to 3
                        if(canvasGroup_wavesManager.alpha == 0)
                        {
                            PlayNextStepAnim();
                        }
                        break;
                }
                break;

            case TutorialType.Deck:
                switch (i_currentStep)
                {
                    case 2: // Step 1 to 2
                        if (go_selectedSpellInDeckRemoveButton.activeSelf)
                        {
                            PlayNextStepAnim();
                        }
                        break;
                    case 3:
                        if (!PlayerData.b_isDeckFull) // Wait for player to remove spell and bool changes to false
                        {
                            PlayNextStepAnim();
                        }
                        break;
                    case 4: // Wait for player to select Rock ball in spells book
                        if (go_rockBallAddButton.activeSelf)
                        {
                            PlayNextStepAnim();
                        }
                        break;
                    case 5: // Wait for player to Add Rock ball to deck
                        if (!go_rockBallAddButton.GetComponent<Button>().interactable)
                        {
                            PlayNextStepAnim();
                        }
                        break;
                    case 6:
                        if (canvasGroup_spellInfo.alpha == 1)
                        {
                            PlayNextStepAnim();
                        }
                        break;
                }
                break;
        }
    }
    #endregion

    #region Methods to Move to next step
    internal void UpdateCurrentStep()
    {
        i_currentStep++;
    }
    internal void UpdateCurrentStepOnce()
    {
        if (!b_checked)
        {
            b_checked = true;
            i_currentStep++;
        }
    }
    public void PlayNextStepAnim()
    {
        animator_Tutorial.SetInteger(animatorIntVariable, i_currentStep);
    }
    internal void SetCheckedToFalse()
    {
        b_checked = false;
    }
    #endregion

    #region Change Text Methods
    internal void UpdateText(string s_newText)
    {
        tmp_displayedTextInChatBox.text = s_newText;
    }
    #endregion

    #region Audio Play Methods
    internal void PlayAudioClipOfCurrentStep()
    {
        aS_tutorial.Stop();
        aS_tutorial.clip = listOf_aC_forTutorial[i_currentStep];
        aS_tutorial.Play();
    }
    public void PlayTutorialButtonSFX()
    {
        aS_tutorial.Stop();
        aS_tutorial.clip = aC_tutorialButton;
        aS_tutorial.Play();
    }
    #endregion

    #region Tutorial Complete Methods
    internal void CompleteTutorial()
    {
        PlayerData.b_tutorialComplete[i_tutorialIndex] = true;
        print("Complete tutorial " + PlayerData.b_tutorialComplete[i_tutorialIndex]);
        SaveData.instance.AssignDataAndSave();
    }
    internal void WelcomeTutorialRewards()
    {
        PlayerData.b_spellCardObtained[1] = true; // Fire ball
        PlayerData.b_spellCardObtained[4] = true; // Fire Wall 
        PlayerData.b_spellCardObtained[2] = true; // Rock ball
        PlayerData.b_spellCardObtained[3] = true; // Water ball
        PlayerData.b_spellCardObtained[7] = true; // Wind Arrow
        PlayerData.b_spellCardObtained[5] = true; // Rock Wall
        PlayerData.b_spellCardObtained[6] = true; // Tornado
        PlayerData.b_spellCardObtained[9] = true; // Ignite

        // Assign Deck
        AssignFirstDeck();
    }
    private void AssignFirstDeck()
    {
        for (int i = 0; i < arrayOf_initialSpellsIndeck.Length; i++)
        {
            int i_checkedSpellId = arrayOf_initialSpellsIndeck[i].i_id;

            // Assign spell in deck by ID (The one which saves player data when game is closed)
            PlayerData.i_spellInDeckByID[i] = i_checkedSpellId;

            // Reassign Spells Prefabs
            PlayerData.go_spellsIndeck[i] = arrayOf_initialSpellsIndeck[i].spellPrefab;

            // Reassign Spells Data objects
            PlayerData.spellsInDeckData[i] = arrayOf_initialSpellsIndeck[i];
        }
        PlayerData.b_isDeckFull = true;
    }
    internal void PlaySceneMusic()
    {
        AudioManager.instance.PlayMusic();
    }
    #endregion

    #region Tutorials Specific Methods

    #region How To Play Tutorial
    internal void PauseGame()
    {
        LevelsGnralManager.b_stopLevel = true;
    }
    internal void UnPauseGame()
    {
        LevelsGnralManager.b_stopLevel = false;
    }
    #endregion

    #endregion
}
