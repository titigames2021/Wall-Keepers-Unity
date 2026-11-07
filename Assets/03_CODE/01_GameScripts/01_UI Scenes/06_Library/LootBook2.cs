using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LootBook2 : MonoBehaviour
{

    public enum lootype { oneCard, fiveCards};
    public lootype e_looType;

    [SerializeField] private TextMeshProUGUI keysAmountTMP;
    [SerializeField] private ScripteableLootBook script_LootBookData;
    [SerializeField] List<Sprite> cardSprite_list;
    [SerializeField] List<int> i_cardId_list;
    [SerializeField] List<int> i_cardAmount_list;//Lista para guardar el total de cartas que tenemos un Spell
    [SerializeField] List<int> i_StarTier;
    [SerializeField] GameObject resultsPanel_GameObject;
    [SerializeField] GameObject card_GameObject;
    private bool b_BookOpened;
   
    //Tirada

    //Le iba a poner aqui el coste de las llaves pero, he decubierto que no te detecta la funcion en el button si le pones + de 1 argumento
    //Tampoco me dejaba seleccionar un Enum, por eso he optado por poner un int en looType



    public void OpenPanel()
    {

        resultsPanel_GameObject.SetActive(true);

    }


    public void OpenBook()
    {
     

        b_BookOpened = true;
        card_GameObject.SetActive(true);
        if (b_BookOpened)
        {
            NextCard();
        }
        
        

    }

    public void NextCard()
    {
        for (int i = 0; i < i_cardId_list.Count; i++)
        {





        }

    }

    public void ShowCard()
    {



    }



    public void LootBook(int looType)
    {
            switch (looType)
            {
                case 1:
                    if(PlayerData.i_keys >= 1)
                    {
                        SubstractKeys(1);
                        GetNewCard(1);
                    }
                    else
                    {
                        NotEnoughtKeys();
                    }
                    break;
                case 5:
                    if( PlayerData.i_keys >= 5)
                    {
                        SubstractKeys(5);
                        GetNewCard(5);
                    }
                    else
                    {
                        NotEnoughtKeys();
                    }
                    break;
                default:
                break;
            }

        OpenPanel();
    }

    private void GetNewCard(int nTiradas)
    {
        for(int i =0; i< nTiradas; i++)
        {

            GetSpell();
        }
      


    }

    private void GetSpell()
    {
        int i_randomResult = Random.Range(0, 100);

        switch (i_randomResult)
        {
            case int n when (n < script_LootBookData.i_dropRateOne):
                GenerateNewSpellAndSetData(1);
                break;

            case int n when (n < script_LootBookData.i_dropRateTwo):
                GenerateNewSpellAndSetData(2);
                break;

            case int n when (n < script_LootBookData.i_dropRateThree):
                i_StarTier.Add(1);
                GenerateNewSpellAndSetData(3);
                break;

            case int n when (n < script_LootBookData.i_dropRateFour):
                GenerateNewSpellAndSetData(4);
                break;

            case int n when (n < script_LootBookData.i_dropRateFive):
                GenerateNewSpellAndSetData(5);
                break;

            default:
                break;
        }

    }

    private void GenerateNewSpellAndSetData(int starTier)
    {
        i_StarTier.Add(starTier);//Guardamos la tier 

        int i_randomResult2=0;
        
        switch (starTier)
        {
            case 1 :
                i_randomResult2 = Random.Range(0, script_LootBookData.NewCard_OneStar.Length);
            break;
            case 2 :
                i_randomResult2 = Random.Range(0, script_LootBookData.NewCard_TwoStar.Length);
                break;
            case 3:
                i_randomResult2 = Random.Range(0, script_LootBookData.NewCard_ThreeStar.Length);
                break;
            case 4:
                i_randomResult2 = Random.Range(0, script_LootBookData.NewCard_FourStar.Length);
                break;
            case 5:
                i_randomResult2 = Random.Range(0, script_LootBookData.NewCard_FiveStar.Length);
                break;
        }



        int i_cardID = script_LootBookData.NewCard_OneStar[i_randomResult2].i_id;//Obtenemos su ID
        i_cardId_list.Add(i_cardID);//Guardamos el ID en la lista de ID's
        Sprite cardSprite = script_LootBookData.NewCard_OneStar[i_randomResult2]._card_Sprite;//Obtenemos el sprite para mostrarlo 
        cardSprite_list.Add(cardSprite);
        int i_cardAmount = PlayerData.i_spell_Cards_Obtained[i_cardID];//Sacamos el total de cartas que tiene de ese Spell
        i_cardAmount_list.Add(i_cardAmount);


    }

    public void  SubstractKeys(int keyCost)
    {

        PlayerData.i_keys -= keyCost;

        keysAmountTMP.text= keyCost.ToString();

    }

    public void NotEnoughtKeys()
    {
        Debug.Log("NotEnougthKeys");
    }






}
