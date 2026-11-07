using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousDmg_StaticInstant : BaseSpellsEffects
{
    #region Unity Life Cycle
    private void FixedUpdate()//1º Loop FireWall
    {
        CheckIfLevelPaused();
        if (!b_effectFinished)
        {
            CastEffectWithTimer();
        }
    }
    internal void OnDisable()//Reset Lightnings
    {
        b_effectFinished = false;
        enemiesScripts_List.Clear();
    }
    #endregion
    internal override void HurtEnemies()
    {
        foreach (Enemy enemy in enemiesScripts_List)
        {
            if (enemy.i_hp > 0)
            {
                enemy.ReceiveDamage((int)f_damage, e_spellElement);
            } 
        }
    }
    #region InstantArea Methods
    //Receive target 
    public override void ActivateEffect(Vector3 starPosition, Vector3 target)
    {
        SetTarget(target);
    }
    private void SetTarget(Vector3 target)
    {
        transform.parent = null;
        gameObject.transform.position = target;

    }
    //Wait until spell effect finish 
    #endregion
  
    #region SFX Play Methods
    internal void StopLoopSfx()
    {
        aS_spellAudioSource.Stop();
    }
    #endregion

    #region Collisions
    // When an enemy enters the area, repeatedly hurt the enemy
    private void  OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == i_EnemyLayer)
        {
            print("Enemy detected");
            AddToEnemiesList(collision.gameObject.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == i_EnemyLayer)
        {
            RemoveFromEnemiesList(collision.gameObject.GetComponent<Enemy>());
        }
    }
    #endregion
}
