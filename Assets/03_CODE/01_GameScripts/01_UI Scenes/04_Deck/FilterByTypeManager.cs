using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterByTypeManager : MonoBehaviour
{
    [Header("Spells Types Lists")]
    [SerializeField] private List<SpellSlot> listOf_FusionSpells = new List<SpellSlot>();
    [SerializeField] private List<SpellSlot> listOf_NotObtainedSpells = new List<SpellSlot>();
    [SerializeField] private List<SpellSlot> listOf_FireSpells = new List<SpellSlot>();
    [SerializeField] private List<SpellSlot> listOf_WaterSpells = new List<SpellSlot>();
    [SerializeField] private List<SpellSlot> listOf_EarthSpells = new List<SpellSlot>();
    [SerializeField] private List<SpellSlot> listOf_WindSpells = new List<SpellSlot>();
    [SerializeField] private List<SpellSlot> listOf_LightingSpells = new List<SpellSlot>();


    #region Unity Life Cycle
    private void Start()
    {
        InitializeLists();
        HideAllSpells();
        ShowAllObtainedSpells();
    }

    #endregion

    #region Initialize Spells Lists
    private void InitializeLists()
    {
        ShowFusionSpells();
        ShowFireSpells();
        ShowWaterSpells();
        ShowEarthSpells();
        ShowWindSpells();
        ShowLightingSpells();
    }
    #endregion

    #region Methods (Called in filters buttons)
    public void HideAllSpells()
    {
        // Not Obtained Spells
        for (int i = 0; i < listOf_NotObtainedSpells.Count; i++)
        {
            if (listOf_NotObtainedSpells[i] != null) // Chek if null
            {
                listOf_NotObtainedSpells[i].gameObject.SetActive(false); // Disactivate
            }
        }
        // Fusion Spells
        for (int i = 0; i < listOf_FusionSpells.Count; i++)
        {
            if (listOf_FusionSpells[i] != null) // Chek if null
            {
                listOf_FusionSpells[i].gameObject.SetActive(false); // Disactivate
            }
        }
        // Fire Spells
        for (int i = 0; i < listOf_FireSpells.Count; i++)
        {
            if (listOf_FireSpells[i] != null) // Chek if null
            {
                listOf_FireSpells[i].gameObject.SetActive(false); // Disactivate
            }
        }
        // Water Spells
        for (int i = 0; i < listOf_WaterSpells.Count; i++)
        {
            if (listOf_WaterSpells[i] != null) // Chek if null
            {
                listOf_WaterSpells[i].gameObject.SetActive(false); // Disactivate
            }
        }
        // Earth Spells
        for (int i = 0; i < listOf_EarthSpells.Count; i++)
        {
            if (listOf_EarthSpells[i] != null) // Chek if null
            {
                listOf_EarthSpells[i].gameObject.SetActive(false); // Disactivate
            }
        }
        // Wind Spells
        for (int i = 0; i < listOf_WindSpells.Count; i++)
        {
            if (listOf_WindSpells[i] != null) // Chek if null
            {
                listOf_WindSpells[i].gameObject.SetActive(false); // Disactivate
            }
        }
        // Lighting Spells
        for (int i = 0; i < listOf_LightingSpells.Count; i++)
        {
            if (listOf_LightingSpells[i] != null) // Chek if null
            {
                listOf_LightingSpells[i].gameObject.SetActive(false); // Disactivate
            }
        }

    }
    public void ShowAllObtainedSpells()
    {
        // Fusion Spells
        /*
        for (int i = 0; i < listOf_FusionSpells.Count; i++)
        {
            if (listOf_FusionSpells[i] != null) // Chek if null
            {
                int i_spellId = listOf_FusionSpells[i].GetSlotSpellInfo().i_id;
                if (PlayerData.b_spellCardObtained[i_spellId]) // Check if obtained
                {
                    listOf_FusionSpells[i].gameObject.SetActive(true); // Activate
                }
            }
        } 
        */

        // Fire Spells
        for (int i = 0; i < listOf_FireSpells.Count; i++)
        {
            if (listOf_FireSpells[i] != null) // Chek if null
            {
                int i_spellId = listOf_FireSpells[i].GetSlotSpellInfo().i_id;
                if (PlayerData.b_spellCardObtained[i_spellId]) // Check if obtained
                {
                    listOf_FireSpells[i].gameObject.SetActive(true); // Activate
                }
            }
        }
        // Water Spells
        for (int i = 0; i < listOf_WaterSpells.Count; i++)
        {
            if (listOf_WaterSpells[i] != null) // Chek if null
            {
                int i_spellId = listOf_WaterSpells[i].GetSlotSpellInfo().i_id;
                if (PlayerData.b_spellCardObtained[i_spellId]) // Check if obtained
                {
                    listOf_WaterSpells[i].gameObject.SetActive(true); // Activate
                }
            }
        }
        // Earth Spells
        for (int i = 0; i < listOf_EarthSpells.Count; i++)
        {
            if (listOf_EarthSpells[i] != null) // Chek if null
            {
                int i_spellId = listOf_EarthSpells[i].GetSlotSpellInfo().i_id;
                if (PlayerData.b_spellCardObtained[i_spellId]) // Check if obtained
                {
                    listOf_EarthSpells[i].gameObject.SetActive(true); // Activate
                }
            }
        }
        // Wind Spells
        for (int i = 0; i < listOf_WindSpells.Count; i++)
        {
            if (listOf_WindSpells[i] != null) // Chek if null
            {
                int i_spellId = listOf_WindSpells[i].GetSlotSpellInfo().i_id;
                if (PlayerData.b_spellCardObtained[i_spellId]) // Check if obtained
                {
                    listOf_WindSpells[i].gameObject.SetActive(true); // Activate
                }
            }
        }
        // Lighting Spells
        for (int i = 0; i < listOf_LightingSpells.Count; i++)
        {
            if (listOf_LightingSpells[i] != null) // Chek if null
            {
                int i_spellId = listOf_LightingSpells[i].GetSlotSpellInfo().i_id;
                if (PlayerData.b_spellCardObtained[i_spellId]) // Check if obtained
                {
                    listOf_LightingSpells[i].gameObject.SetActive(true); // Activate
                }
            }
        }
    }
    public void ShowNotObtainedSpells()
    {
        HideAllSpells();

        for (int i = 0; i < listOf_NotObtainedSpells.Count; i++)
        {
            listOf_NotObtainedSpells[i].gameObject.SetActive(true);
        }
    }
    public void ShowFusionSpells()
    {
        for (int i = 0; i < listOf_FusionSpells.Count; i++)
        {
            if (listOf_FusionSpells[i] != null)
            {
                ScripteableCard spellData = listOf_FusionSpells[i].GetSlotSpellInfo(); // Get Fusion Spell Info
                if (PlayerData.b_spellCardObtained[spellData.i_spellRequired_1_Id] &&
                    PlayerData.b_spellCardObtained[spellData.i_spellRequired_2_Id]) // Check if spells required for fusion are obtained
                {
                    listOf_FusionSpells[i].gameObject.SetActive(true);
                    listOf_FusionSpells[i].button_card.interactable = true;
                }
                else // If required spells not obtained hide, block interactions and add to not obtained list
                {
                    listOf_FusionSpells[i].gameObject.SetActive(false);
                    listOf_FusionSpells[i].button_card.interactable = false;
                    listOf_NotObtainedSpells.Add(listOf_FusionSpells[i]);
                }
            }
        }
    }
    public void ShowFireSpells()
    {
        for (int i = 0; i < listOf_FireSpells.Count; i++)
        {
            if (listOf_FireSpells[i] != null)
            {
                ScripteableCard spellData = listOf_FireSpells[i].GetSlotSpellInfo(); // Get Fusion Spell Info
                if (PlayerData.b_spellCardObtained[spellData.i_id]) // Check if spells required for fusion are obtained
                {
                    listOf_FireSpells[i].gameObject.SetActive(true);
                    listOf_FireSpells[i].button_card.interactable = true;
                }
                else // If required spells not obtained hide, block interactions and add to not obtained list
                {
                    listOf_FireSpells[i].gameObject.SetActive(false);
                    listOf_FireSpells[i].button_card.interactable = false;
                    listOf_NotObtainedSpells.Add(listOf_FireSpells[i]);
                }
            }
        }
    }
    public void ShowWaterSpells()
    {
        for (int i = 0; i < listOf_WaterSpells.Count; i++)
        {
            if (listOf_WaterSpells[i] != null)
            {
                ScripteableCard spellData = listOf_WaterSpells[i].GetSlotSpellInfo(); // Get Fusion Spell Info
                if (PlayerData.b_spellCardObtained[spellData.i_id]) // Check if spells required for fusion are obtained
                {
                    listOf_WaterSpells[i].gameObject.SetActive(true);
                    listOf_WaterSpells[i].button_card.interactable = true;
                }
                else // If required spells not obtained hide, block interactions and add to not obtained list
                {
                    listOf_WaterSpells[i].gameObject.SetActive(false);
                    listOf_WaterSpells[i].button_card.interactable = false;
                    listOf_NotObtainedSpells.Add(listOf_WaterSpells[i]);
                }
            }
        }
    }
    public void ShowEarthSpells()
    {
        for (int i = 0; i < listOf_EarthSpells.Count; i++)
        {
            if (listOf_EarthSpells[i] != null)
            {
                ScripteableCard spellData = listOf_EarthSpells[i].GetSlotSpellInfo(); // Get Fusion Spell Info
                if (PlayerData.b_spellCardObtained[spellData.i_id]) // Check if spells required for fusion are obtained
                {
                    listOf_EarthSpells[i].gameObject.SetActive(true);
                    listOf_EarthSpells[i].button_card.interactable = true;
                }
                else // If required spells not obtained hide, block interactions and add to not obtained list
                {
                    listOf_EarthSpells[i].gameObject.SetActive(false);
                    listOf_EarthSpells[i].button_card.interactable = false;
                    listOf_NotObtainedSpells.Add(listOf_EarthSpells[i]);
                }
            }
        }
    }
    public void ShowWindSpells()
    {
        for (int i = 0; i < listOf_WindSpells.Count; i++)
        {
            if (listOf_WindSpells[i] != null)
            {
                ScripteableCard spellData = listOf_WindSpells[i].GetSlotSpellInfo(); // Get Fusion Spell Info
                if (PlayerData.b_spellCardObtained[spellData.i_id]) // Check if spells required for fusion are obtained
                {
                    listOf_WindSpells[i].gameObject.SetActive(true);
                    listOf_WindSpells[i].button_card.interactable = true;
                }
                else // If required spells not obtained hide, block interactions and add to not obtained list
                {
                    listOf_WindSpells[i].gameObject.SetActive(false);
                    listOf_WindSpells[i].button_card.interactable = false;
                    listOf_NotObtainedSpells.Add(listOf_WindSpells[i]);
                }
            }
        }
    }
    public void ShowLightingSpells()
    {
        for (int i = 0; i < listOf_LightingSpells.Count; i++)
        {
            if (listOf_LightingSpells[i] != null)
            {
                ScripteableCard spellData = listOf_LightingSpells[i].GetSlotSpellInfo(); // Get Fusion Spell Info
                if (PlayerData.b_spellCardObtained[spellData.i_id]) // Check if spells required for fusion are obtained
                {
                    listOf_LightingSpells[i].gameObject.SetActive(true);
                    listOf_LightingSpells[i].button_card.interactable = true;
                }
                else // If required spells not obtained hide, block interactions and add to not obtained list
                {
                    listOf_LightingSpells[i].gameObject.SetActive(false);
                    listOf_LightingSpells[i].button_card.interactable = false;
                    listOf_NotObtainedSpells.Add(listOf_LightingSpells[i]);
                }
            }
        }
    }
    #endregion
}
