using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedWavesResultsPanels : MonoBehaviour
{
    #region Complete or Fail Variables
    [SerializeField] private int i_levelID;
    [SerializeField] private bool b_isBossRushLevel;
    [SerializeField] Animator animator_lmtWavesResultsPanel;
    private int i_animatorVariables = Animator.StringToHash("Show");
    [SerializeField] GameObject go_levelCompleteMenu;
    [SerializeField] GameObject go_levelFailedMenu;
    #endregion

    #region Rewards Control variables
    [Header("Rewards Control Variables")]
    [SerializeField] private GameObject go_rewardsAlreadyClaimed;
    [SerializeField] private bool b_spellRewarded;
    [SerializeField] private int i_spellID, i_spellAmount;
    [SerializeField] private bool b_elementalRunesRewarded;
    [SerializeField] private int i_runeID, i_runesAmount;
    [SerializeField] private bool b_manaKeysRewarded;
    [SerializeField] private int i_manaKeysObtained;
    [SerializeField] private bool b_manaEnergyReward;
    [SerializeField] private int i_manaEnergyObtained;
    #endregion

    #region Bug Control variables
    // To avoid bug that calls this script events even from other scenes
    bool b_scriptIsInScene = false;
    #endregion

    #region Unity Life Cycle
    // Start is called before the first frame update
    void Start()
    {
        b_scriptIsInScene = true;
        // Subscribe to events
        WallManager.e_OnWallDestroyed += WallManager_e_OnWallDestroyed;
        RoundsManager.ev_OnLevelComplete += RoundsManager_ev_OnLevelComplete;
    }
    private void OnDestroy()
    {
        WallManager.e_OnWallDestroyed -= WallManager_e_OnWallDestroyed;
        RoundsManager.ev_OnLevelComplete -= RoundsManager_ev_OnLevelComplete;
    }
    #endregion


    #region Events Methods
    private void WallManager_e_OnWallDestroyed(object sender, EventArgs e)
    {
        if(b_scriptIsInScene)
            LevelFailed();
    }
    private void RoundsManager_ev_OnLevelComplete(object sender, EventArgs e)
    {
        if (b_scriptIsInScene)
            LevelComplete();
    }
    #endregion

    #region Complete or Failed Methods
    public void LevelFailed()
    {
        // Stop game
        LevelsGnralManager.b_stopLevel = true;

        // Activa Level failed panel
        if(go_levelFailedMenu != null)
            go_levelFailedMenu.SetActive(true);

        // Reset control variable
        b_scriptIsInScene = false;

        // Unsuscribe from events to avoid weird errors
        WallManager.e_OnWallDestroyed -= WallManager_e_OnWallDestroyed;
        RoundsManager.ev_OnLevelComplete -= RoundsManager_ev_OnLevelComplete;

        // Show level failed Panel
        if(animator_lmtWavesResultsPanel != null)
            animator_lmtWavesResultsPanel.SetTrigger(i_animatorVariables);
    }
    private void LevelComplete()
    {
        // Stop game
        LevelsGnralManager.b_stopLevel = true;

        // Activa Level complete panel
        if(go_levelCompleteMenu != null)
            go_levelCompleteMenu.SetActive(true);

        // Show level complete Panel
        if(animator_lmtWavesResultsPanel != null)
            animator_lmtWavesResultsPanel.SetTrigger(i_animatorVariables);

        // Check for Boss Rush Levels. If Boss Rush don't do logic from levels checks
        if (b_isBossRushLevel)
        {
            // Give Boss Rush Level Reward
            if (b_elementalRunesRewarded) // Get runes reward
            {
                PlayerData.i_elementalRunesInStock[i_runeID] += i_runesAmount;
            }
            return;
        }

        // Check if this level was already complete
        if(PlayerData.b_levelComplete[i_levelID]) // If already completed then dont give rewards
        {
            // Show Reward already claimed visuals
            if(go_rewardsAlreadyClaimed != null)
                go_rewardsAlreadyClaimed.SetActive(true);
        }
        else // Give Rewards and Mark reward as obtained
        {
            // Check if Reward is spell
            if (b_spellRewarded)
            {
                // If spell obtained already directly add spell amount
                PlayerData.b_spellCardObtained[i_spellID] = true;
                PlayerData.i_spell_Cards_Obtained[i_spellID] += i_spellAmount;
            }
            if (b_elementalRunesRewarded) // Get runes reward
            {
                PlayerData.i_elementalRunesInStock[i_runeID] += i_runesAmount;
            }
            if (b_manaKeysRewarded)
            {
                PlayerData.i_keys += i_manaKeysObtained;
            }
            if (b_manaEnergyReward)
            {
                PlayerData.i_mana += i_manaEnergyObtained;
            }
        }

        // Mark level as Complete
        PlayerData.b_levelComplete[i_levelID] = true;

        // Reset control variable
        b_scriptIsInScene = false;

        // Unsuscribe from events to avoid weird errors
        WallManager.e_OnWallDestroyed -= WallManager_e_OnWallDestroyed;
        RoundsManager.ev_OnLevelComplete -= RoundsManager_ev_OnLevelComplete;

        // Save Data
        SaveData.instance.AssignDataAndSave();
    }
    #endregion
}
