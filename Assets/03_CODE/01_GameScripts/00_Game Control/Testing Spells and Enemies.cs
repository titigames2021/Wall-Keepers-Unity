using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSpellsandEnemies : MonoBehaviour
{
    public List<GameObject> spellsToTest = new List<GameObject>();   
    private void Awake()
    {
        ReAssignSpellsINDECK();
    }

    private void ReAssignSpellsINDECK()
    {
        for(int i = 0; i < spellsToTest.Count; i++)
        {
            PlayerData.go_spellsIndeck[i] = spellsToTest[i];
        }
    }
}
