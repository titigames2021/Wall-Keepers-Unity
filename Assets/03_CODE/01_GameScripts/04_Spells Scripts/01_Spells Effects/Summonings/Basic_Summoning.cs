using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Summoning : Obstacle_StaticInstant
{

    #region Reassigned Variables
    [SerializeField] float f_newFlinchForce = 1; 
    #endregion

    #region Special Animators variable
    int i_animatorAttackBool = Animator.StringToHash("Attacking");
    #endregion

    #region Special SFXs
    [SerializeField] private AudioClip aC_summoningAttackSfx;
    #endregion

    #region Unity Life Cycle
    private void Start()
    {
        f_flinchForce = f_newFlinchForce;
    }
    private void FixedUpdate()
    {
        CheckIfLevelPaused();
        if (!b_effectFinished)
        {
            CastEffectWithTimer();
            // Check for enemies inside area of Attack and if so play Attack Anim

            for (int i = 0; i < enemiesScripts_List.Count; i++)
            {
                if( enemiesScripts_List[i].e_enemyState == Enemy.EnemyState.Defeated)
                {
                    enemiesScripts_List.Remove(enemiesScripts_List[i]);
                    //RemoveFromEnemiesList(enemiesScripts_List[i]);
                }
            }


            if (enemiesScripts_List.Count > 0)
            {
                effect_Animator.SetBool(i_animatorAttackBool, true);
            }
            else
            {
                effect_Animator.SetBool(i_animatorAttackBool, false);
            }
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

    #region Coroutines 
    // When enemies collide with the wall, they stop and begin attacking it.
    private IEnumerator StopEnemyAfterDelay(Enemy enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        enemy.rb_Enemy.velocity = Vector2.zero;
        enemy.rb_Enemy.angularVelocity = 0f;  // Si también quieres detener cualquier rotación
    }
    #endregion

    #region Colliders
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == i_EnemyLayer)
        {
            AddToEnemiesList(collision.gameObject.GetComponent<Enemy>());
        }
    }
    #endregion

    #region Additional Methods
    public void PlaySummningAttackHitSfx()
    {
        aS_spellAudioSource.PlayOneShot(aC_summoningAttackSfx);
    }
    #endregion
}
