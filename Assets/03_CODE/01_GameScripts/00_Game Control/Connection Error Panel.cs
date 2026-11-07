using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionErrorPanel : MonoBehaviour
{
    #region Singleton
    public static ConnectionErrorPanel instance { get; private set; } // Can be read by all scripts but only modified by this
    #endregion

    public GameObject go_connectionErrorPanel;

    private void Awake()
    {
        // Implement Singleton Pattern
        if (instance != null) // Error check to avoid more than one Instance of this script to be created
        {
            Debug.LogError("Tere is more than one ConnectionErrorPanel! " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this; // Equal the instance to this script
    }

    internal void ShowConnectionErrorPanel()
    {

    }
}
