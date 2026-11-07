using UnityEngine;

public static class PlayerData
{
    internal static bool b_gameTermsAccepted;

    internal static int i_keys;
    internal static int i_mana;
    internal static bool[] b_spellCardObtained = new bool[100];
    internal static int[] i_spell_Cards_Obtained = new int[100];
    internal static int[] i_spell_Card_Level = new int[100];
    internal static int[] i_awakening_Card_Level = new int[100];
    internal static bool[] b_canAwake = new bool[100];
    // Deck
    internal static GameObject[] deck_GameObjects = new GameObject[8]; // Don't save in cloud
    internal static GameObject[] go_spellsIndeck = new GameObject[8]; // Don't save in cloud
    internal static ScripteableCard[] spellsInDeckData = new ScripteableCard[8]; // Don't save in cloud
    internal static int[] i_spellInDeckByID = new int[8]; // Used to saved spells in deck (Is reassigned in main Scene)
    internal static int[] i_CardSlotNumber = new int[8];
    internal static bool b_isDeckFull;
    // Scores
    internal static int i_maxRound;

    internal static bool b_firstDataSave;

    #region Daily Missions
    // Requirements
    internal static int i_gamesPlayed_DailyMissionCount;
    internal static bool b_makeOnePull_DailyMissionComplete;
    internal static bool b_improveOneSpell_DailyMissionComplete;
    internal static bool b_allDailyMissionComplete;
    // Claimed status
    internal static bool b_dailyMission_One_Claimed, b_dailyMission_Two_Claimed,
        b_dailyMission_Three_Claimed, b_dailyMission_Four_Claimed, b_dailyMission_Five_Claimed;
    #endregion

    #region Story Levels Complete Control
    internal static bool[] b_levelComplete = new bool[100];
    #endregion

    #region Tutorial Data
    internal static bool[] b_tutorialComplete = new bool[25];
    #endregion

    #region Elemental Runes
    // Runes IDs: Fire = 0 // Water = 1 // Earth = 2 // Wind = 3 // Lighting = 4
    internal static int[] i_elementalRunesInStock = new int[100];
    #endregion

    #region Shop Data
    // Spells available in shop
    internal static bool[] b_spellBoughtAlready = new bool[3];
    internal static int[] shopBuyableSpells = new int[3];
    internal static int[] shopBuyableERunes = new int[3];
    internal static int[] i_elementalRunesPurchasesLeftInShop = new int[3];
    #endregion

    #region Purchased Items Data
    internal static bool b_dailyFreeClaimed;
    internal static bool b_removeAdsBought;
    internal static bool b_wizzardPassBought;
    internal static int i_lastSeasonPassBought;
    #endregion

    #region Date and Time
    internal static int i_lastDayLogInShop, i_lastDayLogInMainMenu;
    #endregion

    #region Magic Pass Data
    internal static int i_lastSeasonPlayedID = 1;
    internal static bool b_premiumPassPurchased;
    internal static int i_magicPassManaExp = 0, i_magicPassLevel = 0;
    internal static bool[] b_magicPassRewardClaimed = new bool[51];
    internal static bool b_seasonPosterShown;
    #endregion

    #region Wizzard Studio Data
    // Used to know how many upgrades has each stage
    internal static int[] i_wallUpgradeStage_TotalUpgrades = new int[6];
    internal static int i_wallHPIncreased = 0;
    internal static int i_wallHPRecoveryPerSecond = 0;
    // Upgrade Magic
    // Magics IDs: (0)General, (1)Fire, (2)Water, (3)Wind, (4)Lighting, (5)Earth
    internal static int[] i_magicUpgradeLevel = new int[30];
    // Spells Attributes Upgrade amount (Use magic IDs to later correctly assign)
    internal static int[] i_magicDamageAttributeUpgradeAmount = new int[30];
    internal static int[] i_magicResistanceAttributeUpgradeAmount = new int[30];
    internal static float[] f_magicDurationAttributeUpgradedAmount = new float[30];
    internal static float[] f_magicSpeedAttributeUpgradedAmount = new float[30];
    #endregion
}
