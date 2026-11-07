using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoScene : MonoBehaviour
{
    #region Variables
    public AudioSource aS_fireStart, aS_Fire, aS_fireLoop;
    #endregion

    #region Load scene Methods
    internal void LoadNextScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    #endregion
    #region Anim Methods
    internal void PlayStartingFireSFX()
    {
        aS_fireStart.Play();
    }
    internal void PlayFireSFX()
    {
        aS_Fire.Play();
    }
    internal void PlayFireLoop()
    {
        aS_fireLoop.Play();
    }
    #endregion
}
