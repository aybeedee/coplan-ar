using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;


public class ObjectHighlighter : MonoBehaviour
{
    public GameObject objectToSpawn;
    public objectspawner objectPlacement;
    int selectedIndex = -1;
    private ARRaycastManager raycastmanager;
    GameObject selected;

    private MeshRenderer meshRenderer;

    [SerializeField]
    public Material originalMaterial;

    [SerializeField]
    public Material boundingMaterial;

    private GameObject previouslySelected;
    private Material previouslySelectedMaterial;

    //public bool isButtonClicked = false;
    private void Start()
    {
        raycastmanager = FindAnyObjectByType<ARRaycastManager>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(buttonClicked);
    }

    // Update is called once per frame
    void Update()
    { 
        // Check for object selection
        if (Input.touchCount > 0)
        {
            //object selection part
            if (Input.GetTouch(0).phase == TouchPhase.Ended)

            {
                Debug.Log("objectHighlighter working");
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {


                    if (previouslySelected != null && previouslySelectedMaterial != null)
                    {
                        previouslySelected.GetComponent<MeshRenderer>().material = previouslySelectedMaterial;
                    }


                    selected = hit.transform.gameObject;
                    int i;
                    // Check if the selected object is in the array
                    for (i = 0; i < objectPlacement.addedPrefabs.Count; i++)
                    {
                        if (selected.transform.position == objectPlacement.addedPrefabs[i].transform.position)
                        {
                            selectedIndex = i;
                            break;
                        }
                    }

                    if (selectedIndex != -1)
                    {
                        meshRenderer = selected.GetComponent<MeshRenderer>();

                        if (meshRenderer == null)
                        {
                            return;
                        }
                        else
                        {
                            // Store the previously selected object and its original material.
                            previouslySelected = selected;
                            previouslySelectedMaterial = objectPlacement.addedPrefabs[i].GetComponent<MeshRenderer>().material;
                            selected.GetComponent<MeshRenderer>().material = boundingMaterial;
                        }
                    }
                }

            }
        }

    }

    private void buttonClicked()
    {
        Debug.Log("Button clicked");
        Button button = GetComponent<Button>();
        // Check the name of the button before spawning the object
        string buttonName = button.name;
        if (buttonName == "Done")
        {
            DeselectObject();
        }
    }

    private void DeselectObject()
    {
        Debug.Log("Original material: " + originalMaterial.name);
        selected.GetComponent<MeshRenderer>().material = originalMaterial;
        meshRenderer = null;
    }
}