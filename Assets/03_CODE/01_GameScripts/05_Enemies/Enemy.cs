using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region Events
    public static event EventHandler onEnemyDefeated;
    private bool b_eventCalled;
    #endregion

    #region Enemy Stats and Info
    [Header("Stats")]
    [SerializeField] internal ScripteableEnemies baseStats;
    internal int i_id, i_hp, i_maxHp, i_damage, i_manaEnergyReleased;
    internal float f_speed, f_startSpeed;
    float f_hpForHealthBar;
    internal float f_rangeOfAction, f_projectailSpeed;
    internal Rigidbody2D rb_Enemy;
    [SerializeField] private Animator enemy_animator;
    private bool b_CannotWalk;
    WallManager wallScript;
    Obstacle_StaticInstant targetObstacle; // cuando lo detecte el raycast guardarlo aqui 
    GameObject go_obstacle;
    GameObject go_wall;
    #endregion

    [Header("Enemy UI Variables")]
    [SerializeField] Image img_fillBar;
    [SerializeField] TextMeshProUGUI tmp_damageTaken;

    [Header("Enemy Sfxs")]
    [SerializeField] private AudioSource aS_enemy;
    [SerializeField] private AudioClip aC_enemyDefeated;

    #region Enemy States
    internal enum EnemyState { Moving, TakingAction, ReceiveDamage, Defeated, Attack, Stop }
    [Header("Enemy States")]
    [SerializeField] internal EnemyState e_enemyState;
    internal bool b_targetDetected = false;
    internal bool b_moveToTarget, b_receivingDamage, b_beingAttracktedBySpell;
    #endregion

    #region Moving Variables
    internal Vector3 targetToMove;
    internal Transform wallToTarget_transform;
    internal Transform moveToTarget_transform;
    internal Transform wall_moveToTarget_transform;
    private Transform wallTarget_transform;
    internal float f_speedDecreasedValue = 1.0f;
    #endregion

    #region Action
    [Header("Action")]
    [SerializeField] BaseEnemyAction enemyAction;
    public int i_spawnNumber;


    #endregion

    #region Inmunities
    [Header("Inmunities")]
    [SerializeField] internal bool b_inmuneToBeingPulledOrPushedByExternalForced;
    #endregion

    #region Animations
    private int i_WalkHash = Animator.StringToHash("Walking"),
        i_AttackHash = Animator.StringToHash("Attack"),
        i_HitHash = Animator.StringToHash("ReceiveHit"),
        i_DefeatedHash = Animator.StringToHash("Defeated");
    #endregion

    #region Unity Life Cycle
    private void OnEnable()
    {
        // Add enemy to list of enemies of this round
        RoundsManager.listOfEnemiesInRound.Add(this);

        GameObject go_wallMoveTo = GameObject.Find("Enemies Wall Move to Target");
        wall_moveToTarget_transform = go_wallMoveTo.transform;
        moveToTarget_transform = wall_moveToTarget_transform;
        RestoreTarget();
        SwitchState(EnemyState.Moving);
    }
    private void OnDisable()
    {
        StopCoroutine(SumUp());
    }
    private void Start()
    {
        SwitchState(EnemyState.Moving);
        GetStats();
        // Set Initial Move to target pos and wall pos.
        GameObject go_wallMoveTo = GameObject.Find("Enemies Wall Move to Target");
        wall_moveToTarget_transform = go_wallMoveTo.transform;
        moveToTarget_transform = wall_moveToTarget_transform;
        RestoreTarget();
    }

    private void Update()
    {
        // Check if level paused
        if (LevelsGnralManager.b_stopLevel)
        {
            enemy_animator.speed = 0;
            return;
        }
        else // Check if Animator Speed is equal to 1
        { 
            if(enemy_animator.speed != 1) // If not set back to 1
            {
                enemy_animator.speed = 1;
            }
        }

        // Add check to stop when wall defeated
        if(go_wall != null)
        {
            if(go_wall.GetComponent<WallManager>().f_wallLife <= 0)
            {
                return;
            }
        }

        if (b_moveToTarget) // No obstacle, moves towards wall
        {
            MoveToTarget();
            return;
        }
        if (b_beingAttracktedBySpell)
        {
            if(e_enemyState == EnemyState.ReceiveDamage)
            {
                return;
            }
            MoveToTarget();
            return;
        }
    }
    private void FixedUpdate()
    {
        if (LevelsGnralManager.b_stopLevel)
        {
            return;
        }
        // Add check to stop when wall defeated
        if (go_wall != null)
        {
            if (go_wall.GetComponent<WallManager>().f_wallLife <= 0)
            {
                return;
            }
        }

        DetectObstacle();
    }
    #endregion

    #region General Methods
    private void DetectObstacle()
    {
        int wallLayer = 1 << 6;   // Wall Layer
        int obstacleLayer = 1 << 7; // Obstacle Layer
        int layerMask = wallLayer | obstacleLayer; // Layer Combination
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, -transform.up, f_rangeOfAction, layerMask);
        //Debug.DrawLine(transform.position, transform.position - transform.up * f_rangeOfAction, Color.red);

        if (raycastHit2D.collider != null)
        {
            if (raycastHit2D.collider.gameObject.GetComponent<WallManager>() != null
                && !b_receivingDamage)
            {
                go_wall = raycastHit2D.collider.gameObject;
                SwitchState(EnemyState.TakingAction);
                return;
            }
            else if (raycastHit2D.collider.gameObject.GetComponent<Obstacle_StaticInstant>() != null
                && !b_receivingDamage)
            {
                go_obstacle = raycastHit2D.collider.gameObject;
              
                SwitchState(EnemyState.TakingAction);
                return;
            }
        }
        else // No obstacle detected
        {
            if (!b_receivingDamage)
            {
                // If enemy state is defeated dont move
                if(e_enemyState == EnemyState.Defeated) { return; }
                // If no obstacle detected and enemy not defeated move towards Wall
                SwitchState(EnemyState.Moving);
            }
        }
    }

    private IEnumerator WaitToTakeAction()
    {
        yield return new WaitForSeconds(0.3f);
        SwitchState(EnemyState.TakingAction);
    }
    internal void SwitchState(EnemyState newState)
    {
        e_enemyState = newState;
        SetAllStatesAnimsToFalse();

        // Regular check
        switch (e_enemyState)
        {
            case EnemyState.Moving:
                go_obstacle = null;
                go_wall = null;
                // Additional check for defeated enemies not dying bug
                if (i_hp > 0)
                {
                    enemy_animator.SetBool(i_WalkHash, true);
                    b_moveToTarget = true;
                }
                break;
            case EnemyState.TakingAction:
                b_moveToTarget = false; // Stop moving
                enemy_animator.SetBool(i_AttackHash, true);
                // Play anim
                break;
            case EnemyState.ReceiveDamage:
                b_moveToTarget = false; // Stop moving
                b_receivingDamage = true; // Indicate enemy is receiving damage
                enemy_animator.SetBool(i_HitHash, true);
                break;
            case EnemyState.Defeated:
                b_moveToTarget = false; // Stop moving
                // Play anim
                enemy_animator.SetTrigger(i_DefeatedHash);
                break;

        }
    }
    private void SetAllStatesAnimsToFalse()
    {
        enemy_animator.SetBool(i_WalkHash, false);
        enemy_animator.SetBool(i_AttackHash, false);
        enemy_animator.SetBool(i_HitHash, false);
    }
    //Metodo para checkear 
    private void GetStats()
    {
        i_id = baseStats.i_id;
        i_hp = baseStats.i_hp;
        i_maxHp = i_hp;
        f_hpForHealthBar = i_hp;
        i_damage = baseStats.i_dmg;
        f_speed = baseStats.f_speed;
        f_rangeOfAction = baseStats.f_RageOfAction;
        f_projectailSpeed = baseStats.f_projectailSpeed;    
        i_manaEnergyReleased = baseStats.i_manaEnergyReleased;

        //Start variables
        f_startSpeed = f_speed;
        wallTarget_transform = moveToTarget_transform;
        rb_Enemy = gameObject.GetComponent<Rigidbody2D>();

    }

    internal void AttackEvent()// When the enemy attacks, we call this event which executes the enemyAction method.
    {
        if(!LevelsGnralManager.b_stopLevel)
            enemyAction.TakeAction(go_obstacle, go_wall);
    }
    private void MoveToTarget()
    {
        var step = f_speed * f_speedDecreasedValue * Time.deltaTime; // calculate distance to move
        if (b_beingAttracktedBySpell)
        {
            targetToMove = new Vector3(moveToTarget_transform.position.x, moveToTarget_transform.position.y, moveToTarget_transform.position.z);
        }
        else // move to wall
        {
            targetToMove = new Vector3(transform.position.x, moveToTarget_transform.position.y, moveToTarget_transform.position.z);
        }
        transform.position = Vector3.MoveTowards(transform.position, targetToMove, step);
    }
    internal void EndReceiveDamage()//Event that triggers when the damage animation ends so that the enemy knows what to do next.
    {
        b_receivingDamage = false;
    }
    #endregion

    #region Health Bar Fill Methods
    IEnumerator SumUp()
    {
        float f_duration = 1f;
        var rate = Mathf.Abs(f_hpForHealthBar - i_hp) / f_duration;
        while (f_hpForHealthBar != i_hp)
        {
            f_hpForHealthBar = Mathf.MoveTowards(f_hpForHealthBar, i_hp, rate * Time.deltaTime);
            img_fillBar.fillAmount = GetFillBarValue();
            yield return null;
        }
    }

    private float GetFillBarValue()
    {
        float fillBarValue = f_hpForHealthBar / i_maxHp;
        if (f_hpForHealthBar >= i_maxHp)
        {
            fillBarValue = 1f;
        }
        return fillBarValue;
    }
    #endregion

    #region External Methods
    internal void RestoreSpeed()
    {
        f_speedDecreasedValue = 1.0f;
    }
    internal void RestoreTarget()
    {
        moveToTarget_transform = wall_moveToTarget_transform;
    }
    internal void OutOfAreaApplyingForceToEnemy()
    {
        b_beingAttracktedBySpell = false;
    }
    internal void ReceiveDamage(int i_damageReceived, BaseSpellsEffects.SpellElement e_spellElement)
    {
        // Check for elemental match for damage bonus or damage reduction
        switch (baseStats.e_ElementalType)
        {
            case ScripteableEnemies.ElementalType.None:
                break;
            case ScripteableEnemies.ElementalType.Earth: // Weak to Fire, Strong to Lighting
                if(e_spellElement == BaseSpellsEffects.SpellElement.Fire)
                {
                    // Damage x2
                    i_damageReceived *= 2;
                }
                else if(e_spellElement == BaseSpellsEffects.SpellElement.Lighting)
                {
                    // Damage /2
                    i_damageReceived /= 2;
                }
                break;
            case ScripteableEnemies.ElementalType.Fire: // Weak to Water, Strong to Wind
                if (e_spellElement == BaseSpellsEffects.SpellElement.Water)
                {
                    // Damage x2
                    i_damageReceived *= 2;
                }
                else if (e_spellElement == BaseSpellsEffects.SpellElement.Wind)
                {
                    // Damage /2
                    i_damageReceived /= 2;
                }
                break;
            case ScripteableEnemies.ElementalType.Water: // Weak to Lighting, Strong to Fire
                if (e_spellElement == BaseSpellsEffects.SpellElement.Lighting)
                {
                    // Damage x2
                    i_damageReceived *= 2;
                }
                else if (e_spellElement == BaseSpellsEffects.SpellElement.Fire)
                {
                    // Damage /2
                    i_damageReceived /= 2;
                }
                break;
            case ScripteableEnemies.ElementalType.Wind: // None 
                break;
            case ScripteableEnemies.ElementalType.Lighting: // Weak to earth, Strong to Water
                if (e_spellElement == BaseSpellsEffects.SpellElement.Earth)
                {
                    // Damage x2
                    i_damageReceived *= 2;
                }
                else if (e_spellElement == BaseSpellsEffects.SpellElement.Water)
                {
                    // Damage /2
                    i_damageReceived /= 2;
                }
                break;
        }

        i_hp -= i_damageReceived;
        tmp_damageTaken.text = i_damageReceived.ToString();
        if (i_hp <= 0)
        {
            SwitchState(EnemyState.Defeated);
            RoundsManager.listOfEnemiesInRound.Remove(this);
            onEnemyDefeated?.Invoke(this, EventArgs.Empty);
            if (!b_eventCalled)
            {
                onEnemyDefeated?.Invoke(this, EventArgs.Empty);
                b_eventCalled = true;
            }
        }
        else
        {
            SwitchState(EnemyState.ReceiveDamage);
        }
        if (gameObject.activeSelf)
        {
            StartCoroutine(SumUp());
        }
    }
    #endregion

    #region SFXs Methods
    internal void PlayEnemyDefeated()
    {
        aS_enemy.Stop();
        aS_enemy.PlayOneShot(aC_enemyDefeated);
    }
    #endregion

    #region Remove from Battlefield and Reset Methods
    internal void RemoveFromBattlefield()
    {
        if (gameObject.activeSelf)
        {
            gameObject.transform.parent = null; // To avoid weird error with wave
            gameObject.SetActive(false); // Disactivate enemy
        }
    }
    internal void ResetEnemy()
    {
        // Set enemy stats and States to base
        //SwitchState(EnemyState.Moving);
        GetStats();
        b_eventCalled = false;
    }
    #endregion

}