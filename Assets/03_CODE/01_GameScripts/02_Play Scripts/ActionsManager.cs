using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ActionsManager : MonoBehaviour
{
    #region Actions control Variables 
    [SerializeField] private Transform transform_Projectail_Cast_Position;
    [SerializeField] internal bool b_busy; // Check if any action on going, can't do other actions while busy is true
    internal bool b_showingCardInfo; // Used to know if Card info is been shown and then in Card Manager when card return to target position don't set busy back to fals
    [SerializeField]private bool b_draggingCard;
    private float f_timer = 0;
    private bool b_timerIsOn;
    [SerializeField] GameObject go_spellInfoPanel, go_StormSprites;
    public bool GetIsDragging()
    {
        return b_draggingCard;
    }
    #endregion

    #region Drag Variables
    private Vector3 v3_cardInitialPosition;
    private Vector2 v2_screenPosition;
    private Vector3 v3_worldPosition;
    [SerializeField] Vector3 v3_projectailCast_Area;
    [SerializeField] private SpellCard_Manager selectedSpell;
    internal SpellCard_Manager GetSelectedSpell() { return selectedSpell; }
    private int i_sortingOrder_WhenDragging = 90, i_defaultSortingOrder = 70;
    [SerializeField]LayerMask spellCards_LayerMask;
    [SerializeField] List<GameObject> effectList_GameObject = new List<GameObject>();
    #endregion

    #region Unity Life Cycle
    private void Start()
    {
        // Block Actions at Start when Start panel is showing
        b_busy = true;
    }
    private void Update()
    {
        if (LevelsGnralManager.b_stopLevel) // if level stopped don't show
        {
            return;
        }

        if (b_timerIsOn)
        {
            f_timer += Time.deltaTime;
        }

        if (b_draggingCard) // if action is card dragging 
        {
            // Check if player stops draggind and tries to drop card
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetMouseButtonUp(0))
            {
                DropCard();
                return;
            }
        }

        if (b_busy && !b_draggingCard) // If action ongoing, and action is not card dragging
        {
            return; //Stop
        }

        if (Input.touchCount > 0) // input for mobile
        {
          
            v2_screenPosition = Input.GetTouch(0).position;
          

            // If no other action ongoing or dragging any card, check if player trying to drag a card.
            GetWorldPositionBasedOnScreenTouchPosition();
            CheckIfTryingToDragCard();
        }
        else if (Input.GetMouseButton(0)) // Input for computers
        {

            Vector3 mousePosition = Input.mousePosition;
            v2_screenPosition = new Vector2(mousePosition.x, mousePosition.y);
            // If no other action ongoing or dragging any card, check if player trying to drag a card.
            GetWorldPositionBasedOnScreenTouchPosition();
            CheckIfTryingToDragCard();
        }
    }
    #endregion

    #region Drag Cards Methods

    private void InitCardDrag()
    {
        b_busy = true;
        b_draggingCard = true;
        // Place card above the others
        SetCardSortingOrder(i_sortingOrder_WhenDragging, selectedSpell);
        selectedSpell.transform_Projectail_Cast_Position = transform_Projectail_Cast_Position;

    }
    //CardMovement
    private void CardDragging()
    {
        selectedSpell.transform.position = new Vector2(v3_worldPosition.x, v3_worldPosition.y);
    }
    private void DropCard()
    {
        b_busy = false;
        b_draggingCard = false;
        // Use bool for merge control
        selectedSpell.b_spellSelectedForFusion = true;

        float f_timeLimitForShowingCardInfoPanel = .2f;
        if (f_timer < f_timeLimitForShowingCardInfoPanel)
        {
            if (LevelsGnralManager.b_stopLevel) // if level stopped don't show
            {
                return;
            }
            // Show card info panel 
            selectedSpell.b_spellSelectedForFusion = false;
            StartCoroutine(ReturnToHandPosition(selectedSpell));
            ShowCardInfoPanel(selectedSpell);
            return;
        }
        b_timerIsOn = false;
        f_timer = 0;

        // Check if trying Fusion
        if (selectedSpell.b_tryingFusion)
        {
            selectedSpell.InstantiateFusedSpellOnhand();

            // Get both spells used for fusion back to deck
            PlayMode_DeckManager.instance.GetSpellBackToDeck(selectedSpell, selectedSpell.v3_currentPosition);
            PlayMode_DeckManager.instance.GetSpellFusedBackToDeckButDontReplace(selectedSpell.spellFusedWith, 
                selectedSpell.spellFusedWith.v3_currentPosition);

            // Resets
            SetCardSortingOrder(i_defaultSortingOrder, selectedSpell);
            selectedSpell.b_spellSelectedForFusion = false;
            selectedSpell.spellFusedWith = null;
            selectedSpell.b_isFusionOption_1 = false;
            selectedSpell.b_isFusionOption_2 = false;
            selectedSpell = null;
            return; // Stop here
        }

        // Check if thrown in battlefield
        if (selectedSpell.b_onBattlefield)//  Battlefield collider 
        {
            // Check if enough Mana to cast
            if (ManaManager.instance.GetCurrentMana()> selectedSpell.GetCardData().i_mana_cost) // selectedSpell.MadnessEnergyCost()
            {
                //Reduce Mana 
                ManaManager.instance.ReduceCurrentMana(selectedSpell.GetCardData().i_mana_cost);
                //Active Effect
                selectedSpell.ActivateSpellEffect();
                //Reset card 
                PlayMode_DeckManager.instance.GetSpellBackToDeck(selectedSpell, selectedSpell.v3_currentPosition);// current position is his position in Hand
            }
            else
            {
                StartCoroutine(ReturnToHandPosition(selectedSpell));
            }
        }
        else
        {
            // Return card to initial position
            //selectedCard.transform.position = cardInitialPosition;
            StartCoroutine(ReturnToHandPosition(selectedSpell));
        }
        SetCardSortingOrder(i_defaultSortingOrder, selectedSpell);
        //selectedSpell.GetCardVisuals().SetCardSortingOrder(i_defaultSortingOrder);

        // Resets
        selectedSpell.spellFusedWith = null;
        selectedSpell.b_spellSelectedForFusion = false;
        selectedSpell = null;
    }

    //A method to return the card to its slot
    private IEnumerator ReturnToHandPosition(SpellCard_Manager selectedSpell)
    {
        while(selectedSpell.transform.position != selectedSpell.v3_currentPosition)
        {
            selectedSpell.transform.position = Vector3.MoveTowards(selectedSpell.transform.position,
                selectedSpell.v3_currentPosition, .5f);
            yield return null;
        }
        //transform.position = selectedSpell.v3_currentPosition;
        SetCardSortingOrder(i_defaultSortingOrder, selectedSpell);
    }
    private void SetCardSortingOrder(int sortingOrder, SpellCard_Manager selectedSpell)
    {
        selectedSpell.GetComponent<Renderer>().sortingOrder = sortingOrder;
        selectedSpell.sprRenderer_fusionBorderAnim.sortingOrder = sortingOrder+1;
    }
    //Get the SpriteRenderer from selectedSpell
    #endregion

    #region Additional Methods 
    private void GetWorldPositionBasedOnScreenTouchPosition()
    {

        v3_worldPosition = Camera.main.ScreenToWorldPoint(v2_screenPosition);
    }
    private void CheckIfTryingToDragCard()
    {
        if (b_draggingCard) // Already dragging card
        {
            CardDragging();
        }
        else // Check if trying to drag card
        {
            RaycastHit2D hit = Physics2D.Raycast(v3_worldPosition, Vector2.zero, 1000f,spellCards_LayerMask); // draw raycast

            if (hit.collider != null) // Raycast hits collider
            {
                 
                SpellCard_Manager spellCardInHand = hit.transform.GetComponent<SpellCard_Manager>();
                if (spellCardInHand != null) // If collider hitted is Spell Card
                {
                    // Start timer to see if trying to see card info only. Timer is checked in Drop method to know touch was fast.
                    b_timerIsOn = true;

                    // Check to indicate if Card can be dragged
                    selectedSpell = spellCardInHand;

                    InitCardDrag();
                }
            }
        }
    }

    #endregion


    #region Spell Info

    private void ShowCardInfoPanel(SpellCard_Manager selectedCard)
    {
        go_spellInfoPanel.SetActive(true);
        b_busy = true; // Prevent player from dragging cards or doing other actions
        b_showingCardInfo = true;
        LevelsGnralManager.b_stopLevel = true;
        PlayMode_Spell_Info.instance.ShowSpellData(selectedCard);
    }

    public void CloseCardInfoPanel()
    {
        LevelsGnralManager.b_stopLevel = false;
        go_spellInfoPanel.SetActive(false);
        b_busy = false; // Prevent player from dragging cards or doing other actions
        b_showingCardInfo = false;
    }

   

    


    #endregion


    #region External Methods
    public void ActiveStormSprites()
    {
        go_StormSprites.SetActive(true);    
    }
    public void DisableStormSprites()
    {
        go_StormSprites.SetActive(false);
    }
    public void SetBusyOFF() {  b_busy = false; }
    #endregion

}
