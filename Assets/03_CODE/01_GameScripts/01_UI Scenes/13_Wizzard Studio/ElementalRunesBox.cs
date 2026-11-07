using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElementalRunesBox : MonoBehaviour
{
    [SerializeField] private ElementalRunes_SO runeData;
    [SerializeField] private TextMeshProUGUI tmp_runesInStock;
    // Start is called before the first frame update
    void Start()
    {
        // Updtae number of runes in stock
        tmp_runesInStock.text = "x"+ PlayerData.i_elementalRunesInStock[runeData.i_runeID].ToString();
    }
}
