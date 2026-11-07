using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPoolerScript : MonoBehaviour
{

    #region Variables
    public static ObjectPoolerScript current;
    public List<GameObject> pooledObject;
    public int poolMaxSize;
    public  List<GameObject> pooledObjectsList;
    public bool willGrow;
    public int bulletType;
    public SaveData saveData;
    private WaitForSeconds loading_WFS = new WaitForSeconds(0.5f);
    #endregion
    #region Unity Life Cycle Methods

    private void Start()
    {
        //Que espere a cargar los datos
        StartCoroutine(Loading());

    }

    private IEnumerator Loading()
    {
        yield return loading_WFS;
        Debug.Log("PoolLoaded");
        SetPool();

    }
    
    #endregion
    # region Pool Methods
    void SetPool()
    {

        current = this;
        pooledObjectsList = new List<GameObject>();

        foreach (GameObject objToPool in pooledObject)
        {
            for (int i = 0; i < poolMaxSize; ++i)
            {
                GameObject obj = Instantiate(objToPool, gameObject.transform);
                obj.name = objToPool.name + "_" + i;
                obj.SetActive(false);
                pooledObjectsList.Add(obj);
            }
        }


    }

    public GameObject GetPooledObject()
    {
        List<int> inactiveIndices = new List<int>();
        int inactiveCount = 0;

        for (int i = 0; i < pooledObjectsList.Count; ++i)
        {
            if (!pooledObjectsList[i].activeInHierarchy)
            {
                inactiveIndices.Add(i);
                inactiveCount++;
            }
        }

        if (inactiveCount > 0)
        {
            int randomIndex = Random.Range(0, inactiveCount);
            int selectedIndex = inactiveIndices[randomIndex];
            GameObject obj = pooledObjectsList[selectedIndex];
            //obj.SetActive(true);
            return obj;
        }

     
        return null;
    }

  


    #endregion

}
