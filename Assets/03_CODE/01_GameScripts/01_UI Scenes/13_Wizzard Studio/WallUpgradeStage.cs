using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WallUpgradeStage : MonoBehaviour
{
    #region Variables
    private enum UpgradeType { WallHP, WallRecovery};
    [Header("Logic variables")]
    [SerializeField] private int i_stageId;
    [SerializeField] private UpgradeType e_upgradeType;
    [SerializeField] private int i_manaCost, i_upgradeAmount, i_maxUpgrades;
    int i_wallBaseHP = 10000;
    [Header("UI variables")]
    [SerializeField] Button _button_Upgrade;
    [SerializeField] GameObject go_lockedIcon;
    [SerializeField] TextMeshProUGUI tmp_manaCost, tmp_upgradeAmount, tmp_totalUpgrades;
    [Header("External Reference")]
    [SerializeField] WallUpgradeStage nextStage;
    [SerializeField] WizzardStudioManager script_wizzardStudioManager;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Assign mana cost to UI Tmp in prefab
        AssignUIVariablesValues();
        // Check Staga state (if max Upgrades already reached)
        CheckIfStageMaxUpgradesReached();
    }

    private void AssignUIVariablesValues()
    {
        if(PlayerData.i_wallUpgradeStage_TotalUpgrades[i_stageId] > i_maxUpgrades)
        {
            PlayerData.i_wallUpgradeStage_TotalUpgrades[i_stageId] = i_maxUpgrades;
        }
        int i_totalUpgradesOfThisStage = PlayerData.i_wallUpgradeStage_TotalUpgrades[i_stageId];
        tmp_manaCost.text = i_manaCost.ToString();
        tmp_upgradeAmount.text = "+" + i_upgradeAmount.ToString();
        tmp_totalUpgrades.text = i_totalUpgradesOfThisStage + "/" + i_maxUpgrades.ToString();
    }
    public void TryToPurchaseStageUpgrade()
    {
        if(PlayerData.i_mana >= i_manaCost)
        {
            // Reduce Mana
            PlayerData.i_mana -= i_manaCost;
            // Show Upgrade Acquired panel
            CompleteStageUpgrade();
            // Update Playe Mana Energy
            script_wizzardStudioManager.ShowManaEnergyReduced(PlayerData.i_mana);
        }
        else
        {
            script_wizzardStudioManager.ShowNotEnoughManaEnergyPanel();
        }
    }
    private void CompleteStageUpgrade()
    {
        // Mark Stage Upgrades +1
        PlayerData.i_wallUpgradeStage_TotalUpgrades[i_stageId] += 1;

        // Add Upgrade effect
        switch (e_upgradeType)
        {
            case UpgradeType.WallHP:
                PlayerData.i_wallHPIncreased += i_upgradeAmount;
                // Show Upgrade acquired panel
                int i_wallTotalHP = i_wallBaseHP + PlayerData.i_wallHPIncreased;
                script_wizzardStudioManager.ShowWallHPIncreased(i_wallTotalHP, i_upgradeAmount);
                break;

            case UpgradeType.WallRecovery:
                PlayerData.i_wallHPRecoveryPerSecond += i_upgradeAmount;
                // Show Upgrade acquired panel 
                script_wizzardStudioManager.ShowWallRecoveryIncreased(PlayerData.i_wallHPRecoveryPerSecond, i_upgradeAmount);
                break;
        }

        // Check if max upgrades reach
        CheckIfStageMaxUpgradesReached();

        // Update visual of total upgrades
        tmp_totalUpgrades.text = PlayerData.i_wallUpgradeStage_TotalUpgrades[i_stageId] + "/" + i_maxUpgrades.ToString();

        // Save Data
        SaveData.instance.AssignDataAndSave();
    }
    private void CheckIfStageMaxUpgradesReached()
    {
        if (PlayerData.i_wallUpgradeStage_TotalUpgrades[i_stageId] >= i_maxUpgrades)
        {
            // Max upgrades reach, block this stage and unblock next stage
            _button_Upgrade.interactable = false;

            if (nextStage != null)
            {
                nextStage.UnblockThisStage();
            }
        }
    }
    internal void UnblockThisStage()
    {
        go_lockedIcon.SetActive(false);
        // Check if current stage also max upgraded
        if (PlayerData.i_wallUpgradeStage_TotalUpgrades[i_stageId] >= i_maxUpgrades)
        {
            // Max upgrades reach, block this stage and unblock next stage
            _button_Upgrade.interactable = false;
        }
        else
        {
            _button_Upgrade.interactable = true;
        }
    }
}
