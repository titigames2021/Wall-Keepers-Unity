using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance { get; private set; } // Can be read by all scripts but only modified by this
    #endregion

    #region Audio Sources
    [Header("Audio Sources")]
    [SerializeField] internal AudioSource aS_music;
    [SerializeField] internal AudioSource aS_SFxs;
    #endregion 

    #region Unity Life Cycle
    private void Awake()
    {
        // Implement Singleton Pattern
        if (instance != null) // Error check to avoid more than one Instance of this script to be created
        {
            Debug.LogError("Tere is more than one ShopManager! " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this; // Equal the instance to this script
    }
    #endregion

    #region Buttons Methods
    public void PlayNewMusicClip(AudioClip aC_newMusicClip)
    {
        aS_music.clip = aC_newMusicClip;
        aS_music.Stop();
        aS_music.Play();
    }
    public void PlaySFxOneTime(AudioClip aC_SFxClip)
    {
        aS_SFxs.PlayOneShot(aC_SFxClip);
    }
    #endregion

    #region General Methods
    public void StopMusicPlaying()
    {
        aS_music.Stop();
    }
    public void PlayMusic()
    {
        aS_music.Play();
    }
    public void StopSFXs()
    {
        aS_SFxs.Stop();
    }
    #endregion
}
