using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultsManager : MonoBehaviour
{

    #region UI visuals
    [Header("UI Panel Variables")]
    [SerializeField] private Animator animator_ResultsPanel;
    [SerializeField] private GameObject go_newRecord, go_bestScore;
    [SerializeField] private TextMeshProUGUI tmp_currentWavesRecord, tmp_wavesCompleted,
        tmp_totalManaObtained, tmp_totalPlayerMana;
    #endregion

    #region External Variables
    [Header("External References")]
    [SerializeField] RoundsManager script_roundsManager;
    #endregion

    #region Music and SFXs
    [Header("Music and Sfx")]
    [SerializeField] private AudioClip aC_resultsMusic;
    [SerializeField] private AudioClip aC_levelComplete;
    #endregion

    #region Demonitos Defeated
    [Header("Demonitos Boxes")]
    [SerializeField] private List<DemonitoBoxDataContainer> listOfDemonitosBoxes = new List<DemonitoBoxDataContainer>();
    private int[] arrayOfDemonitosDefeatedById = new int[100];
    #endregion

    #region Animator Control Variables
    int i_animatorTrigger = Animator.StringToHash("Show");
    #endregion

    #region Rewards Stats
    private enum LevelType { ClosedRewardLevel, EndlessWavesLvl}
    [SerializeField] private LevelType e_levelType;   
    int i_manaObtained = 0;
    #endregion

    #region Control Variables
    private float f_currentPlayerManaAmount;
    #endregion

    #region UNITY LIFE CYCLE
    void Start()
    { 
        // Subscribe to events
        Enemy.onEnemyDefeated += Enemy_onEnemyDefeated;
        WallManager.e_OnWallDestroyed += WallManager_e_OnWallDestroyed;

        // Set initial Mana Energy obtained and player mana
        f_currentPlayerManaAmount = PlayerData.i_mana;
        i_manaObtained = 0;

        // Unpause Level
        LevelsGnralManager.b_stopLevel = false;

        // Reset enemies defeated count
        for (int i = 0; i < arrayOfDemonitosDefeatedById.Length; i++)
        {
            arrayOfDemonitosDefeatedById[i] = 0;
        }
    }
    #endregion

    #region Events Methods
    private void Enemy_onEnemyDefeated(object sender, System.EventArgs e)
    {
        Enemy defeatedEnemy = (Enemy)sender;
        // Get enemy mana energy released
        i_manaObtained += defeatedEnemy.i_manaEnergyReleased;
        // Save enemy count for number of enemies defeated of this type
        arrayOfDemonitosDefeatedById[defeatedEnemy.i_id]++;
        // Save type of enemy defeated for later count of defeated enemies by type
    }
    private void WallManager_e_OnWallDestroyed(object sender, System.EventArgs e)
    {
        LevelsGnralManager.b_stopLevel = true;
        ShowResults();
    }

    public void ShowResults()
    {
        // Update Results Panel info
        ShowDemonitosDefeated();
        ShowNewRecordOrBestScore();
        ShowWavesCompleted();
        ShowManaObtained();

        // Reset Waves to avoid bugs
        script_roundsManager.i_currentWave = 1;

        // Add Mana Energy Obtained to Player current Mana
        if(e_levelType == LevelType.EndlessWavesLvl)
        {
            PlayerData.i_mana += i_manaObtained;
            ShowSumUpTotalManaEnergy(PlayerData.i_mana, i_manaObtained);
        }

        // Pause Enemies Actions
        LevelsGnralManager.b_stopLevel = true;

        // Play finish SFX
        AudioManager.instance.PlaySFxOneTime(aC_levelComplete);

        //Show results panel
        if(animator_ResultsPanel != null)
            animator_ResultsPanel.SetTrigger(i_animatorTrigger);

        // Play Results Panel Music
        AudioManager.instance.PlayNewMusicClip(aC_resultsMusic);

        // Save Data
        SaveData.instance.AssignDataAndSave();
    }
    #endregion

    #region Results Methods
    private void ShowDemonitosDefeated()
    {
        int i_demonitoID = 0;
        int i_countOfDemonitosDefeated = 0;
        for(int i = 0; i < listOfDemonitosBoxes.Count; i++)
        {
            i_demonitoID = listOfDemonitosBoxes[i].demonitoInfo.i_id;
            i_countOfDemonitosDefeated = arrayOfDemonitosDefeatedById[i_demonitoID];
            if(i_countOfDemonitosDefeated > 0)
            {
                listOfDemonitosBoxes[i].tmp_numberOfDemonitosDefeated.text = i_countOfDemonitosDefeated.ToString();
                if (listOfDemonitosBoxes[i] != null)
                {
                    listOfDemonitosBoxes[i].gameObject.SetActive(true);
                }
            }
            else
            {
                if (listOfDemonitosBoxes[i] != null)
                    listOfDemonitosBoxes[i].gameObject.SetActive(false);
            }
        }
    }
    private void ShowNewRecordOrBestScore()
    {
        if (script_roundsManager.i_currentWave - 1 > PlayerData.i_maxRound) // New Record
        {
            // Save New Record
            PlayerData.i_maxRound = script_roundsManager.i_currentWave - 1;
            tmp_wavesCompleted.text = (script_roundsManager.i_currentWave - 1).ToString();
            // Show New Record text and hide Best Score text
            if (go_newRecord != null)
                go_newRecord.SetActive(true);
            if (go_bestScore != null)
                go_bestScore.SetActive(false);
        }
        else // Not best score
        {
            // Pass Best score to TMP visual
            tmp_currentWavesRecord.text = PlayerData.i_maxRound.ToString();
            tmp_wavesCompleted.text = (script_roundsManager.i_currentWave - 1).ToString();
            // Show New Record text and hide Best Score text
            if (go_newRecord != null)
                go_newRecord.SetActive(false);
            if(go_bestScore != null)
                go_bestScore.SetActive(true);
        }
    }
    private void ShowWavesCompleted()
    {
        int wavesCompleted = script_roundsManager.i_currentWave - 1;
        tmp_wavesCompleted.text = wavesCompleted.ToString();
    }
    private void ShowManaObtained()
    {
        tmp_totalPlayerMana.text = PlayerData.i_mana.ToString();
        tmp_totalManaObtained.text = "+" + i_manaObtained.ToString();
    }
    internal void ShowSumUpTotalManaEnergy(float f_newTotalManaAmount, float f_manaObtained)
    {
        // Stop coroutines to avoid weird effects
        StopCoroutine(SumUpTotalMana(f_newTotalManaAmount));
        StopCoroutine(ReduceManaObtained(f_manaObtained));
        // Start coroutines to show mana obtained effect
        StartCoroutine(SumUpTotalMana(f_newTotalManaAmount));
        StartCoroutine(ReduceManaObtained(f_manaObtained));
    }
    IEnumerator SumUpTotalMana(float f_newTotalManaAmount)
    {
        yield return new WaitForSeconds(2f);
        float f_duration = 1f;
        var rate = Mathf.Abs(f_newTotalManaAmount - f_currentPlayerManaAmount) / f_duration;
        while (f_currentPlayerManaAmount != f_newTotalManaAmount)
        {
            f_currentPlayerManaAmount = Mathf.MoveTowards(f_currentPlayerManaAmount, f_newTotalManaAmount, rate * Time.deltaTime);
            tmp_totalPlayerMana.text = ((int)f_currentPlayerManaAmount).ToString("f0");

            yield return null;
        }
    }
    IEnumerator ReduceManaObtained(float f_manaObtained)
    {
        yield return new WaitForSeconds(2f);
        float f_targetAmount = 0;
        float f_duration = 1f;
        var rate = Mathf.Abs(f_targetAmount - f_manaObtained) / f_duration;
        while (f_manaObtained != f_targetAmount)
        {
            f_manaObtained = Mathf.MoveTowards(f_manaObtained, f_targetAmount, rate * Time.deltaTime);
            tmp_totalManaObtained.text = "+" + ((int)f_manaObtained).ToString("f0");
            if (f_manaObtained == f_targetAmount)
            {
                tmp_totalManaObtained.gameObject.SetActive(false);
            }
            yield return null;
        }
    }
    #endregion



}
