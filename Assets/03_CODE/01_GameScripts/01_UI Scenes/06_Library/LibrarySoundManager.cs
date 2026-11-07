using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibrarySoundManager : MonoBehaviour
{

    #region Sound Variables
    [Header("External Components")]
    [SerializeField] PullResultsManager script_pullResultsManager;

    [Header("Audio Sources")]
    [SerializeField] AudioSource aS_sceneSFXs;
    [SerializeField] AudioSource aS_sceneMusic;
    [Header("Music Clips")]
    [SerializeField] AudioClip aC_pullOpening_mainMusic;
    [SerializeField] List<AudioClip> aC_pulls_mainMusic = new List<AudioClip>();
    [Header("SFXs Clips")]
    [SerializeField] AudioClip aC_bookOpen;
    [SerializeField] AudioClip aC_bookPages;
    [Header("Tier Rarity SFXs Clips")]
    [SerializeField] AudioClip[] sFXofTier = new AudioClip[5];
    #endregion

    #region Pull Opening Music and SFXs
    public void PlayPullOpeningMusic()
    {
        aS_sceneMusic.Stop();
        aS_sceneMusic.clip = aC_pullOpening_mainMusic;
        aS_sceneMusic.Play();
    }
    public void StopPullOpeningMusicAndPlayPullMusic()
    {
        aS_sceneMusic.Stop();
        aS_sceneMusic.clip = aC_pulls_mainMusic[LibraryShopManager.instance.i_currentPull];
        aS_sceneMusic.Play();
    }
    public void PlayOpenBookSFX()
    {
        aS_sceneSFXs.PlayOneShot(aC_bookOpen);
    }
    public void PlayPagesSFX()
    {
        aS_sceneSFXs.PlayOneShot(aC_bookPages);
    }
    public void PlayTierSFX()
    {
        List<int> obtainedSpellsTiers = script_pullResultsManager.GetSpellTier();
        int index = script_pullResultsManager.GetCurrentShowingCard();
        aS_sceneSFXs.PlayOneShot(sFXofTier[obtainedSpellsTiers[index]]);
    }
    #endregion

    public void PlayCardsTierAnimations()
    {
        script_pullResultsManager.PlayCardTierAnim();
    }

    internal void RestartObtainedCardAnimation()
    {
        script_pullResultsManager.obtainedCard_Animator.SetTrigger(
            script_pullResultsManager.i_obtainedCard_startVariable);
    }
}
