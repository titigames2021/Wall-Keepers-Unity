using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levels_Boxes : MonoBehaviour
{
    // Contains Level UI elements and manages its state (Available, Complete, Blocked)

    #region UI Variables
    [SerializeField] internal int i_levelID; // Use to know which ID is this level in player Data
    [SerializeField] internal int i_levelSceneIndex; // Use to know which scene to load
    [SerializeField] internal Button button_Level;
    [SerializeField] internal GameObject go_rewardClaimedIcon;
    [SerializeField] internal GameObject go_levelBlockedVisual;

    [Header("Info to pass to panels")]
    [SerializeField] internal string s_levelDay;
    [SerializeField] internal string s_levelTotalWaves;
    #endregion
}
