using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class HeavyMeleeAttack : BaseEnemyAction
{
    #region Action
    internal override void TakeAction(GameObject obstacle, GameObject wall)
    {
        if (obstacle != null)
        {
            Obstacle_StaticInstant obstacleStats = obstacle.GetComponent<Obstacle_StaticInstant>();
            if (obstacleStats != null)
            {
                AttackObstacle(obstacleStats);
                return;
            }
        }

        if (wall != null)
        {
            WallManager wallStats = wall.GetComponent<WallManager>();
            if (wallStats != null)
            {
                AttackWall(wallStats);
            }
        }
    }

    internal void AttackObstacle(Obstacle_StaticInstant obstacleStats)
    {
        obstacleStats.ReceiveDamage(i_damage);
    }

    internal void AttackWall(WallManager wallStats)
    {
        wallStats.ReceiveHit(i_damage);
    }
    #endregion
}
