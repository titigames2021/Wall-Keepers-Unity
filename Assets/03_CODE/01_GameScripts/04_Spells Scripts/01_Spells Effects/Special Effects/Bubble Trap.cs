using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class BubbleTrap : BaseSpellsEffects
{
    #region Variables
    private Vector3 v3_endPosition;
    private Vector3 v3_startPosition;
    [SerializeField] private Vector3 v3_Projectail_Cast_Position;
    private bool b_startMovement;
    private Quaternion InitialRotation;
    [SerializeField] private bool b_finishAnimation;
    private float f_journeyLength;
    private float startTime;
    #endregion

    #region Bubble Special Variables
    private Enemy trappedEnemy;
    bool b_enemyTrappedAlready;
    #endregion

    #region Unity Life Cycle
    private void Start()
    {
        InitializeVars();
    }

    private void Update()
    {
        CheckIfLevelPaused();
        if (LevelsGnralManager.b_stopLevel)
        {
            return;
        }
        if (b_spellEffectActiveInBattlefield && b_startMovement)
        {
            ProjectileMovement();
        }
         
        if (b_spellEffectActiveInBattlefield && !b_startMovement)
        {
            if (!b_effectFinished)
            {
                BubbleTimer();
            }
        }
    }
    internal void OnDisable()// Reset 
    {
        b_effectFinished = false;
    }
    #endregion

    #region Base Methods
    private void InitializeVars()
    {
        InitialRotation = transform.rotation;
        f_initialSpeed = f_speed;
    }
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
        v3_endPosition = target;
        f_journeyLength = Vector3.Distance(v3_startPosition, v3_endPosition);
    }

    private void ProjectileMovement()
    {
        //ROTATE TOWARDS TARGET
        Vector3 directionToTarget = v3_endPosition - transform.position;
        float angulo = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo - 90));
        //INTERPOLATION
        //We need to know the proportion of the journey that has been completed, using fractionOfJourney to make a good interpolation between the start and end positions.
        //Without this, the projectile takes the same time to reach all positions.
        float f_distCovered = (Time.time - startTime) * f_speed;
        float fractionOfJourney = f_distCovered / f_journeyLength;
        transform.position = Vector3.Lerp(v3_startPosition, v3_endPosition, fractionOfJourney);
        if (transform.position == v3_endPosition)
        {
            // Trap Enemies inside Bubble
            StartBubbleTrap();
            b_startMovement = false;
        }
    }

    // When everything is finished, the card is disabled
    private void StartBubbleTrap()
    {
        effect_Animator.SetTrigger(i_EnterEnd);
    }
    private void BubbleTimer()
    {
        if (!LevelsGnralManager.b_stopLevel && !b_effectFinished)
        {
            f_counter += Time.deltaTime;
            if (f_counter >= f_duration)
            {
                Explosion();
                f_counter = 0f;
                b_effectFinished = true;
            }
        }
    }
    public void TrapSmallSizeEnemyInsideBubble()
    {
        // Check if enemy already trapped inside bubble
        if(trappedEnemy != null) { return; }

        if(b_enemyTrappedAlready) { return; }

        // trap first hitted enemy inside Bubble
        for (int i = 0; i < enemiesScripts_List.Count; i++)
        {
            // Check for first small size enemy hitted
            if (enemiesScripts_List[i].baseStats.e_size == ScripteableEnemies.Size.small)
            {
                trappedEnemy = enemiesScripts_List[i];
                trappedEnemy.f_speed = 0f;
                trappedEnemy.transform.position = transform.position;
                b_enemyTrappedAlready = true;
                print("Trapped enemy is " + trappedEnemy.name);
                break;
            }
        }
    }
    public void ReleaseEnenmyFromBubble()
    {
        if(trappedEnemy != null)
        {
            trappedEnemy.f_speed = trappedEnemy.f_startSpeed;
            trappedEnemy = null;
        }
    }
    public void SetTrappedEnemyToNull()
    {
        print("Reset trapped enemy");
        if (trappedEnemy != null)
            trappedEnemy = null;

        b_enemyTrappedAlready = false;
    }
    private void Explosion()
    {
        print("Explode");
        ReleaseEnenmyFromBubble();
        effect_Animator.SetTrigger(i_EnterExtraLoop);
    }
    #endregion

    #region Animation Methods
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == i_EnemyLayer)
        {
            RemoveFromEnemiesList(collision.gameObject.GetComponent<Enemy>());
        }
    }

    internal override void HurtEnemies()
    {
        foreach (Enemy enemy in enemiesScripts_List)
        {
            enemy.ReceiveDamage((int)f_damage, e_spellElement);
        }
    }
    #endregion
}
