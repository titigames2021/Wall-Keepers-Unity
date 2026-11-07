using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ManaManager : MonoBehaviour
{
    #region Variables
    public static ManaManager instance { get; private set; } // Can be read by all scripts but only modified by this
    [SerializeField] private float f_currentManaValue;
    private int i_MaxManaValue = 10;
    [SerializeField] private Image img_manaBarFront;
    [SerializeField] private Image img_manaBarBack;
    [SerializeField] private float f_manaIncreaseSpeed;
    [SerializeField] private TextMeshProUGUI manaTMP;
    #endregion
    #region Unity Life Cycle
    void Start()
    {
        InitializeMana();
       
    }
    private void Awake()
    {
        // Implement Singleton Pattern
        if (instance != null) // Error check to avoid more than one Instance of this script to be created
        {
            Debug.LogError("There is more than one ManaManager! " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this; // Equal the instance to this script
    }
    private void FixedUpdate()
    {
        CheckIfLevelPaused();
        IncreaseMana();
    }
    #endregion
    #region Init Methods
    //Initialize Mana Value to MAX
    private void InitializeMana()
    {
        //f_currentManaValue = i_MaxManaValue;
    }
    #endregion
    #region Increase & Decrease Bars
    private void IncreaseMana()
    {
        if (f_currentManaValue <= i_MaxManaValue)
        {
            f_currentManaValue += Time.deltaTime * f_manaIncreaseSpeed;
            IncreaseManaBar();
        }
      
    }
    //Method called by ActionsManager when Player cast a card 
    public void ReduceCurrentMana(int amount)
    {
        f_currentManaValue -= amount;
        ReduceBothBars();
        // Update Mana Used for Magic Pass Experience obtained
        PlayerData.i_magicPassManaExp += amount;
    }
    //To decrease Mana Bars
    private void ReduceBothBars()
    {
        img_manaBarBack.fillAmount = GetManaBarFillValue();
        img_manaBarFront.fillAmount = GetTrucateFillValue();
        UpdateManaNumber();
    }
    //Auto Increase Mana 
    private void IncreaseManaBar()
    {
        img_manaBarBack.fillAmount = GetManaBarFillValue();//Increase Back Bar 
        if(img_manaBarBack.fillAmount >= img_manaBarFront.fillAmount + 0.1f)//Increase Front Bar, this only goes up 0,1 by 0,1
        {
            img_manaBarFront.fillAmount = GetTrucateFillValue();//Needs a truncate to fit well into the mana grid.
        }
        UpdateManaNumber();
    }
    private void UpdateManaNumber()
    {
        manaTMP.text = ((int)f_currentManaValue).ToString();
    }
    #endregion
    #region Get Variables
    //Get ManaValue normalized 
    private float GetManaBarFillValue()
    {
        float fillBarValue = f_currentManaValue / i_MaxManaValue;
        if (f_currentManaValue >= i_MaxManaValue)
        {
            fillBarValue = 1f;
        }
        return fillBarValue;
    }
    private float GetTrucateFillValue()
    {
        return Mathf.Floor(GetManaBarFillValue() * 10) / 10;
    }
    public float GetCurrentMana()
    {
        return f_currentManaValue;
    }
    #endregion
    #region Detect Pause
    internal void CheckIfLevelPaused()
    {
        if (LevelsGnralManager.b_stopLevel)
        {
            f_manaIncreaseSpeed = 0;
            return;
        }
        else // Check if Animator Speed is equal to 1
        {
            if (f_manaIncreaseSpeed != 1) // If not set back to 1
            {
                f_manaIncreaseSpeed = 1;
            }
        }
    }
    #endregion
}
