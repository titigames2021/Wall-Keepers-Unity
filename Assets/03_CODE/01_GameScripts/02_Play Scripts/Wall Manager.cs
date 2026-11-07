using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class WallManager : MonoBehaviour
{
    #region Variables
    [Header("Wall variables")]
    [SerializeField] internal Image img_wallLifeBar;
    internal float f_wallLife;
    float f_MaxWallLife = 10000f;
    public static WallManager instance { get; private set; } // Can be read by all scripts but only modified by this
    public static event EventHandler e_OnWallDestroyed;
    [SerializeField] private float f_lifeBar_SpeedDecrease;

    // Wall recovery control variables
    float f_timer = 0, f_timeUntilNextRecovery = 1;
    #endregion
    #region Unity Life Cycle
    private void Awake()
    {
        // Implement Singleton Pattern
        if (instance != null) // Error check to avoid more than one Instance of this script to be created
        {
            Debug.LogError("There is more than one WallManager! " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this; // Equal the instance to this script
    }
    private void Start()
    {
        InitializeWallLife();

        f_timer = 0;
    }
    private void Update()
    {

        // If Wall life not equal to max life && Wall Recovery greater than 0
        if (f_wallLife < f_MaxWallLife
            && PlayerData.i_wallHPRecoveryPerSecond > 0) 
        {
            if (LevelsGnralManager.b_stopLevel)
            {
                return;
            }
            f_timer += Time.deltaTime;
            if(f_timer >= f_timeUntilNextRecovery)
            {
                f_timer = 0;
                float newWallHP = f_wallLife + PlayerData.i_wallHPRecoveryPerSecond;
                if(newWallHP > f_MaxWallLife)
                {
                    newWallHP = f_MaxWallLife;
                }
                HealWall(newWallHP);
            }
        }
    }
    #endregion

    #region Wall Methods
    void InitializeWallLife()
    {
        // Calculate Wall life with Wall Upgrades
        int i_wallLifeAddingWallUpgrades = (int)f_MaxWallLife + PlayerData.i_wallHPIncreased;
        f_MaxWallLife = i_wallLifeAddingWallUpgrades;
        f_wallLife = f_MaxWallLife;
    }
    //Method accessed by the enemy to deal damage to the wall
    public void ReceiveHit(float EnemyDmg)
    {
        f_wallLife -= EnemyDmg;
        // img_wallLifeBar.fillAmount = GetHpFillBarValue();
        StartCoroutine(DecreaseLifeBar());

        if (f_wallLife <= 0)
        {
            e_OnWallDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }
    internal void HealWall(float newWallHP)
    {
        //StopCoroutine(DecreaseLifeBar());
        f_wallLife = newWallHP;
        img_wallLifeBar.fillAmount = GetHpFillBarValue();
        // img_wallLifeBar.fillAmount = GetHpFillBarValue();
        // ShowWallHPIncrease(f_wallRecovery);
    }
    //To Lerp lifeBar value 
    private IEnumerator DecreaseLifeBar()
    {
        float elapsedTime = 0;
        while (elapsedTime < f_lifeBar_SpeedDecrease)
        {
            img_wallLifeBar.fillAmount = Mathf.Lerp(img_wallLifeBar.fillAmount, GetHpFillBarValue(), elapsedTime / f_lifeBar_SpeedDecrease);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }

    //Get LifeBarValue normalized 
    private float GetHpFillBarValue()
    {
        float fillBarValue = (float)f_wallLife / f_MaxWallLife;
        if (f_wallLife >= f_MaxWallLife)
        {
            fillBarValue = 1f;
        }
        return fillBarValue;
    }


    #endregion
}
