using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FundingWizzardsPanelManager : MonoBehaviour
{
    public Animator fundingWizzardsPanel_Animator;
    private int i_animatorString = Animator.StringToHash("Visible");

    public void OpenFundingWizzards()
    {
        fundingWizzardsPanel_Animator.SetBool(i_animatorString, true);
    }
    public void CloseFundingWizzards()
    {
        fundingWizzardsPanel_Animator.SetBool(i_animatorString, false);
    }
}
