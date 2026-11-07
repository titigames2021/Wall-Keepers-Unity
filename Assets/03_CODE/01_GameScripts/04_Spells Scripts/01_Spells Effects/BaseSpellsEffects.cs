using NUnit;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
//using static UnityEngine.EventSystems.EventTrigger;

public class BaseSpellsEffects : MonoBehaviour
{
    #region Local Stats variables
    [SerializeField] internal Animator effect_Animator;
    internal bool b_spellEffectActiveInBattlefield = false;
    protected float f_damage, f_resistance, f_speed, f_duration, f_repeatRate, f_slowDownValue, f_initialSpeed;
    protected bool b_mergedCard;
    internal Transform spellCardParent;
    [SerializeField] internal List <Enemy> enemiesScripts_List = new List<Enemy>();
    protected int i_EnemyLayer = 10;
    public int GetEnemyLayer() { return i_EnemyLayer; }
    internal enum SpellElement { None, Fire, Earth, Water, Wind, Lighting };
    [SerializeField] internal SpellElement e_spellElement;

    #endregion

    #region Animations
    [Header("Animations")]
    protected int i_EnterLoop = Animator.StringToHash("Enter Loop"),
        i_EnterEnd = Animator.StringToHash("Enter End"),
        i_EnterExtraLoop = Animator.StringToHash("Enter Extra Loop"),
        i_EnterExtraEnd = Animator.StringToHash("Enter Extra End");
    #endregion

    #region Spell Sfx
    [Header("Spell Stats")]
    [SerializeField] protected AudioSource aS_spellAudioSource;
    [SerializeField] protected AudioClip aC_spellCastSfx;
    [SerializeField] protected AudioClip aC_hitSfx;
    #endregion

    #region Time Control
    protected float f_counter;
    protected bool b_effectStarted, b_effectFinished;
    #endregion

    #region Local Common Methods
    internal void SetSpellStatsInSpellEffect(Transform tParent, float dmg, float res, float speed, float dur, float repeatRate, float slowDownValue)
    {
        spellCardParent = tParent;
        f_damage = dmg;
        f_resistance = res;
        f_speed = speed;
        f_duration = dur;
        f_repeatRate = repeatRate;
        f_slowDownValue = slowDownValue;
    }
    public virtual void ActivateEffect(Vector3 starPosition, Vector3 target)
    {
        // Activate every single effect, override in each spell
    }
    internal void ResetEffect()
    {
        enemiesScripts_List.Clear();
        b_spellEffectActiveInBattlefield = false;
        transform.parent = spellCardParent;
        transform.position = spellCardParent.position;
        gameObject.SetActive(false);
    }
    #endregion

    #region Enemies 
    internal void AddToEnemiesList(Enemy enemy)
    {
        enemiesScripts_List.Add(enemy);
    }

    internal void RemoveFromEnemiesList(Enemy enemy)
    {
        enemiesScripts_List.Remove(enemy);
    }
    internal void RestoreEnemyTarget()
    {
        foreach (Enemy enemy in enemiesScripts_List)
        {
            enemy.RestoreTarget();
        }
    }

    internal virtual void HurtEnemies()
    {
        foreach (Enemy enemy in enemiesScripts_List)
        {
            enemy.ReceiveDamage((int)f_damage, e_spellElement);
        }
    }

    #endregion

    #region Animations and SFX
    protected void StartEndAnimation()
    {
        effect_Animator.SetTrigger(i_EnterEnd);
    }

    protected void StartLoopAnimation()
    {
        effect_Animator.SetTrigger(i_EnterLoop);
        b_effectStarted = true;
    }
    internal void PlaySpellCastSfx()
    {
        aS_spellAudioSource.PlayOneShot(aC_spellCastSfx);
    }

    #endregion

    internal void PlayHitSfx()
    {
        aS_spellAudioSource.Stop();
        aS_spellAudioSource.PlayOneShot(aC_hitSfx);
    }
    #region Time control
    internal void CheckIfLevelPaused()
    {

        if (LevelsGnralManager.b_stopLevel)
        {
            effect_Animator.speed = 0;
            return;
        }
        else // Check if Animator Speed is equal to 1
        {
            if (effect_Animator.speed != 1) // If not set back to 1
            {
                effect_Animator.speed = 1;
            }
        }
    }
   // For spells that stay on the battlefield for a while, we would need a timer that  replace a corrutine, so that they can be paused properly.
    internal void CastEffectWithTimer()
    {
        if (!LevelsGnralManager.b_stopLevel && b_effectStarted)
        {
            f_counter += Time.deltaTime;
            if (f_counter >= f_duration)
            {
                effect_Animator.SetTrigger(i_EnterEnd);
                f_counter = 0f;
                b_effectFinished = true;
            }
        }
    }
    internal void CastEffectWithTimerLakes()
    {
        if (!LevelsGnralManager.b_stopLevel && b_effectStarted)
        {
            f_counter += Time.deltaTime;
            if (f_counter >= f_duration)
            {
                effect_Animator.SetTrigger(i_EnterExtraEnd);
                f_counter = 0f;
                b_effectFinished = true;
            }
        }
    }
    #endregion 


}
