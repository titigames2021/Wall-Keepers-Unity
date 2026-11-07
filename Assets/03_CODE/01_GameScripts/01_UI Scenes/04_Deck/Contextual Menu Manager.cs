using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContextualMenuManager : MonoBehaviour
{
    #region Variables
    public static ContextualMenuManager instance { get; private set; }
    [SerializeField] private GameObject go_NotEnough_ManaMenu;
    [SerializeField] private GameObject go_NotEnough_KeysMenu;
    [SerializeField] private GameObject go_DeckIsFull;
    #endregion
    #region Unity Life Cycle
    private void Awake()
    {
        // Implement Singleton Pattern
        if (instance != null) // Error check to avoid more than one Instance of this script to be created
        {
            Debug.LogError("There is more than one ContextualMenuManager! " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this; // Equal the instance to this script
    }
    #endregion
    #region Methods
    public void ShowDeckNotFull()
    {
        go_DeckIsFull.SetActive(true);
    }

    public void ShowNotEnoughManaPanel()
    {
        go_NotEnough_ManaMenu.SetActive(true);
    }
    public void ShowNotEnoughKeysPanel()
    {
        go_NotEnough_KeysMenu.SetActive(true); 
    }
    public void CloseManaPanel()
    {
        go_NotEnough_ManaMenu.SetActive(false);  
    }
    public void CloseKeysPanel()
    {
        go_NotEnough_KeysMenu.SetActive(false);
    }
    public void CloseDecksIsNotFullPanel()
    {
        go_DeckIsFull.SetActive(false);
    }
    #endregion

}
