using UnityEngine;

[CreateAssetMenu(fileName = "CardStats", menuName = "ScriptableObjects/CardStats", order = 1)]
public class ScripteableCard : ScriptableObject
{
    [Header("Base Info and Stats")]
    public int i_id;
    [TextArea(5,20)]
    public string Description;
    public enum SpellType { Normal, Fusion };
    public SpellType e_spellType;
    public int i_spellStarsTier;
    public int i_mana_cost;
    public float f_dmg;
    public float f_duration;  
    public float f_projectileSpeed;
    public float f_resistance;

    [Header("Elemental Rune ID ")]
    public ElementalRunes_SO requiredElementalRune;

    [Header("Spell Art and Prefab")]
    public Sprite _card_Sprite;
    public Sprite _onField_Sprite;// QUE ES MEJOR COJER EL SPRITE DE AQUI O EN SPELLCARD QUE EN FUNCION DEL TIPO DE CARTA COJA UNO U OTRO ?
    [SerializeField] internal GameObject spellPrefab;

    [Header("SFxs")]
    public AudioClip castClip;
    public AudioClip impactClip;
    public int i_starLevel = 1;

    [Header("Fusion Cards Required Combination and Normal Cards (Possible Combinations")]
    [SerializeField] internal Sprite spr_spellRequiredCard_1;
    [SerializeField] internal Sprite spr_spellRequiredCard_2;
    [SerializeField] internal int i_spellRequired_1_Id, i_spellRequired_2_Id;


    public float f_slowDown_value;
    public float f_attackInterval;


    #region Delete
    public float f_repeatingDmgCoolDown;//LakeLifeTime
    public enum castingType {None, Projectile, Instant, AllMap };
    public castingType e_CastingType;
    public enum effect { None, Impact, Area, ImpactArea };
    public effect e_Effect;
    public enum element { None,Fire, Earth, Water, Wind, Electric};
    public element e_Element;
    public enum element2 { None, Fire, Earth, Water, Wind, Electric };
    public element2 e_2Element;
    
    [Header("ContinousDmg")]
    //Para cuando metamos mas

    [Header("FusedCards")]
    public bool b_isMerged;
    public Sprite fusionSprite1;
    public Sprite fusionSprite2;
    public Sprite fusionSprite3;
    public Sprite fusionSprite4;  
    public Sprite fusionSprite5;
    public bool b_canAwake;
    public int i_awakenLevel = 1;
   
    #endregion

}
