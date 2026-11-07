using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSlot : BaseSlot
{
    [Header("Deck Slot")]
    [SerializeField] Sprite spr_deckSlot_default;
    [SerializeField] internal int i_deckSlotIndex;

    #region Local Methods
    public void CheckIfDeckSlotFilledAndCheckIfShowOrHideButtons()
    {
        if(spellBaseData != null)
        {
            CheckIfShowOrHideButtons();
        }
    }
    public void RemoveSpellFromDeck()
    {
        // Remove from saved data
        PlayerData.go_spellsIndeck[i_deckSlotIndex]= null;
        PlayerData.deck_GameObjects[i_deckSlotIndex] = null;
        PlayerData.spellsInDeckData[i_deckSlotIndex] = null;
        PlayerData.i_spellInDeckByID[i_deckSlotIndex] = 0;
        // Mark deck as not complete
        PlayerData.b_isDeckFull = false;
        // Back to default sprite
        img_spellInSlotVisual.sprite = spr_deckSlot_default;
        // Empty slot
        spellPrefab = null;
        spellBaseData = null;

        // Hide Buttons
        HideButtons();

        // Save data
        SaveData.instance.AssignDataAndSave();
    }
    #endregion
}
