using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAction : MonoBehaviour
{
    [Header("Get Enemy Script with stats")]
    [SerializeField] protected Enemy enemyStats;
    #region Local variables
    protected int i_hp, i_damage;
    protected float f_speed;
    protected float f_projectailSpeed;
    #endregion

    #region Common Methods

    private void Start()
    {
        GetEnemyStats();
    }
    internal void GetEnemyStats()
    {
        i_hp = enemyStats.i_hp;
        i_damage = enemyStats.i_damage;
        f_speed = enemyStats.f_speed;
        f_projectailSpeed = enemyStats.f_projectailSpeed;
    }
    internal virtual void TakeAction(GameObject obstacle, GameObject wall)
    {
        // Define each enemy action in here
    }
    #endregion

}
