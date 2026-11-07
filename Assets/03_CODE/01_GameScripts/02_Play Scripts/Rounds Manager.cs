using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RoundsManager : MonoBehaviour
{
    #region
    public static event EventHandler ev_OnLevelComplete;
    #endregion

    #region UI Round/Waves counter panel
    [Header("Next Wave Panel Variables")]
    [SerializeField] Animator animator_NextWave;
    [SerializeField] private TMP_Text tmp_nextWaveText;
    [SerializeField] private TMP_Text tmp_nextWaveText_PauseMenu;
    #endregion

    #region Animator Variables
    private int i_startTriggerHash = Animator.StringToHash("Start");
    private int i_nextWaveTriggerHash = Animator.StringToHash("Next Wave");
    #endregion

    #region Waves/Rounds count variables
    internal int i_currentWave;
    internal static List<Enemy> listOfEnemiesInRound = new List<Enemy>();
    #endregion

    #region External Variables
    EnemiesManager script_enemiesManager;
    #endregion

    #region UNITY life Cycle
    private void Start()
    {
        // Get Enemies Manager script to access spawn enemies method
        GameObject go = GameObject.Find("Enemies Manager");
        if(go != null)
        {
            script_enemiesManager = go.GetComponent<EnemiesManager>();
        }

        i_currentWave = 1;
        print("Current Wave is " + i_currentWave);
        print("Current Max Wave is " + script_enemiesManager.GetLevelMaxRounds());

        // Subscribe to events
        Enemy.onEnemyDefeated += Enemy_onEnemyDefeated;

        // Reset list of Enemies at Start
        listOfEnemiesInRound.Clear();
    }
    private void OnDestroy()
    {
        Enemy.onEnemyDefeated -= Enemy_onEnemyDefeated;
    }
    private void Enemy_onEnemyDefeated(object sender, System.EventArgs e)
    {
        // Check if still spawning enemies
        if (!script_enemiesManager.IsSpawningEnemies())
        {
            // Check list of enemies of this round
            if(listOfEnemiesInRound.Count <= 0)
            {
                // Add check coming from enemies manager to know if level has limited rounds and if last round was this.
                // if so end level and show results panel (for Limited Rounds Levels)
                if(script_enemiesManager.GetLevelMaxRounds() != 0) // 0 means endless wave mode, no max round
                {
                    // use bool to avoid error of checking other levels Max Rounds
                    if (i_currentWave == script_enemiesManager.GetLevelMaxRounds() && script_enemiesManager.b_checkThisEnemiesManager) // Max Wave reached
                    {
                        // Level Complete, show level comlete panel
                        print("Level Complete");
                        script_enemiesManager.b_checkThisEnemiesManager = false;
                        i_currentWave = 1; // reset Current Wave
                        ev_OnLevelComplete?.Invoke(this, EventArgs.Empty);
                        return;
                    }
                }

                // All enemies of this wave/round defeated, move to next round
                i_currentWave++;
                tmp_nextWaveText.text = "Wave " + i_currentWave.ToString();
                tmp_nextWaveText_PauseMenu.text = "Wave " + i_currentWave.ToString();
                
                if(animator_NextWave != null) // Special check as Round Manager instance destroys the original
                    animator_NextWave.SetTrigger(i_nextWaveTriggerHash);
            }
        }
    }
    #endregion

    #region Local Methods
    public void StartEndlessWaveLevel()
    {
        animator_NextWave.SetTrigger(i_startTriggerHash);
    }
    internal void SpawnFirstWaveEnemies()
    {
        script_enemiesManager.SetSpawnEnemiesToTrue();
        LevelsGnralManager.b_stopLevel = false;
    }
    #endregion
}
