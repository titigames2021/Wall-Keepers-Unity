using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardsPoolManager : MonoBehaviour
{

    [SerializeField] private GameObject[] pooledCards_GameObjects = new GameObject[24];
    [SerializeField] private GameObject[] allCardsList_GameObjects = new GameObject[24];
    [SerializeField] private int[] PlayerDataSlotNumber = new int[8];



    private void Awake()
    {
        PlayerDataSlotNumber[0] = 0;
        //Guardamos de forma provisional esta mano de cartas
        for(int i = 0; i < PlayerData.i_CardSlotNumber.Length; i++)
        {
            PlayerData.i_CardSlotNumber[i] =PlayerDataSlotNumber[i];

        }
    }

    private void Start()
    {
        CreatePool();
    }


    private void CreatePool()
    {


        for (int r = 0; r < 3; ++r) // Para tener 2 cartas de cada tipo
        {
            for (int i = 0; i < PlayerData.i_CardSlotNumber.Length; ++i)
            {
                if (PlayerData.i_CardSlotNumber[i] != 0)
                {

                    if (r <= 0)
                    {
                        GameObject obj = Instantiate(allCardsList_GameObjects[PlayerData.i_CardSlotNumber[i]], gameObject.transform);
                        obj.name = obj.name + r;
                        obj.SetActive(false);
                        pooledCards_GameObjects[i] = obj;
                    }
                    else
                    {
                        GameObject obj = Instantiate(allCardsList_GameObjects[PlayerData.i_CardSlotNumber[i]], gameObject.transform);
                        obj.name = obj.name + r;
                        obj.SetActive(false);
                        if (r >= 2)
                        {
                            pooledCards_GameObjects[i + 16] = obj;

                        }
                        else
                        {
                            pooledCards_GameObjects[i + 8] = obj;

                        }
                    }
                }
                else
                {
                    Debug.Log("Null");
                }
            }
        }



    }



}
