using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    #region Rounds Control variables, Passes info to round manager
    [Header("Max Rounds")]
    [Tooltip("For endless waves is O")]
    [SerializeField] private int i_levelMaxRound;
    internal bool b_checkThisEnemiesManager;
    public int GetLevelMaxRounds()
    {
        return i_levelMaxRound;
    }
    #endregion
    // Manages Pool of enemies and Spawn Methods

    #region Lists of Demonitos by Ranks
    [Header("Demonitos")]
    [SerializeField] private List<Enemy> listOf_enemies_Rank_1 = new List<Enemy>();
    [SerializeField] private List<Enemy> listOf_enemies_Rank_2 = new List<Enemy>();
    [SerializeField] private List<Enemy> listOf_enemies_Rank_3 = new List<Enemy>();
    [SerializeField] private List<Enemy> listOf_enemies_Rank_4 = new List<Enemy>();
    [SerializeField] private List<Enemy> listOf_enemies_Rank_5 = new List<Enemy>();
    #endregion

    #region Spawn Points
    [Header("Spawn Points")]
    [SerializeField] private List<Transform> listOf_spawnPoints = new List<Transform>();
    private bool[] arrayOf_spawnPointsBlocked = new bool[10]; // Used to avoid spawning more than 1 enemy on the same spawn point at the same time.
    float f_spawnRate = 1.0f;
    float f_spawnTimer = 0.0f;
    #endregion

    #region Spawn Rules
    [SerializeField] private bool b_spawnEnemies;
    [SerializeField] private bool b_spawnBoss;
    [SerializeField] private GameObject go_bossEnemy;
    [SerializeField] private Transform t_bossEnemySpwanPoint;
    // Start with 5 enemies of rank 1, every round get +1 enemy of current rank and +1 enemy of next rank if previous rank already has 10 enemies
    int i_countForNextRankEnemies = 10, i_rank_1_Enemies = 5, i_rank_2_Enemies = 0, i_rank_3_Enemies = 0,
        i_rank_4_Enemies = 0, i_rank_5_Enemies = 0;
    private int i_enemiesToSpawnOfRank_1 = 5, i_enemiesToSpawnOfRank_2, i_enemiesToSpawnOfRank_3,
        i_enemiesToSpawnOfRank_4, i_enemiesToSpawnOfRank_5;
    #endregion

    #region Unity Life Cycle
    private void Awake()
    {
        b_checkThisEnemiesManager = true;
    }
    private void LateUpdate()
    {
        // Check if game paused
        if (LevelsGnralManager.b_stopLevel)
        {
            return;
        }

        // Check if must spawn enemies
        if (!b_spawnEnemies) { return; } // Stop

        // Check max limit of enemies in screen to avoid lag
        if(RoundsManager.listOfEnemiesInRound.Count > 50)
        {
            return;
        }

        // Spawn enemies
        // While this round total enemies have not been spawn all, keep spawning.
        // Use Spawn Timer to avoid spawning enemies too close
        f_spawnTimer += Time.deltaTime;
        if (f_spawnTimer >= f_spawnRate)
        {
            f_spawnTimer = 0;
            SpawnEnemies(); // Spawns enemies according to the round counters
        }

        // Check if all enemies spawned for this round
        if (AllEnemiesSpawned())
        {
            b_spawnEnemies = false; // Stop spawning enemies
            EnemiesNumberForNextRound(); // Set Next round enemies counts
        }
    }
    #endregion


    #region Methods to Spawn Enemies
    internal void SpawnEnemies()
    {
        if (i_enemiesToSpawnOfRank_1 > 0) // Still enemies left to spawn
        {
            // Get Spawn point
            int i_spawnPoint = Random.Range(0, listOf_spawnPoints.Count);
            // Check if spawn point is valid
            if (!IsSpawnPointValid(i_spawnPoint)) // Spawn Point blocked
            {
                return;
            }

            // Do special Check for Boss Rush Play Mode
            if (b_spawnBoss)
            {
                b_spawnBoss = false;
                if(go_bossEnemy != null)
                {
                    GameObject go_newEnemy = Instantiate(go_bossEnemy, t_bossEnemySpwanPoint.position, Quaternion.identity);

                    // Add new enemy to list
                    //Enemy newEnemy = go_newEnemy.GetComponent<Enemy>();
                    //RoundsManager.listOfEnemiesInRound.Add(newEnemy);

                    // Activate enemy
                    go_newEnemy.SetActive(true);
                }
            }

            // Get random enemy of current rank
            int i_random = Random.Range(0, listOf_enemies_Rank_1.Count);

            // If enemy not spawned already, spawn
            if (!listOf_enemies_Rank_1[i_random].gameObject.activeSelf) // Enemy not active
            {
                // Reset enemy
                listOf_enemies_Rank_1[i_random].ResetEnemy();
                // Activate enemy in place
                listOf_enemies_Rank_1[i_random].transform.position = listOf_spawnPoints[i_spawnPoint].position;
                listOf_enemies_Rank_1[i_random].gameObject.SetActive(true);
            }
            else // Enemy already Spawned, instantiate new enemy of this type and added to list
            {
                // Instantiate new enemy
                GameObject go_newEnemy = Instantiate(listOf_enemies_Rank_1[i_random].gameObject,
                    listOf_spawnPoints[i_spawnPoint].position, Quaternion.identity);

                // Add new enemy to list
                Enemy newEnemy = go_newEnemy.GetComponent<Enemy>();
                listOf_enemies_Rank_1.Add(newEnemy);

                // Activate enemy
                go_newEnemy.SetActive(true);
            }
            // If enemy spawned, reduce enemies to spawn count
            i_enemiesToSpawnOfRank_1--;
            // Block Spawn point and reset after x seconds
            arrayOf_spawnPointsBlocked[i_spawnPoint] = true;
            StartCoroutine(ResetSpawnPoint(i_spawnPoint));
        }
        if (i_enemiesToSpawnOfRank_2 > 0) // Still enemies left to spawn
        {
            // Get Spawn point
            int i_spawnPoint = Random.Range(0, listOf_spawnPoints.Count);
            // Check if spawn point is valid
            if (!IsSpawnPointValid(i_spawnPoint)) // Spawn Point blocked
            {
                return;
            }

            // Get random enemy of current rank
            int i_random = Random.Range(0, listOf_enemies_Rank_2.Count);

            // If enemy not spawned already, spawn
            if (!listOf_enemies_Rank_2[i_random].gameObject.activeSelf) // Enemy not active
            {
                // Reset enemy
                listOf_enemies_Rank_2[i_random].ResetEnemy();
                // Activate enemy in place
                listOf_enemies_Rank_2[i_random].transform.position = listOf_spawnPoints[i_spawnPoint].position;
                listOf_enemies_Rank_2[i_random].gameObject.SetActive(true);
            }
            else // Enemy already Spawned, instantiate new enemy of this type and added to list
            {
                // Instantiate new enemy
                GameObject go_newEnemy = Instantiate(listOf_enemies_Rank_2[i_random].gameObject,
                    listOf_spawnPoints[i_spawnPoint].position, Quaternion.identity);

                // Add new enemy to list
                Enemy newEnemy = go_newEnemy.GetComponent<Enemy>();
                listOf_enemies_Rank_2.Add(newEnemy);

                // Activate enemy
                go_newEnemy.SetActive(true);
            }
            // If enemy spawned, reduce enemies to spawn count
            i_enemiesToSpawnOfRank_2--;
            // Block Spawn point and reset after x seconds
            arrayOf_spawnPointsBlocked[i_spawnPoint] = true;
            StartCoroutine(ResetSpawnPoint(i_spawnPoint));
        }
        if (i_enemiesToSpawnOfRank_3 > 0) // Still enemies left to spawn
        {
            // Get Spawn point
            int i_spawnPoint = Random.Range(0, listOf_spawnPoints.Count);
            // Check if spawn point is valid
            if (!IsSpawnPointValid(i_spawnPoint)) // Spawn Point blocked
            {
                return;
            }

            // Get random enemy of current rank
            int i_random = Random.Range(0, listOf_enemies_Rank_3.Count);

            // If enemy not spawned already, spawn
            if (!listOf_enemies_Rank_3[i_random].gameObject.activeSelf) // Enemy not active
            {
                // Reset enemy
                listOf_enemies_Rank_3[i_random].ResetEnemy();
                // Activate enemy in place
                listOf_enemies_Rank_3[i_random].transform.position = listOf_spawnPoints[i_spawnPoint].position;
                listOf_enemies_Rank_3[i_random].gameObject.SetActive(true);
            }
            else // Enemy already Spawned, instantiate new enemy of this type and added to list
            {
                // Instantiate new enemy
                GameObject go_newEnemy = Instantiate(listOf_enemies_Rank_3[i_random].gameObject,
                    listOf_spawnPoints[i_spawnPoint].position, Quaternion.identity);

                // Add new enemy to list
                Enemy newEnemy = go_newEnemy.GetComponent<Enemy>();
                listOf_enemies_Rank_3.Add(newEnemy);

                // Activate enemy
                go_newEnemy.SetActive(true);
            }
            // If enemy spawned, reduce enemies to spawn count
            i_enemiesToSpawnOfRank_3--;
            // Block Spawn point and reset after x seconds
            arrayOf_spawnPointsBlocked[i_spawnPoint] = true;
            StartCoroutine(ResetSpawnPoint(i_spawnPoint));
        }
        if (i_enemiesToSpawnOfRank_4 > 0) // Still enemies left to spawn
        {
            // Get Spawn point
            int i_spawnPoint = Random.Range(0, listOf_spawnPoints.Count);
            // Check if spawn point is valid
            if (!IsSpawnPointValid(i_spawnPoint)) // Spawn Point blocked
            {
                return;
            }

            // Get random enemy of current rank
            int i_random = Random.Range(0, listOf_enemies_Rank_4.Count);

            // If enemy not spawned already, spawn
            if (!listOf_enemies_Rank_4[i_random].gameObject.activeSelf) // Enemy not active
            {
                // Reset enemy
                listOf_enemies_Rank_4[i_random].ResetEnemy();
                // Activate enemy in place
                listOf_enemies_Rank_4[i_random].transform.position = listOf_spawnPoints[i_spawnPoint].position;
                listOf_enemies_Rank_4[i_random].gameObject.SetActive(true);
            }
            else // Enemy already Spawned, instantiate new enemy of this type and added to list
            {
                // Instantiate new enemy
                GameObject go_newEnemy = Instantiate(listOf_enemies_Rank_4[i_random].gameObject,
                    listOf_spawnPoints[i_spawnPoint].position, Quaternion.identity);

                // Add new enemy to list
                Enemy newEnemy = go_newEnemy.GetComponent<Enemy>();
                listOf_enemies_Rank_4.Add(newEnemy);

                // Activate enemy
                go_newEnemy.SetActive(true);
            }
            // If enemy spawned, reduce enemies to spawn count
            i_enemiesToSpawnOfRank_4--;
            // Block Spawn point and reset after x seconds
            arrayOf_spawnPointsBlocked[i_spawnPoint] = true;
            StartCoroutine(ResetSpawnPoint(i_spawnPoint));
        }
        if (i_enemiesToSpawnOfRank_5 > 0) // Still enemies left to spawn
        {
            // Get Spawn point
            int i_spawnPoint = Random.Range(0, listOf_spawnPoints.Count);
            // Check if spawn point is valid
            if (!IsSpawnPointValid(i_spawnPoint)) // Spawn Point blocked
            {
                return;
            }

            // Get random enemy of current rank
            int i_random = Random.Range(0, listOf_enemies_Rank_5.Count);

            // If enemy not spawned already, spawn
            if (!listOf_enemies_Rank_5[i_random].gameObject.activeSelf) // Enemy not active
            {
                // Reset enemy
                listOf_enemies_Rank_5[i_random].ResetEnemy();
                // Activate enemy in place
                listOf_enemies_Rank_5[i_random].transform.position = listOf_spawnPoints[i_spawnPoint].position;
                listOf_enemies_Rank_5[i_random].gameObject.SetActive(true);
            }
            else // Enemy already Spawned, instantiate new enemy of this type and added to list
            {
                // Instantiate new enemy
                GameObject go_newEnemy = Instantiate(listOf_enemies_Rank_5[i_random].gameObject,
                    listOf_spawnPoints[i_spawnPoint].position, Quaternion.identity);

                // Add new enemy to list
                Enemy newEnemy = go_newEnemy.GetComponent<Enemy>();
                listOf_enemies_Rank_5.Add(newEnemy);

                // Activate enemy
                go_newEnemy.SetActive(true);
            }
            // If enemy spawned, reduce enemies to spawn count
            i_enemiesToSpawnOfRank_5--;
            // Block Spawn point and reset after x seconds
            arrayOf_spawnPointsBlocked[i_spawnPoint] = true;
            StartCoroutine(ResetSpawnPoint(i_spawnPoint));
        }
    }
    private bool IsSpawnPointValid(int i_spawnPointIndex)
    {
        if (arrayOf_spawnPointsBlocked[i_spawnPointIndex])
        {
            return false; // Spawn point blocked
        }
        else {  return true; } // Spawn Point Available
    }
    private bool AllEnemiesSpawned()
    {
        int i_totalEnemiesToSpawn = i_enemiesToSpawnOfRank_1 + i_enemiesToSpawnOfRank_2 + i_enemiesToSpawnOfRank_3
            + i_enemiesToSpawnOfRank_4 + i_enemiesToSpawnOfRank_5;
        if (i_totalEnemiesToSpawn <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void EnemiesNumberForNextRound()
    {
        // If current round enemies all spawned then Increase number of enemies for next round
        i_rank_1_Enemies++;
        if(i_rank_1_Enemies >= i_countForNextRankEnemies)
        {
            i_rank_2_Enemies++;
        }
        if (i_rank_2_Enemies >= i_countForNextRankEnemies)
        {
            i_rank_3_Enemies++;
        }
        if (i_rank_3_Enemies >= i_countForNextRankEnemies)
        {
            i_rank_4_Enemies++;
        }
        if (i_rank_4_Enemies >= i_countForNextRankEnemies)
        {
            i_rank_5_Enemies++;
        }

        // Reset Enemies to Spawn
        ResetEnemiesToSpawn();
    }
    #endregion

    #region Reset Methods
    private void ResetEnemiesToSpawn()
    {
        i_enemiesToSpawnOfRank_1 = i_rank_1_Enemies;
        i_enemiesToSpawnOfRank_2 = i_rank_2_Enemies;
        i_enemiesToSpawnOfRank_3 = i_rank_3_Enemies;
        i_enemiesToSpawnOfRank_4 = i_rank_4_Enemies;
        i_enemiesToSpawnOfRank_5 = i_rank_5_Enemies;
    }
    IEnumerator ResetSpawnPoint(int i_spawnPointIndex)
    {
        float f_time = 1f;
        yield return new WaitForSeconds(f_time);
        arrayOf_spawnPointsBlocked[i_spawnPointIndex] = false;
    }
    #endregion

    #region Access Methods
    internal bool IsSpawningEnemies()
    {
        return b_spawnEnemies;
    }
    internal void SetSpawnEnemiesToTrue()
    {
        b_spawnEnemies = true;
    }
    #endregion
}
