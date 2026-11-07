using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseSlot : MonoBehaviour
{
    // Holds Common variables for Deck slot and Spell Slot

    #region Slots Common Variables
    // Spell Data
    [SerializeField] protected ScripteableCard spellBaseData;
    [SerializeField] internal GameObject spellPrefab;

    // Slot Spell Art
    [SerializeField] internal Image img_spellInSlotVisual;

    // Slot Buttons
    [SerializeField] internal Button button_addOrRemoveSpellToFromDeck;
    [SerializeField] internal Button button_showSpellInfo;
    [SerializeField] internal Button button_card;
    #endregion

    #region Access methods
    internal ScripteableCard GetSlotSpellInfo()
    {
        return spellBaseData;
    }
    internal void SetSpellInfo(ScripteableCard spellBaseData)
    {
        this.spellBaseData = spellBaseData;
    }
    #endregion

    #region Unity Life Cycle
    private void Start()
    {
        DeckManager.instance.OnSelectedSlotChanged += DeckManager_OnSelectedSlotChanged;
    }
    #endregion

    #region Events Methods
    private void DeckManager_OnSelectedSlotChanged(object sender, EventArgs e)
    {
        if(DeckManager.instance.currentSelectedSlot != this)
        {
            //Debug.Log("Hides Buttons");
            HideButtons();
        }
        else { Debug.Log("Dont hide "+ gameObject.name); }
    }
    #endregion

    #region Buttons Methods
    public void CheckIfShowOrHideButtons()
    {
        DeckManager.instance.currentSelectedSlot = this;
        DeckManager.instance.FireEventOnSlotSelectedChanged();

        if (button_addOrRemoveSpellToFromDeck.gameObject.activeSelf) // Button already active
        {
            HideButtons();
        }
        else // Button not active
        {
            ShowButtons();
        }

    }
    public void ShowButtons()
    {
        // Mark Slot as Current Selected
        DeckManager.instance.currentSelectedSlot = this;
        // Show Buttons
        if(spellBaseData.e_spellType == ScripteableCard.SpellType.Normal)
        {
            button_addOrRemoveSpellToFromDeck.gameObject.SetActive(true);
        }
        button_showSpellInfo.gameObject.SetActive(true);
    }
    public void HideButtons()
    {
        // Mark as current selected null
        DeckManager.instance.currentSelectedSlot = null;

        // Hide Buttons
        button_addOrRemoveSpellToFromDeck.gameObject.SetActive(false);
        button_showSpellInfo.gameObject.SetActive(false);
    }
    #endregion

    #region Panels Methods
    public void ShowSpellInfoPanel()
    {
        DeckManager.instance.OpenSpellInfoPanel(spellBaseData);
    }
    #endregion


}
