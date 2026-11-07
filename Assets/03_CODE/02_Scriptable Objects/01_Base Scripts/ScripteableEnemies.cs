using UnityEngine;
[CreateAssetMenu(fileName = "EnemieStats", menuName = "ScriptableEnemie/EnemieStats", order = 1)]
public class ScripteableEnemies : ScriptableObject
{
    [Header("Stats")]
    public Sprite sprite_bestiary;
    public int i_id;
    public int i_hp;
    public int i_dmg;
    public float f_speed;
    public float f_RageOfAction, f_projectailSpeed;
    public int i_manaEnergyReleased;
    [TextArea(5,15)]
    public string Description;

    [Header("SFXs")]
    public AudioClip deathClip;


    public enum Clan { None, Chaos, Newt, Earth, Shadow };
    [Header("Types")]
    public Clan e_Clan;

    public enum ElementalType { None, Earth, Fire, Water, Wind, Lighting };
    public ElementalType e_ElementalType;

    public enum Size { small, medium, large };
    [Header("Size")]
    public Size e_size;
}
