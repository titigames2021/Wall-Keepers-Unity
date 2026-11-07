
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LootBook", menuName = "ScriptableObjects/LootBook", order = 1)]
public class ScripteableLootBook : ScriptableObject
{
    [Header("INFO TIRADAS")]
    public string s_name;
    public string description;
    public int id;
    public int i_dropRateOne;//0-30
    public int i_dropRateTwo;//31-60
    public int i_dropRateThree;//61-85
    public int i_dropRateFour;//85-95
    public int i_dropRateFive;//95-100
              

    [Header("CARTAS OBTENIBLES")]
    public ScripteableCard[] NewCard_OneStar;
    public ScripteableCard[] NewCard_TwoStar;
    public ScripteableCard[] NewCard_ThreeStar;
    public ScripteableCard[] NewCard_FourStar;
    public ScripteableCard[] NewCard_FiveStar;

    // n random 1-100
    // if n<droprate2 = 2 card 
    
}
