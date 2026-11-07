using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemonitoInfoBox : MonoBehaviour
{
    [Header("Local Prefab Components")]
    [SerializeField] internal Image img_demonitoIcon;
    [SerializeField] private ScripteableEnemies demonitoBaseStatsAndInfo;

    [Header("External references")]
    [SerializeField] internal DemonitosBookManager script_demonitosBookManager;

    public void ShowSelectedDemonitoInfo()
    {
        script_demonitosBookManager.ShowEnemyData(demonitoBaseStatsAndInfo, img_demonitoIcon);
    }
}
