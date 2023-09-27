using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;


public class BtnManager : MonoBehaviour
{
    public GameObject objectToSpawn;
    public objectspawner objectPlacement;
    int selectedIndex = -1;
    public Button rotateLeftButton;
    public Button rotateRightButton;
    public Button doneButton;
    private ARRaycastManager raycastmanager;
    private Vector2 touchPosition = default;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public Color clickColor = Color.green;
    private Color normalColor;
    GameObject selected;
    private GameObject nestedObject;
    bool move_freely = false;

    private MeshRenderer meshRenderer;

    [SerializeField]
    public Material originalMaterial;

    [SerializeField]
    public Material boundingMaterial;


    //public bool isButtonClicked = false;
    private void Start()
    {
       
        raycastmanager = FindAnyObjectByType<ARRaycastManager>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(buttonClicked);
        rotateLeftButton = GameObject.Find("rotateLeft").GetComponent<Button>();
        rotateRightButton = GameObject.Find("rotateRight").GetComponent<Button>();
        doneButton = GameObject.Find("Done").GetComponent<Button>();

        DisableButtons();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if any UI element is being interacted with
        if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)
        {
            // A UI element is being interacted with; don't execute object selection or movement logic
            return;
        }

        // Check for object selection
        if (Input.touchCount > 0)
        {
            //object selection part
            if (Input.GetTouch(0).phase == TouchPhase.Ended)

            {
                Debug.Log("object selection worked");
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    selected = hit.transform.gameObject;

                    // Check if the selected object is in the array
                    for (int i = 0; i < objectPlacement.addedPrefabs.Count; i++)
                    {
                        if (selected.transform.position == objectPlacement.addedPrefabs[i].transform.position)
                        {
                            selectedIndex = i;
                            break;
                        }
                    }

                    if (selectedIndex != -1)
                    {
                        EnableButtons();

                        meshRenderer = selected.GetComponent<MeshRenderer>();
                        
                        if (meshRenderer == null)
                        {
                            Debug.LogError("Mesh Renderer not found on the selected game object!");
                            return;
                        }
                        else
                        {
                            // Set the original material before changing it
                            //originalMaterial = selected.GetComponent<MeshRenderer>().material;

                            //if table u need to wrtie that in table there is box as child in unity that would be mesh renderer write logic after sessional
                            Debug.Log("original material after being selected: " + originalMaterial.name);
                            Debug.Log("Selected object is: "+selected.name);
                            Debug.Log("selected object bonding material before: "+ selected.GetComponent<MeshRenderer>().material);
                            selected.GetComponent<MeshRenderer>().material = boundingMaterial;
                            Debug.Log("selected object bonding material after: " + selected.GetComponent<MeshRenderer>().material);
                        }

                        move_freely = true;
                    }

                } 

            }

            if(doneButton.interactable == true)
            {
                if (selected != null && move_freely == true)
                {
                    // Object movement
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        touchPosition = Input.GetTouch(0).position;

                        if (raycastmanager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                        {
                            var hitPose = hits[0].pose;
                            selected.transform.position = hitPose.position;
                            selected.transform.rotation = hitPose.rotation;
                        }
                    }
                }
            } 
        }      

    }

    public void EnableButtons()
    {
        rotateLeftButton.interactable = true;
        rotateRightButton.interactable = true;
        doneButton.interactable = true;
    }

    public void DisableButtons()
    {
        rotateLeftButton.interactable = false;
        rotateRightButton.interactable = false;
        doneButton.interactable = false;
    }

    private void DeselectObject()
    {
        Debug.Log("Original material: " + originalMaterial.name);
        selected.GetComponent<MeshRenderer>().material = originalMaterial;
        meshRenderer = null;
        //originalMaterial = null;
        objectPlacement.addedPrefabs[selectedIndex] = selected;
        selectedIndex = -1;
        selected = null;
        move_freely = false;
        DisableButtons();
    }

    private void buttonClicked()
    {
        Debug.Log("Button clicked");
        Button button = GetComponent<Button>();
        // Check the name of the button before spawning the object
        string buttonName = button.name;

        if (buttonName == "chair" || buttonName == "table" || buttonName == "wall")
        {
            objectPlacement.SetObjectToSpawn(objectToSpawn);
        }
        if (buttonName == "rotateLeft")
        {
            RotatePrefabLeft();
        }
        if (buttonName == "rotateRight")
        {
            RotatePrefabRight();
        }
        if (buttonName == "Done")
        {
            DeselectObject();
        }
    }

    public void RotatePrefabLeft()
    {
        if (selected != null)
        {
            selected.transform.Rotate(0, 10f, 0);
        }
    }

    public void RotatePrefabRight()
    {
        if (selected != null)
        {
            selected.transform.Rotate(0, -10f, 0);
        }
    }

}