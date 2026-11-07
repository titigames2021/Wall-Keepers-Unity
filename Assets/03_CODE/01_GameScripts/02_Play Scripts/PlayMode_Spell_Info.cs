using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayMode_Spell_Info : MonoBehaviour
{
    #region Singleton
    public static PlayMode_Spell_Info instance { get; private set; }

    #endregion
    #region Spell Stats
    [Header("Spell Info Panel Variables")]
    [SerializeField] private Image img_selectedSpellCard;
    [SerializeField] private TextMeshProUGUI tmp_spellDescription;
    [Header("Spell Stats Variables")]
    [SerializeField] private GameObject go_spellDamageStats;
    [SerializeField] private GameObject go_spellResistanceStats, go_spellSpeedStats, go_spellDurationStats;
    [SerializeField]
    internal TextMeshProUGUI tmp_spellStar, tmp_spellLevel, tmp_spellAwkLevel,
        tmp_damageAlone,
        tmp_resistanceAlone,
        tmp_speedAlone,
        tmp_durationAlone;
    #endregion
    #region Unity Life Cycle
    private void Awake()
    {
        // Implement Singleton Pattern
        if (instance != null) // Error check to avoid more than one Instance of this script to be created
        {
            Debug.LogError("There is more than one PlayMode_Spell_Info! " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this; // Equal the instance to this script
    }
    #endregion
    #region Spell Info
    public void ShowSpellData(SpellCard_Manager spellCard)
    {
        ScripteableCard scripteableCard = spellCard.GetCardData();
        // Disactive not applicable stats (if base stat is 0 disactivate)
        if (scripteableCard.f_dmg > 0) { go_spellDamageStats.SetActive(true); } else { go_spellDamageStats.SetActive(false); }
        if (scripteableCard.f_resistance > 0) { go_spellResistanceStats.SetActive(true); } else { go_spellResistanceStats.SetActive(false); }
        if (scripteableCard.f_projectileSpeed> 0) { go_spellSpeedStats.SetActive(true); } else { go_spellSpeedStats.SetActive(false); }
        if (scripteableCard.f_duration > 0) { go_spellDurationStats.SetActive(true); } else { go_spellDurationStats.SetActive(false); }
        //Asign Data 
        img_selectedSpellCard.sprite = scripteableCard._card_Sprite;//Card Sprite 
        tmp_spellDescription.text = scripteableCard.Description;//Description
        tmp_spellStar.text = scripteableCard.i_spellStarsTier.ToString();//STAR LEVEL
        tmp_spellLevel.text = PlayerData.i_spell_Card_Level[scripteableCard.i_id].ToString();//LEVEL
        tmp_spellAwkLevel.text = PlayerData.i_awakening_Card_Level[scripteableCard.i_id].ToString();//AWAKENING LEVEL
        tmp_damageAlone.text = spellCard.f_damage.ToString();//DAMAGE
        tmp_resistanceAlone.text = spellCard.f_resistance.ToString();//RESISTANCE
        tmp_speedAlone.text = spellCard.f_speed.ToString();//SPEED
        tmp_durationAlone.text = spellCard.f_duration.ToString();//DURATION
    }
    #endregion


}
