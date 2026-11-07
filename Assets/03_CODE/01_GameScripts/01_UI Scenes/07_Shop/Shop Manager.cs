using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    // IMPORTANT INFO:
    // Spells and Runes can only be purchased once every day.
    // Spells and Mana Runes available for purchase are updated daily.
    // Mana Runes can only be purchased 5 times per day. Reset every new day.
    //

    #region Singleton
    public static ShopManager instance { get; private set; } // Can be read by all scripts but only modified by this
    #endregion

    #region Local Variables for actions control
    // Player Currencies
    [Header("Player Currencies")]
    [SerializeField] TextMeshProUGUI tmp_playerManaEnergy;
    [SerializeField] TextMeshProUGUI tmp_playerManaKeys;

    // Info Panels
    [Header("Info Panels")]
    [SerializeField] GameObject go_spellsInfoPanel;
    [SerializeField] GameObject go_spellBoughtPanel;
    [SerializeField] GameObject go_itemBoughtPanel;
    [SerializeField] Image img_spellCard, img_itemBought;
    [SerializeField] TextMeshProUGUI tmp_spellDescription, tmp_itemBoughtAmount;
    private bool b_playFillBarEffect;
    [Header("Spell Bought Info Panels")]
    [SerializeField] Image img_spellBoughtCard;
    [SerializeField] GameObject go_spellFillBarObjects, go_spellNewObjects;
    [SerializeField] Image img_fillBar;
    [SerializeField] TextMeshProUGUI tmp_numberOfCards, tmp_cardsUntilNextImprovement;
    
    // Spells Related
    internal bool b_showFillEffect; // For spells already obtained panel after buying spell
    private float f_numberOfCards, f_finalTotalNumberOfCards;
    private int i_cardsUntilNextImprovement;


    [Header("Spells to buy Info and Variables")]
    [SerializeField] private List<ShopBuyableSpells> allBuyableSpellsDataObjects = new List<ShopBuyableSpells>(); // All spells data objects
    private ShopBuyableSpells[] arrayOf_displayedShopBuyableSpells = new ShopBuyableSpells[3]; // All spells data objects
    [SerializeField] private List<BuyableItemManager> buyableSpells = new List<BuyableItemManager>(); // Spells shown today in the shop
    [SerializeField] private TextMeshProUGUI[] tmp_spellReward = new TextMeshProUGUI[3]; // New or x14
    [Header("Elemental Runes")]
    [SerializeField] private List<ElementalRunes_SO> buyableElementalRunes_List = new List<ElementalRunes_SO>();
    private ElementalRunes_SO[] arrayOf_buyableElementalRunesDisplayed = new ElementalRunes_SO[3];
    [SerializeField] private List<BuyableItemManager> buyableERunes = new List<BuyableItemManager>();
    #endregion

    #region Unity Life Cycle
    private void Start()
    {
        // Implement Singleton Pattern
        if (instance != null) // Error check to avoid more than one Instance of this script to be created
        {
            Debug.LogError("Tere is more than one ShopManager! " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this; // Equal the instance to this script

        UpdatePlayerCurrencies();
        CheckDisplayedRewards();
        SaveData.instance.AssignDataAndSave(); // To avoid players refreshing shop if items don't like them.
    }
    #endregion

    #region Local Methods
    internal void UpdatePlayerCurrencies()
    {
        tmp_playerManaEnergy.text = PlayerData.i_mana.ToString();
        tmp_playerManaKeys.text = PlayerData.i_keys.ToString();
    }
    private void CheckDisplayedRewards() // Checks if new day and updates rewards showing and state
    {
        if(PlayerData.i_lastDayLogInShop != DateTime.Now.Day) // Is new Day, refresh shop items
        {
            Debug.Log("Refresh, New Day");
            RefreshShopItems();
            PlayerData.i_lastDayLogInShop = DateTime.Now.Day; // Save New day as last day log in
        }
        else // Not new day
        {
            CheckShopItemsState();
            AssignAlreadyDisplayedElementalRunes();
        }
    }
    private void RefreshShopItems()
    {
        // Refresh available spells and mark them as not bought already
        for (int i = 0; i < PlayerData.b_spellBoughtAlready.Length; i++) // Mark spells as not bought already
        {
            PlayerData.b_spellBoughtAlready[i] = false;
        }
        // Get new spells and place them
        GetNewDaySpells();
        // Get new elemental Runes
        GetNewDayElementalRunes();
    }
    #endregion

    #region Spells Methods
    private void GetNewDaySpells()
    {
        // Get 3 random spells from the list of allbuyables spells
        int i_spellsDisplayed = 3;
        for(int i = 0; i < i_spellsDisplayed; i++)
        {
            int i_randomNumber = UnityEngine.Random.Range(0, allBuyableSpellsDataObjects.Count); // Get random spell
            PlayerData.shopBuyableSpells[i] = i_randomNumber; // Assign displayed spell to list
            arrayOf_displayedShopBuyableSpells[i] = allBuyableSpellsDataObjects[i_randomNumber]; // Assign to Local List
            allBuyableSpellsDataObjects.RemoveAt(i_randomNumber); // remove to avoid repeating
        }

        AssignDataOfNewSpellsDisplayedInShop(); // Show Spells Displayed Data (Card art and cost)

    }
    private void CheckShopItemsState()
    {
        // Get spells displayed
        AssignDataOfAlreadyDisplayedSpellSInShop();

        // Check which spells are already bought
        for (int i = 0; i < PlayerData.b_spellBoughtAlready.Length; i++) // Mark spells as not bought already
        {
            if (PlayerData.b_spellBoughtAlready[i]) // If spell alreay bought, block so we can't buy it again
            {
                buyableSpells[i].BlockSpell_AlreadyBought();
            }
        }
    }
    private void AssignDataOfNewSpellsDisplayedInShop()
    {
        // Assign spells displayed info to Buyable items
        for (int i = 0; i < PlayerData.shopBuyableSpells.Length; i++)
        {
            buyableSpells[i].spellBuyableData = arrayOf_displayedShopBuyableSpells[i]; // Assign spell data for Buy method
            buyableSpells[i].SetSpellCard(arrayOf_displayedShopBuyableSpells[i].GetSpellCadrArt()); // Assign Spell Card
            buyableSpells[i].SetItemPrice(arrayOf_displayedShopBuyableSpells[i].GetSpellCost()); // Get Spell Cost
            if (!PlayerData.b_spellCardObtained[buyableSpells[i].spellBuyableData.GetSpellID()])
            {
                tmp_spellReward[i].text = "NEW";
            }
            else
            {
                tmp_spellReward[i].text = "x14";
            }
        }
    }
    private void AssignDataOfAlreadyDisplayedSpellSInShop()
    {
        // Assign spells displayed info to Buyable items
        for (int i = 0; i < PlayerData.shopBuyableSpells.Length; i++)
        {
            // Get Data Saved 
            // get spell displayed Index in array
            arrayOf_displayedShopBuyableSpells[i] = allBuyableSpellsDataObjects[PlayerData.shopBuyableSpells[i]];
            allBuyableSpellsDataObjects.RemoveAt(PlayerData.shopBuyableSpells[i]);

            buyableSpells[i].spellBuyableData = arrayOf_displayedShopBuyableSpells[i]; // Assign spell data for Buy method
            buyableSpells[i].SetSpellCard(arrayOf_displayedShopBuyableSpells[i].GetSpellCadrArt()); // Assign Spell Card
            buyableSpells[i].SetItemPrice(arrayOf_displayedShopBuyableSpells[i].GetSpellCost()); // Get Spell Cost
            if (!PlayerData.b_spellCardObtained[buyableSpells[i].spellBuyableData.GetSpellID()])
            {
                tmp_spellReward[i].text = "NEW";
            }
            else
            {
                tmp_spellReward[i].text = "x14";
            }
        }
    }
    #endregion

    #region Elemental Runes Methods
    private void GetNewDayElementalRunes()
    {
        for(int i = 0; i < PlayerData.i_elementalRunesPurchasesLeftInShop.Length; i++)
        {
            int i_dailyPurchases = 5;
            PlayerData.i_elementalRunesPurchasesLeftInShop[i] = i_dailyPurchases;
        }

        // Get 3 random Elemental Runes from the list of All elemental Runes
        int i_runesDisplayed = 3;
        for (int i = 0; i < i_runesDisplayed; i++)
        {
            int i_randomNumber = UnityEngine.Random.Range(0, buyableElementalRunes_List.Count); // Get random rune
            PlayerData.shopBuyableERunes[i] = i_randomNumber; // Assign displayed rune to list
            arrayOf_buyableElementalRunesDisplayed[i] = buyableElementalRunes_List[i_randomNumber];
            buyableElementalRunes_List.RemoveAt(i_randomNumber); // remove to avoid repeating

            buyableERunes[i].UpdateRunePurchasesLeft(); // Update Text showing purchases left
        }

        AssignNewDisplayedElementalRunes();
    }
    private void AssignAlreadyDisplayedElementalRunes()
    {
        // Get saved Elemental Runes from the list of All elemental Runes
        int i_runesDisplayed = 3;
        for (int i = 0; i < i_runesDisplayed; i++)
        {
            arrayOf_buyableElementalRunesDisplayed[i] = buyableElementalRunes_List[PlayerData.shopBuyableERunes[i]];
            buyableElementalRunes_List.RemoveAt(PlayerData.shopBuyableERunes[i]); // remove to avoid repeating
            buyableERunes[i].UpdateRunePurchasesLeft(); // Update Text showing purchases left
        }

        // Assign Displayed E rune SO data and Sprite icon
        for (int i = 0; i < buyableERunes.Count; i++)
        {
            buyableERunes[i].elementalRuneData = arrayOf_buyableElementalRunesDisplayed[i];
            buyableERunes[i].SetElementalRuneIcon(buyableERunes[i].elementalRuneData.spr_runeIcon);
        }

        // Check if number of purchases is bigger than 0, if not, activate Block Purchase GameObject
        for (int i = 0; i < PlayerData.i_elementalRunesPurchasesLeftInShop.Length; i++)
        {
            if (PlayerData.i_elementalRunesPurchasesLeftInShop[i] <= 0) // If no more purhcases left
            {
                buyableERunes[i].BlockERunePurchase(); // Block purchases activating go
            }
        }
    }
    internal void CheckRunesPurchaseLeft()
    {
        // Check if number of purchases is bigger than 0, if not, activate Block Purchase GameObject
        for (int i = 0; i < PlayerData.i_elementalRunesPurchasesLeftInShop.Length; i++)
        {
            buyableERunes[i].UpdateRunePurchasesLeft(); // Update Text showing purchases left

            if (PlayerData.i_elementalRunesPurchasesLeftInShop[i] <= 0) // If no more purhcases left
            {
                buyableERunes[i].BlockERunePurchase(); // Block purchases activating go
            }
        }

        AssignNewDisplayedElementalRunes();
    }
    private void AssignNewDisplayedElementalRunes()
    {
        // Assign Displayed E rune SO data and Sprite icon
        for (int i = 0; i < buyableERunes.Count; i++)
        {
            buyableERunes[i].elementalRuneData = arrayOf_buyableElementalRunesDisplayed[i];
            buyableERunes[i].SetElementalRuneIcon(buyableERunes[i].elementalRuneData.spr_runeIcon);
        }
    }
    #endregion

    #region Methods For Common Panels
    internal void ShowSpellObtainedPanel(int i_spellID, int i_spellCardsBefore, int i_totalCardsNow, Sprite spr_spellCard, bool newCard)
    {
        //Assign Spell Card Sprite
        img_spellBoughtCard.sprite = spr_spellCard;

        // Check if new card or not
        if (newCard)
        {
            // Hide Bar if new and show "New" text instead
            go_spellFillBarObjects.SetActive(false);
            go_spellNewObjects.SetActive(true);
            go_spellBoughtPanel.SetActive(true);
        }
        else
        {
            f_numberOfCards = i_spellCardsBefore;
            f_finalTotalNumberOfCards = i_totalCardsNow;
            
            // Assign Spell Cards required until next improvement
            int i_spellCardCostIPL = SpellsData.i_spell_Cards_Cost_Increase_Per_Level;
            i_cardsUntilNextImprovement = i_spellCardCostIPL + PlayerData.i_spell_Card_Level[i_spellID] * i_spellCardCostIPL;
            tmp_cardsUntilNextImprovement.text = "/" + i_cardsUntilNextImprovement.ToString();

            // Assign Spell cards number before buying to TMP  Nº of cards and int number of cards before buying
            tmp_numberOfCards.text = f_numberOfCards.ToString();

            // Show Bar if not new and show fill bar and fill effect
            go_spellNewObjects.SetActive(false); // Hide New Text
            go_spellFillBarObjects.SetActive(true); // Show Fill Bar objects
            
            go_spellBoughtPanel.SetActive(true);
            StartCoroutine(SumUp(i_totalCardsNow, i_spellID));
        }
    }
    public void ShowBuyableSpellInfoPanel(int i_buyableSpellIndex)
    {
        // buyableSpells[i_buyableSpellIndex]
        // Assign Spell Card
        img_spellCard.sprite = buyableSpells[i_buyableSpellIndex].spellBuyableData.GetSpellCadrArt();
        // Assign Spell description
        tmp_spellDescription.text = buyableSpells[i_buyableSpellIndex].spellBuyableData.GetSpellData().Description;
        // Activate Spell Info Panel
        go_spellsInfoPanel.SetActive(true);
    }
    public void ShowItemBoughtPanel(Sprite itemImage, int i_itemAmount)
    {
        img_itemBought.sprite = itemImage;
        tmp_itemBoughtAmount.text = "x" + i_itemAmount;
        go_itemBoughtPanel.SetActive(true);
    }
    public void HideSpellsInfoPanel()
    {
        go_spellsInfoPanel.SetActive(false);
    }
    public void HideSpellBoughtPanel()
    {
        go_spellBoughtPanel.SetActive(false);
    }
    public void HideItemBoughtPanel()
    {
        go_itemBoughtPanel.SetActive(false);
    }
    #endregion

    #region Spells Sum Up Effect Methods
    IEnumerator SumUp(float targetValue, int i_spellID)
    {
        GetNumberOfCardsForNextImprovement(i_spellID);
        float f_duration = 1f;
        var rate = Mathf.Abs(targetValue - f_numberOfCards) / f_duration;
        while (f_numberOfCards != targetValue)
        {
            f_numberOfCards = Mathf.MoveTowards(f_numberOfCards, targetValue, rate * Time.deltaTime);
            tmp_numberOfCards.text = ((int)f_numberOfCards).ToString();
            img_fillBar.fillAmount = GetFillBarValue();
            yield return null;
        }
    }

    private float GetFillBarValue()
    {
        float fillBarValue = (float)f_numberOfCards / i_cardsUntilNextImprovement;
        if (f_numberOfCards >= i_cardsUntilNextImprovement)
        {
            fillBarValue = 1f;
        }
        return fillBarValue;
    }
    private void GetNumberOfCardsForNextImprovement(int i_spellID)
    {
        // Get How many cards the spells needs for next improvement
        i_cardsUntilNextImprovement = SpellsData.i_spell_Cards_Cost_Increase_Per_Level +
            SpellsData.i_spell_Cards_Cost_Increase_Per_Level * PlayerData.i_spell_Card_Level[i_spellID];
    }
    #endregion

}
