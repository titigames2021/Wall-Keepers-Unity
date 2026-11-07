using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllBattlefieldPreview : MonoBehaviour
{
    public Vector3 postionInBattlefield;
    // Start is called before the first frame update
    void Start()
    {
        // Move out of card parent to avoid being moved with card
        transform.parent = null;
        // Center to battlefield
        transform.position = postionInBattlefield;
    }
}
