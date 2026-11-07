using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown_StaticProjectail : BaseSpellsEffects
{
    #region Variables
    private Vector3 v3_endPosition;
    private Vector3 v3_startPosition;
    [SerializeField] private Vector3 v3_Projectail_Cast_Position;
    private float f_journeyLength;
    private float startTime;
    private bool b_startMovement;
    private Quaternion InitialRotation;
    private int i_start_spriteSortingOrder;
    private SpriteRenderer spell_SpriteRenderer;
    #endregion

    #region Unity Life Cycle
    private void Start()
    {
        GetStartVariables();
    }

    private void GetStartVariables()
    {
        spell_SpriteRenderer = GetComponent<SpriteRenderer>();
        i_start_spriteSortingOrder = spell_SpriteRenderer.sortingOrder;
    }
    private void FixedUpdate()
    {
        CheckIfLevelPaused();
        if (LevelsGnralManager.b_stopLevel)
        {
            return;
        }
        if (b_spellEffectActiveInBattlefield && b_startMovement)
        {
            ProjectailMovement();
        }
        if (!b_effectFinished)
        {
            CastEffectWithTimerLakes();
        }
    }
    internal void OnDisable()//Reset Lightnings
    {
        b_effectFinished = false;
    }
    #endregion

    #region Base Methods
    public override void ActivateEffect(Vector3 starPosition, Vector3 target)
    {
        // Set new behaviour for Projectiles
        CastBall(starPosition, target);
    }
    #endregion

    #region Projectile Methods
    // Method to save the target position
    public void CastBall(Vector3 starPosition, Vector3 target)
    {
        transform.parent = null;
        transform.position = starPosition;
        v3_startPosition = starPosition;
        print(starPosition);
        v3_endPosition = target;
        f_journeyLength = Vector3.Distance(v3_startPosition, v3_endPosition);
    }
    private void ProjectailMovement()
    {
        //ROTATE TOWARDS TARGET
        Vector3 directionToTarget = v3_endPosition - transform.position;
        float angulo = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo - 90));
        //INTERPOLATION
        //We need to know the proportion of the journey that has been completed, using fractionOfJourney to make a good interpolation between the start and end positions.
        //Without this, the projectile takes the same time to reach all positions.
        float f_distCovered = (Time.time - startTime) * 1.0f * f_speed;
        float fractionOfJourney = f_distCovered / f_journeyLength;
        transform.position = Vector3.Lerp(v3_startPosition, v3_endPosition, fractionOfJourney);
        if (transform.position == v3_endPosition)
        {
            Explosion();
            b_startMovement = false;
        }
    }
    // When everything is finished, the card is disabled
    private void Explosion()
    {
        effect_Animator.SetTrigger(i_EnterEnd);
    }
    private void StartLake()
    {
        effect_Animator.SetTrigger(i_EnterExtraLoop);
        b_effectStarted = true;
    }
    internal void StartMovement()
    {
        startTime = Time.time;
        effect_Animator.SetTrigger(i_EnterLoop);
        b_startMovement = true;
    }

    internal void RestoreRotation()
    {
        transform.rotation = InitialRotation;
    }
    internal void Exploded()
    {
        spell_SpriteRenderer.sortingOrder = spell_SpriteRenderer.sortingOrder - 1;
    }
    private void ResetVariables()
    {
        spell_SpriteRenderer.sortingOrder = i_start_spriteSortingOrder;
    }

    #endregion
    #region Collisions
    // When an enemy enters the area, repeatedly hurt the enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == i_EnemyLayer)// Enemy layer 
        {
            /*A Diferencia de otros scripts de Area, aqui hay que modificar una variable del enemigo
            justo cuando entra, por lo que creo que es mejor hacerlo individualmente,
            cuando un enemigo entra y no hacer un foreach a una lista */
           Enemy enemyScript =  collision.gameObject.GetComponent<Enemy>();
           SlowDown(enemyScript);
            AddToEnemiesList(collision.gameObject.GetComponent<Enemy>());// Esto es para el daño de impacto

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == i_EnemyLayer)
        {
            RemoveFromEnemiesList(collision.gameObject.GetComponent<Enemy>());  
            Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
            EnemyOut(enemyScript);
        }
      
    }
    internal void SlowDown(Enemy enemy)
    {
        
            enemy.f_speedDecreasedValue = f_slowDownValue;// To slow down I'll change this value of Enemy to decrease speed * f_slowDownValue 
        
    }
    private void EnemyOut(Enemy enemy)
    {
        enemy.RestoreSpeed();    
    }
    #endregion
}
