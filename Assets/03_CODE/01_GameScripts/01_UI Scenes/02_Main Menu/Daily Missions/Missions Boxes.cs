using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionsBoxes : MonoBehaviour
{
    // Missions Visual Elements and Local logics
    [Header("Referenced Scripts")]
    [SerializeField] private MainMenuManager script_mainMenuManager;
    [SerializeField] private DailyMissionsManager script_dailyMissionsManager;
    #region Visual Elements
    [Header("Visual Elements")]
    [SerializeField] internal TextMeshProUGUI tmp_missionsCompletedText;
    [SerializeField] internal GameObject go_canClaimEffect, go_claimedEffect;
    #endregion

    #region Logic Elements
    [Header("Logic Elements")]
    [SerializeField] private int i_currentMissionNumber;
    [SerializeField] internal Button button_missionClaim;

    #endregion


    #region Local Methods
    internal void SetMissionText(int i_currentCompleted) // Rewrite mission with current completed updated
    {
        tmp_missionsCompletedText.text = i_currentCompleted.ToString();
    }
    #endregion

    #region Claim Mission Rewards methods
    public void ClaimManaReward(int i_manaRewarded)
    {
        button_missionClaim.interactable = false;
        go_canClaimEffect.SetActive(false);
        go_claimedEffect.SetActive(true);

        PlayerData.i_mana += i_manaRewarded;
        script_mainMenuManager.UpdateEffectManaEnergy(PlayerData.i_mana);
        MarkMissionsAsClaimed();

        script_dailyMissionsManager.CheckIfMoreMissionsToBeClaimed();

        SaveData.instance.AssignDataAndSave();
    }
    public void ClaimManaKeysRewards(int i_keysRewarded)
    {
        button_missionClaim.interactable = false;
        go_canClaimEffect.SetActive(false);
        go_claimedEffect.SetActive(true);

        PlayerData.i_keys += i_keysRewarded;
        script_mainMenuManager.ShowSumUpManaKeys(PlayerData.i_keys);
        MarkMissionsAsClaimed();

        script_dailyMissionsManager.CheckIfMoreMissionsToBeClaimed();

        SaveData.instance.AssignDataAndSave();
    }
    private void MarkMissionsAsClaimed()
    {
        switch (i_currentMissionNumber)
        {
            case 1:
                PlayerData.b_dailyMission_One_Claimed = true;
                break;
            case 2:
                PlayerData.b_dailyMission_Two_Claimed = true;
                break;
            case 3:
                PlayerData.b_dailyMission_Three_Claimed = true;
                break;
            case 4:
                PlayerData.b_dailyMission_Four_Claimed = true;
                break;
            case 5:
                PlayerData.b_dailyMission_Five_Claimed = true;
                break;
        }
    }
    #endregion
}
