using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WizzardStudioManager : MonoBehaviour
{
    #region Variables
    [Header("Main UI Elements")]
    [SerializeField] TextMeshProUGUI tmp_player_manaEnergy;
    [SerializeField] TextMeshProUGUI tmp_player_manaKeys;
    private float f_currentPlayerManaEnergy;

    [Header("Wall Upgrades Elements")]
    [SerializeField] TextMeshProUGUI tmp_wallAttributeUpgraded;
    [SerializeField] TextMeshProUGUI tmp_wallAttributeUpgradeValue;
    [SerializeField] TextMeshProUGUI tmp_wallHP, tmp_wallRecovery;
    [SerializeField] Animator _wallUpgradeCompletePanel_animator, _notEnoughManaEnergyPanel_animator;
    [SerializeField] GameObject go_wallHPIcon, go_wallRecoveryIcon;
    float f_wallTotalHp, f_wallRecoveryAmountPerSecond;

    [Header("Magic Upgrades")]
    [SerializeField] Animator _magicUpgradeCompletePanel_animator;
    [SerializeField] TextMeshProUGUI tmp_magicAttributeUpgraded, tmp_magicAttributeIncrease;
    float f_attributeValueBeforeUpgrade, f_attributeUpgradeValueDisplayed;
    [SerializeField] GameObject[] go_arrayOfAttributesIcons = new GameObject[4];

    int i_animatorsString = Animator.StringToHash("Visible");
    #endregion

    #region Unity Life Cycle
    private void Start()
    {
        UpdateCurrencies();
        UpdateAtStartWallHpAndRecovery();
    }
    #endregion

    #region Currencies Related Methods
    private void UpdateCurrencies()
    {
        tmp_player_manaEnergy.text = PlayerData.i_mana.ToString();
        f_currentPlayerManaEnergy = PlayerData.i_mana;
        tmp_player_manaKeys.text = PlayerData.i_keys.ToString();
    }
    internal void ShowManaEnergyReduced(float f_newPlayerManaEnergy)
    {
        StartCoroutine(ReduceManaEnergy(f_newPlayerManaEnergy));
    }
    IEnumerator ReduceManaEnergy(float f_newPlayerManaEnergy)
    {
        float f_duration = 1f;
        var rate = Mathf.Abs(f_newPlayerManaEnergy - f_currentPlayerManaEnergy) / f_duration;
        while (f_currentPlayerManaEnergy != f_newPlayerManaEnergy)
        {
            f_currentPlayerManaEnergy = Mathf.MoveTowards(f_currentPlayerManaEnergy, f_newPlayerManaEnergy, rate * Time.deltaTime);
            tmp_player_manaEnergy.text = ((int)f_currentPlayerManaEnergy).ToString();
            yield return null;
        }
    }
    #endregion

    #region Show and Hide Panels Methods
    public void ShowIndicatedPanel(Animator selectedPanel)
    {
        selectedPanel.SetBool(i_animatorsString, true);
    }
    public void HideIndicatedPanel(Animator selectedPanel)
    {
        selectedPanel.SetBool(i_animatorsString, false);
    }

    internal void ShowNotEnoughManaEnergyPanel()
    {
        ShowIndicatedPanel(_notEnoughManaEnergyPanel_animator);
    }
    #endregion

    #region Wall Upgrades Related methods
    private void UpdateAtStartWallHpAndRecovery()
    {
        // Show current Wall HP (Start HP is 10.000)
        f_wallTotalHp = 10000 + PlayerData.i_wallHPIncreased;
        tmp_wallHP.text = f_wallTotalHp.ToString();
        // Show current Wall recovery amount per second
        tmp_wallRecovery.text = PlayerData.i_wallHPRecoveryPerSecond.ToString();
        f_wallRecoveryAmountPerSecond = PlayerData.i_wallHPRecoveryPerSecond;
    }
    internal void ShowWallHPIncreased(float f_newWallHP, float f_amountIncreased)
    {
        // Show Wall HP icon
        go_wallHPIcon.SetActive(true);
        go_wallRecoveryIcon.SetActive(false);
        // Show Panel
        tmp_wallAttributeUpgraded.text = f_wallTotalHp.ToString();
        ShowIndicatedPanel(_wallUpgradeCompletePanel_animator);
        StartCoroutine(ShowWallHPIncreasedEffect(f_newWallHP));
        tmp_wallAttributeUpgradeValue.text = "+" + f_amountIncreased.ToString();
        StartCoroutine(ShowAmountIncreasedReduceEffect(f_amountIncreased));
    }
    IEnumerator ShowWallHPIncreasedEffect(float f_newWallHP)
    {
        yield return new WaitForSeconds(.5f);
        float f_duration = 1f;
        var rate = Mathf.Abs(f_newWallHP - f_wallTotalHp) / f_duration;
        tmp_wallAttributeUpgraded.text = f_wallTotalHp.ToString();

        while (f_wallTotalHp != f_newWallHP)
        {
            f_wallTotalHp = Mathf.MoveTowards(f_wallTotalHp, f_newWallHP, rate * Time.deltaTime);
            tmp_wallHP.text = ((int)f_wallTotalHp).ToString();
            tmp_wallAttributeUpgraded.text = ((int)f_wallTotalHp).ToString();
            yield return null;
        }
    }
    IEnumerator ShowAmountIncreasedReduceEffect(float f_amountIncreased)
    {
        int f_amountIncreasedTargetValue = 0;
        yield return new WaitForSeconds(.5f);
        float f_duration = 1f;
        var rate = Mathf.Abs(f_amountIncreasedTargetValue - f_amountIncreased) / f_duration;
        tmp_wallAttributeUpgradeValue.text = "+" + f_amountIncreased.ToString();

        while (f_amountIncreased != f_amountIncreasedTargetValue)
        {
            f_amountIncreased = Mathf.MoveTowards(f_amountIncreased, f_amountIncreasedTargetValue, rate * Time.deltaTime);
            tmp_wallAttributeUpgradeValue.text = "+" + ((int)f_amountIncreased).ToString();
            yield return null;
        }
    }

    // Recovery 
    internal void ShowWallRecoveryIncreased(float f_newWallRecovery, float f_amountIncreased)
    {
        // Show Wall HP icon
        go_wallHPIcon.SetActive(false);
        go_wallRecoveryIcon.SetActive(true);
        // Show Panel
        tmp_wallAttributeUpgraded.text = f_wallRecoveryAmountPerSecond.ToString();
        ShowIndicatedPanel(_wallUpgradeCompletePanel_animator);
        StartCoroutine(ShowWallRecoveryIncreasedEffect(f_newWallRecovery));
        tmp_wallAttributeUpgradeValue.text = "+" + f_amountIncreased.ToString();
        StartCoroutine(ShowAmountIncreasedReduceEffect(f_amountIncreased));
    }
    IEnumerator ShowWallRecoveryIncreasedEffect(float f_newWallRecoveryPerSecond)
    {
        yield return new WaitForSeconds(.1f);
        float f_duration = 1f;
        var rate = Mathf.Abs(f_newWallRecoveryPerSecond - f_wallRecoveryAmountPerSecond) / f_duration;
        tmp_wallAttributeUpgraded.text = f_wallRecoveryAmountPerSecond.ToString();

        while (f_wallRecoveryAmountPerSecond != f_newWallRecoveryPerSecond)
        {
            f_wallRecoveryAmountPerSecond = Mathf.MoveTowards(f_wallRecoveryAmountPerSecond, f_newWallRecoveryPerSecond, rate * Time.deltaTime);
            tmp_wallAttributeUpgraded.text = ((int)f_wallRecoveryAmountPerSecond).ToString();
            tmp_wallRecovery.text = ((int)f_wallRecoveryAmountPerSecond).ToString();
            yield return null;
        }
    }

    #endregion

    #region Magic Upgrades
    internal void ShowMagicAttributeUpgraded(int i_indexForAttributeUpgradedIcon, float f_attributeValue, float f_amountIncreased)
    {
        // Show Attribute icon (First set all off and the activate the corresponding one)
        foreach(GameObject attributeIcon in go_arrayOfAttributesIcons)
        {
            attributeIcon.SetActive(false);
        }
        go_arrayOfAttributesIcons[i_indexForAttributeUpgradedIcon].SetActive(true);

        // Set initial values of TMP and control variable
        f_attributeValueBeforeUpgrade = f_attributeValue;
        tmp_magicAttributeUpgraded.text = f_attributeValueBeforeUpgrade.ToString();
        tmp_magicAttributeIncrease.text = "+" + f_amountIncreased.ToString();
        f_attributeValueBeforeUpgrade = f_attributeValue;

        // Show Panel
        ShowIndicatedPanel(_magicUpgradeCompletePanel_animator);

        // Start Coroutines
        float f_newAttributeValue = f_attributeValueBeforeUpgrade + f_amountIncreased;
        StartCoroutine(ShowMagicAttributeIncreasedEffect(f_newAttributeValue));
        StartCoroutine(ShowMagicAttributeAmountIncreasedReducedEffect(f_amountIncreased));
    }
    IEnumerator ShowMagicAttributeIncreasedEffect(float f_newAttributeValue)
    {
        yield return new WaitForSeconds(1f);
        float f_duration = 1f;
        var rate = Mathf.Abs(f_newAttributeValue - f_attributeValueBeforeUpgrade) / f_duration;

        while (f_attributeValueBeforeUpgrade != f_newAttributeValue)
        {
            f_attributeValueBeforeUpgrade = Mathf.MoveTowards(f_attributeValueBeforeUpgrade, f_newAttributeValue, rate * Time.deltaTime);
            tmp_magicAttributeUpgraded.text = (f_attributeValueBeforeUpgrade).ToString("f2");
            yield return null;
        }
    }
    IEnumerator ShowMagicAttributeAmountIncreasedReducedEffect(float f_amountIncreased)
    {
        int f_amountIncreasedTargetValue = 0;
        yield return new WaitForSeconds(1f);
        float f_duration = 1f;
        var rate = Mathf.Abs(f_amountIncreasedTargetValue - f_amountIncreased) / f_duration;

        while (f_amountIncreased != f_amountIncreasedTargetValue)
        {
            f_amountIncreased = Mathf.MoveTowards(f_amountIncreased, f_amountIncreasedTargetValue, rate * Time.deltaTime);
            tmp_magicAttributeIncrease.text = "+" + f_amountIncreased.ToString("f2");
            yield return null;
        }
    }

    #endregion
}
