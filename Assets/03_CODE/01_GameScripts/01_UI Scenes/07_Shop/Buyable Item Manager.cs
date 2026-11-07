using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyableItemManager : MonoBehaviour
{
    /*
     * Controls Buyable Item price and type of price (Money or Mana Energy)
     */
    private enum PriceType { ManaEnergy, Euros}
    [Header("Items Buyable Info")]
    [SerializeField] PriceType e_priceType;
    [SerializeField] private float f_itemPrice;
    [SerializeField] private int i_itemQuantity;
    [SerializeField] private AudioClip aC_purchasedComplete;
    internal enum ItemType { Spell, ManaEnergy, ManaKeys, ElementalRune }
    [SerializeField] internal ItemType e_itemType;
    internal enum RuneType { None, Fire, Water, Earth, Wind, Lighting }

    #region Specific types variables
    [Header("Mana energy and Mana Keys icon for results panel")]
    [SerializeField] internal Sprite spr_itemIcon;
    // Spells
    [Header("Spells Buyable item Data")]
    [SerializeField] private GameObject go_spellBlockedPanel;
    [SerializeField] internal Image img_displayedSpellCard;
    internal ShopBuyableSpells spellBuyableData;
    [SerializeField] private TextMeshProUGUI tmp_spellCost;
    [SerializeField] private int i_spellItem_index;

    [Header("Rune Type (None if not Elemental Rune Item Type")]
    [SerializeField] internal Image img_eRuneBuyableItem;
    internal ElementalRunes_SO elementalRuneData;
    [SerializeField] private int i_runeItem_index;
    [SerializeField] private TextMeshProUGUI tmp_eRunesPurchasesLeft; // Number of purchases Left

    #endregion

    #region Access Methods
    internal float GetItemPrice() { return f_itemPrice; }
    internal void SetItemPrice(float f_itemPrice) 
    { 
        this.f_itemPrice = f_itemPrice;
        tmp_spellCost.text = f_itemPrice.ToString();
    }
    #endregion

    #region Buy Methods
    public void BuyItem()
    {
        switch (e_priceType)
        {
            case PriceType.ManaEnergy:
                if(PlayerData.i_mana >= f_itemPrice) // Has enough Mana
                {
                    AudioManager.instance.PlaySFxOneTime(aC_purchasedComplete);
                    switch (e_itemType)
                    {
                        case ItemType.Spell:
                            PlayerData.i_mana -= (int)f_itemPrice;
                            int i_spellID = spellBuyableData.GetSpellID();
                            if (PlayerData.b_spellCardObtained[i_spellID]) // If we already have card
                            {
                                // Add Spell cards of bought spell
                                int i_spellCardsBefore = PlayerData.i_spell_Cards_Obtained[i_spellID];
                                Sprite spellCard_sprite = spellBuyableData.GetSpellCadrArt();
                                PlayerData.i_spell_Cards_Obtained[i_spellID] += i_itemQuantity;

                                //Show Menu with card art and fill bar effect adding cards
                                Debug.Log("Show Menu with card art and fill bar effect adding cards");
                                ShopManager.instance.ShowSpellObtainedPanel(i_spellID, i_spellCardsBefore, 
                                    PlayerData.i_spell_Cards_Obtained[i_spellID], spellCard_sprite, false);
                                ShopManager.instance.b_showFillEffect = true;
                            }
                            else // If we don't have card show as Unblock option
                            {
                                PlayerData.b_spellCardObtained[i_spellID] = true; // Mark as obtained
                                Sprite spellCard_sprite = spellBuyableData.GetSpellCadrArt();
                                Debug.Log("Show Menu with card art and New text instead of fill effect");
                                // Show Menu with card art and New text instead of fill effect
                                ShopManager.instance.ShowSpellObtainedPanel(i_spellID, 0, 0, spellCard_sprite, true);
                            }
                            Debug.Log("Mark Spell as already bought today");
                            BlockSpell_AlreadyBought();
                            break;

                        case ItemType.ManaKeys:
                            PlayerData.i_mana -= (int)f_itemPrice;
                            PlayerData.i_keys += i_itemQuantity;
                            // Show Purchased item panel and update Keys Currency displayed in Scene
                            ShopManager.instance.ShowItemBoughtPanel(spr_itemIcon, i_itemQuantity);
                            break;

                        case ItemType.ElementalRune:
                            PlayerData.i_mana -= (int)f_itemPrice; // reduce player mana energy by Item cost
                            PlayerData.i_elementalRunesPurchasesLeftInShop[i_runeItem_index] -= 1; // Reduce shop purhcases left
                            // Add Rune to player data
                            PlayerData.i_elementalRunesInStock[elementalRuneData.i_runeID] += i_itemQuantity;
                            // Show Bought Item Panel
                            ShopManager.instance.ShowItemBoughtPanel(elementalRuneData.spr_runeIcon, i_itemQuantity);
                            // Check if more Rune Purchases Left - use i_runeItem_index
                            if (PlayerData.i_elementalRunesPurchasesLeftInShop[i_runeItem_index] <= 0)
                            {
                                BlockERunePurchase();
                            }
                            // Update Rune Purchases Left Text
                            UpdateRunePurchasesLeft();
                            break;
                    }
                }
                else
                {
                    ContextualMenuManager.instance.ShowNotEnoughManaPanel();
                }
                break;
            case PriceType.Euros:
                Debug.Log("Process with payment verification");
                switch (e_itemType)
                {
                    case ItemType.ManaEnergy:
                        break;
                }
                break;
        }
        // Update mana Currency in scene
        ShopManager.instance.UpdatePlayerCurrencies();
        SaveData.instance.AssignDataAndSave();
    }
    #endregion

    #region Spells Buyable Item Display Methods
    public void BlockSpell_AlreadyBought()
    {
        PlayerData.b_spellBoughtAlready[i_spellItem_index] = true;
        go_spellBlockedPanel.SetActive(true);
    }
    internal void SetSpellCard(Sprite spellCard)
    {
        img_displayedSpellCard.sprite = spellCard;
    }
    #endregion

    #region Elemental Runes
    internal void SetElementalRuneIcon(Sprite spr_elementalRuneIcon)
    {
        img_eRuneBuyableItem.sprite = spr_elementalRuneIcon;
    }
    internal void UpdateRunePurchasesLeft()
    {
        tmp_eRunesPurchasesLeft.text = PlayerData.i_elementalRunesPurchasesLeftInShop[i_runeItem_index].ToString();
    }
    internal void BlockERunePurchase()
    {
        go_spellBlockedPanel.SetActive(true);
    }
    #endregion

}
