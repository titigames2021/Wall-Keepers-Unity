using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardsResultsPanels : MonoBehaviour
{
    #region UI Scene Variables
    [Header("UI elements for displaying results")]
    [SerializeField] private GameObject go_newCard;
    [SerializeField] private GameObject go_alreadyObtainedCard;
    [SerializeField] private TextMeshProUGUI cardsObtained_TMP;
    [SerializeField] private TextMeshProUGUI cardsInStock_TMP, cardsRequiredForNextImprovement;
    [SerializeField] private Image img_totalCardsFillBar;

    [Header("Card Turn SFXs")]
    [SerializeField] private AudioClip aC_cardTurn;
    [SerializeField] private AudioSource aS_cardTurn;

    #endregion

    #region Locally Stored Variables
    private int i_cardsObtained, i_cardsInStock, i_cardsRequiredForNextImprovement, i_newTotalCards;
    float f_currentValue, f_countDuration = 1;
    #endregion

    #region Unity Life Cycle
    IEnumerator SumUp(float targetValue)
    {
        var rate = Mathf.Abs(targetValue - f_currentValue) / f_countDuration;
        while(f_currentValue != targetValue)
        {
            f_currentValue = Mathf.MoveTowards(f_currentValue, targetValue, rate*Time.deltaTime);
            cardsInStock_TMP.text = ((int)f_currentValue).ToString();
            img_totalCardsFillBar.fillAmount = GetFillBarValue();
            yield return null;
        }
    }
    #endregion

    #region Local Method
    public void SetInfoToBeShown(bool b_isNewCard, int _i_cardsObtained, int _i_cardsInStock, int _i_cardsRequiredForNextImprovement)
    {
        if (!b_isNewCard)
        {
            go_newCard.SetActive(true);
            go_alreadyObtainedCard.SetActive(false);
        }
        else
        {
            go_newCard.SetActive(false);
            go_alreadyObtainedCard.SetActive(true);

            i_cardsObtained = _i_cardsObtained;
            i_cardsInStock = _i_cardsInStock;
            i_cardsRequiredForNextImprovement = _i_cardsRequiredForNextImprovement;
        }
    }
    private float GetFillBarValue()
    {
        float fillBarValue = (float)f_currentValue / i_cardsRequiredForNextImprovement;
        if (i_cardsInStock >= i_cardsRequiredForNextImprovement)
        {
            fillBarValue = 1f;
        }
        return fillBarValue;
    }
    #endregion

    #region Animation Events Methods
    internal void ShowCardsObtainedNumberAndPlayFillBarEffect()
    {
        // Set initial values
        cardsObtained_TMP.text = "x" + i_cardsObtained.ToString();
        cardsInStock_TMP.text = i_cardsInStock.ToString();
        cardsRequiredForNextImprovement.text = "/" + i_cardsRequiredForNextImprovement.ToString();

        // prepare variables for Sum up effect
        i_newTotalCards = i_cardsInStock + i_cardsObtained;
        f_currentValue = i_cardsInStock;
        img_totalCardsFillBar.fillAmount = GetFillBarValue();

        // Get Cards obtained, cards in stock before pull, cards required for next improvement, fill bar
        // Play fill bar effect and cards sum up effect
        StartCoroutine(SumUp(i_newTotalCards));
    }
    internal void PlayCardTurnSFX()
    {
        aS_cardTurn.PlayOneShot(aC_cardTurn);
    }
    #endregion
}
