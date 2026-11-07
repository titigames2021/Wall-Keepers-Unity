using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyMissionsManager : MonoBehaviour
{
    // Manages current state of daily missions and daily reset

    #region Daily Missions variables
    // 1 - Play one game; 2 - Play 3 games; 3 - Make one pull; 4 - Improve one card; 5 complete all missions.
    [SerializeField] MissionsBoxes script_dailyMission_1, script_dailyMission_2, script_dailyMission_3, script_dailyMission_4,
        script_dailyMission_5;
    [SerializeField] Animator canClaimIcon_animator;
    int i_animatorString = Animator.StringToHash("CanClaim");
    public static int i_countOfDMissionsClaimable = 0;
    #endregion

    #region Unity Life Cycle
    private void Start()
    {
        i_countOfDMissionsClaimable = 0;

        CheckIfNewDayAndUpdateDailyMissions();

        SaveData.instance.AssignDataAndSave();
    }
    #endregion

    private void CheckIfNewDayAndUpdateDailyMissions()
    {
        if (PlayerData.i_lastDayLogInMainMenu != DateTime.Now.Day) // Is new Day, refresh shop items
        {
            print("Resets new day");
            ResetAllDailyMissions();
            // Reset Shop Daily Free
            ResetShopDailyFreeManaEnergy();
            PlayerData.i_lastDayLogInMainMenu = DateTime.Now.Day; // Save New day as last day log in
        }

        SetDailyMissionsState();
    }

    private void ResetAllDailyMissions()
    {
        PlayerData.i_gamesPlayed_DailyMissionCount = 0;
        PlayerData.b_makeOnePull_DailyMissionComplete = false;
        PlayerData.b_improveOneSpell_DailyMissionComplete = false;
        PlayerData.b_allDailyMissionComplete = false;

        // reset Claimed control variable
        PlayerData.b_dailyMission_One_Claimed = false;
        PlayerData.b_dailyMission_Two_Claimed = false;
        PlayerData.b_dailyMission_Three_Claimed = false;
        PlayerData.b_dailyMission_Four_Claimed = false;
        PlayerData.b_dailyMission_Five_Claimed = false;

        SaveData.instance.AssignDataAndSave();
    }

    private void ResetShopDailyFreeManaEnergy()
    {
        PlayerData.b_dailyFreeClaimed = false;
    }

    private void SetDailyMissionsState()
    {
        // For Mission Five - All missions complete
        bool b_allMissionsComplete = true;
        int i_countOfMissionsComplete = 0;

        // Mission 1 - Play 1 Game
        int i_missionOneGamesPlayedRequired = 1;
        // Update text 
        script_dailyMission_1.tmp_missionsCompletedText.text = PlayerData.i_gamesPlayed_DailyMissionCount.ToString();
        if (PlayerData.i_gamesPlayed_DailyMissionCount >= i_missionOneGamesPlayedRequired)
        {
            i_countOfMissionsComplete += 1;
            // Check if mission already claimed
            if (PlayerData.b_dailyMission_One_Claimed) // Show claimed already
            {
                script_dailyMission_1.go_claimedEffect.SetActive(true);
            }
            else // Show can claim
            {
                script_dailyMission_1.go_canClaimEffect.SetActive(true);
                script_dailyMission_1.button_missionClaim.interactable = true;
                ShowCanClaimIcon();
                i_countOfDMissionsClaimable++;
            }
        }
        else
        {
            b_allMissionsComplete = false;
        }

        // Mission 2 - Play 3 Games
        int i_missionTwoGamesPlayedRequired = 3;
        // Update text 
        script_dailyMission_2.tmp_missionsCompletedText.text = PlayerData.i_gamesPlayed_DailyMissionCount.ToString();
        if (PlayerData.i_gamesPlayed_DailyMissionCount >= i_missionTwoGamesPlayedRequired)
        {
            i_countOfMissionsComplete += 1;
            // Check if mission already claimed
            if (PlayerData.b_dailyMission_Two_Claimed) // Show claimed already
            {
                script_dailyMission_2.go_claimedEffect.SetActive(true);
            }
            else // Show can claim
            {
                script_dailyMission_2.go_canClaimEffect.SetActive(true);
                script_dailyMission_2.button_missionClaim.interactable = true;
                ShowCanClaimIcon();
                i_countOfDMissionsClaimable++;
            }
        }
        else
        {
            b_allMissionsComplete = false;
        }

        // Mission 3 - Make 1 Pull
        if (PlayerData.b_makeOnePull_DailyMissionComplete)
        {
            i_countOfMissionsComplete += 1;
            // Update text 
            script_dailyMission_3.tmp_missionsCompletedText.text = "1";
            // Check if mission already claimed
            if (PlayerData.b_dailyMission_Three_Claimed) // Show claimed already
            {
                script_dailyMission_3.go_claimedEffect.SetActive(true);
            }
            else // Show can claim
            {
                script_dailyMission_3.go_canClaimEffect.SetActive(true);
                script_dailyMission_3.button_missionClaim.interactable = true;
                ShowCanClaimIcon();
                i_countOfDMissionsClaimable++;
            }
        }
        else
        {
            b_allMissionsComplete = false;
            // Update text 
            script_dailyMission_3.tmp_missionsCompletedText.text = "0";
        }

        // Mission 4 - Improve 1 spell
        if (PlayerData.b_improveOneSpell_DailyMissionComplete)
        {
            i_countOfMissionsComplete += 1;
            // Update text 
            script_dailyMission_4.tmp_missionsCompletedText.text = "1";
            // Check if mission already claimed
            if (PlayerData.b_dailyMission_Four_Claimed) // Show claimed already
            {
                script_dailyMission_4.go_claimedEffect.SetActive(true);
            }
            else // Show can claim
            {
                script_dailyMission_4.go_canClaimEffect.SetActive(true);
                script_dailyMission_4.button_missionClaim.interactable = true;
                ShowCanClaimIcon();
                i_countOfDMissionsClaimable++;
            }
        }
        else
        {
            b_allMissionsComplete = false;
            // Update text 
            script_dailyMission_4.tmp_missionsCompletedText.text = "0";
        }

        // Mission 5 - Complete all daily missions
        PlayerData.b_allDailyMissionComplete = b_allMissionsComplete;
        script_dailyMission_5.tmp_missionsCompletedText.text = i_countOfMissionsComplete.ToString();
        if (PlayerData.b_allDailyMissionComplete)
        {
            // Check if mission already claimed
            if (PlayerData.b_dailyMission_Five_Claimed) // Show claimed already
            {
                script_dailyMission_5.go_claimedEffect.SetActive(true);
            }
            else // Show can claim
            {
                script_dailyMission_5.go_canClaimEffect.SetActive(true);
                script_dailyMission_5.button_missionClaim.interactable = true;
                ShowCanClaimIcon();
                i_countOfDMissionsClaimable++;
            }
        }
    }

    private void ShowCanClaimIcon()
    {
        canClaimIcon_animator.SetBool(i_animatorString, true);
    }
    private void HideCanClaimIcon()
    {
        canClaimIcon_animator.SetBool(i_animatorString, false);
    }
    internal void CheckIfMoreMissionsToBeClaimed()
    {
        i_countOfDMissionsClaimable--;
        if(i_countOfDMissionsClaimable <= 0)
        {
            i_countOfDMissionsClaimable = 0;
            HideCanClaimIcon();
        }
    }
}
