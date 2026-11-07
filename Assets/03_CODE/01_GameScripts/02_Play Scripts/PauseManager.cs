using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject go_pausePanel;
   public void PauseGame()
    {
        LevelsGnralManager.b_stopLevel = true;
        go_pausePanel.SetActive(true);
    }

    public void ContinueGame()
    {
        LevelsGnralManager.b_stopLevel = false;
        go_pausePanel.SetActive(false);
    }
}
