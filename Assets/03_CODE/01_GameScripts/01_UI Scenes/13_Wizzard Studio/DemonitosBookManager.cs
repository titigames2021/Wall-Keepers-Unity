using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DemonitosBookManager : MonoBehaviour
{
    #region Variables
    [Header("Visual UI elements")]
    [SerializeField] Image img_demonito;
    [SerializeField] TextMeshProUGUI tmp_description, tmp_range,tmp_hp,tmp_attack,tmp_speed;
    [Header("Elemental Icons")]
    [SerializeField] GameObject go_earthIcon;
    [SerializeField] GameObject go_fireIcon, go_waterIcon, go_windIcon, go_lightingIcon;
    [Header("Demonitos Boxes for Filtering")]
    [SerializeField] private List<GameObject> listOf_go_demonitosOfClan_Chaos;
    [SerializeField] private List<GameObject> listOf_go_demonitosOfClan_Newt;
    [SerializeField] private List<GameObject> listOf_go_demonitosOfClan_Fang;

    #endregion

    #region Show Demonitos info Methods
    public void ShowEnemyData(ScripteableEnemies enemyData, Image img_selectedDemonitoIcon)
    {
        // Visual
        img_demonito.sprite = enemyData.sprite_bestiary;
        //img_demonito.sprite = img_selectedDemonitoIcon.sprite;
        //img_demonito.transform.localScale = img_selectedDemonitoIcon.transform.localScale;
        //img_demonito.transform.position = img_selectedDemonitoIcon.transform.position;

        // Stats and info
        tmp_description.text = enemyData.Description;
        tmp_hp.text = enemyData.i_hp.ToString();
        tmp_attack.text = enemyData.i_dmg.ToString();
        tmp_speed.text = enemyData.f_speed.ToString();

        if (enemyData.f_RageOfAction > 3.0f)
        {
            tmp_range.text = "RANGED";
        }
        else
        {
            tmp_range.text = "MELEE";
        }

        SetAllElementalIconsOFF();
        switch (enemyData.e_ElementalType)
        {
            case ScripteableEnemies.ElementalType.None:
                break;
            case ScripteableEnemies.ElementalType.Earth:
                go_earthIcon.SetActive(true);
                break;
            case ScripteableEnemies.ElementalType.Fire:
                go_fireIcon.SetActive(true);
                break;
            case ScripteableEnemies.ElementalType.Water:
                go_waterIcon.SetActive(true);
                break;
            case ScripteableEnemies.ElementalType.Wind:
                go_windIcon.SetActive(true);
                break;
            case ScripteableEnemies.ElementalType.Lighting:
                go_lightingIcon.SetActive(true);
                break;
        }
    }
    private void SetAllElementalIconsOFF()
    {
        go_earthIcon.SetActive(false);
        go_fireIcon.SetActive(false);
        go_waterIcon.SetActive(false);
        go_windIcon.SetActive(false);
        go_lightingIcon.SetActive(false);
    }
    #endregion

    #region Filter Demonitos Methods
    public void ShowAll()
    {
        foreach(GameObject demonitoBox in listOf_go_demonitosOfClan_Chaos)
        {
            if(!demonitoBox.activeSelf)
            demonitoBox.SetActive(true);
        }
        foreach (GameObject demonitoBox in listOf_go_demonitosOfClan_Newt)
        {
            if (!demonitoBox.activeSelf)
                demonitoBox.SetActive(true);
        }
    }
    public void HideAll()
    {
        foreach (GameObject demonitoBox in listOf_go_demonitosOfClan_Chaos)
        {
            if (demonitoBox.activeSelf)
                demonitoBox.SetActive(false);
        }
        foreach (GameObject demonitoBox in listOf_go_demonitosOfClan_Newt)
        {
            if (demonitoBox.activeSelf)
                demonitoBox.SetActive(false);
        }
        foreach (GameObject demonitoBox in listOf_go_demonitosOfClan_Fang)
        {
            if (demonitoBox.activeSelf)
                demonitoBox.SetActive(false);
        }
    }
    public void ShowChaosClanDemonitos()
    {
        foreach (GameObject demonitoBox in listOf_go_demonitosOfClan_Chaos)
        {
            if (!demonitoBox.activeSelf)
                demonitoBox.SetActive(true);
        }
    }
    public void ShowNewtClanDemonitos()
    {
        foreach (GameObject demonitoBox in listOf_go_demonitosOfClan_Newt)
        {
            if (!demonitoBox.activeSelf)
                demonitoBox.SetActive(true);
        }
    }
    public void ShowFangClanDemonitos()
    {
        foreach (GameObject demonitoBox in listOf_go_demonitosOfClan_Fang)
        {
            if (!demonitoBox.activeSelf)
                demonitoBox.SetActive(true);
        }
    }
    #endregion
}
