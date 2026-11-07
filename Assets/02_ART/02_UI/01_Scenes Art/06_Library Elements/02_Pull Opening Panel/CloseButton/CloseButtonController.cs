using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloseButtonController : MonoBehaviour
{
    [SerializeField] Animator animator_Button;
    [SerializeField] PullResultsManager pullResultsManager;
    protected int i_Pressed = Animator.StringToHash("Pressed");
   public void ButtonPressed()
    {
        animator_Button.SetTrigger(i_Pressed);
    }
    internal void AfterPressedAnimation()//Called in Animation
    {
        pullResultsManager.EndPullOpening();
    }
}
