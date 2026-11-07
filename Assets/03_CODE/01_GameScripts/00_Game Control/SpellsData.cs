using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpellsData 
{
    #region Spells Stats Base Data
    // Level Up Costs
    internal static int i_spell_Cards_Cost_Increase_Per_Level = 2; // For level 0 cost is 2
    internal static int i_spell_ManaEnergy_Cost_Increase_Per_Level = 250; // For level 0 cost is 250
    internal static int i_elemetalRunes_Cost_Increase_Per_Level = 5; // For level 0 cost is 250

    // Awakening Costs

    // Stats Improvement
    // by Level
    internal static float f_damageIncreaseByLevel = 2;
    internal static float f_resistanceIncreaseByLevel = 2;
    internal static float f_speedIncreaseByLevel = 0.1f;
    internal static float f_durationIncreaseByLevel = 0.1f;
    // by Awakening
    internal static float f_damageIncreaseByAwakening = 10;
    internal static float f_resistanceIncreaseByAwakening = 10;
    internal static float f_speedIncreaseByAwakening = 0.3f;
    internal static float f_durationIncreaseByAwakening = 0.3f;
    #endregion

}
