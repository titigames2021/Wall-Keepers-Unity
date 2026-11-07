using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStorm_AllArea_Effect : BaseSpellsEffects
{
    #region Variables
    [SerializeField] private List<GameObject> go_oneThunder = new List<GameObject>();
    [SerializeField] private GameObject go_CastBorders;
    #endregion

    #region BaseSpells Methods
    public override void ActivateEffect(Vector3 starPosition, Vector3 target)
    {
        SetTarget(target);
    }
    private void SetTarget(Vector3 target)
    {
        transform.parent = null;
        gameObject.transform.position = target;
    }
    #endregion

    #region Unity Life Cycle
    private void FixedUpdate()
    {
        CheckIfLevelPaused();
        if (!b_effectFinished)
        {
            CastEffectWithTimer();
        }
    }
    internal void OnDisable()//Reset Lightnings
    {
        b_effectFinished = false;
        for (int i = 0; i < go_oneThunder.Count; i++)
        {
            go_oneThunder[i].gameObject.GetComponent<OneLightningEffect>().b_lightningInstantiated = false;
            // Activate effect
            go_oneThunder[i].gameObject.SetActive(false);
        }
    }
    #endregion

    #region LStorm Methods
    internal IEnumerator CastInBattlefield()//First, we cast the storm for the determined duration.
    {
        WaitForSeconds wait_durationInBattlefield = new WaitForSeconds(f_duration);
        yield return wait_durationInBattlefield;
        effect_Animator.SetTrigger(i_EnterEnd);
    }
    internal void CastOneLightning()
    {
        gameObject.transform.position = GenerateRandomPosition();
        ActivateOneLightning();
    }
    internal void ActivateOneLightning()
    {
        // Take first spell effect in list which is not active already
        for (int i = 0; i < go_oneThunder.Count; i++)
        {
            // Check if spell effect is not still active in battlefield
            if (!go_oneThunder[i].gameObject.GetComponent<OneLightningEffect>().b_lightningInstantiated) // Not in battlefield
            {
                go_oneThunder[i].gameObject.GetComponent<OneLightningEffect>().SetDmg(f_damage);
                // Set spell state to active
                go_oneThunder[i].gameObject.GetComponent<OneLightningEffect>().b_lightningInstantiated = true;
                // Activate effect
                go_oneThunder[i].gameObject.SetActive(true);
                // Stop loop to activate effect once
                break;
            }
        }
    }
    private Vector3 GenerateRandomPosition()
    {
        // Generate random x and y positions
        float randomX = Random.Range(-5f, 5f);
        float randomY = Random.Range(-6f, 12f);
        // Create a random position Vector3
        return new Vector3(randomX, randomY, 0f);
    }
    internal void ShowBorders()
    {
        go_CastBorders.transform.parent = null;
        go_CastBorders.transform.position = new Vector3(0.0f, 3.2f, 0.0f);
    }

    internal void OccultBorders()
    {
        go_CastBorders.transform.parent = transform;

    }
    #endregion

    #region SFX Play Methods
    internal void StopLoopSfx()
    {
        aS_spellAudioSource.Stop();
    }
    #endregion

    #region Collisions
    private void  OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == i_EnemyLayer)
        {
            AddToEnemiesList(collision.gameObject.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == i_EnemyLayer)
        {
            RemoveFromEnemiesList(collision.gameObject.GetComponent<Enemy>());
        }
        
    }
    #endregion
}
