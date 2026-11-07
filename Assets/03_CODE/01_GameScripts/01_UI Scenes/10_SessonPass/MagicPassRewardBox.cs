using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPassRewardBox : MonoBehaviour
{
    [Header("Control Variables")]
    [SerializeField] private int i_passLevel;
    private enum AccessType { Free, Premium}
    [SerializeField] private AccessType e_accesstype;
    private enum RewardType { Mana, Keys, FeaturedSpell }
    [SerializeField] private RewardType e_rewardType;
    [Tooltip("In case of Featured Spell Reward, Reward amount indicates Spell ID")]
    [SerializeField] private int i_rewardAmount;

    [Header("UI Box Variables")]
    [SerializeField] private GameObject go_claimedIcon;
    [SerializeField] private GameObject go_lvlNotReachedPanel, go_premiumRewardBlockPanel;

    [Header("External References")]
    [SerializeField] private SeasonPassManager script_seasonPassManager;


    #region UNITY LIFE CYCLE
    private void Start()
    {
        // Subscribe to events
        script_seasonPassManager.OnMagicPassLevelUpdated += Script_seasonPassManager_OnMagicPassLevelUpdated;
    }
    #endregion

    #region Event Calls
    private void Script_seasonPassManager_OnMagicPassLevelUpdated(object sender, System.EventArgs e)
    {
        UpdateState();
    }
    #endregion
    #region Local Methods
    private void UpdateState()
    {
        switch (e_accesstype)
        {
            case AccessType.Free:
                break;

            case AccessType.Premium:
                // Check if Premium Pass Purchased
                if (PlayerData.b_premiumPassPurchased)
                {
                    go_premiumRewardBlockPanel.SetActive(false);
                }
                break;
        }

        // Check if Player reached this level in pass
        if(i_passLevel <= PlayerData.i_magicPassLevel)
        {
            go_lvlNotReachedPanel.SetActive(false);
            // Check if reward already claimed
            if (PlayerData.b_magicPassRewardClaimed[i_passLevel])
            {
                MarkRewardAsClaimed();
            }
        }
    }
    private void MarkRewardAsClaimed()
    {
        go_claimedIcon.SetActive(true);
    }
    // Method Use in Button
    public void ClaimReward()
    {
        // Mark Reward as Claimed
        PlayerData.b_magicPassRewardClaimed[i_passLevel] = true;
        // Update UI visual state of Reward
        MarkRewardAsClaimed();
        // Give reward
        switch (e_rewardType)
        {
            case RewardType.Mana:
                PlayerData.i_mana += i_rewardAmount;
                script_seasonPassManager.PlayCollectManaEnergy();
                script_seasonPassManager.UpdateManaEnergyCurrency();
                script_seasonPassManager.ShowClaimedRewardPanel(i_rewardAmount,true, false);
                break;

            case RewardType.Keys:
                PlayerData.i_keys += i_rewardAmount;
                script_seasonPassManager.PlayCollectManaKeys();
                script_seasonPassManager.UpdateManaKeysCurrency();
                script_seasonPassManager.ShowClaimedRewardPanel(i_rewardAmount, false, true);
                break;

            case RewardType.FeaturedSpell:
                // MARK SPELL as OBTAINED
                PlayerData.b_spellCardObtained[i_rewardAmount] = true;
                int i_featuredSpellCardAmountObtained = 50;
                PlayerData.i_spell_Cards_Obtained[i_rewardAmount] += i_featuredSpellCardAmountObtained;
                script_seasonPassManager.PlayCollectFeaturedSpell();
                break;
        }

        SaveData.instance.AssignDataAndSave();
    }
    #endregion
}
