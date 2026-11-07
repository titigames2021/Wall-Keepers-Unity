using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayModesManager : MonoBehaviour
{
    #region Local Variables
    [Header("Currencies")]
    [SerializeField] private TextMeshProUGUI tmp_playerManaEnergy;
    [SerializeField] private TextMeshProUGUI tmp_playerManaKeys;
    [Header("Levels Mode")]
    [SerializeField] Animator animator_levelsViewPort;
    [SerializeField] Transform transform_levelsPanel;
    [SerializeField] GameObject go_startLevelPanel;
    [SerializeField] TextMeshProUGUI tmp_selectedDayLevel, tmp_levelTotalWaves;
    [SerializeField] List<Levels_Boxes> listOf_levels = new List<Levels_Boxes>();
    private int i_lastLevelUnblocked = 0; // Use to set panel just in next level not unblocked

    [Header("Endless Waves Mode")]
    [SerializeField] Animator animator_endlessWaves;
    int i_animatorBool_Show = Animator.StringToHash("Show");
    [SerializeField] private TMP_Text tmp_wavesRecord;

    [Header("Boss Rush")]
    [SerializeField] Animator animator_bossRush;
    // Use same animator bool as Endless Waves
    #endregion

    #region Unity Life Cycle
    private void Start()
    {
        CheckLevelsState(); // Levels Play mode
        SetLevelsPanelToLastLevelUnlockedHeight(); // Play mode
        GetWavesRecord(); // Endless Waves
        UpdatePlayerCurrencies();
    }
    #endregion

    private void UpdatePlayerCurrencies()
    {
        tmp_playerManaEnergy.text = PlayerData.i_mana.ToString();
        tmp_playerManaKeys.text = PlayerData.i_keys.ToString();
    }

    #region Levels
    public void ShowLevelsViewPort()
    {
        animator_levelsViewPort.SetBool(i_animatorBool_Show, true);
    }
    public void CloseLevelsViewPort()
    {
        animator_levelsViewPort.SetBool(i_animatorBool_Show, false);
    }
    public void ShowSelectedLevelStartPanel(Levels_Boxes selectedLevel)
    {
        tmp_selectedDayLevel.text = "Day " + selectedLevel.s_levelDay;
        tmp_levelTotalWaves.text = "Total Waves " + selectedLevel.s_levelTotalWaves;
        go_startLevelPanel.SetActive(true);
    }
    private void CheckLevelsState() // Check if level is available, complete or blocked
    {
        for (int i = 0; i < listOf_levels.Count; i++)
        {
            if (PlayerData.b_levelComplete[i]) // If level complete
            {
                listOf_levels[i].button_Level.interactable = true; // Set interactable to true so it can be replayed
                listOf_levels[i].go_levelBlockedVisual.SetActive(false); // Remove blocked panel
                listOf_levels[i].go_rewardClaimedIcon.SetActive(true); // Mark as complete
                i_lastLevelUnblocked += 1;

                // Unblock next level
                if (i+1 < listOf_levels.Count) // Check if any level left
                {
                    listOf_levels[i+1].button_Level.interactable = true; // Set interactable to true so it can be replayed
                    listOf_levels[i+1].go_levelBlockedVisual.SetActive(false); // Remove blocked panel
                }
            }
        }

        // Unblock always first level
        listOf_levels[0].button_Level.interactable = true; // Set interactable to true so it can be replayed or played
        listOf_levels[0].go_levelBlockedVisual.SetActive(false);
    }
    private void SetLevelsPanelToLastLevelUnlockedHeight()
    {
        float f_startingHeight = -151.969f;
        float f_heightIncreaseByLevel = 300f;
        float f_height = f_startingHeight + i_lastLevelUnblocked * f_heightIncreaseByLevel;
        transform_levelsPanel.position = new Vector3(transform_levelsPanel.position.x, 
            f_height,
            transform_levelsPanel.position.z);
    }
    #endregion

    #region Endless Waves Methods
    public void PlayEndlessWavesMode()
    {
        animator_endlessWaves.SetBool(i_animatorBool_Show, true);
    }
    public void CloseEndlessWavesPlayMode()
    {
        animator_endlessWaves.SetBool(i_animatorBool_Show, false);
    }

    public void GetWavesRecord()
    {
        tmp_wavesRecord.text = "Record: "+PlayerData.i_maxRound.ToString();
    }
    #endregion

    #region Boss Rush Methods
    public void ShowBossRushLevelsViewPort()
    {
        animator_bossRush.SetBool(i_animatorBool_Show, true);
    }
    public void CloseBossRushLevelsViewPort()
    {
        animator_bossRush.SetBool(i_animatorBool_Show, false);
    }
    #endregion

}
