using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMoreEnemies : BaseEnemyAction
{
    [Header("Summoning Variables")]
    [SerializeField] private List<Transform> t_listOfSummoningSpots;
    [SerializeField] private List<Transform> t_listOfpossibleSummonings;
    internal override void TakeAction(GameObject obstacle, GameObject wall)
    {
        // Summon Enemies - one for each slot
        SummonEnemies();
    }

    private void SummonEnemies()
    {
        for(int i = 0; i < t_listOfSummoningSpots.Count; i++)
        {
            // Get random enemy of current rank
            int i_random = Random.Range(0, t_listOfpossibleSummonings.Count);
            // Instantiate new enemy
            GameObject go_newEnemy = Instantiate(t_listOfpossibleSummonings[i_random].gameObject,
                t_listOfSummoningSpots[i].position, Quaternion.identity);

            // Add new enemy to list
            //Enemy newEnemy = go_newEnemy.GetComponent<Enemy>();
            //RoundsManager.listOfEnemiesInRound.Add(newEnemy);

            // Activate enemy
            go_newEnemy.SetActive(true);
        }
    }
}
