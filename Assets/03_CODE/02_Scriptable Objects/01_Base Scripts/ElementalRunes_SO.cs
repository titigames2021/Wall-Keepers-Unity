using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Elemental Runes", menuName = "ScriptableObjects/Elemental Runes", order = 1)]
public class ElementalRunes_SO : ScriptableObject
{

    [SerializeField] internal int i_runeID;
    [SerializeField] internal Sprite spr_runeIcon;
    internal enum RuneElement { Fire, Water, Earth, Wind, Lighting}
    [SerializeField] internal RuneElement e_runeElement;
}
