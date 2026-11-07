using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;

public class LoadTransition : MonoBehaviour
{


    [SerializeField] Animator transitionAnimator;
    private float f_transitionTime = 1.0f;
    public void LoadLevel(int levelIndex)
    {
        
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1; 
        }
        StartCoroutine(FadeLoad(levelIndex));

    }
    IEnumerator FadeLoad(int levelIndex)
    {
        transitionAnimator.SetTrigger("start");
        yield return new WaitForSeconds(f_transitionTime);
        SceneManager.LoadScene(levelIndex);

    }
}
