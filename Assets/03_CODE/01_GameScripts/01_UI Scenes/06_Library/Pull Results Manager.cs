using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PullResultsManager : MonoBehaviour
{
    // Here we store every pull results and manage Pull results panel transitions
    [Header("External references")]
    [SerializeField] private CardsResultsPanels script_cardResultsPanel;
    [SerializeField] private GameObject resultsPanel;


    [Header("Pulls Data")]
    [SerializeField] private List<LibraryPullsData> libraryPullsData = new List<LibraryPullsData>();

    #region Regular Pull opening control variables
    private int i_pullOpeningTimes = 0; // How many times we must do the pull
    int i_numberOfCardsObtained = 14;

    // Pull results stored lists
    private int i_currentSearchedCard = 0;
    private List<int> cardObtainedID_list = new List<int>();
    private List<Sprite> cardObtainedSprite_list = new List<Sprite>();
    private List<int> cardObtainedTier_list = new List<int>();
    private List<int> numberOfCardsBeforePull_list = new List<int>();
    private List<bool> cardObtainedBeforePull_list = new List<bool>();

    // Exposed Variables
    public List<int> GetSpellTier()
    {
        return cardObtainedTier_list;
    }
    #endregion

    #region Pull Animations Variables
    [Header("Pull Results Animators")]
    [SerializeField] private Animator generalPullOpening_Animator;
    [SerializeField] private Animator spellRarityEffect_Animator;
    [SerializeField] internal Animator obtainedCard_Animator;

    #region Animators variables
    private int i_generalPullOpening_controlVariable = Animator.StringToHash("NextAnim"); // Int:
    // 0 = Default State, 1 = Book Opening anim, 2 = Pass Pages, 3 = Pass Pages again (iterate between 2-3 for multiple cards), 4 = Results overview
    
    private int i_spellRarityEffect_controlVariable = Animator.StringToHash("Stars"); // Int
    // 1 = 1 star, 2 = 2 stars, 3 = 3 stars, 4 = 4 stars & 5 = 5 stars

    internal int i_obtainedCard_controlVariable = Animator.StringToHash("Showing"); // Bool
    internal int i_obtainedCard_startVariable = Animator.StringToHash("StartAnim"); // Trigger
    #endregion

    #region Obtained Cards Showing Variables
    [Header("Obtained Cards Showing Variables")]
    [SerializeField] private Image img_obtainCardShowing;
    [SerializeField] private Image[] img_obtainedCard_ResultsReviewPanel = new Image[5];
    #endregion

    private int i_currentShowingCard = 0;
    public int GetCurrentShowingCard()
    {
        return i_currentShowingCard;
    }
    #endregion

    #region Pulls Methods
    public void TryPull(int i_pullCost)
    {
        if(i_pullCost <= PlayerData.i_keys)
        {
            // Daily Mission
            PlayerData.b_makeOnePull_DailyMissionComplete = true;

            LibraryShopManager.instance.UpdatePlayerManaKeys(i_pullCost); // Reduces Player mana keys by pull cost
            // Indicate how many time pull is done
            i_pullOpeningTimes = i_pullCost;
            ReStartLists(i_pullCost);

            // Start Pull opening
            StartPullOpening();
        }
        else
        {
          ContextualMenuManager.instance.ShowNotEnoughKeysPanel();
        }
    }

    private void StartPullOpening()
    {
        i_currentShowingCard = 0; // Reset current showing card back to 0

        // Get Whole Pull Results and show pull result 1st animation
        for (int i = 0; i < i_pullOpeningTimes; i++)
        {
            i_currentSearchedCard = i;
            GetCardFromPull();
        }
        // Save Results
        SaveData.instance.AssignDataAndSave();
        // Start Pull Results Animation
        PlayPullOpeningAnimation();
    }
    
    private void GetCardFromPull()
    {
        int i_oneStarChance = libraryPullsData[LibraryShopManager.instance.i_currentPull].GetOneStarChances();
        int i_twoStarChance = libraryPullsData[LibraryShopManager.instance.i_currentPull].GetTwoStarChances();
        int i_threeStarChance = libraryPullsData[LibraryShopManager.instance.i_currentPull].GetThreeStarChances();
        int i_fourStarChance = libraryPullsData[LibraryShopManager.instance.i_currentPull].GetFourStarChances();
        // Five star chances are the gap between 4 star chances and 100%
        
        // Now indicate which Rarity has been obtained
        int i_pullPercentageObtained = GetRandomNumber(100);
        List<ScripteableCard> possibleSpells = new List<ScripteableCard>();

        // Get obtained spell info
        if (i_pullPercentageObtained <= i_oneStarChance)
        {
            possibleSpells = libraryPullsData[LibraryShopManager.instance.i_currentPull].GetOneStarSpellsInPull();
        }
        // 2 Stars Chance
        else if(i_pullPercentageObtained > i_oneStarChance && i_pullPercentageObtained <= i_twoStarChance)
        {
            possibleSpells = libraryPullsData[LibraryShopManager.instance.i_currentPull].GetTwoStarSpellsInPull();
        }
        // 3 Stars Chance
        else if(i_pullPercentageObtained > i_twoStarChance && i_pullPercentageObtained <= i_threeStarChance)
        {
            possibleSpells = libraryPullsData[LibraryShopManager.instance.i_currentPull].GetThreeStarSpellsInPull();
        }
        // 4 Stars Chance
        else if(i_pullPercentageObtained > i_threeStarChance && i_pullPercentageObtained <= i_fourStarChance)
        {
            possibleSpells = libraryPullsData[LibraryShopManager.instance.i_currentPull].GetFourStarSpellsInPull();
        }
        else // Five Stars Chance
        {
            possibleSpells = libraryPullsData[LibraryShopManager.instance.i_currentPull].GetFiveStarSpellsInPull();
        }

        // Locally save results
        int spellObtainedWithinList = GetRandomNumber(possibleSpells.Count);
        Debug.Log(spellObtainedWithinList);
        cardObtainedID_list[i_currentSearchedCard] = possibleSpells[spellObtainedWithinList].i_id;
        cardObtainedSprite_list[i_currentSearchedCard] = possibleSpells[spellObtainedWithinList]._card_Sprite;
        cardObtainedTier_list[i_currentSearchedCard] = possibleSpells[spellObtainedWithinList].i_spellStarsTier;

        // Store state before pull for later properly show progression in Pull Panel Animation
        int numberOfCardsObtainedBeforePull = PlayerData.i_spell_Cards_Obtained[cardObtainedID_list[i_currentSearchedCard]];
        numberOfCardsBeforePull_list[i_currentSearchedCard] = numberOfCardsObtainedBeforePull;
        bool cardObtained = PlayerData.b_spellCardObtained[cardObtainedID_list[i_currentSearchedCard]];
        cardObtainedBeforePull_list[i_currentSearchedCard] = cardObtained;

        // Update Card new data (Nº of cards & obtained state)
        UpdateCardSavedInfo();

        // For testing
        Debug.Log("Card Obtained ID is " + cardObtainedID_list[i_currentSearchedCard]);
        Debug.Log("Card Obtained Tier is " + cardObtainedTier_list[i_currentSearchedCard]);
        Debug.Log("Card already Obtained " + cardObtained);
    }
    #endregion

    #region Pull Results Review Methods
    private void PassShowingCardResults()
    {
        int currentCardId = cardObtainedID_list[i_currentShowingCard];
        bool b_isNewCard = cardObtainedBeforePull_list[i_currentShowingCard];
        int i_cardsInStock = numberOfCardsBeforePull_list[i_currentShowingCard];
        script_cardResultsPanel.SetInfoToBeShown(b_isNewCard, i_numberOfCardsObtained, i_cardsInStock, 14);
    }
    #endregion

    #region Pull Animations
    private void PlayPullOpeningAnimation()
    {
        int openBook = 1;
        generalPullOpening_Animator.SetInteger(i_generalPullOpening_controlVariable, openBook);
        //PlayCardTierAnim();
    }
    public void PlayCardTierAnim()
    {
        int cardObtainedTier = cardObtainedTier_list[i_currentShowingCard];
        Debug.Log("Tier supossed to be showing " + cardObtainedTier);
        spellRarityEffect_Animator.SetInteger(i_spellRarityEffect_controlVariable, cardObtainedTier);
        Debug.Log("Tier in animator is " + spellRarityEffect_Animator.GetInteger(i_spellRarityEffect_controlVariable));
    }
    public void EndPullOpening()
    {
        int closePullResultsPanel = 0;
        generalPullOpening_Animator.SetInteger(i_generalPullOpening_controlVariable, closePullResultsPanel);
        // Reset the others
        //int i_defaultAnim = 0;
        //spellRarityEffect_Animator.SetInteger(i_spellRarityEffect_controlVariable, i_defaultAnim);
        obtainedCard_Animator.SetBool(i_obtainedCard_controlVariable, false);
    }
    #endregion

    #region Buttons Methods
    public void PlayerTouchesCard()
    {
        // If card is already showing
        if (obtainedCard_Animator.GetBool(i_obtainedCard_controlVariable))
        {
            CheckIfMoreCardsToBePulled();
            obtainedCard_Animator.SetBool(i_obtainedCard_controlVariable, false); // Reset this animation
            resultsPanel.SetActive(false);
        }
        else 
        {
            if(i_pullOpeningTimes > i_currentShowingCard)
            {
                PassShowingCardResults();
                obtainedCard_Animator.SetBool(i_obtainedCard_controlVariable, true);
                img_obtainCardShowing.sprite = cardObtainedSprite_list[i_currentShowingCard];
                i_currentShowingCard++; // Set next card to be shown
            }
            else
            {
                CheckIfMoreCardsToBePulled();
            }
        }
    }
    private void CheckIfMoreCardsToBePulled()
    {
        if(i_currentShowingCard < i_pullOpeningTimes) // If there are still cards to be shown
        {
            // Play again animation of card showing
            // Check if we are in animation 2 or 3 to change to the other
            int i_currentAnimationOfGnralPanel = generalPullOpening_Animator.GetInteger(i_generalPullOpening_controlVariable);
            if (i_currentAnimationOfGnralPanel == 1)
            {
                int i_nextAnimation = 2;
                generalPullOpening_Animator.SetInteger(i_generalPullOpening_controlVariable, i_nextAnimation);
            }
            if (i_currentAnimationOfGnralPanel == 2)
            {
                int i_nextAnimation = 3;
                generalPullOpening_Animator.SetInteger(i_generalPullOpening_controlVariable, i_nextAnimation);
            }
            if(i_currentAnimationOfGnralPanel == 3)
            {
                int i_nextAnimation = 2;
                generalPullOpening_Animator.SetInteger(i_generalPullOpening_controlVariable, i_nextAnimation);
            }
        }
        else // No more cards show results overview
        {
            int i_resultsOverviewAnimation = 4;
            // Update General Results Panel Showing Cards
            UpdateResultsOverviewPanel();
            // Show General Results Panel
            generalPullOpening_Animator.SetInteger(i_generalPullOpening_controlVariable, i_resultsOverviewAnimation);
        }
    }

    public void SkipToResultsOverviewPanel()
    {
        int i_nextAnimation = 4;
        UpdateResultsOverviewPanel();
        generalPullOpening_Animator.SetInteger(i_generalPullOpening_controlVariable, i_nextAnimation);
    }
    #endregion

    #region Extra Methods
    private int GetRandomNumber(int i_maxNumber)
    {
        int randomNumber = Random.Range(0, i_maxNumber);
        return randomNumber;
    }
    private void ReStartLists(int i_listSize)
    {
        cardObtainedID_list.Clear();
        cardObtainedSprite_list.Clear();
        cardObtainedTier_list.Clear();
        numberOfCardsBeforePull_list.Clear();
        cardObtainedBeforePull_list.Clear();

        for(int i = 0; i < i_listSize; i++) // Create lists with default values
        {
            cardObtainedID_list.Add(0);
            cardObtainedSprite_list.Add(null);
            cardObtainedTier_list.Add(0);
            numberOfCardsBeforePull_list.Add(0);
            cardObtainedBeforePull_list.Add(false);
        }
    }
    private void UpdateCardSavedInfo()
    {
        if (PlayerData.b_spellCardObtained[cardObtainedID_list[i_currentSearchedCard]])
        {
            // If card already obtained then add more cards of that spell type
            PlayerData.i_spell_Cards_Obtained[cardObtainedID_list[i_currentSearchedCard]] += i_numberOfCardsObtained;
        }
        else
        {
            // If card not obtained previously, mark as obtained
            PlayerData.b_spellCardObtained[cardObtainedID_list[i_currentSearchedCard]] = true;
        }
    }
    private void UpdateResultsOverviewPanel()
    {
        for (int i = 0; i < img_obtainedCard_ResultsReviewPanel.Length; i++)
        {
            img_obtainedCard_ResultsReviewPanel[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < i_pullOpeningTimes; i++)
        {
            img_obtainedCard_ResultsReviewPanel[i].gameObject.SetActive(true);
            img_obtainedCard_ResultsReviewPanel[i].sprite = cardObtainedSprite_list[i];
        }
    }

    internal void RestartObtainedCardAnimation()
    {
        obtainedCard_Animator.SetBool(i_obtainedCard_controlVariable, false);
    }
    #endregion
}
