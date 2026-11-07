using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowProjectile : BaseEnemyAction
{
    private bool b_startProjectileMovement;
    [SerializeField] List<GameObject> listofProjectiles = new List<GameObject>(); 
 
    internal override void TakeAction(GameObject obstacle, GameObject wall)
    {
        if (obstacle != null)
        {
            Obstacle_StaticInstant obstacleStats = obstacle.GetComponent<Obstacle_StaticInstant>();
            if (obstacleStats != null)
            {
                ActivateOneProjectail(obstacle);
                return;
            }
        }
        if (wall != null)
        {
            WallManager wallStats = wall.GetComponent<WallManager>();
            if (wallStats != null)
            {
                ActivateOneProjectail(wall);
            }
        }
    }
    private void ActivateOneProjectail( GameObject target)
    {
        for (int i = 0; i < listofProjectiles.Count; i++)
        {
            if (!listofProjectiles[i].GetComponent<EnemyProjectile>().b_startProjectileMovement)
            {
                //Set Projectile stats 
                listofProjectiles[i].GetComponent<EnemyProjectile>().SetStats(gameObject.transform.position, target, f_projectailSpeed, i_damage);
                listofProjectiles[i].SetActive(true);
                break;
            }
            else
            {
                print("No more projectails");
            }
        }
    }
    internal void OnDisable()//Reset Lightnings
    {
        for (int i = 0; i < listofProjectiles.Count; i++)
        {
            listofProjectiles[i].gameObject.GetComponent<EnemyProjectile>().ResetProjectile();
        }
    }


}
