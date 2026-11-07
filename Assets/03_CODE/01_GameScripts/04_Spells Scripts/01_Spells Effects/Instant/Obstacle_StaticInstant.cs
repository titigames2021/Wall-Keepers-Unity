using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class Obstacle_StaticInstant : BaseSpellsEffects
{
    #region Variables
    private bool b_flinched, b_canFlinch;
    private WaitForSeconds flintchTimer_WFS = new WaitForSeconds(0.5f);
    internal float f_flinchForce = 3.5f;
    private float f_stopEnemyDelay = 0.3f;
    #endregion

    #region Unity Life Cycle
    private void FixedUpdate()
    {
        CheckIfLevelPaused();
        if (!b_effectFinished)
        {
            CastEffectWithTimer();
        }
    }
    private void OnDisable()
    {
        b_effectFinished = false;
    }
    #endregion

    #region Inherit Methods
    public override void ActivateEffect(Vector3 starPosition, Vector3 target)
    {
        SetTarget(target);
    }
      
    private void SetTarget(Vector3 target)
    {
        transform.parent = null;
        gameObject.transform.position = target;
    }
    #endregion

    #region RockWall Methods
    // When enemies collide with the wall, they stop and begin attacking it.
    public void Flinch()
    {
        foreach (Enemy enemy in enemiesScripts_List)
        {
            if (enemy.f_speed <= 0) { return; } // Don't push enemies trapped

            enemy.rb_Enemy.AddForce(transform.up * f_flinchForce, ForceMode2D.Impulse);
            StartCoroutine(StopEnemyAfterDelay(enemy, f_stopEnemyDelay));
        }
    }
    private IEnumerator StopEnemyAfterDelay(Enemy enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        enemy.rb_Enemy.velocity = Vector2.zero;
        enemy.rb_Enemy.angularVelocity = 0f;  // Si también quieres detener cualquier rotación
    }
    internal bool ReceiveDamage(int i_damageReceived)
    {
        f_resistance -= i_damageReceived;
        if (f_resistance < 0)
        {
            StartEndAnimation();
            return false;
        }
        else
        {
              return true;
        }
      
    }
    #endregion

    #region Colliders
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == i_EnemyLayer)
        {
            AddToEnemiesList(collision.gameObject.GetComponent<Enemy>());
        }
    }
    #endregion
}
