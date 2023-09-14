using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BtnManager : MonoBehaviour
{
    public GameObject objectToSpawn;
    public objectspawner objectPlacement;
    public bool isButtonClicked = false;
    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(SpawnObject);

    }
    private void SpawnObject()
    {
       objectPlacement.SetObjectToSpawn(objectToSpawn);
    }
}