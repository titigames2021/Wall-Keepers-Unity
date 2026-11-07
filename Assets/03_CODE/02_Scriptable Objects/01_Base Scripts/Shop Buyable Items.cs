using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop Buyable Items", menuName = "ScriptableObjects/Shop Items", order = 4)]
public class ShopBuyableSpells : ScriptableObject
{
    [Header("Spell Info")]
    [SerializeField] private int i_spell_ID;
    [SerializeField] private int i_spellStarTier;
    [SerializeField] private Sprite spr_spellCard;
    [SerializeField] private ScripteableCard spellData_sObject;

    [Header("Spell Costs")]
    [SerializeField] private int i_spellCost; // 1-500/ 2-1000/ 3-3000/ 4-10000/ 5-30000

    #region Access Methods
    public int GetSpellID() { return i_spell_ID; }
    public Sprite GetSpellCadrArt() { return spr_spellCard; }
    public int GetSpellCost() {  return i_spellCost; }
    public ScripteableCard GetSpellData() { return spellData_sObject; }
    #endregion
}
