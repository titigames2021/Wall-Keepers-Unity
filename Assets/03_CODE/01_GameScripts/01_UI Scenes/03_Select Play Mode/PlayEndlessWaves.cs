using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayEndlessWaves : MonoBehaviour
{
    [SerializeField] private GameObject go_EndlessWavesBanner;

    internal void OpenBanner()
    {
        go_EndlessWavesBanner.SetActive(true);

    }

    public void PlayEndlessWavesScene( int sceneNumber)
    {
        if (PlayerData.b_isDeckFull)
        {
            SceneManager.LoadScene(sceneNumber);
        }
        else
        {
            ContextualMenuManager.instance.ShowDeckNotFull();
        }
       

    }

}
