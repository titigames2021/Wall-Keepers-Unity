using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DemonitoBoxDataContainer : MonoBehaviour
{
    // Stores demonitos boxes info to show demonitos defeated

    [SerializeField] internal ScripteableEnemies demonitoInfo;

    [Header("UI Variables")]
    [SerializeField] internal TextMeshProUGUI tmp_numberOfDemonitosDefeated;
}
