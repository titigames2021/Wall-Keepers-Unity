using UnityEngine;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using Unity.Services.Core;

public class SaveData : MonoBehaviour
{
    #region Singleton
    public static SaveData instance { get; private set; } // Can be read by all scripts but only modified by this
    #endregion

    private Container playerData_Container = new Container();

    #region Unity Life Cycle Methods
    async void Awake()
    {
        await UnityServices.InitializeAsync();
    }
    private void Start()
    {
        // Implement Singleton Pattern
        if (instance != null) // Error check to avoid more than one Instance of this script to be created
        {
            Debug.LogError("Tere is more than one LibraryShopManager! " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this; // Equal the instance to this script

        // Reset(); // Only when we want to delete data
        // Load Player Saved Data
        // LoadAndAssignData();       
    }
    #endregion

    #region Save and Load Data Methods
    public void AssignDataAndSave()
    {
        // Get data from PlayerData
        //playerData_Container.ContainerGetDataFromPlayerData();
        // Save data to Json
        //SaveToJson();
        // Save Data to Unity Cloud
        SaveDataToCloud();
    }
    public void SaveToJson()
    {
        string playerData = JsonUtility.ToJson(playerData_Container);
        string filepath = Application.persistentDataPath + "/playerData.json";
        Debug.Log(filepath);
        System.IO.File.WriteAllText(filepath, playerData);
        Debug.Log("Data saved");
    }
    public void LoadAndAssignData()
    {
        // Load Data from Cloud
        LoadDataFromCloud();
        // Load saved data
        //LoadFromJson();

        // Pass saved data to PlayerData
        //playerData_Container.ContainerPassDataToPlayerData();
    }
    public void LoadFromJson()
    {
        string filepath = Application.persistentDataPath + "/playerData.json";
        string playerData = System.IO.File.ReadAllText(filepath);

        playerData_Container = JsonUtility.FromJson<Container>(playerData);
        Debug.Log("Data Loaded");
    }

    #endregion

    #region Reset Data
    // Call first to reset Data
    public void Reset()
    {
        SaveToJson();
    }
    #endregion

    #region Save Data To Unity Cloud
    public async void SaveDataToCloud()
    {
        // Example
        /*
        var playerData = new Dictionary<string, object>{
          {"firstKeyName", "a text value"},
          {"secondKeyName", 123}
        };
        var result = await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
        Debug.Log($"Saved data {string.Join(',', playerData)}");
        */

        var playerCurrencies = new Dictionary<string, object>{
          {"PlayerManaEnergy", PlayerData.i_mana},
          {"PlayerManaKeys", PlayerData.i_keys},
          {"SpellCardsObtained", PlayerData.b_spellCardObtained},
          {"NumberOfSpellsObtainedOfEachSpell", PlayerData.i_spell_Cards_Obtained},
          {"SpellsLevels", PlayerData.i_spell_Card_Level},
          {"SpellsAwakeningLevels", PlayerData.i_awakening_Card_Level},
          {"spellCanbeAwaken", PlayerData.b_canAwake},
          {"spellsInDeck", PlayerData.i_spellInDeckByID},
          {"cardSlotNumber", PlayerData.i_CardSlotNumber},
          {"deckIsFull", PlayerData.b_isDeckFull},
          {"EndlessWavesMaxRound", PlayerData.i_maxRound},
          {"b_firstDataSave", PlayerData.b_firstDataSave},
          {"gamesPlayed_DailyMissionCount", PlayerData.i_gamesPlayed_DailyMissionCount},
          {"MakeOnePull_DailyMission", PlayerData.b_makeOnePull_DailyMissionComplete},
          {"improveOneSpell_DailyMission", PlayerData.b_improveOneSpell_DailyMissionComplete},
          {"allMissionsCompleted_DailyMission", PlayerData.b_allDailyMissionComplete},
          {"dailyMissionOneClaimed", PlayerData.b_dailyMission_One_Claimed},
          {"dailyMissionTwoClaimed", PlayerData.b_dailyMission_Two_Claimed},
          {"dailyMissionThreeClaimed", PlayerData.b_dailyMission_Three_Claimed},
          {"dailyMissionFourClaimed", PlayerData.b_dailyMission_Four_Claimed},
          {"dailyMissionFiveClaimed", PlayerData.b_dailyMission_Five_Claimed},
          {"levelsCompleteStatus", PlayerData.b_levelComplete},
          {"tutorialsCompleteStatus", PlayerData.b_tutorialComplete},
          {"elementalRunesInStock", PlayerData.i_elementalRunesInStock},
          {"spellInShopBoughtAlready", PlayerData.b_spellBoughtAlready},
          {"shopBuyableSpells", PlayerData.shopBuyableSpells},
          {"shopBuyableERunes", PlayerData.shopBuyableERunes},
          {"runesLeftInShop", PlayerData.i_elementalRunesPurchasesLeftInShop},
          {"removeAdsState", PlayerData.b_removeAdsBought},
          {"currentWizzardPassBought", PlayerData.b_wizzardPassBought},
          {"seasonPassBoughtID", PlayerData.i_lastSeasonPassBought},
          {"lastDayLogInShop", PlayerData.i_lastDayLogInShop},
          {"lastDayLogInMainMenu", PlayerData.i_lastDayLogInMainMenu},
          {"i_magicPassManaExp", PlayerData.i_magicPassManaExp},
          {"i_magicPassLevel", PlayerData.i_magicPassLevel},
          {"b_magicPassRewardClaimed", PlayerData.b_magicPassRewardClaimed},
          {"b_premiumPassPurchased", PlayerData.b_premiumPassPurchased},
          {"i_lastSeasonPlayedID", PlayerData.i_lastSeasonPlayedID},
          {"b_seasonPosterShown", PlayerData.b_seasonPosterShown},
          {"b_dailyFreeClaimed", PlayerData.b_dailyFreeClaimed},
          {"b_gameTermsAccepted", PlayerData.b_gameTermsAccepted},
          #region Wizzard Studio Attributes
          {"i_wallUpgradeStage_TotalUpgrades", PlayerData.i_wallUpgradeStage_TotalUpgrades},
          {"i_wallHPIncreased", PlayerData.i_wallHPIncreased},
          {"i_wallHPRecoveryPerSecond", PlayerData.i_wallHPRecoveryPerSecond},
          {"i_magicUpgradeLevel", PlayerData.i_magicUpgradeLevel},
          {"i_magicDamageAttributeUpgradeAmount", PlayerData.i_magicDamageAttributeUpgradeAmount},
          {"i_magicResistanceAttributeUpgradeAmount", PlayerData.i_magicResistanceAttributeUpgradeAmount},
          {"f_magicDurationAttributeUpgradedAmount", PlayerData.f_magicDurationAttributeUpgradedAmount},
          {"f_magicSpeedAttributeUpgradedAmount", PlayerData.f_magicSpeedAttributeUpgradedAmount},
          #endregion
        };
        var result = await CloudSaveService.Instance.Data.Player.SaveAsync(playerCurrencies);
        Debug.Log($"Saved data {string.Join(',', playerCurrencies)}");
    }

    public async void LoadDataFromCloud()
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> {
          "PlayerManaEnergy", "PlayerManaKeys", "SpellCardsObtained", "NumberOfSpellsObtainedOfEachSpell", "SpellsLevels",
          "SpellsAwakeningLevels", "SpellsAwakeningLevels", "spellCanbeAwaken", "spellsInDeck", "cardSlotNumber", "deckIsFull",
          "EndlessWavesMaxRound", "b_firstDataSave", "gamesPlayed_DailyMissionCount", "MakeOnePull_DailyMission", "improveOneSpell_DailyMission",
          "allMissionsCompleted_DailyMission", "dailyMissionOneClaimed", "dailyMissionTwoClaimed", "dailyMissionThreeClaimed", "dailyMissionFourClaimed",
          "dailyMissionFiveClaimed", "levelsCompleteStatus", "tutorialsCompleteStatus", "elementalRunesInStock", "spellInShopBoughtAlready", "shopBuyableERunes",
          "runesLeftInShop", "removeAdsState", "currentWizzardPassBought", "seasonPassBoughtID", "lastDayLogInShop", "lastDayLogInMainMenu",
          "i_magicPassManaExp","i_magicPassLevel","b_magicPassRewardClaimed","b_premiumPassPurchased","shopBuyableSpells","i_wallUpgradeStage_TotalUpgrades",
          "i_wallHPIncreased","i_wallHPRecoveryPerSecond","i_magicUpgradeLevel","i_magicDamageAttributeUpgradeAmount","i_magicResistanceAttributeUpgradeAmount",
          "f_magicDurationAttributeUpgradedAmount","f_magicSpeedAttributeUpgradedAmount","i_lastSeasonPlayedID","b_seasonPosterShown","b_dailyFreeClaimed",
          "b_gameTermsAccepted",
        });

        // Mana Energy (Int)
        if (playerData.TryGetValue("PlayerManaEnergy", out var playerSavedMana))
        {
            PlayerData.i_mana = playerSavedMana.Value.GetAs<int>();
        }
        // Mana Keys (Int)
        if (playerData.TryGetValue("PlayerManaKeys", out var playerSavedManaKeys))
        {
            PlayerData.i_keys = playerSavedManaKeys.Value.GetAs<int>();
        }
        // Spells Obtained (Array of bools)
        if (playerData.TryGetValue("SpellCardsObtained", out var SpellCardsObtained))
        {
            PlayerData.b_spellCardObtained = SpellCardsObtained.Value.GetAs<bool[]>();
        }
        // Number of Spells Obtained of each spell (Array of ints)
        if (playerData.TryGetValue("NumberOfSpellsObtainedOfEachSpell", out var NumberOfSpellsObtainedOfEachSpell))
        {
            //Debug.Log($"NumberOfSpellsObtainedOfEachSpell value: {NumberOfSpellsObtainedOfEachSpell.Value.GetAs<int>()}");
            PlayerData.i_spell_Cards_Obtained = NumberOfSpellsObtainedOfEachSpell.Value.GetAs<int[]>();
        }
        // Spells Levels (Array of ints)
        if (playerData.TryGetValue("SpellsLevels", out var SpellsLevels))
        {
            //Debug.Log($"SpellsLevels value: {SpellsLevels.Value.GetAs<int>()}");
            PlayerData.i_spell_Card_Level = SpellsLevels.Value.GetAs<int[]>();
        }
        // Spells Awakening Levels (Array of ints)
        if (playerData.TryGetValue("SpellsAwakeningLevels", out var SpellsAwakeningLevels))
        {
            //Debug.Log($"SpellsAwakeningLevels value: {SpellsAwakeningLevels.Value.GetAs<int>()}");
            PlayerData.i_awakening_Card_Level = SpellsAwakeningLevels.Value.GetAs<int[]>();
        }
        // Spells Which can be awaken (Array of bools)
        if (playerData.TryGetValue("spellCanbeAwaken", out var spellCanbeAwaken))
        {
            //Debug.Log($"spellCanbeAwaken value: {spellCanbeAwaken.Value.GetAs<int>()}");
            PlayerData.b_canAwake = spellCanbeAwaken.Value.GetAs<bool[]>();
        }
        // Spells In Deck (Array of ints)
        if (playerData.TryGetValue("spellsInDeck", out var spellsInDeck))
        {
            //Debug.Log($"spellsInDeck value: {spellsInDeck.Value.GetAs<int>()}");
            PlayerData.i_spellInDeckByID = spellsInDeck.Value.GetAs<int[]>();
        }
        // Deck slots data (Array of ints)
        if (playerData.TryGetValue("cardSlotNumber", out var cardSlotNumber))
        {
            //Debug.Log($"cardSlotNumber value: {cardSlotNumber.Value.GetAs<int>()}");
            PlayerData.i_CardSlotNumber = cardSlotNumber.Value.GetAs<int[]>();
        }
        // Deck full or not state (Array of ints)
        if (playerData.TryGetValue("deckIsFull", out var deckIsFull))
        {
            //Debug.Log($"deckIsFull value: {deckIsFull.Value.GetAs<int>()}");
            PlayerData.b_isDeckFull = deckIsFull.Value.GetAs<bool>();
        }
        // Max wave reached in endless waves mode (int)
        if (playerData.TryGetValue("EndlessWavesMaxRound", out var EndlessWavesMaxRound))
        {
            //Debug.Log($"EndlessWavesMaxRound value: {EndlessWavesMaxRound.Value.GetAs<int>()}");
            PlayerData.i_maxRound = EndlessWavesMaxRound.Value.GetAs<int>();
        }
        // Is first data save done check (bool)
        if (playerData.TryGetValue("b_firstDataSave", out var b_firstDataSave))
        {
            //Debug.Log($"b_firstDataSave value: {b_firstDataSave.Value.GetAs<int>()}");
            PlayerData.b_firstDataSave = b_firstDataSave.Value.GetAs<bool>();
        }
        // Daily Missions Games played count (int)
        if (playerData.TryGetValue("gamesPlayed_DailyMissionCount", out var gamesPlayed_DailyMissionCount))
        {
            //Debug.Log($"gamesPlayed_DailyMissionCount value: {gamesPlayed_DailyMissionCount.Value.GetAs<int>()}");
            PlayerData.i_gamesPlayed_DailyMissionCount = gamesPlayed_DailyMissionCount.Value.GetAs<int>();
        }
        // Daily Missions Games Make one pull (bool)
        if (playerData.TryGetValue("MakeOnePull_DailyMission", out var MakeOnePull_DailyMission))
        {
            //Debug.Log($"gamesPlayed_DailyMissionCount value: {gamesPlayed_DailyMissionCount.Value.GetAs<int>()}");
            PlayerData.b_makeOnePull_DailyMissionComplete = MakeOnePull_DailyMission.Value.GetAs<bool>();
        }
        // Daily Missions improve one spell (bool)
        if (playerData.TryGetValue("improveOneSpell_DailyMission", out var improveOneSpell_DailyMission))
        {
            //Debug.Log($"improveOneSpell_DailyMission value: {improveOneSpell_DailyMission.Value.GetAs<int>()}");
            PlayerData.b_improveOneSpell_DailyMissionComplete = improveOneSpell_DailyMission.Value.GetAs<bool>();
        }
        // Daily Missions improve one spell (bool)
        if (playerData.TryGetValue("allMissionsCompleted_DailyMission", out var allMissionsCompleted_DailyMission))
        {
            //Debug.Log($"allMissionsCompleted_DailyMission value: {allMissionsCompleted_DailyMission.Value.GetAs<int>()}");
            PlayerData.b_allDailyMissionComplete = allMissionsCompleted_DailyMission.Value.GetAs<bool>();
        }
        // Daily Missions Claimed status (bool)
        if (playerData.TryGetValue("dailyMissionOneClaimed", out var dailyMissionOneClaimed))
        {
            //Debug.Log($"dailyMissionOneClaimed value: {dailyMissionOneClaimed.Value.GetAs<int>()}");
            PlayerData.b_dailyMission_One_Claimed = dailyMissionOneClaimed.Value.GetAs<bool>();
        }
        // Daily Missions Claimed status (bool)
        if (playerData.TryGetValue("dailyMissionTwoClaimed", out var dailyMissionTwoClaimed))
        {
            //Debug.Log($"dailyMissionTwoClaimed value: {dailyMissionTwoClaimed.Value.GetAs<int>()}");
            PlayerData.b_dailyMission_Two_Claimed = dailyMissionTwoClaimed.Value.GetAs<bool>();
        }
        // Daily Missions Claimed status (bool)
        if (playerData.TryGetValue("dailyMissionThreeClaimed", out var dailyMissionThreeClaimed))
        {
            //Debug.Log($"dailyMissionThreeClaimed value: {dailyMissionThreeClaimed.Value.GetAs<int>()}");
            PlayerData.b_dailyMission_Three_Claimed = dailyMissionThreeClaimed.Value.GetAs<bool>();
        }
        // Daily Missions Claimed status (bool)
        if (playerData.TryGetValue("dailyMissionFourClaimed", out var dailyMissionFourClaimed))
        {
            //Debug.Log($"dailyMissionFourClaimed value: {dailyMissionThreeClaimed.Value.GetAs<int>()}");
            PlayerData.b_dailyMission_Four_Claimed = dailyMissionFourClaimed.Value.GetAs<bool>();
        }
        // Daily Missions Claimed status (bool)
        if (playerData.TryGetValue("dailyMissionFiveClaimed", out var dailyMissionFiveClaimed))
        {
            //Debug.Log($"dailyMissionFiveClaimed value: {dailyMissionFiveClaimed.Value.GetAs<int>()}");
            PlayerData.b_dailyMission_Five_Claimed = dailyMissionFiveClaimed.Value.GetAs<bool>();
        }
        // Story Levels Completion status (array of bool)
        if (playerData.TryGetValue("levelsCompleteStatus", out var levelsCompleteStatus))
        {
            //Debug.Log($"levelsCompleteStatus value: {levelsCompleteStatus.Value.GetAs<int>()}");
            PlayerData.b_levelComplete = levelsCompleteStatus.Value.GetAs<bool[]>();
        }

        // Tutorials Completion status (array of bool)
        if (playerData.TryGetValue("tutorialsCompleteStatus", out var tutorialsCompleteStatus))
        {
            PlayerData.b_tutorialComplete = tutorialsCompleteStatus.Value.GetAs<bool[]>();
        }
        // elemental Runes In Stock (array of int)
        if (playerData.TryGetValue("elementalRunesInStock", out var elementalRunesInStock))
        {
            //Debug.Log($"elementalRunesInStock value: {elementalRunesInStock.Value.GetAs<int>()}");
            PlayerData.i_elementalRunesInStock = elementalRunesInStock.Value.GetAs<int[]>();
        }
        // Shop items (spells) left (array of bool)
        if (playerData.TryGetValue("spellInShopBoughtAlready", out var spellInShopBoughtAlready))
        {
            //Debug.Log($"spellInShopBoughtAlready value: {spellInShopBoughtAlready.Value.GetAs<int>()}");
            PlayerData.b_spellBoughtAlready = spellInShopBoughtAlready.Value.GetAs<bool[]>();
        }
        // Shop items (available runes) (array of ints)
        if (playerData.TryGetValue("shopBuyableERunes", out var shopBuyableERunes))
        {
            //Debug.Log($"shopBuyableERunes value: {shopBuyableERunes.Value.GetAs<int>()}");
            PlayerData.shopBuyableERunes = shopBuyableERunes.Value.GetAs<int[]>();
        }
        // Shop items (available runes left) (array of ints)
        if (playerData.TryGetValue("runesLeftInShop", out var runesLeftInShop))
        {
            //Debug.Log($"runesLeftInShop value: {runesLeftInShop.Value.GetAs<int>()}");
            PlayerData.i_elementalRunesPurchasesLeftInShop = runesLeftInShop.Value.GetAs<int[]>();
        }
        // Removed Ads (bool)
        if (playerData.TryGetValue("removeAdsState", out var removeAdsState))
        {
            //Debug.Log($"removeAdsState value: {removeAdsState.Value.GetAs<int>()}");
            PlayerData.b_removeAdsBought = removeAdsState.Value.GetAs<bool>();
        }
        // Season Pass obtained status (bool)
        if (playerData.TryGetValue("currentWizzardPassBought", out var currentWizzardPassBought))
        {
            //Debug.Log($"removeAdsState value: {removeAdsState.Value.GetAs<int>()}");
            PlayerData.b_wizzardPassBought = currentWizzardPassBought.Value.GetAs<bool>();
        }
        // Last Season Pass bought ID (int)
        if (playerData.TryGetValue("seasonPassBoughtID", out var seasonPassBoughtID))
        {
            //Debug.Log($"seasonPassBoughtID value: {seasonPassBoughtID.Value.GetAs<int>()}");
            PlayerData.i_lastSeasonPassBought = seasonPassBoughtID.Value.GetAs<int>();
        }
        // Last day log in in shop (int)
        if (playerData.TryGetValue("lastDayLogInShop", out var lastDayLogInShop))
        {
            //Debug.Log($"lastDayLogInShop value: {lastDayLogInShop.Value.GetAs<int>()}");
            PlayerData.i_lastDayLogInShop = lastDayLogInShop.Value.GetAs<int>();
        }
        // Last day log in in Main Menu scene (int)
        if (playerData.TryGetValue("lastDayLogInMainMenu", out var lastDayLogInMainMenu))
        {
            PlayerData.i_lastDayLogInMainMenu = lastDayLogInMainMenu.Value.GetAs<int>();
        }
        // Spells available to buy in shop (int)
        if (playerData.TryGetValue("shopBuyableSpells", out var shopBuyableSpells))
        {
            PlayerData.shopBuyableSpells = shopBuyableSpells.Value.GetAs<int[]>();
        }
        if (playerData.TryGetValue("b_dailyFreeClaimed", out var b_dailyFreeClaimed))
        {
            PlayerData.b_dailyFreeClaimed = b_dailyFreeClaimed.Value.GetAs<bool>();
        }
        if (playerData.TryGetValue("b_gameTermsAccepted", out var b_gameTermsAccepted))
        {
            PlayerData.b_gameTermsAccepted = b_gameTermsAccepted.Value.GetAs<bool>();
        }
        #region Magic Pass Data
        if (playerData.TryGetValue("i_magicPassManaExp", out var i_magicPassManaExp))
        {
            PlayerData.i_magicPassManaExp = i_magicPassManaExp.Value.GetAs<int>();
        }
        if (playerData.TryGetValue("i_magicPassLevel", out var i_magicPassLevel))
        {
            PlayerData.i_magicPassLevel = i_magicPassLevel.Value.GetAs<int>();
        }
        if (playerData.TryGetValue("b_magicPassRewardClaimed", out var b_magicPassRewardClaimed))
        {
            PlayerData.b_magicPassRewardClaimed = b_magicPassRewardClaimed.Value.GetAs<bool[]>();
        }
        if (playerData.TryGetValue("b_premiumPassPurchased", out var b_premiumPassPurchased))
        {
            PlayerData.b_premiumPassPurchased = b_premiumPassPurchased.Value.GetAs<bool>();
        }
        if (playerData.TryGetValue("i_lastSeasonPlayedID", out var i_lastSeasonPlayedID))
        {
            PlayerData.i_lastSeasonPlayedID = i_lastSeasonPlayedID.Value.GetAs<int>();
        }
        if (playerData.TryGetValue("b_seasonPosterShown", out var b_seasonPosterShown))
        {
            PlayerData.b_seasonPosterShown = b_seasonPosterShown.Value.GetAs<bool>();
        }
        #endregion

        #region Wizzard Studio Data
        // Wall Upgrades by each stage
        if (playerData.TryGetValue("i_wallUpgradeStage_TotalUpgrades", out var i_wallUpgradeStage_TotalUpgrades))
        {
            PlayerData.i_wallUpgradeStage_TotalUpgrades = i_wallUpgradeStage_TotalUpgrades.Value.GetAs<int[]>();
        }
        // Amount of Wall HP Increased
        if (playerData.TryGetValue("i_wallHPIncreased", out var i_wallHPIncreased))
        {
            PlayerData.i_wallHPIncreased = i_wallHPIncreased.Value.GetAs<int>();
        }
        // Amount of Wall HP recovery per second
        if (playerData.TryGetValue("i_wallHPRecoveryPerSecond", out var i_wallHPRecoveryPerSecond))
        {
            PlayerData.i_wallHPRecoveryPerSecond = i_wallHPRecoveryPerSecond.Value.GetAs<int>();
        }
        // Magics Upgrade Levels
        if (playerData.TryGetValue("i_magicUpgradeLevel", out var i_magicUpgradeLevel))
        {
            PlayerData.i_magicUpgradeLevel = i_magicUpgradeLevel.Value.GetAs<int[]>();
        }
        // Magics Damage Attribute Upgraded
        if (playerData.TryGetValue("i_magicDamageAttributeUpgradeAmount", out var i_magicDamageAttributeUpgradeAmount))
        {
            PlayerData.i_magicDamageAttributeUpgradeAmount = i_magicDamageAttributeUpgradeAmount.Value.GetAs<int[]>();
        }
        // Magics Resistance Attribute Upgraded
        if (playerData.TryGetValue("i_magicResistanceAttributeUpgradeAmount", out var i_magicResistanceAttributeUpgradeAmount))
        {
            PlayerData.i_magicResistanceAttributeUpgradeAmount = i_magicResistanceAttributeUpgradeAmount.Value.GetAs<int[]>();
        }
        // Magics Duration Attribute Upgraded
        if (playerData.TryGetValue("f_magicDurationAttributeUpgradedAmount", out var f_magicDurationAttributeUpgradedAmount))
        {
            PlayerData.f_magicDurationAttributeUpgradedAmount = f_magicDurationAttributeUpgradedAmount.Value.GetAs<float[]>();
        }
        // Magics Speed Attribute Upgraded
        if (playerData.TryGetValue("f_magicSpeedAttributeUpgradedAmount", out var f_magicSpeedAttributeUpgradedAmount))
        {
            PlayerData.f_magicSpeedAttributeUpgradedAmount = f_magicSpeedAttributeUpgradedAmount.Value.GetAs<float[]>();
        }
        
        #endregion

        print("All data loaded");
        AuthenticationManager.b_dataAlreadyLoadedFromCloud = true;
    }
    #endregion

}

#region Data Container
[System.Serializable]
public class Container
{
    // This Script is used to store data from player
    #region Player Data Container
    public int i_mana= 0;
    public int i_keys = 0;
    // Spells Data
    public bool[] b_spellCardObtained = new bool[100];
    public int[] i_spell_Cards_Obtained = new int[100];
    public int[] i_spell_Card_Level = new int[100];
    public  int[] i_awakening_Card_Level = new int[100];
    // Deck
    public GameObject[] deck_GameObjects = new GameObject[8];
    public GameObject[] go_spellsIndeck = new GameObject[8];
    public ScripteableCard[] spellsInDeckData = new ScripteableCard[8];
    public int[] i_CardSlotNumber = new int[8];
    public bool b_isDeckFull;
    public int[] i_spellInDeckByID = new int[8];

    //
    public int i_totalCardsInDeck;
    public int i_maxRound;
    public bool b_firstDataSave;
    public bool[] b_canAwake = new bool[100];

    #region Daily Missions
    // Mission requirements
    public int i_gamesPlayed_DailyMissionCount;
    public bool b_makeOnePull_DailyMissionComplete;
    public bool b_improveOneSpell_DailyMissionComplete;
    public bool b_allDailyMissionComplete;
    // Claimed status
    public bool b_dailyMission_One_Claimed, b_dailyMission_Two_Claimed,
        b_dailyMission_Three_Claimed, b_dailyMission_Four_Claimed, b_dailyMission_Five_Claimed;
    #endregion

    #region Story Levels Complete Control
    public bool[] b_levelComplete = new bool[100];
    #endregion

    #region Tutorial Data
    public bool[] b_tutorialComplete = new bool[25];
    #endregion

    #region Elemental Runes
    public int[] i_elementalRunesInStock = new int[100];
    #endregion

    #region Shop Data
    public bool[] b_spellBoughtAlready = new bool[3];
    public int[] shopBuyableSpells = new int[3];
    public int[] shopBuyableERunes = new int[3];
    public int[] i_elementalRunesPurchasesLeftInShop = new int[3];

    #endregion

    #region Purchased Items Data
    public bool b_removeAdsBought;
    public bool b_wizzardPassBought;
    public int i_lastSeasonPassBought;
    #endregion

    #region Date and Time
    public int i_lastDayLogInShop = -1, i_lastDayLogInMainMenu = -1;
    #endregion

    #endregion

    internal void ContainerGetDataFromPlayerData() // Before saving data from container we get the data updated from Player Data
    {
        // Get updated data from player Data
        i_mana = PlayerData.i_mana;
        i_keys = PlayerData.i_keys;
        b_spellCardObtained = PlayerData.b_spellCardObtained;
        i_spell_Cards_Obtained = PlayerData.i_spell_Cards_Obtained;
        i_spell_Card_Level = PlayerData.i_spell_Card_Level;
        i_awakening_Card_Level = PlayerData.i_awakening_Card_Level;
        i_CardSlotNumber = PlayerData.i_CardSlotNumber;
        i_maxRound = PlayerData.i_maxRound;
        b_firstDataSave = PlayerData.b_firstDataSave;
        b_canAwake = PlayerData.b_canAwake;
        go_spellsIndeck = PlayerData.go_spellsIndeck;

        #region Daily Missions
        i_gamesPlayed_DailyMissionCount = PlayerData.i_gamesPlayed_DailyMissionCount;
        b_makeOnePull_DailyMissionComplete = PlayerData.b_makeOnePull_DailyMissionComplete;
        b_improveOneSpell_DailyMissionComplete = PlayerData.b_improveOneSpell_DailyMissionComplete;
        b_allDailyMissionComplete = PlayerData.b_allDailyMissionComplete;
        // Claimed Status
        b_dailyMission_One_Claimed = PlayerData.b_dailyMission_One_Claimed;
        b_dailyMission_Two_Claimed = PlayerData.b_dailyMission_Two_Claimed;
        b_dailyMission_Three_Claimed = PlayerData.b_dailyMission_Three_Claimed;
        b_dailyMission_Four_Claimed = PlayerData.b_dailyMission_Four_Claimed;
        b_dailyMission_Five_Claimed = PlayerData.b_dailyMission_Five_Claimed;
        #endregion

        #region Story Levels
        b_levelComplete = PlayerData.b_levelComplete;
        #endregion

        #region Tutorials Data
        b_tutorialComplete = PlayerData.b_tutorialComplete;
        #endregion

        #region Deck
        deck_GameObjects = PlayerData.deck_GameObjects;
        spellsInDeckData = PlayerData.spellsInDeckData;
        b_isDeckFull = PlayerData.b_isDeckFull;
        i_spellInDeckByID = PlayerData.i_spellInDeckByID;
        #endregion

        #region Elemental Runes
    i_elementalRunesInStock = PlayerData.i_elementalRunesInStock;
        #endregion

        #region Shop Data
        b_spellBoughtAlready = PlayerData.b_spellBoughtAlready;
        shopBuyableSpells = PlayerData.shopBuyableSpells;
        shopBuyableERunes = PlayerData.shopBuyableERunes;
        i_elementalRunesPurchasesLeftInShop = PlayerData.i_elementalRunesPurchasesLeftInShop;
        #endregion

        #region Purchased Items Data
        b_removeAdsBought = PlayerData.b_removeAdsBought;
        b_wizzardPassBought = PlayerData.b_wizzardPassBought;
        i_lastSeasonPassBought = PlayerData.i_lastSeasonPassBought;
        #endregion

        #region Date and Time
        i_lastDayLogInShop = PlayerData.i_lastDayLogInShop;
        i_lastDayLogInMainMenu = PlayerData.i_lastDayLogInMainMenu;
        #endregion
    }
    internal void ContainerPassDataToPlayerData()
    {
        PlayerData.i_maxRound= i_maxRound;
        PlayerData.i_mana = i_mana;
        PlayerData.i_keys = i_keys;
        PlayerData.b_spellCardObtained= b_spellCardObtained;
        //PlayerData.deck_GameObjects = deck_GameObjects;
        PlayerData.go_spellsIndeck = go_spellsIndeck;
        PlayerData.i_spell_Card_Level = i_spell_Card_Level;
        PlayerData.i_awakening_Card_Level = i_awakening_Card_Level;
        PlayerData.i_spell_Cards_Obtained = i_spell_Cards_Obtained;
        PlayerData.i_CardSlotNumber = i_CardSlotNumber;
        PlayerData.b_firstDataSave= b_firstDataSave;
        PlayerData.b_canAwake = b_canAwake;

        #region Daily Missions
        PlayerData.i_gamesPlayed_DailyMissionCount = i_gamesPlayed_DailyMissionCount;
        PlayerData.b_makeOnePull_DailyMissionComplete = b_makeOnePull_DailyMissionComplete;
        PlayerData.b_improveOneSpell_DailyMissionComplete = b_improveOneSpell_DailyMissionComplete;
        PlayerData.b_allDailyMissionComplete = b_allDailyMissionComplete;
        // Claimed Status
        PlayerData.b_dailyMission_One_Claimed = b_dailyMission_One_Claimed;
        PlayerData.b_dailyMission_Two_Claimed = b_dailyMission_Two_Claimed;
        PlayerData.b_dailyMission_Three_Claimed = b_dailyMission_Three_Claimed;
        PlayerData.b_dailyMission_Four_Claimed = b_dailyMission_Four_Claimed;
        PlayerData.b_dailyMission_Five_Claimed = b_dailyMission_Five_Claimed;
        #endregion

        #region Story Levels
        PlayerData.b_levelComplete = b_levelComplete;
        #endregion

        #region Tutorials Data
        PlayerData.b_tutorialComplete = b_tutorialComplete;
        #endregion

        #region Deck
        PlayerData.deck_GameObjects = deck_GameObjects;
        PlayerData.spellsInDeckData = spellsInDeckData;
        PlayerData.b_isDeckFull = b_isDeckFull;
        PlayerData.i_spellInDeckByID = i_spellInDeckByID;
        #endregion

        #region Elemental Runes
        PlayerData.i_elementalRunesInStock = i_elementalRunesInStock;
        #endregion

        #region Shop Data
        PlayerData.b_spellBoughtAlready = b_spellBoughtAlready;
        PlayerData.shopBuyableSpells = shopBuyableSpells;
        PlayerData.shopBuyableERunes = shopBuyableERunes;
        PlayerData.i_elementalRunesPurchasesLeftInShop = i_elementalRunesPurchasesLeftInShop;
        #endregion

        #region Purchased Items Data
        PlayerData.b_removeAdsBought = b_removeAdsBought;
        PlayerData.b_wizzardPassBought = b_wizzardPassBought;
        PlayerData.i_lastSeasonPassBought = i_lastSeasonPassBought;
        #endregion

        #region Date and Time
        PlayerData.i_lastDayLogInShop = i_lastDayLogInShop;
        PlayerData.i_lastDayLogInMainMenu = i_lastDayLogInMainMenu;
        #endregion
    }


}
#endregion




