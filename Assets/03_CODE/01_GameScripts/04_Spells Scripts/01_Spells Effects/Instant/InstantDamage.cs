using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDamage : BaseSpellsEffects
{
    #region Unity Life Cycle
    private void FixedUpdate()
    {
        CheckIfLevelPaused();
    }
    #endregion  
    #region InstantArea Methods
    //Receive target 
    public override void ActivateEffect(Vector3 starPosition, Vector3 target)
    {
        SetTarget(target);
    }
    //Wait until spell effect finish 
     private void SetTarget(Vector3 target)
    {
        transform.parent = null;
        gameObject.transform.position = target;

    }
    #endregion
    
    #region Collisions
    // When the enemy collides with this effect (when the collider is enabled), it calls the enemy's ReceiveDamage function.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == i_EnemyLayer)
        {
            AddToEnemiesList(collision.gameObject.GetComponent<Enemy>());
        }
    }
    #endregion
}
