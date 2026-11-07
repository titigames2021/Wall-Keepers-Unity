using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pulls", menuName = "ScriptableObjects/Pulls Data", order = 3)]
public class LibraryPullsData : ScriptableObject
{
    [Header("Pull Info")]
    [SerializeField] private int i_pull_ID;
    [SerializeField] private string s_pullName;

    [Header("Pull Possible Results and %")]
    [SerializeField] private int i_oneStarChances, i_twoStarChances, i_threeStarChances, i_fourStarChances;

    [SerializeField] private List<ScripteableCard> oneStar_Spells;
    [SerializeField] private List<ScripteableCard> twoStar_Spells;
    [SerializeField] private List<ScripteableCard> threeStar_Spells;
    [SerializeField] private List<ScripteableCard> fourStar_Spells;
    [SerializeField] private List<ScripteableCard> fiveStar_Spells;

    #region Access Methods
    public int GetPullID() {  return i_pull_ID; }
    public string GetPullName() {  return s_pullName; }
    public int GetOneStarChances() {  return i_oneStarChances; }
    public int GetTwoStarChances() {  return i_twoStarChances; }
    public int GetThreeStarChances() { return i_threeStarChances; }
    public int GetFourStarChances() { return i_fourStarChances; }
    public List<ScripteableCard> GetOneStarSpellsInPull() { return oneStar_Spells; }
    public List<ScripteableCard> GetTwoStarSpellsInPull() { return twoStar_Spells; }
    public List<ScripteableCard> GetThreeStarSpellsInPull() { return threeStar_Spells; }
    public List<ScripteableCard> GetFourStarSpellsInPull() { return fourStar_Spells; }
    public List<ScripteableCard> GetFiveStarSpellsInPull() { return fiveStar_Spells; }
    #endregion
}
