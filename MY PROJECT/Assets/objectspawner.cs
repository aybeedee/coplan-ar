using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class objectspawner : MonoBehaviour
{
    public GameObject objTospawn;
    private logic placementIndicator;
    private bool canSpawn = true; // Flag to control object spawning

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
        //    if (canSpawn && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //    {
        //        // Ensure that a prefab is assigned to this variable
        //        if (objTospawn != null)
        //        {
        //            Instantiate(objTospawn, placementIndicator.transform.position, placementIndicator.transform.rotation);
        //        }
        //    }
        //    else
        //    {
        //        canSpawn = true;
        //    }
        //}
    }
    public void SetObjectToSpawn(GameObject objectToSpawn)
    {
        objTospawn = objectToSpawn;
        Instantiate(objTospawn, placementIndicator.transform.position, placementIndicator.transform.rotation);
        objTospawn = null;
    }
    // Add a method to toggle the canSpawn flag
    public void ToggleSpawnPermission(bool allowSpawn)
    {
        canSpawn = allowSpawn;
    }
}
