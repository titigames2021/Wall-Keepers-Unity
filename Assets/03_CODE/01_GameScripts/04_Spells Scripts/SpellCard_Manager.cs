using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SpellCard_Manager : MonoBehaviour
{
    ActionsManager script_actionsManager;

    #region Spells variables
    internal Vector3 v3_currentPosition, v3_cardScale;
    [Header("Spell Stats")]
    [SerializeField] private ScripteableCard spellBaseData;
    [SerializeField] private GameObject _spellEffect_GameObject;
    public bool b_onBattlefield;
    [SerializeField] private GameObject go_spellPreviewOnBattlefield;
    [SerializeField] private Sprite _targetSprite;
    private SpriteRenderer _card_SpriteRenderer;
    public bool b_cardColocation_Finished;
    #endregion

    #region Spell Stats variables
    private int i_spellLevel, i_spellAwakeningLvl;
    internal float f_damage, f_resistance, f_speed, f_duration, f_repeatRate, f_slowDownValue;
    private int i_battlefieldLayer = 9;
    #endregion

    #region Spell Effect variables
    [Header("Spell effect")]
    [SerializeField] internal BaseSpellsEffects spellEffectPrefab;
    [SerializeField] internal List<BaseSpellsEffects> listOf_spellEffects = new List<BaseSpellsEffects>();
    [SerializeField] internal int i_numberOfSpellEffectsInStock;
    [SerializeField] public Transform transform_Projectail_Cast_Position;
    #endregion


    #region Spells Merge Control variables
    [Header("Fusion Results and effects")]
    [SerializeField] private GameObject go_fusionSpellOption_1;
    [SerializeField] private GameObject go_fusionSpellOption_2;
    [SerializeField] internal SpriteRenderer sprRenderer_fusionBorderAnim;
    internal bool b_tryingFusion, b_spellSelectedForFusion, b_isFusionOption_1, b_isFusionOption_2, b_FusedCard;
    internal SpellCard_Manager spellFusedWith; // Use to send back to deck spell with which we are fusing this
    private List<SpellCard_Manager> listOf_fusionSpellsPrefabs_option_1 = new List<SpellCard_Manager>();
    private List<SpellCard_Manager> listOf_fusionSpellsPrefabs_option_2 = new List<SpellCard_Manager>();

    // Animator variables
    #region Animator
    [SerializeField] private Animator animatorOf_spellCard;
    private int i_canFuseHash = Animator.StringToHash("canFuse");
    private Transform transform_FusedCardStartPostition;
    #endregion

    #endregion

    #region Unity Life Cycle
    private void Start()
    {
        InitializedCard();
        SetSpellInitialStats();
        InitializeSpellEffectsPrefabs();

        // Get Actions Manager for use in Fuse logic
        GameObject go = GameObject.FindGameObjectWithTag("ActionsManager");
        script_actionsManager = go.GetComponent<ActionsManager>();
    }
    private void OnEnable()
    {
        // Get Spell Card Size and save it locally
        v3_cardScale = transform.localScale;
    }
    private void OnDisable()
    {
        b_cardColocation_Finished = false;  
    }
    #endregion

    #region Spell Base Data and Stats
    public ScripteableCard GetCardData()
    {
        return spellBaseData;
    }
    private void SetSpellInitialStats()
    {
        // Get Spell Level and Awakening Level
        int spellID = spellBaseData.i_id;
        i_spellLevel = PlayerData.i_spell_Card_Level[spellID];
        i_spellAwakeningLvl = PlayerData.i_awakening_Card_Level[spellID];

        #region Get Spells Stats Increase by Magic Upgrades
        float f_damageIncreased = 0;
        float f_resitanceIncreased = 0;
        float f_durationIncreased = 0;
        float f_speedIncreased = 0;

        #region Stats Increase by Magic Upgrades
        int i_elementID = 0; // Local variables to store element ID
        // Get Element ID
        switch (spellBaseData.e_Element)
        {
            case ScripteableCard.element.None: // If no element ignore
                break;
            case ScripteableCard.element.Fire: // Element ID = 1
                i_elementID = 1;
                break;
            case ScripteableCard.element.Water:  // Element ID = 2
                i_elementID = 2;
                break;
            case ScripteableCard.element.Wind: // Element ID = 3
                i_elementID = 3;
                break;
            case ScripteableCard.element.Electric: // Element ID = 4
                i_elementID = 4;
                break;
            case ScripteableCard.element.Earth: // Element ID = 5
                i_elementID = 5;
                break;
        }

        // Damage
        if (spellBaseData.f_dmg > 0) // Check that spell has damage to avoid adding non used attribute
        {
            // First Add Gnral Magic Upgrades that apply to all spells. ID = 0
            f_damageIncreased += PlayerData.i_magicDamageAttributeUpgradeAmount[0];
            
            // Last Add Spell Element Specific Upgrades
            // Check if element ID is not 0 to avoid adding twice Gnral Magic Upgrades
            if(i_elementID != 0)
            {
                // Add element damage Upgrade
                f_damageIncreased += PlayerData.i_magicDamageAttributeUpgradeAmount[i_elementID];
            }
        }
        // Resistance
        if (spellBaseData.f_resistance > 0) // Check that spell has damage to avoid adding non used attribute
        {
            // First Add Gnral Magic Upgrades that apply to all spells. ID = 0
            f_resitanceIncreased += PlayerData.i_magicResistanceAttributeUpgradeAmount[0];
            
            // Last Add Spell Element Specific Upgrades
            // Check if element ID is not 0 to avoid adding twice Gnral Magic Upgrades
            if (i_elementID != 0)
            {
                // Add element damage Upgrade
                f_resitanceIncreased += PlayerData.i_magicResistanceAttributeUpgradeAmount[i_elementID];
            }
        }
        // Duration
        if (spellBaseData.f_duration > 0) // Check that spell has damage to avoid adding non used attribute
        {
            // First Add Gnral Magic Upgrades that apply to all spells. ID = 0
            f_durationIncreased += PlayerData.f_magicDurationAttributeUpgradedAmount[0];
            // Last Add Spell Element Specific Upgrades
            // Check if element ID is not 0 to avoid adding twice Gnral Magic Upgrades
            if (i_elementID != 0)
            {
                // Add element damage Upgrade
                f_durationIncreased += PlayerData.f_magicDurationAttributeUpgradedAmount[i_elementID];
            }
        }
        // Speed
        if (spellBaseData.f_projectileSpeed > 0) // Check that spell has damage to avoid adding non used attribute
        {
            // First Add Gnral Magic Upgrades that apply to all spells. ID = 0
            f_speedIncreased += PlayerData.f_magicSpeedAttributeUpgradedAmount[0];
            
            // Last Add Spell Element Specific Upgrades
            // Check if element ID is not 0 to avoid adding twice Gnral Magic Upgrades
            if (i_elementID != 0)
            {
                // Add element damage Upgrade
                f_speedIncreased += PlayerData.f_magicSpeedAttributeUpgradedAmount[i_elementID];
            }
        }

        #endregion

        #endregion

        #region Get spell stats updated to level and awakening level
        // Damage
        f_damage = spellBaseData.f_dmg +
                f_damageIncreased +
                SpellsData.f_damageIncreaseByLevel * i_spellLevel +
                SpellsData.f_damageIncreaseByAwakening * i_spellAwakeningLvl;
        // Resistance
        f_resistance = spellBaseData.f_resistance +
                f_resitanceIncreased +
                SpellsData.f_resistanceIncreaseByLevel * i_spellLevel +
                SpellsData.f_resistanceIncreaseByAwakening * i_spellAwakeningLvl;
        // Speed
        f_speed = spellBaseData.f_projectileSpeed +
                f_speedIncreased +
                SpellsData.f_speedIncreaseByLevel * i_spellLevel +
                SpellsData.f_speedIncreaseByAwakening * i_spellAwakeningLvl; // if 0 is instant casting
        // Duration
        f_duration = spellBaseData.f_duration +
                f_durationIncreased +
                SpellsData.f_durationIncreaseByLevel * i_spellLevel +
                SpellsData.f_durationIncreaseByAwakening * i_spellAwakeningLvl;
        //Repeat Rate, Always the same value
        f_repeatRate = spellBaseData.f_attackInterval;
        //Slow down (MudBall), Always the same
        f_slowDownValue = spellBaseData.f_slowDown_value;

        #endregion
    }
    #endregion

    #region Spell Effects
    private void InitializeSpellEffectsPrefabs()
    {

        for (int i = 0; i < i_numberOfSpellEffectsInStock; i++)
        {
            BaseSpellsEffects spellEffect = Instantiate(spellEffectPrefab, transform);
            listOf_spellEffects.Add(spellEffect);
        }

        // initialize Stats
        foreach (BaseSpellsEffects spellsEffects in listOf_spellEffects)
        {
            // Pass to spell effect spells stats updated with level and awakening level
            spellsEffects.SetSpellStatsInSpellEffect(transform, f_damage, f_resistance, f_speed, f_duration, f_repeatRate, f_slowDownValue);
            spellsEffects.gameObject.SetActive(false);
        }
    }
    internal void ActivateSpellEffect()
    {
        // Take first spell effect in list which is not active already
        for (int i = 0; i < listOf_spellEffects.Count; i++)
        {
            // Check if spell effect is not still active in battlefield
            if (!listOf_spellEffects[i].b_spellEffectActiveInBattlefield) // Not in battlefield
            {
                // Reset Stats
                listOf_spellEffects[i].SetSpellStatsInSpellEffect(transform, f_damage, f_resistance, f_speed, f_duration, f_repeatRate, f_slowDownValue);
                // Activate Spell effect
                listOf_spellEffects[i].ActivateEffect(transform_Projectail_Cast_Position.position, transform.position);
                // Set spell state to active
                listOf_spellEffects[i].b_spellEffectActiveInBattlefield = true;
                // Activate effect
                listOf_spellEffects[i].gameObject.SetActive(true);
                // Stop loop to activate effect once
                break;
            }
            else
            {
                Debug.Log("No lo coge");
            }
        }
    }

    #endregion

    #region Disable & Initialize 
    private void InitializedCard()
    {
        _card_SpriteRenderer = GetComponent<SpriteRenderer>();  
        gameObject.GetComponent<SpriteRenderer>().sprite = spellBaseData._card_Sprite;
    }

    public void DisableCard()
    {
        gameObject.SetActive(false);
    }

    public void SetCardScale()
    {
        transform.localScale = v3_cardScale;
    }
    #endregion

    #region Merge Methods
    internal void ShowCanMergeOption_Anim()
    {
        animatorOf_spellCard.SetBool(i_canFuseHash, true);
    }
    internal void StopFuseAnim()
    {
        animatorOf_spellCard.SetBool(i_canFuseHash, false);
    }
    internal void InstantiateFusedSpellOnhand()
    {
        if (b_isFusionOption_1)
        {
            if(listOf_fusionSpellsPrefabs_option_1.Count > 0)
            {
                for (int i = 0; i < listOf_fusionSpellsPrefabs_option_1.Count; i++)
                {
                    if (!listOf_fusionSpellsPrefabs_option_1[i].gameObject.activeSelf) // Checked already instantiated prefabs
                    {
                        listOf_fusionSpellsPrefabs_option_1[i].transform.position = transform_FusedCardStartPostition.position;
                        listOf_fusionSpellsPrefabs_option_1[i].gameObject.SetActive(true);
                        listOf_fusionSpellsPrefabs_option_1[i].v3_currentPosition = transform_FusedCardStartPostition.position;
                        break;
                    }
                    else // All Active in hand, instantiate new one
                    {
                        // Create fusion option 1
                        GameObject obj = Instantiate(go_fusionSpellOption_1, transform_FusedCardStartPostition.position, transform.rotation,
                            transform.parent);
                        SetFusedCardResultStats(obj);
                    }
                }
            }
            else
            {
                // Create fusion option 1
                GameObject obj = Instantiate(go_fusionSpellOption_1, transform_FusedCardStartPostition.position, transform.rotation,
                    transform.parent);
                SetFusedCardResultStats(obj);
            }
        }
        else if (b_isFusionOption_2)
        {
            if (listOf_fusionSpellsPrefabs_option_1.Count > 0)
            {
                for (int i = 0; i < listOf_fusionSpellsPrefabs_option_2.Count; i++)
                {
                    if (!listOf_fusionSpellsPrefabs_option_2[i].gameObject.activeSelf) // Checked already instantiated prefabs
                    {
                        listOf_fusionSpellsPrefabs_option_2[i].transform.position = transform_FusedCardStartPostition.position;
                        listOf_fusionSpellsPrefabs_option_2[i].gameObject.SetActive(true);
                        listOf_fusionSpellsPrefabs_option_2[i].v3_currentPosition = transform_FusedCardStartPostition.position;
                        break;
                    }
                    else // All Active in hand, instantiate new one
                    {
                        // Create fusion option 1
                        GameObject obj = Instantiate(go_fusionSpellOption_2, transform_FusedCardStartPostition.position, transform.rotation,
                            transform.parent);
                        SetFusedCardResultStats(obj);

                    }
                }
            }
            else
            {
                // Create fusion option 1
                GameObject obj = Instantiate(go_fusionSpellOption_2, transform_FusedCardStartPostition.position, transform.rotation,
                    transform.parent);
                SetFusedCardResultStats(obj);
            }
        }
    }
    private void SetFusedCardResultStats(GameObject obj)
    {
        SpellCard_Manager fusedCard= obj.GetComponent<SpellCard_Manager>();
        fusedCard.v3_currentPosition = obj.transform.position;
        fusedCard.b_FusedCard = true;
    }
    #endregion
    
    #region Collisions
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(script_actionsManager != null)
        {
            if (script_actionsManager.GetSelectedSpell() != this)
            {
                return;
            }
        }
        
        if (collision.gameObject.tag == gameObject.tag) // If same tag then both are Spell Cards
        {
            // Get detected spell data
            SpellCard_Manager detectedSpell = collision.gameObject.GetComponent<SpellCard_Manager>();

            // Check if collides with other spell card (for fusion checks)
            
            if (b_cardColocation_Finished) 
            {
                // Check if detected card is a valid fusion option
                if (spellBaseData.i_spellRequired_1_Id == detectedSpell.GetCardData().i_id) // Fusion spell option 1
                {
                    if(spellFusedWith != null) // if any previous spell for fusion, stop previous spell can fuse anim
                    {
                        spellFusedWith.StopFuseAnim();
                    }
                    // Show spell which is fusing with anim
                    detectedSpell.ShowCanMergeOption_Anim();

                    // Play Can Fuse Animation
                    ShowCanMergeOption_Anim();
                    // Save Fusion card position to later instantiate fused card there
                    transform_FusedCardStartPostition = collision.transform;
                    // Set control bools
                    b_tryingFusion = true;
                    b_isFusionOption_1 = true;
                    b_isFusionOption_2 = false;
                    spellFusedWith = detectedSpell;
                }
                else if (spellBaseData.i_spellRequired_2_Id == detectedSpell.GetCardData().i_id) // Fusion spell option 2
                {
                    if (spellFusedWith != null) // if any previous spell for fusion, stop previous spell can fuse anim
                    {
                        spellFusedWith.StopFuseAnim();
                    }

                    // Show spell which is fusing with anim
                    detectedSpell.ShowCanMergeOption_Anim();

                    // Play Can Fuse Animation 
                    ShowCanMergeOption_Anim();
                    // Save Fusion card position to later instantiate fused card there
                    transform_FusedCardStartPostition = collision.transform;
                    // Set control bools
                    b_tryingFusion = true;
                    b_isFusionOption_1 = false;
                    b_isFusionOption_2 = true;
                    spellFusedWith = detectedSpell;
                }
            }


            /*
            // Check if trying fusion already and update to this (stop previous)
            if (spellFusedWith != null)
            {   
                spellFusedWith.StopFuseAnim();
                spellFusedWith = detectedSpell;
            }
            else
            {
                spellFusedWith = detectedSpell;
            }
            */
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == i_battlefieldLayer)
        {
            b_onBattlefield = true;
            _card_SpriteRenderer.sprite = spellBaseData._onField_Sprite;
            go_spellPreviewOnBattlefield.SetActive(true);

            // Block Fusion, prevents fusion from happening when throwing a spell
            b_tryingFusion = false;
            // Stop can fuse animations
            StopFuseAnim();
            if(spellFusedWith != null)
            {
                spellFusedWith.StopFuseAnim();
                b_tryingFusion = false;
                b_isFusionOption_1 = false;
                b_isFusionOption_2 = false;
            }
            spellFusedWith = null;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == i_battlefieldLayer)
        {
            _card_SpriteRenderer.sprite = spellBaseData._card_Sprite;
            b_onBattlefield = false;
            go_spellPreviewOnBattlefield.SetActive(false);
            b_cardColocation_Finished = true;
        }

        if (script_actionsManager != null)
        {
            if (script_actionsManager.GetSelectedSpell() != this)
            {
                return;
            }
        }

        // Get detected spell data
        if (collision.gameObject.tag == gameObject.tag)
        {
            // Get detected spell data
            SpellCard_Manager detectedSpellOut = collision.gameObject.GetComponent<SpellCard_Manager>();

            if (spellBaseData.i_spellRequired_1_Id == detectedSpellOut.GetCardData().i_id) // Fusion spell option 1
            {
                // Check if other spell possible for fusion
                if (spellFusedWith != null) // Null check for error avoid
                {
                    // Stop spell fused with anim
                    detectedSpellOut.StopFuseAnim();

                    if (spellFusedWith != detectedSpellOut) // Is current spell for fusion selected this?
                    {
                        // NO, another possible fusion spell is being selected
                    }
                    else // Yes, no other fusion spell is being selected
                    {
                        // Stop Can Fuse Animation
                        StopFuseAnim();
                        b_tryingFusion = false;
                        b_isFusionOption_1 = false;
                    }
                }

                /*
                // Stop Can Fuse Animation
                StopFuseAnim();
                // Stop other spell anim
                if(spellFusedWith != null)
                    spellFusedWith.StopFuseAnim();
                b_tryingFusion = false;
                b_isFusionOption_1 = false;
                */

            }
            else if (spellBaseData.i_spellRequired_2_Id == detectedSpellOut.GetCardData().i_id) // Fusion spell option 2
            {
                // Check if other spell possible for fusion
                if (spellFusedWith != null) // Null check for error avoid
                {
                    // Stop spell fused with anim
                    detectedSpellOut.StopFuseAnim();

                    if (spellFusedWith != detectedSpellOut) // Is current spell for fusion selected this?
                    {
                        // NO, another possible fusion spell is being selected
                    }
                    else // Yes, no other fusion spell is being selected
                    {
                        // Stop Can Fuse Animation
                        StopFuseAnim();
                        b_tryingFusion = false;
                        b_isFusionOption_2 = false;
                    }
                }
            }
        }
    }
    #endregion
}


