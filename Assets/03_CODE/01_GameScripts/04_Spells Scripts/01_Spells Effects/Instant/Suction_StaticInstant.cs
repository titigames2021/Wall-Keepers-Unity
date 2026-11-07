using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class Suction_StaticInstant : BaseSpellsEffects
{
    #region Additional SFX
    [Header("Additional SFX")]
    [SerializeField] AudioClip aC_loopSfx;
    #endregion

    #region Unity Life Cycle
    private void FixedUpdate()
    {
        CheckIfLevelPaused();
        if (b_spellEffectActiveInBattlefield)
        {
            if (!b_effectFinished)
            {
                CastEffectWithTimer();
            }
        }
    }
    private void OnDisable()
    {
         b_effectFinished = false;
        
        foreach (Enemy enemy in enemiesScripts_List)
        {
            print("Disable");
            enemy.RestoreTarget();
            enemy.OutOfAreaApplyingForceToEnemy();
        }
        // Empty List when effect is over
        enemiesScripts_List.Clear();
    }
    #endregion

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

    internal void ResetSucctionAreaSpell()
    {
        b_spellEffectActiveInBattlefield = false;
        transform.parent = spellCardParent;
        transform.position = spellCardParent.position;
        gameObject.SetActive(false);
    }


    internal IEnumerator CastInBattlefield()
    {
        WaitForSeconds wait_durationInBattlefield = new WaitForSeconds(f_duration);
        yield return wait_durationInBattlefield;
        
    }


 
  
    #endregion

    #region Suck Methods
    internal void Suck()
    {
        foreach (Enemy enemy in enemiesScripts_List)
        {
            enemy.moveToTarget_transform = gameObject.transform;
            enemy.b_beingAttracktedBySpell = true;
        }
    }

    #endregion

    #region SFX Play Methods
    internal void PlayLoopSfx()
    {
        aS_spellAudioSource.clip = aC_loopSfx;
        aS_spellAudioSource.Play();
    }
    internal void StopLoopSfx()
    {
        aS_spellAudioSource.Stop();
    }
    #endregion

    #region Collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == i_EnemyLayer)
        {
            AddToEnemiesList(collision.gameObject.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == i_EnemyLayer)
        {
            collision.gameObject.GetComponent<Enemy>().RestoreTarget();
            collision.gameObject.GetComponent<Enemy>().OutOfAreaApplyingForceToEnemy();
            RemoveFromEnemiesList(collision.gameObject.GetComponent<Enemy>());
        }
    }
    
    #endregion
}
