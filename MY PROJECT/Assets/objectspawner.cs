using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class objectspawner : MonoBehaviour
{
    public GameObject objTospawn;
    public logic placementIndicator;
    private bool canSpawn = true; // Flag to control object spawning
    public Touch touch;
   
    public List<GameObject> addedPrefabs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        placementIndicator = FindObjectOfType<logic>();
        objTospawn = null;
        
    }

    // Update is called once per frame
    void Update()
    {


        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{ 
        //    // Ensure that a prefab is assigned to this variable
        //    if (objTospawn != null)
        //    {
        //        Instantiate(objTospawn, placementIndicator.transform.position, placementIndicator.transform.rotation);
        //    }
        //}
    }
    public void SetObjectToSpawn(GameObject objectToSpawn)
    {
        
        objTospawn = objectToSpawn;
        GameObject addedPrefab = Instantiate(objTospawn, placementIndicator.transform.position, placementIndicator.transform.rotation);
        objTospawn = null;
        addedPrefabs.Add(addedPrefab);
        Debug.Log("Number of objects in the list: " + addedPrefabs.Count);
    }

    
}
