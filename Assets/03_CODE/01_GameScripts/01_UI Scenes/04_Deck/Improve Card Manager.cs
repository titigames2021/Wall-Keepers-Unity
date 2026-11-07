using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ImproveCardManager : MonoBehaviour
{
    // Two functions: 1 for Level Up, 2 For Awakening lvl up

    [Header("Spell Level Up Animation")]
    [SerializeField] private Animator animator_lvlUPEffect;
    // Elements ID are: 1-Fire/ 2-Water/ 3-Earth/ 4-Wind/ 5-Lighting
    private int i_animatorIntVariable = Animator.StringToHash("ElementID");
    int i_elementID;
    [Header("Block Actions Panel")]
    [SerializeField] List<AudioClip> listOf_elementsAudioClip = new List<AudioClip>();

    [Header("Block Actions Panel")]
    [SerializeField] private GameObject go_actionsBlockPanel;
    #region Locally Stored variables
    int i_spellID;
    float f_currentDamage, f_currentResistance, f_currentSpeed, f_currentDuration;
    #endregion

    public void LevelUpSpell()
    {
        // Daily Missions Improve one spell
        PlayerData.b_improveOneSpell_DailyMissionComplete = true;

        // Activate Actions Block Panel
        go_actionsBlockPanel.SetActive(true);

        #region Spell Stats
        // Get Spell Current Stats 
        GetSpellCurrentStats();
        // Set new Stats
        float newDamage = f_currentDamage += SpellsData.f_damageIncreaseByLevel;
        float newResistance = f_currentResistance += SpellsData.f_resistanceIncreaseByLevel;
        float newSpeed = f_currentSpeed += SpellsData.f_speedIncreaseByLevel;
        float newDuration = f_currentDuration += SpellsData.f_durationIncreaseByLevel;
        #endregion

        // Increase Spell Level
        i_spellID = DeckManager.instance.currentSelectedSlot.GetSlotSpellInfo().i_id;
        PlayerData.i_spell_Card_Level[i_spellID] += 1;
        DeckManager.instance.tmp_spellLevel.text = PlayerData.i_spell_Card_Level[i_spellID].ToString();

        // Play Animation Effect (Check Element)
        IndicateSpellElementIDForAnimation();
        animator_lvlUPEffect.SetInteger(i_animatorIntVariable, i_elementID); // Play Improve card effect over card anim
        StartCoroutine(StopLevelUpAnim());
        //i_elementID = 0;
        //animator_lvlUPEffect.SetInteger(i_animatorIntVariable, 0); // Set to 0 to avoid animation looping
        // Play SFXs
        AudioManager.instance.StopMusicPlaying();
        StartCoroutine(RestartSceneMusic());
        AudioManager.instance.PlaySFxOneTime(listOf_elementsAudioClip[i_elementID]);

        // Show Stats increase
        ShowStatsAndIncreaseEffect(newDamage, newResistance, newSpeed, newDuration);
        // Reduce Player Mana Energy
        PlayerData.i_mana -= DeckManager.instance.i_manaEnergyCost;
        DeckManager.instance.tmp_playerManaEnergy.text = PlayerData.i_mana.ToString();

        // Reduce Number of Cards in Stock and play fill down effect (reduce text and update Cards Cost)
        PlayerData.i_spell_Cards_Obtained[i_spellID] -= DeckManager.instance.i_spellCardsCost;
        ShowSpellCardsReduceEffect();
    }
    public void AwakenSpell()
    {
        // Activate Actions Block Panel
        go_actionsBlockPanel.SetActive(true);

        #region Spell Stats
        // Get Spell Current Stats 
        GetSpellCurrentStats();
        // Set new Stats
        float newDamage = f_currentDamage += SpellsData.f_damageIncreaseByLevel;
        float newResistance = f_currentResistance += SpellsData.f_resistanceIncreaseByLevel;
        float newSpeed = f_currentSpeed += SpellsData.f_speedIncreaseByLevel;
        float newDuration = f_currentDuration += SpellsData.f_durationIncreaseByLevel;
        #endregion

        // Increase Spell Awakening Level
        int i_spellID = DeckManager.instance.currentSelectedSlot.GetSlotSpellInfo().i_id;
        PlayerData.i_awakening_Card_Level[i_spellID] += 1;
        DeckManager.instance.tmp_spellAwkLevel.text = PlayerData.i_awakening_Card_Level[i_spellID].ToString();

        // Play Animation Effect (Check Element)
        IndicateSpellElementIDForAnimation();
        animator_lvlUPEffect.SetInteger(i_animatorIntVariable, i_elementID); // Play Improve card effect over card anim
        //i_elementID = 0;
        //animator_lvlUPEffect.SetInteger(i_animatorIntVariable, 0); // Set to 0 to avoid animation looping
        StartCoroutine(StopLevelUpAnim());
        // Play SFXs
        AudioManager.instance.StopMusicPlaying();
        StartCoroutine(RestartSceneMusic());
        AudioManager.instance.PlaySFxOneTime(listOf_elementsAudioClip[i_elementID]);

        // Show Stats increase
        ShowStatsAndIncreaseEffectForAwakening(newDamage, newResistance, newSpeed, newDuration);
        // Reduce Player Mana Energy
        PlayerData.i_mana -= DeckManager.instance.i_manaEnergyCost;
        DeckManager.instance.tmp_playerManaEnergy.text = PlayerData.i_mana.ToString();

        // Reduce Number of Elemental Runes in Stock
        ScripteableCard spellInfo = DeckManager.instance.currentSelectedSlot.GetSlotSpellInfo();
        PlayerData.i_elementalRunesInStock[spellInfo.requiredElementalRune.i_runeID] -= DeckManager.instance.i_elementalRunesCost;
        float newElementalRunesInStock = PlayerData.i_elementalRunesInStock[spellInfo.requiredElementalRune.i_runeID];
        ShowElementalRunesReduceEffect(newElementalRunesInStock, i_spellID);
    }
    IEnumerator StopLevelUpAnim()
    {
        yield return new WaitForSeconds(1);
        animator_lvlUPEffect.SetInteger(i_animatorIntVariable, 0);
    }
    private void GetSpellCurrentStats()
    {
        f_currentDamage = DeckManager.instance.f_damage;
        f_currentResistance = DeckManager.instance.f_resistance;
        f_currentSpeed = DeckManager.instance.f_speed;
        f_currentDuration = DeckManager.instance.f_duration;
    }
    private void IndicateSpellElementIDForAnimation()
    {
        i_elementID = 0;
        switch (DeckManager.instance.currentSelectedSlot.GetSlotSpellInfo().e_Element)
        {
            case ScripteableCard.element.None: break;
            case ScripteableCard.element.Fire: i_elementID = 1; break;
            case ScripteableCard.element.Water: i_elementID = 2; break;
            case ScripteableCard.element.Earth: i_elementID = 3; break;
            case ScripteableCard.element.Wind: i_elementID = 4; break;
            case ScripteableCard.element.Electric: i_elementID = 5; break;
        }
    }

    #region Sum Up and Reduce visual methods
    private void ShowStatsAndIncreaseEffect(float newDamage, float newResistance, float newSpeed, float newDuration)
    {
        DeckManager.instance.ShowStatsAlone();
        // Play increase effect
        // Damage
        float f_currentDamageBeforeImprovement = f_currentDamage - SpellsData.f_damageIncreaseByLevel;
        StartCoroutine(DamageSumUp(newDamage, f_currentDamageBeforeImprovement));
        // Resistance 
        float f_currentResistanceBeforeImprovement = f_currentResistance - SpellsData.f_resistanceIncreaseByLevel;
        StartCoroutine(ResistanceSumUp(newResistance, f_currentResistanceBeforeImprovement));
        // Speed
        float f_currentSpeedBeforeImprovement = f_currentSpeed - SpellsData.f_speedIncreaseByLevel;
        StartCoroutine(SpeedSumUp(newSpeed, f_currentSpeedBeforeImprovement));
        // Duration
        float f_currentDurationBeforeImprovement = f_currentDuration - SpellsData.f_durationIncreaseByLevel;
        StartCoroutine(DurationSumUp(newDuration, f_currentDurationBeforeImprovement));
    }
    private void ShowStatsAndIncreaseEffectForAwakening(float newDamage, float newResistance, float newSpeed, float newDuration)
    {
        DeckManager.instance.ShowStatsAlone();
        // Play increase effect
        // Damage
        float f_currentDamageBeforeImprovement = f_currentDamage - SpellsData.f_damageIncreaseByAwakening;
        StartCoroutine(DamageSumUp(newDamage, f_currentDamageBeforeImprovement));
        // Resistance 
        float f_currentResistanceBeforeImprovement = f_currentResistance - SpellsData.f_resistanceIncreaseByAwakening;
        StartCoroutine(ResistanceSumUp(newResistance, f_currentResistanceBeforeImprovement));
        // Speed
        float f_currentSpeedBeforeImprovement = f_currentSpeed - SpellsData.f_speedIncreaseByAwakening;
        StartCoroutine(SpeedSumUp(newSpeed, f_currentSpeedBeforeImprovement));
        // Duration
        float f_currentDurationBeforeImprovement = f_currentDuration - SpellsData.f_durationIncreaseByAwakening;
        StartCoroutine(DurationSumUp(newDuration, f_currentDurationBeforeImprovement));
    }
    private void ShowSpellCardsReduceEffect()
    {
        StartCoroutine(CardsFillReduce(PlayerData.i_spell_Cards_Obtained[i_spellID], i_spellID));
    }
    private void ShowElementalRunesReduceEffect(float targetValue, int i_spellId)
    {
        StartCoroutine(ElemetalRunesReduceEffect(targetValue, i_spellId));
    }

    #region Stats Increase Effects (Visual for lvl up or awakening)
    IEnumerator DamageSumUp(float targetValue, float f_statValueShown)
    {
        float f_duration = 1f;
        var rate = Mathf.Abs(targetValue - f_statValueShown) / f_duration;
        while (f_statValueShown != targetValue)
        {
            f_statValueShown = Mathf.MoveTowards(f_statValueShown, targetValue, rate * Time.deltaTime);
            DeckManager.instance.tmp_damageAlone.text = ((int)f_statValueShown).ToString("f0");
            yield return null;
        }
    }
    IEnumerator ResistanceSumUp(float targetValue, float f_statValueShown)
    {
        float f_duration = 1f;
        var rate = Mathf.Abs(targetValue - f_statValueShown) / f_duration;
        while (f_statValueShown != targetValue)
        {
            f_statValueShown = Mathf.MoveTowards(f_statValueShown, targetValue, rate * Time.deltaTime);
            DeckManager.instance.tmp_resistanceAlone.text = ((int)f_statValueShown).ToString("f0");
            yield return null;
        }
    }
    IEnumerator SpeedSumUp(float targetValue, float f_statValueShown)
    {
        float f_duration = 1f;
        var rate = Mathf.Abs(targetValue - f_statValueShown) / f_duration;
        while (f_statValueShown != targetValue)
        {
            f_statValueShown = Mathf.MoveTowards(f_statValueShown, targetValue, rate * Time.deltaTime);
            DeckManager.instance.tmp_speedAlone.text = f_statValueShown.ToString("f2");
            yield return null;
        }
    }
    IEnumerator DurationSumUp(float targetValue, float f_statValueShown)
    {
        float f_duration = 1f;
        var rate = Mathf.Abs(targetValue - f_statValueShown) / f_duration;
        while (f_statValueShown != targetValue)
        {
            f_statValueShown = Mathf.MoveTowards(f_statValueShown, targetValue, rate * Time.deltaTime);
            DeckManager.instance.tmp_durationAlone.text = f_statValueShown.ToString("f2");
            yield return null;
        }
    }
    #endregion

    #region Other Visual Effects
    IEnumerator CardsFillReduce(float targetValue, int i_spellID)
    {
        GetNumberOfCardsForNextImprovement(i_spellID);
        float f_duration = 1f;
        float i_cardsInStock = DeckManager.instance.i_cardsInStock;
        var rate = Mathf.Abs(targetValue - i_cardsInStock) / f_duration;
        while (i_cardsInStock != targetValue)
        {
            i_cardsInStock = Mathf.MoveTowards(i_cardsInStock, targetValue, rate * Time.deltaTime);
            DeckManager.instance.tmp_cardsInStock.text = ((int)i_cardsInStock).ToString();
            DeckManager.instance.img_cardsRequiredFillBar.fillAmount = GetFillBarValue(i_cardsInStock);
            yield return null;
        }

        // After Effects are played show new costs and update buttons
        // Check if Next is Lvl Up or Awaken and if Conditions are met
        UpdateNewCostAndButtons(i_spellID);
        UpdateSpellStats();

        // Disactivate Block Action Panel
        go_actionsBlockPanel.SetActive(false);
    }
    IEnumerator ElemetalRunesReduceEffect(float targetValue, int i_spellID)
    {
        GetNumberOfCardsForNextImprovement(i_spellID);
        float f_duration = 1f;
        float i_elemetalRunesInStock = DeckManager.instance.i_elementalRunesInStock;
        var rate = Mathf.Abs(targetValue - i_elemetalRunesInStock) / f_duration;
        while (i_elemetalRunesInStock != targetValue)
        {
            i_elemetalRunesInStock = Mathf.MoveTowards(i_elemetalRunesInStock, targetValue, rate * Time.deltaTime);
            DeckManager.instance.tmp_elementalRunesRequiredInStock.text = ((int)i_elemetalRunesInStock).ToString();
            yield return null;
        }

        // After Effects are played show new costs and update buttons
        // Check if Next is Lvl Up or Awaken and if Conditions are met
        UpdateNewCostAndButtons(i_spellID);
        UpdateSpellStats();

        // Disactivate Block Action Panel
        go_actionsBlockPanel.SetActive(false);
        AudioManager.instance.PlayMusic();
    }
    IEnumerator RestartSceneMusic()
    {
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlayMusic();
    }
    private float GetFillBarValue(float i_cardsInStock)
    {
        float fillBarValue = (float)i_cardsInStock / DeckManager.instance.i_spellCardsCost;
        if (i_cardsInStock >= DeckManager.instance.i_spellCardsCost)
        {
            fillBarValue = 1f;
        }
        return fillBarValue;
    }
    
    private void GetNumberOfCardsForNextImprovement(int i_spellID)
    {
        // Get How many cards the spells needs for next improvement
        DeckManager.instance.i_spellCardsCost = SpellsData.i_spell_Cards_Cost_Increase_Per_Level +
            SpellsData.i_spell_Cards_Cost_Increase_Per_Level * PlayerData.i_spell_Card_Level[i_spellID];
    }
    #endregion

    #endregion

    #region Update Spell Info Panel Buttons and Stats
    private void UpdateNewCostAndButtons(int i_spellID)
    {
        // Set New Costs for improvement
        DeckManager.instance.UpdateSpellLvlUPOrAwakenCosts(i_spellID, DeckManager.instance.currentSelectedSlot.GetSlotSpellInfo());
        // Update buttons, check if next lvl up or awaken and if possible
        DeckManager.instance.UpdateButtons(i_spellID, DeckManager.instance.currentSelectedSlot.GetSlotSpellInfo());
    }
    private void UpdateSpellStats()
    {
        DeckManager.instance.UpdateStats(DeckManager.instance.currentSelectedSlot.GetSlotSpellInfo());
    }
    #endregion
}
