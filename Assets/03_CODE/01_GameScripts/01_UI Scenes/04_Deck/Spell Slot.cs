using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSlot : BaseSlot
{
    #region Deck interaction methods
    public void BlockUseButtonIfDeckSlotsComplete()
    {
       
        button_addOrRemoveSpellToFromDeck.interactable = true;

        // Now check if Deck is complete and if so mark interactable as false
        if (PlayerData.b_isDeckFull)
        {
            button_addOrRemoveSpellToFromDeck.interactable = false;
        }
        if(DeckManager.instance.CheckIfSpellAlreadyInDeck(GetSlotSpellInfo())) // Check if this spell is already in deck
        {
            // Already in deck. Bloc Use button
            button_addOrRemoveSpellToFromDeck.interactable = false;
        }
    }

    public void AddSpellToDeck()
    {
        DeckManager.instance.AssignNextEmptyDeckSlot(GetSlotSpellInfo()._card_Sprite, GetSlotSpellInfo(), spellPrefab);
        // Block button to avoid adding same spell multiple times
        button_addOrRemoveSpellToFromDeck.interactable = false;
    }
    #endregion

}
