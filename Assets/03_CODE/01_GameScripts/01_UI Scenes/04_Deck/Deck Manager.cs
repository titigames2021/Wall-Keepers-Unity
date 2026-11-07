using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    #region Singleton
    public static DeckManager instance { get; private set; } // Can be read by all scripts but only modified by this
    #endregion

    #region Events
    public event EventHandler OnSelectedSlotChanged;
    #endregion

    #region Deck Main Variables
    internal BaseSlot currentSelectedSlot; // To know which slot is currently selected
    [Header("Deck Slots")]
    [SerializeField] internal DeckSlot[] deckSlots_array = new DeckSlot[8];

    [Header("Player Currencies")]
    [SerializeField] internal TextMeshProUGUI tmp_playerManaEnergy;
    #endregion

    #region Spell Info Panel
    [Header("Spell Info Panel Variables")]
    [SerializeField] Animator animator_SpellInfoPanel;
    private int i_animatorTriggerVar = Animator.StringToHash("Fade");
    [SerializeField] private Image img_selectedSpellCard;
    [SerializeField] private Image img_smallCard_RequiredCostInFillBar;
    [SerializeField] private TextMeshProUGUI tmp_spellDescription;
    [Header("Spell Stats Variables")]
    [SerializeField] private GameObject go_spellDamageStats;
    [SerializeField] private GameObject go_spellResistanceStats, go_spellSpeedStats, go_spellDurationStats;
    [SerializeField]
    internal TextMeshProUGUI tmp_spellStar, tmp_spellLevel, tmp_spellAwkLevel,
        tmp_damageAlone, tmp_damageWithImprovement, tmp_damageImprovement,
        tmp_resistanceAlone, tmp_resistanceWithImprovement, tmp_resistanceImprovement,
        tmp_speedAlone, tmp_speedWithImprovement, tmp_speedImprovement,
        tmp_durationAlone, tmp_durationWithImprovement, tmp_durationImprovement;
    [Header("Spell Level Up")]
    [SerializeField] private GameObject go_levelUpItems;
    [SerializeField] internal Image img_cardsRequiredFillBar;
    [SerializeField] internal TextMeshProUGUI tmp_cardsInStock, tmp_cardsUntilNextLevel;

    [Header("Spell Awakening")]
    [SerializeField] private GameObject go_awakenItems;
    [SerializeField] private Button awakenButton;
    [SerializeField] private Image img_elementalRuneRequiredIcon;
    [SerializeField] internal TextMeshProUGUI tmp_elementalRunesRequiredInStock, tmp_elementalRunesRequired;

    [Header("Mana Energy Cost for lvl up or awakening and button")]
    [SerializeField] private TextMeshProUGUI tmp_manaEnergyCost;
    [SerializeField] private TextMeshProUGUI tmp_manaEnergyCostInAwakenButton;
    [SerializeField] private TextMeshProUGUI tmp_lvlUpOrAwakentext;
    [SerializeField] private Button button_levelUp;

    [Header("Fusion Spells Special items")]
    [SerializeField] private GameObject go_fusionSpellsSpecialItems;
    [SerializeField] private Image img_spellRequired_1, img_spellRequired_2;
    #endregion

    #region Spell level Up and Awaken Variables
    internal int i_manaEnergyCost;
    internal int i_spellCardsCost;
    internal int i_elementalRunesCost, i_elementalRunesInStock;
    internal int i_cardsInStock;
    #endregion

    #region Spell Stats
    internal float f_damage, f_resistance, f_speed, f_duration;
    #endregion

    #region Unity Life Cycle
    void Awake()
    {
       
        // Implement Singleton Pattern
        if (instance != null) // Error check to avoid more than one Instance of this script to be created
        {
            Debug.LogError("Tere is more than one DeckManager! " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this; // Equal the instance to this script
    }
    private void Start()
    {
        tmp_playerManaEnergy.text = PlayerData.i_mana.ToString();
        AssignSpellsInDeckAtStart();
    }
    #endregion

    #region Events
    internal void FireEventOnSlotSelectedChanged()
    {
        OnSelectedSlotChanged?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Unselect Selected Slot
    public void UnselectSlot()
    {
        if(currentSelectedSlot != null)
        {
            currentSelectedSlot.HideButtons();
            currentSelectedSlot = null;
        }
    }
    #endregion

    #region Deck Slots Control
    private void AssignSpellsInDeckAtStart()
    {
        // Assign saved Spells in deck to their current slots
        for (int i = 0; i < deckSlots_array.Length; i++)
        {
            if(PlayerData.spellsInDeckData[deckSlots_array[i].i_deckSlotIndex] != null)
            {
                deckSlots_array[i].SetSpellInfo(PlayerData.spellsInDeckData[deckSlots_array[i].i_deckSlotIndex]);
                deckSlots_array[i].spellPrefab = PlayerData.spellsInDeckData[deckSlots_array[i].i_deckSlotIndex].spellPrefab;
                deckSlots_array[i].img_spellInSlotVisual.sprite = PlayerData.spellsInDeckData[deckSlots_array[i].i_deckSlotIndex]._card_Sprite;
            }
        }
    }
    internal void AssignNextEmptyDeckSlot(Sprite spr_spellCard, ScripteableCard spellData, GameObject spellPrefab)
    {
        for (int i = 0; i < deckSlots_array.Length; i++)
        {
            if (deckSlots_array[i].GetSlotSpellInfo() == null)
            {
                // Assign Spell Card
                deckSlots_array[i].img_spellInSlotVisual.sprite = spr_spellCard;
                // Assign Spell Data
                deckSlots_array[i].SetSpellInfo(spellData);
                // Assign Spell Prefab
                deckSlots_array[i].spellPrefab = spellPrefab;
                PlayerData.deck_GameObjects[deckSlots_array[i].i_deckSlotIndex] = deckSlots_array[i].spellPrefab;
                // ---Only Works When coming from Deck (Not when initializing game)
                PlayerData.spellsInDeckData[deckSlots_array[i].i_deckSlotIndex] = spellData;
                PlayerData.go_spellsIndeck[i] = spellPrefab;
                // ---
                PlayerData.i_spellInDeckByID[i] = spellData.i_id; // Store spell in deck using its ID
                break;
            }
        }

        // Check if deck full
        PlayerData.b_isDeckFull = true; // Make true and then check if any slot is empty, if so make false
        for (int i = 0; i < deckSlots_array.Length; i++)
        {
            if (deckSlots_array[i].GetSlotSpellInfo() == null)
            {
                PlayerData.b_isDeckFull = false;
                break;
            }
        }

        // Save data
        SaveData.instance.AssignDataAndSave();
    } 

    internal bool CheckIfSpellAlreadyInDeck(ScripteableCard spellData)
    {
        for (int i = 0; i < deckSlots_array.Length; i++)
        {
            if (deckSlots_array[i].GetSlotSpellInfo() != null)
            {
                if(deckSlots_array[i].GetSlotSpellInfo() == spellData)
                {
                    return true;
                }
            }
        }

        return false;
    }
    #endregion

    #region Spell Info Panel Methods
    public void OpenSpellInfoPanel(ScripteableCard spellInfo)
    {
        // Assign Spell Card Sprite and descriptio
        img_selectedSpellCard.sprite = spellInfo._card_Sprite;
        tmp_spellDescription.text = spellInfo.Description;
        // Locally save spell element for later animations when levelling up or awakening.

        // Get Spell Level
        int spellID = spellInfo.i_id;
        int i_spellLevel = PlayerData.i_spell_Card_Level[spellID];
        tmp_spellLevel.text = i_spellLevel.ToString();
        int i_spellAwakeningLvl = PlayerData.i_awakening_Card_Level[spellID];
        tmp_spellAwkLevel.text = i_spellAwakeningLvl.ToString();

        #region Assign Spell Stats
        int spellStar = spellInfo.i_spellStarsTier;
        tmp_spellStar.text = spellStar.ToString();
        
        // Update stats and stats visuals
        UpdateStats(spellInfo);
        #endregion

        // Disactive not applicable stats (if base stat is 0 disactivate)
        if (f_damage > 0) { go_spellDamageStats.SetActive(true); } else {  go_spellDamageStats.SetActive(false);}
        if(f_resistance > 0) { go_spellResistanceStats.SetActive(true); } else { go_spellResistanceStats.SetActive(false); }
        if(f_speed > 0) { go_spellSpeedStats.SetActive(true);} else {  go_spellSpeedStats.SetActive(false); }
        if(f_duration > 0) { go_spellDurationStats.SetActive(true); } else { go_spellDurationStats.SetActive(false); }

        // Level Up and Awakening Costs (Both in Mana and Required items)
        int manaEnergyCostPerlevel = SpellsData.i_spell_ManaEnergy_Cost_Increase_Per_Level;
        i_manaEnergyCost = manaEnergyCostPerlevel + i_spellLevel * manaEnergyCostPerlevel; // Level up and Awakening have the same Mana energy cost
        i_spellCardsCost = SpellsData.i_spell_Cards_Cost_Increase_Per_Level + SpellsData.i_spell_Cards_Cost_Increase_Per_Level * i_spellLevel;
        i_elementalRunesCost = SpellsData.i_elemetalRunes_Cost_Increase_Per_Level+ 
            i_spellAwakeningLvl * SpellsData.i_elemetalRunes_Cost_Increase_Per_Level;
        // Mana Cost
        tmp_manaEnergyCost.text = i_manaEnergyCost.ToString();
        tmp_manaEnergyCostInAwakenButton.text  = i_manaEnergyCost.ToString();
        // Level Up items
        i_cardsInStock = PlayerData.i_spell_Cards_Obtained[spellID];
        tmp_cardsInStock.text = i_cardsInStock.ToString();
        tmp_cardsUntilNextLevel.text = "/"+ i_spellCardsCost.ToString();
        // Awakening items
        i_elementalRunesInStock = PlayerData.i_elementalRunesInStock[spellInfo.requiredElementalRune.i_runeID];
        tmp_elementalRunesRequiredInStock.text = i_elementalRunesInStock.ToString();
        tmp_elementalRunesRequired.text = "/" + i_elementalRunesCost.ToString();
        img_elementalRuneRequiredIcon.sprite = spellInfo.requiredElementalRune.spr_runeIcon;

        button_levelUp.interactable = false;
        awakenButton.interactable = false;
        go_fusionSpellsSpecialItems.SetActive(false); // Set Off Fusion Spells Items by dafault

        // Check if spell ready for Awakening - Level Cap reached and Awakening level not increased yet
        // At level 10 - Awk level 1 and so on
        tmp_lvlUpOrAwakentext.text = "Awaken"; // changed later if level up instead
        bool b_awakeningReady = true;
        if (i_spellLevel == 10)
        {
            if(i_spellAwakeningLvl == 0)
            {
                // Show Awakening options
                go_levelUpItems.SetActive(false);
                go_fusionSpellsSpecialItems.SetActive(false);
                go_awakenItems.SetActive(true);
                awakenButton.gameObject.SetActive(true);
                if (i_elementalRunesInStock >= i_elementalRunesCost)
                {
                    if (PlayerData.i_mana >= i_manaEnergyCost)
                    {
                        awakenButton.interactable = true;
                        UpdateStatsImprovement(SpellsData.f_damageIncreaseByAwakening, SpellsData.f_resistanceIncreaseByAwakening,
                                SpellsData.f_speedIncreaseByAwakening, SpellsData.f_durationIncreaseByAwakening);
                        ShowStatsWithImprovement();
                    }
                    else { ShowStatsAlone(); }
                }
                else { ShowStatsAlone(); }
            }
            else { b_awakeningReady = false; }
        }
        else if (i_spellLevel == 20)
        {
            if (i_spellAwakeningLvl == 1)
            {
                // Show Awakening options
                go_levelUpItems.SetActive(false);
                go_fusionSpellsSpecialItems.SetActive(false);
                go_awakenItems.SetActive(true);
                awakenButton.gameObject.SetActive(true);
                if (i_elementalRunesInStock >= i_elementalRunesCost)
                {
                    if (PlayerData.i_mana >= i_manaEnergyCost)
                    {
                        awakenButton.interactable = true;
                        UpdateStatsImprovement(SpellsData.f_damageIncreaseByAwakening, SpellsData.f_resistanceIncreaseByAwakening,
                        SpellsData.f_speedIncreaseByAwakening, SpellsData.f_durationIncreaseByAwakening);
                        ShowStatsWithImprovement();
                    }
                    else { ShowStatsAlone(); }
                }
                else { ShowStatsAlone(); }
            }
            else { b_awakeningReady = false; }
        }
        else if(i_spellLevel == 30)
        {
            if (i_spellAwakeningLvl == 2)
            {
                // Show Awakening options
                go_levelUpItems.SetActive(false);
                go_fusionSpellsSpecialItems.SetActive(false);
                go_awakenItems.SetActive(true);
                awakenButton.gameObject.SetActive(true);
                if (i_elementalRunesInStock >= i_elementalRunesCost)
                {
                    if (PlayerData.i_mana >= i_manaEnergyCost)
                    {
                        awakenButton.interactable = true;
                        UpdateStatsImprovement(SpellsData.f_damageIncreaseByAwakening, SpellsData.f_resistanceIncreaseByAwakening,
                        SpellsData.f_speedIncreaseByAwakening, SpellsData.f_durationIncreaseByAwakening);
                        ShowStatsWithImprovement();
                    }
                    else { ShowStatsAlone(); }
                }
                else { ShowStatsAlone(); }
            }
            else { b_awakeningReady = false; }
        }
        else if (i_spellLevel == 40)
        {
            if (i_spellAwakeningLvl == 3)
            {
                // Show Awakening options
                go_levelUpItems.SetActive(false);
                go_fusionSpellsSpecialItems.SetActive(false);
                go_awakenItems.SetActive(true);
                awakenButton.gameObject.SetActive(true);
                if (i_elementalRunesInStock >= i_elementalRunesCost)
                {
                    if (PlayerData.i_mana >= i_manaEnergyCost)
                    {
                        awakenButton.interactable = true;
                        UpdateStatsImprovement(SpellsData.f_damageIncreaseByAwakening, SpellsData.f_resistanceIncreaseByAwakening,
                        SpellsData.f_speedIncreaseByAwakening, SpellsData.f_durationIncreaseByAwakening);
                        ShowStatsWithImprovement();
                    }
                    else { ShowStatsAlone(); }
                }
                else { ShowStatsAlone(); }
            }
            else { b_awakeningReady = false; }
        }
        else if (i_spellLevel == 50)
        {
            if (i_spellAwakeningLvl == 4)
            {
                // Show Awakening options
                go_levelUpItems.SetActive(false);
                go_fusionSpellsSpecialItems.SetActive(false);
                go_awakenItems.SetActive(true);
                awakenButton.gameObject.SetActive(true);
                if (i_elementalRunesInStock >= i_elementalRunesCost)
                {
                    if(PlayerData.i_mana >= i_manaEnergyCost)
                    {
                        awakenButton.interactable = true;
                        UpdateStatsImprovement(SpellsData.f_damageIncreaseByAwakening, SpellsData.f_resistanceIncreaseByAwakening,
                        SpellsData.f_speedIncreaseByAwakening, SpellsData.f_durationIncreaseByAwakening);
                        ShowStatsWithImprovement();
                    }
                    else { ShowStatsAlone(); }
                }
                else { ShowStatsAlone(); }
            }
            else { b_awakeningReady = false; }
        }
        else { b_awakeningReady = false; }

        if (!b_awakeningReady) // Spell Not ready for Awakening Show Level Up options
        {
            go_awakenItems.SetActive(false);
            awakenButton.gameObject.SetActive(false);
            // Check if Spell is fusion or not and Show level up buttons
            switch (spellInfo.e_spellType)
            {
                case ScripteableCard.SpellType.Normal:
                    tmp_lvlUpOrAwakentext.text = "Level Up";
                    go_levelUpItems.SetActive(true);
                    img_smallCard_RequiredCostInFillBar.sprite = spellInfo._card_Sprite;
                    // Show Spell Card Required Fill bar
                    img_cardsRequiredFillBar.fillAmount = GetFillBarValue(i_cardsInStock, i_spellCardsCost);
                    // Check if enough for Level Up
                    if (i_cardsInStock >= i_spellCardsCost)
                    {
                        if (PlayerData.i_mana >= i_manaEnergyCost)
                        {
                            button_levelUp.interactable = true;
                            UpdateStatsImprovement(SpellsData.f_damageIncreaseByLevel, SpellsData.f_resistanceIncreaseByLevel,
                                SpellsData.f_speedIncreaseByLevel, SpellsData.f_durationIncreaseByLevel);
                            ShowStatsWithImprovement();
                            print("Enseña stats con mejora disponible");
                        }
                        else
                        {
                            ShowStatsAlone();
                        }
                    }
                    else
                    {
                        ShowStatsAlone();
                    }
                    break;
                case ScripteableCard.SpellType.Fusion:
                    tmp_lvlUpOrAwakentext.text = "Level Up";
                    // Assign Required Cards for fusion spell Sprites
                    img_spellRequired_1.sprite = spellInfo.spr_spellRequiredCard_1;
                    img_spellRequired_2.sprite = spellInfo.spr_spellRequiredCard_2;
                    go_fusionSpellsSpecialItems.SetActive(true); // Activate Fusion Special Items
                    go_levelUpItems.SetActive(false);
                    go_awakenItems.SetActive(false);
                    if (PlayerData.i_mana >= i_manaEnergyCost)
                    {
                        button_levelUp.interactable = true;
                        UpdateStatsImprovement(SpellsData.f_damageIncreaseByLevel, SpellsData.f_resistanceIncreaseByLevel,
                            SpellsData.f_speedIncreaseByLevel, SpellsData.f_durationIncreaseByLevel);
                        ShowStatsWithImprovement();
                    }
                    else
                    {
                        ShowStatsAlone();
                    }
                    break;
            }
        }

        // Additional Check to avoid errors when displaying Awaken requirements only
        if (go_awakenItems.activeSelf)
        {
            go_levelUpItems.SetActive(false);
            go_fusionSpellsSpecialItems.SetActive(false);
        }

        animator_SpellInfoPanel.SetTrigger(i_animatorTriggerVar);
    }
    public void HideSpellInfoPanel()
    {
        animator_SpellInfoPanel.SetTrigger(i_animatorTriggerVar);
    }
    #endregion

    #region Helper methods
    private float GetFillBarValue(float f_numberOfCards, int i_cardsUntilNextImprovement)
    {
        float fillBarValue = (float)f_numberOfCards / i_cardsUntilNextImprovement;
        if (f_numberOfCards >= i_cardsUntilNextImprovement)
        {
            fillBarValue = 1f;
        }
        return fillBarValue;
    }
    internal void UpdateStats(ScripteableCard spellInfo)
    {
        int i_spellLevel = PlayerData.i_spell_Card_Level[spellInfo.i_id];
        int i_spellAwakeningLvl = PlayerData.i_awakening_Card_Level[spellInfo.i_id];
        f_damage = spellInfo.f_dmg;
        if (f_damage > 0)
        {
            f_damage = spellInfo.f_dmg +
                SpellsData.f_damageIncreaseByLevel * i_spellLevel +
                SpellsData.f_damageIncreaseByAwakening * i_spellAwakeningLvl;
        }
        f_resistance = spellInfo.f_resistance;
        if(f_resistance > 0)
        {
            f_resistance = spellInfo.f_resistance +
                SpellsData.f_resistanceIncreaseByLevel * i_spellLevel +
                SpellsData.f_resistanceIncreaseByAwakening * i_spellAwakeningLvl;
        }
        f_speed = spellInfo.f_projectileSpeed;
        if(f_speed > 0)
        {
            f_speed = spellInfo.f_projectileSpeed +
                SpellsData.f_speedIncreaseByLevel * i_spellLevel +
                SpellsData.f_speedIncreaseByAwakening * i_spellAwakeningLvl; // if 0 is instant casting
        }
        f_duration = spellInfo.f_duration;
        if(f_duration > 0)
        {
            f_duration = spellInfo.f_duration +
                SpellsData.f_durationIncreaseByLevel * i_spellLevel +
                SpellsData.f_durationIncreaseByAwakening * i_spellAwakeningLvl;
        }

        // Damage
        tmp_damageAlone.text = f_damage.ToString();
        tmp_damageWithImprovement.text = f_damage.ToString();
        // Resistance
        tmp_resistanceAlone.text = f_resistance.ToString();
        tmp_resistanceWithImprovement.text = f_resistance.ToString() ;
        // Speed
        tmp_speedAlone.text = f_speed.ToString();
        tmp_speedWithImprovement.text = f_speed.ToString();
        //Duration
        tmp_durationAlone.text = f_duration.ToString();
        tmp_durationWithImprovement.text = f_duration.ToString();
    }
    private void UpdateStatsImprovement(float f_damageImprove, float f_resistanceImprove, float f_speedImprove, float f_durationImprove)
    {
        // Damage
        tmp_damageImprovement.text = "+" + f_damageImprove.ToString();
        // Resistance
        tmp_resistanceImprovement.text = "+" + f_resistanceImprove.ToString();
        // Speed
        tmp_speedImprovement.text = "+" + f_speedImprove.ToString();
        // Duration
        tmp_durationImprovement.text = "+" + f_durationImprove.ToString();
    }
    internal void ShowStatsAlone()
    {
        // Damage
        tmp_damageAlone.gameObject.SetActive(true);
        tmp_damageWithImprovement.gameObject.SetActive(false);
        tmp_damageImprovement.gameObject.SetActive(false);
        // Resistance
        tmp_resistanceAlone.gameObject.SetActive(true);
        tmp_resistanceWithImprovement.gameObject.SetActive(false);
        tmp_resistanceImprovement.gameObject.SetActive(false);
        // Speed
        tmp_speedAlone.gameObject.SetActive(true);
        tmp_speedWithImprovement.gameObject.SetActive(false);
        tmp_speedImprovement.gameObject.SetActive(false);
        //Duration
        tmp_durationAlone.gameObject.SetActive(true);
        tmp_durationWithImprovement.gameObject.SetActive(false);
        tmp_durationImprovement.gameObject.SetActive(false);
    }
    private void ShowStatsWithImprovement()
    {
        // Damage
        tmp_damageAlone.gameObject.SetActive(false);
        tmp_damageWithImprovement.gameObject.SetActive(true);
        tmp_damageImprovement.gameObject.SetActive(true);
        // Resistance
        tmp_resistanceAlone.gameObject.SetActive(false);
        tmp_resistanceWithImprovement.gameObject.SetActive(true);
        tmp_resistanceImprovement.gameObject.SetActive(true);
        // Speed
        tmp_speedAlone.gameObject.SetActive(false);
        tmp_speedWithImprovement.gameObject.SetActive(true);
        tmp_speedImprovement.gameObject.SetActive(true);
        //Duration
        tmp_durationAlone.gameObject.SetActive(false);
        tmp_durationWithImprovement.gameObject.SetActive(true);
        tmp_durationImprovement.gameObject.SetActive(true);
    }
    #endregion

    #region Redone Methods to use in Improve Card manager
    internal void UpdateSpellLvlUPOrAwakenCosts(int i_spellID, ScripteableCard spellInfo)
    {
        // Get Spell Level and Awk lvl
        int i_spellLevel = PlayerData.i_spell_Card_Level[i_spellID];
        int i_spellAwakeningLvl = PlayerData.i_awakening_Card_Level[i_spellID];

        // Level Up and Awakening Costs (Both in Mana and Required items)
        int manaEnergyCostPerlevel = SpellsData.i_spell_ManaEnergy_Cost_Increase_Per_Level;
        i_manaEnergyCost = manaEnergyCostPerlevel + i_spellLevel * manaEnergyCostPerlevel; // Level up and Awakening have the same Mana energy cost
        i_spellCardsCost = SpellsData.i_spell_Cards_Cost_Increase_Per_Level + SpellsData.i_spell_Cards_Cost_Increase_Per_Level * i_spellLevel;
        i_elementalRunesCost = SpellsData.i_elemetalRunes_Cost_Increase_Per_Level +
            i_spellAwakeningLvl * SpellsData.i_elemetalRunes_Cost_Increase_Per_Level;
        // Mana Cost
        tmp_manaEnergyCost.text = i_manaEnergyCost.ToString();
        tmp_manaEnergyCostInAwakenButton.text = i_manaEnergyCost.ToString();
        // Level Up items
        int i_cardsInStock = PlayerData.i_spell_Cards_Obtained[i_spellID];
        tmp_cardsInStock.text = i_cardsInStock.ToString();
        tmp_cardsUntilNextLevel.text = "/" + i_spellCardsCost.ToString();
        // Awakening items
        int i_elementalRunesInStock = PlayerData.i_elementalRunesInStock[spellInfo.requiredElementalRune.i_runeID];
        tmp_elementalRunesRequiredInStock.text = i_elementalRunesInStock.ToString();
        tmp_elementalRunesRequired.text = "/" + i_elementalRunesCost.ToString();
        img_elementalRuneRequiredIcon.sprite = spellInfo.requiredElementalRune.spr_runeIcon;
    }
    internal void UpdateButtons(int i_spellID, ScripteableCard spellInfo)
    {
        // Get Spell Level and Awk lvl
        int i_spellLevel = PlayerData.i_spell_Card_Level[i_spellID];
        int i_spellAwakeningLvl = PlayerData.i_awakening_Card_Level[i_spellID];

        // For Level 
        int i_cardsInStock = PlayerData.i_spell_Cards_Obtained[i_spellID];

        // For Awakening
        int i_elementalRunesInStock = PlayerData.i_elementalRunesInStock[spellInfo.requiredElementalRune.i_runeID];

        button_levelUp.interactable = false;
        // Check if spell ready for Awakening - Level Cap reached and Awakening level not increased yet
        // At level 10 - Awk level 1 and so on
        tmp_lvlUpOrAwakentext.text = "Awaken"; // changed later if level up instead
        bool b_awakeningReady = true;
        if (i_spellLevel == 10)
        {
            if (i_spellAwakeningLvl == 0)
            {
                // Show Awakening options
                go_levelUpItems.SetActive(false);
                go_fusionSpellsSpecialItems.SetActive(false);
                go_awakenItems.SetActive(true);
                awakenButton.gameObject.SetActive(true);
                if (i_elementalRunesInStock >= i_elementalRunesCost)
                {
                    if (PlayerData.i_mana >= i_manaEnergyCost)
                    {
                        awakenButton.interactable = true;
                        UpdateStatsImprovement(SpellsData.f_damageIncreaseByAwakening, SpellsData.f_resistanceIncreaseByAwakening,
                                SpellsData.f_speedIncreaseByAwakening, SpellsData.f_durationIncreaseByAwakening);
                        ShowStatsWithImprovement();
                    }
                    else { ShowStatsAlone(); }
                }
                else { ShowStatsAlone(); }
            }
            else { b_awakeningReady = false; }
        }
        else if (i_spellLevel == 20)
        {
            if (i_spellAwakeningLvl == 1)
            {
                // Show Awakening options
                go_levelUpItems.SetActive(false);
                go_fusionSpellsSpecialItems.SetActive(false);
                go_awakenItems.SetActive(true);
                awakenButton.gameObject.SetActive(true);
                if (i_elementalRunesInStock >= i_elementalRunesCost)
                {
                    if (PlayerData.i_mana >= i_manaEnergyCost)
                    {
                        awakenButton.interactable = true;
                        UpdateStatsImprovement(SpellsData.f_damageIncreaseByAwakening, SpellsData.f_resistanceIncreaseByAwakening,
                        SpellsData.f_speedIncreaseByAwakening, SpellsData.f_durationIncreaseByAwakening);
                        ShowStatsWithImprovement();
                    }
                    else { ShowStatsAlone(); }
                }
                else { ShowStatsAlone(); }
            }
            else { b_awakeningReady = false; }
        }
        else if (i_spellLevel == 30)
        {
            if (i_spellAwakeningLvl == 2)
            {
                // Show Awakening options
                go_levelUpItems.SetActive(false);
                go_fusionSpellsSpecialItems.SetActive(false);
                go_awakenItems.SetActive(true);
                awakenButton.gameObject.SetActive(true);
                if (i_elementalRunesInStock >= i_elementalRunesCost)
                {
                    if (PlayerData.i_mana >= i_manaEnergyCost)
                    {
                        awakenButton.interactable = true;
                        UpdateStatsImprovement(SpellsData.f_damageIncreaseByAwakening, SpellsData.f_resistanceIncreaseByAwakening,
                        SpellsData.f_speedIncreaseByAwakening, SpellsData.f_durationIncreaseByAwakening);
                        ShowStatsWithImprovement();
                    }
                    else { ShowStatsAlone(); }
                }
                else { ShowStatsAlone(); }
            }
            else { b_awakeningReady = false; }
        }
        else if (i_spellLevel == 40)
        {
            if (i_spellAwakeningLvl == 3)
            {
                // Show Awakening options
                go_levelUpItems.SetActive(false);
                go_fusionSpellsSpecialItems.SetActive(false);
                go_awakenItems.SetActive(true);
                awakenButton.gameObject.SetActive(true);
                if (i_elementalRunesInStock >= i_elementalRunesCost)
                {
                    if (PlayerData.i_mana >= i_manaEnergyCost)
                    {
                        awakenButton.interactable = true;
                        UpdateStatsImprovement(SpellsData.f_damageIncreaseByAwakening, SpellsData.f_resistanceIncreaseByAwakening,
                        SpellsData.f_speedIncreaseByAwakening, SpellsData.f_durationIncreaseByAwakening);
                        ShowStatsWithImprovement();
                    }
                    else { ShowStatsAlone(); }
                }
                else { ShowStatsAlone(); }
            }
            else { b_awakeningReady = false; }
        }
        else if (i_spellLevel == 50)
        {
            if (i_spellAwakeningLvl == 4)
            {
                // Show Awakening options
                go_levelUpItems.SetActive(false);
                go_fusionSpellsSpecialItems.SetActive(false);
                go_awakenItems.SetActive(true);
                awakenButton.gameObject.SetActive(true);
                if (i_elementalRunesInStock >= i_elementalRunesCost)
                {
                    if (PlayerData.i_mana >= i_manaEnergyCost)
                    {
                        awakenButton.interactable = true;
                        UpdateStatsImprovement(SpellsData.f_damageIncreaseByAwakening, SpellsData.f_resistanceIncreaseByAwakening,
                        SpellsData.f_speedIncreaseByAwakening, SpellsData.f_durationIncreaseByAwakening);
                        ShowStatsWithImprovement();
                    }
                    else { ShowStatsAlone(); }
                }
                else { ShowStatsAlone(); }
            }
            else if (i_spellAwakeningLvl == 5)
            {
                go_levelUpItems.SetActive(false);
                go_fusionSpellsSpecialItems.SetActive(false);
                go_awakenItems.SetActive(true);
                awakenButton.gameObject.SetActive(false);
                ShowStatsAlone();

                tmp_lvlUpOrAwakentext.text = "MAX UPGRADE";
            }
            else { b_awakeningReady = false; }
        }
        else { b_awakeningReady = false; }
        if (!b_awakeningReady) // Spell Not ready for Awakening Show Level Up options
        {
            go_awakenItems.SetActive(false);
            awakenButton.gameObject.SetActive(false);
            // Check if Spell is fusion or not and Show level up buttons
            switch (spellInfo.e_spellType)
            {
                case ScripteableCard.SpellType.Normal:
                    tmp_lvlUpOrAwakentext.text = "Level Up";
                    go_levelUpItems.SetActive(true);
                    img_smallCard_RequiredCostInFillBar.sprite = spellInfo._card_Sprite;
                    // Show Spell Card Required Fill bar
                    img_cardsRequiredFillBar.fillAmount = GetFillBarValue(i_cardsInStock, i_spellCardsCost);
                    // Check if enough for Level Up
                    if (i_cardsInStock >= i_spellCardsCost)
                    {
                        if (PlayerData.i_mana >= i_manaEnergyCost)
                        {
                            button_levelUp.interactable = true;
                            UpdateStatsImprovement(SpellsData.f_damageIncreaseByLevel, SpellsData.f_resistanceIncreaseByLevel,
                                SpellsData.f_speedIncreaseByLevel, SpellsData.f_durationIncreaseByLevel);
                            ShowStatsWithImprovement();
                        }
                        else
                        {
                            ShowStatsAlone();
                        }
                    }
                    else
                    {
                        ShowStatsAlone();
                    }
                    break;
                case ScripteableCard.SpellType.Fusion:
                    tmp_lvlUpOrAwakentext.text = "Level Up";
                    img_spellRequired_1.sprite = spellInfo.spr_spellRequiredCard_1;
                    img_spellRequired_2.sprite = spellInfo.spr_spellRequiredCard_2;
                    go_fusionSpellsSpecialItems.SetActive(true);
                    go_levelUpItems.SetActive(false);
                    go_awakenItems.SetActive(false);
                    if (PlayerData.i_mana >= i_manaEnergyCost)
                    {
                        button_levelUp.interactable = true;
                        UpdateStatsImprovement(SpellsData.f_damageIncreaseByLevel, SpellsData.f_resistanceIncreaseByLevel,
                            SpellsData.f_speedIncreaseByLevel, SpellsData.f_durationIncreaseByLevel);
                        ShowStatsWithImprovement();
                    }
                    else
                    {
                        ShowStatsAlone();
                    }
                    break;
            }
        }
    }
    #endregion
}
