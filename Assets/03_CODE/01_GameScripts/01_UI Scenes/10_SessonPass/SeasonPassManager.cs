using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeasonPassManager : MonoBehaviour
{
    #region Events
    internal event EventHandler OnMagicPassLevelUpdated;
    #endregion

    #region Variables
    private int i_passMaxLevel = 50, i_passCurrentLevel = 0, 
        i_manaUsed, i_manaToUseToReachNextLevel, i_baseManaToReachNextLvl = 50;
    private float f_currentLevelBarFillAmount, f_currentPlayerManaDisplaying, f_currentPlayerManaKeysDisplaying;
    float f_moveDifferenceBetwenLevels = 325;

    [Header("Scene UI variables")]
    [SerializeField] private RectTransform rewardsContentPanel;
    [SerializeField] private float[] f_arrayOf_rewardsContentPanelPositionsByLevel = new float[51];
    [SerializeField] Image img_fillBar;
    [SerializeField] TextMeshProUGUI tmp_manaLeftToNextLevel, tmp_currentMagicPassLevel,
        tmp_playerManaEnergy, tmp_playerManaKeys;
    [Header("Scene AUDIO")]
    [SerializeField] AudioSource aS_sceneSFx;
    [SerializeField] AudioClip aC_collectManaEnergy, aC_collectManaKeys, aC_collecFeaturedSpell;
    [Header("CLAIMED REWARDS PANELS")]
    [SerializeField] Animator featuredSpell_animator;
    [SerializeField] Animator claimedReward_animator;
    [SerializeField] GameObject go_manaEnergyIcon, go_manaKeysIcon;
    [SerializeField] TextMeshProUGUI tmp_amountObtained;
    int i_animatorString = Animator.StringToHash("Visible");
    #endregion

    #region Unity Life Cycle
    private void Start()
    {
        // Update Player Currencies
        UpdateManaEnergyCurrency();
        UpdateManaKeysCurrency();

        // Calculate current exp and current exp required for next level and then check for updating level and visuals
        i_manaUsed = PlayerData.i_magicPassManaExp;
        i_manaToUseToReachNextLevel = i_baseManaToReachNextLvl + PlayerData.i_magicPassLevel * 10;
        StartCoroutine(UpdateCurrentPassLevelAndExp());
    }
    #endregion

    #region Methods to Update lvl
    private void UpdatelvlBar()
    {
        img_fillBar.fillAmount = GetPassLevelNormalized();
    }
    public float GetPassLevelNormalized() // To use Image fill component instead of Sliders
    {
        return (float)i_passCurrentLevel / i_passMaxLevel;
    }
    public void IncreaseOneLevel()
    {
        // Increase Level
        i_passCurrentLevel++;
        // Show level increase
        StartCoroutine(IncreaseLevel(GetPassLevelNormalized()));
    }
    public void IncreaseSeveralLevels(int levelsIncreased)
    {
        // Increase Level
        i_passCurrentLevel += levelsIncreased;
        // Show level increase
        //StartCoroutine(IncreaseLevel(GetPassLevelNormalized()));
        img_fillBar.fillAmount = GetPassLevelNormalized();
    }
    IEnumerator IncreaseLevel(float f_newLevelBarFillAmount)
    {
        float f_duration = 1f;
        var rate = Mathf.Abs(f_newLevelBarFillAmount - f_currentLevelBarFillAmount) / f_duration;
        while (f_currentLevelBarFillAmount != f_newLevelBarFillAmount)
        {
            f_currentLevelBarFillAmount = Mathf.MoveTowards(f_currentLevelBarFillAmount, f_newLevelBarFillAmount, rate * Time.deltaTime);
            img_fillBar.fillAmount = f_currentLevelBarFillAmount;
            yield return null;
        }
    }
    private void UpdateLevelAndExpVisuals()
    {
        // Visuals: Level Bar & Exp to next level Bar
        // Check if Max level reached
        if(PlayerData.i_magicPassLevel >= i_passMaxLevel)
        {
            PlayerData.i_magicPassLevel = i_passMaxLevel;
            tmp_manaLeftToNextLevel.gameObject.SetActive(false);
        }
        IncreaseSeveralLevels(PlayerData.i_magicPassLevel);
        tmp_currentMagicPassLevel.text = "Level " + PlayerData.i_magicPassLevel.ToString();
        PlayerData.i_magicPassManaExp = i_manaUsed;
        tmp_manaLeftToNextLevel.text = PlayerData.i_magicPassManaExp + "/" + i_manaToUseToReachNextLevel;

        // Move Content panel to current level

        #region Adjust move Difference according to level
        int i_moveToIndexPos = 0;
        if(PlayerData.i_magicPassLevel == 0) { i_moveToIndexPos = 0; }
        else
        {
            i_moveToIndexPos = PlayerData.i_magicPassLevel;
        }

        rewardsContentPanel.anchoredPosition = new Vector3(rewardsContentPanel.localPosition.x,
            f_arrayOf_rewardsContentPanelPositionsByLevel[i_moveToIndexPos],
            rewardsContentPanel.localPosition.z);
        #endregion
    }
    IEnumerator UpdateCurrentPassLevelAndExp()
    {
        while (i_manaUsed >= i_manaToUseToReachNextLevel)
        {
            // Calculate current level and current mana used and mana to use
            i_manaToUseToReachNextLevel = i_baseManaToReachNextLvl + PlayerData.i_magicPassLevel * 10;
            i_manaUsed -= i_manaToUseToReachNextLevel; // reduce mana used according to mana required to next lvl
            if(i_manaUsed < 0) { i_manaUsed = 0; }
            PlayerData.i_magicPassLevel++;
        }

        // If mana used is lower than mana required to use to next level STOP
        if (i_manaUsed < i_manaToUseToReachNextLevel)
        {
            UpdateLevelAndExpVisuals();
            // Fire Event to make all Reward boxes update their state
            OnMagicPassLevelUpdated?.Invoke(this, EventArgs.Empty);
            yield return null;
        }
        
    }

    #endregion

    #region Update Currencies
    public void UpdateManaEnergyCurrency()
    {
        StartCoroutine(UpdateManaEnergy(PlayerData.i_mana));
    }
    public void UpdateManaKeysCurrency()
    {
        StartCoroutine(UpdateManaKeys(PlayerData.i_keys));
    }
    IEnumerator UpdateManaEnergy(float f_newManaEnergy)
    {
        float f_duration = 1f;
        var rate = Mathf.Abs(f_newManaEnergy - f_currentPlayerManaDisplaying) / f_duration;
        while (f_currentPlayerManaDisplaying != f_newManaEnergy)
        {
            f_currentPlayerManaDisplaying = Mathf.MoveTowards(f_currentPlayerManaDisplaying, f_newManaEnergy, rate * Time.deltaTime);
            int manaRounded = (int)f_currentPlayerManaDisplaying;
            tmp_playerManaEnergy.text = manaRounded.ToString();
            yield return null;
        }
    }
    IEnumerator UpdateManaKeys(float f_newManaKeys)
    {
        float f_duration = 1f;
        var rate = Mathf.Abs(f_newManaKeys - f_currentPlayerManaKeysDisplaying) / f_duration;
        while (f_currentPlayerManaKeysDisplaying != f_newManaKeys)
        {
            f_currentPlayerManaKeysDisplaying = Mathf.MoveTowards(f_currentPlayerManaKeysDisplaying, f_newManaKeys, rate * Time.deltaTime);
            int keysRounded = (int)f_currentPlayerManaKeysDisplaying;
            tmp_playerManaKeys.text = keysRounded.ToString();
            yield return null;
        }
    }
    #endregion

    #region SFXs Methods
    internal void PlayCollectManaEnergy()
    {
        aS_sceneSFx.PlayOneShot(aC_collectManaEnergy);
    }
    internal void PlayCollectManaKeys()
    {
        aS_sceneSFx.PlayOneShot(aC_collectManaKeys);
    }
    internal void PlayCollectFeaturedSpell()
    {
        aS_sceneSFx.PlayOneShot(aC_collecFeaturedSpell);
    }
    #endregion

    #region Show Claimed Rewards Methods
    public void ShowFeaturedSpellPanel()
    {
        featuredSpell_animator.SetBool(i_animatorString, true);
    }
    public void HideFeaturedSpellPanel()
    {
        featuredSpell_animator.SetBool(i_animatorString, false);
    }
    public void ShowClaimedRewardPanel(int i_amountObtained, bool b_isManaEnergy, bool b_isMKeys)
    {
        tmp_amountObtained.text = "+" + i_amountObtained.ToString();
        go_manaEnergyIcon.SetActive(b_isManaEnergy);
        go_manaKeysIcon.SetActive(b_isMKeys);
        claimedReward_animator.SetBool(i_animatorString, true);
    }
    public void HideClaimedRewardPanel()
    {
        claimedReward_animator.SetBool(i_animatorString, false);
    }
    #endregion
}



