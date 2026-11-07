using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TsunamiWaveeffect : BaseSpellsEffects
{
    #region Variables
    private bool b_startMoving;
    [SerializeField] internal Vector3 targetToMove, startPosition, v3_correctSizeWithNoParent;
    [SerializeField] private float f_waveSpeed;
    #endregion

    #region Unity Life Cycle
    private void Update()
    {
        CheckIfLevelPaused();

        // Move wave
        if (b_startMoving)
        {
            if (!LevelsGnralManager.b_stopLevel)
            {
                f_counter += Time.deltaTime;
                if (f_counter >= f_duration)
                {
                    effect_Animator.SetTrigger(i_EnterEnd);
                    f_counter = 0f;
                    b_startMoving = false;
                }
                else
                {
                    // Move wave forward
                    var step = f_waveSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, targetToMove, step);
                }
            }
        }
    }
    private void OnDisable()
    {
        GetEnemiesOutOfWave();
        f_counter = 0;
        b_effectFinished = false;
        b_startMoving = false;
        b_spellEffectActiveInBattlefield = false;
    }
    #endregion

    #region Inherit Methods
    public override void ActivateEffect(Vector3 starPosition, Vector3 target)
    {
        SetTarget(target);
        b_spellEffectActiveInBattlefield = true;
        // Reset size
        transform.localScale = v3_correctSizeWithNoParent;
    }

    private void SetTarget(Vector3 target)
    {
        transform.parent = null;
        gameObject.transform.position = startPosition;
    }
    #endregion

    #region Tsunami Exclusive methods
    public void GetEnemiesOutOfWave()
    {
        foreach(Enemy enemyInsideWave in enemiesScripts_List)
        {
            if (enemyInsideWave != null) 
            {
                // Option 1
                enemyInsideWave.f_speed = enemyInsideWave.f_startSpeed;
                enemyInsideWave.transform.parent = null;
                enemyInsideWave.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    internal void StartMovingWave()
    {
        b_startMoving = true;
    }
    #endregion

    #region Colliders
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == i_EnemyLayer)
        {
            // Store enemies inside to later release them
            AddToEnemiesList(collision.gameObject.GetComponent<Enemy>());
            Enemy enemyHitted = collision.gameObject.GetComponent<Enemy>();

            // Check if enemy is not inmune to being pull
            if (!enemyHitted.b_inmuneToBeingPulledOrPushedByExternalForced)
            {
                // Set as child to move enemies with it
                collision.transform.parent = transform;
            }
            // Deal damage once to enemies
            enemyHitted.ReceiveDamage((int)f_damage, e_spellElement);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == i_EnemyLayer)
        {
            collision.transform.parent = null;
            collision.transform.localScale = new Vector3(1, 1, 1);

            //Enemy enemyHitted = collision.gameObject.GetComponent<Enemy>();
            //enemyHitted.RestoreTarget();
            //enemyHitted.OutOfAreaApplyingForceToEnemy();
        }
    }
    #endregion
}
