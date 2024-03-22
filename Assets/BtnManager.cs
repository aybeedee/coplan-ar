using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;


public class BtnManager : MonoBehaviour
{
    //public GameObject objTospawn;
    //public GameObject objectToSpawn;
    //public objectspawner objectPlacement;

    public GameObject chairPrefab;
    public GameObject wallPrefab;
    public GameObject tablePrefab;

    private List<GameObject> addedPrefabs = new List<GameObject>();

    private logic placementIndicator;

    int selectedIndex = -1;
    private Button chairButton;
    private Button tableButton;
    private Button wallButton;
    private Button doneButton;
    private Button deleteButton;
    private GameObject slider_panel;
    private GameObject selection_panel;

    private ARRaycastManager raycastmanager;
    private Vector2 touchPosition = default;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    GameObject selected;
    bool move_freely = false;

    private MeshRenderer meshRenderer;

    [SerializeField]
    public Material originalMaterial;

    [SerializeField]
    public Material boundingMaterial;

    private GameObject previouslySelected;
    private Material previouslySelectedMaterial;

    private Vector2 initialTouch1Pos;
    private Vector2 initialTouch2Pos;
    private float initialRotationAngle;
    private bool isRotating = false;

    [SerializeField]
    private Slider scaleSliderY;

    [SerializeField]
    private Slider scaleSliderZ;

    bool ScalinginProgress = false;

    private void Awake()
    {
        Debug.Log("TEST1");
        if (scaleSliderY != null)
        {
            Debug.Log("TEST2");
            scaleSliderY.onValueChanged.AddListener(ScaleChangedY);
        }
        if (scaleSliderZ != null)
        {
            Debug.Log("TEST3");
            scaleSliderZ.onValueChanged.AddListener(ScaleChangedZ);
        }
    }
    private void Start()
    {
        doneButton = GameObject.Find("Done").GetComponent<Button>();
        chairButton = GameObject.Find("chair").GetComponent<Button>();
        tableButton = GameObject.Find("table").GetComponent<Button>();
        wallButton = GameObject.Find("wall").GetComponent<Button>();
        deleteButton = GameObject.Find("Delete").GetComponent<Button>();
        slider_panel = GameObject.Find("Scaling Panel");
        selection_panel = GameObject.Find("Button Panel");
        slider_panel.SetActive(false);

        Debug.Log("chairButton: "+ chairButton.name);
        Debug.Log("wallButton: " + tableButton.name);
        Debug.Log("tableButton: " + wallButton.name);
        Debug.Log("doneButton: " + doneButton.name);

        placementIndicator = FindObjectOfType<logic>();

        Debug.Log("placement indicator script working");

        raycastmanager = FindAnyObjectByType<ARRaycastManager>();

        chairButton.onClick.AddListener(SpawnChair);
        tableButton.onClick.AddListener(SpawnTable);
        wallButton.onClick.AddListener(SpawnWall);
        doneButton.onClick.AddListener(DeselectObject);
        deleteButton.onClick.AddListener(DeleteObject);

        // Assuming you have a reference to your slider
        scaleSliderY.onValueChanged.AddListener(EndScaling);
        scaleSliderZ.onValueChanged.AddListener(EndScaling);

        DisableButtons();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if any UI element is being interacted with
        if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)
        {
            return;
        }
        else
        {
            if (ScalinginProgress == true)
            {
                return;
            }
            else
            {
                // Check for object selection
                if (Input.touchCount == 1)
                {

                    //object selection part
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)

                    {
                        Debug.Log("object selection worked");
                        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit))
                        {


                            if (previouslySelected != null)
                            {
                                previouslySelected.GetComponent<MeshRenderer>().material = originalMaterial;
                            }


                            selected = hit.transform.gameObject;
                            int i;
                            // Check if the selected object is in the array
                            for (i = 0; i < addedPrefabs.Count; i++)
                            {
                                if (selected.transform.position == addedPrefabs[i].transform.position)
                                {
                                    selectedIndex = i;
                                    break;
                                }
                            }
                            ObjectHighlighting();
                        }

                    }
                    movement();

                }

                Rotation();
            }
        }
        
        
    }

    public void EnableButtons()
    {
        doneButton.interactable = true;
        deleteButton.interactable = true;
        //scaleSliderY.interactable = true;
        //scaleSliderZ.interactable = true;
    }

    public void DisableButtons()
    {
        doneButton.interactable = false;
        deleteButton.interactable = false;
        //scaleslidery.interactable = false;
        //scalesliderz.interactable = false;
        if (slider_panel != null)
        {
            slider_panel.SetActive(false);
        }
    }

    private void DeselectObject()
    {
        Debug.Log("Presviouly material : " + originalMaterial);
        selected.GetComponent<MeshRenderer>().material = originalMaterial;
        meshRenderer = null;
        previouslySelectedMaterial = null;

        addedPrefabs[selectedIndex] = selected;
        selectedIndex = -1;
        selected = null;
        move_freely = false;
        DisableButtons();
    }

    public void Rotation()
    {
        if (doneButton.interactable == true)
        {
            if (selected != null)
            {
                if (Input.touchCount == 2)
                {
                    Debug.Log("in rotation function");
                    // Get the first two touches
                    Touch touch1 = Input.GetTouch(0);
                    Touch touch2 = Input.GetTouch(1);

                    // Check if the touches just began
                    if (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began)
                    {
                        // Store initial touch positions and rotation angle
                        initialTouch1Pos = touch1.position;
                        initialTouch2Pos = touch2.position;
                        initialRotationAngle = Mathf.Atan2(initialTouch2Pos.y - initialTouch1Pos.y, initialTouch2Pos.x - initialTouch1Pos.x) * Mathf.Rad2Deg;
                        isRotating = true;
                    }
                    if (selected.name == "ChairChild" || selected.name == "pCube1")
                    {
                        // Continue rotation if already started
                        if (isRotating)
                        {
                            Vector2 currentTouch1Pos = touch1.position;
                            Vector2 currentTouch2Pos = touch2.position;
                            float currentRotationAngle = Mathf.Atan2(currentTouch2Pos.y - currentTouch1Pos.y, currentTouch2Pos.x - currentTouch1Pos.x) * Mathf.Rad2Deg;

                            // Calculate the rotation difference
                            float deltaRotation = currentRotationAngle - initialRotationAngle;

                            // Apply the rotation to the target object (around the Y-axis)
                            selected.transform.Rotate(Vector3.up, deltaRotation, Space.World);

                            // Update initial rotation angle for the next frame
                            initialRotationAngle = currentRotationAngle;

                            // Update initial touch positions for the next frame
                            initialTouch1Pos = currentTouch1Pos;
                            initialTouch2Pos = currentTouch2Pos;
                        }
                        else
                        {
                            isRotating = false;
                        }
                    }
                    if (selected.name == "desk")
                    {
                        // Continue rotation if already started
                        if (isRotating)
                        {
                            Vector2 currentTouch1Pos = touch1.position;
                            Vector2 currentTouch2Pos = touch2.position;
                            float currentRotationAngle = Mathf.Atan2(currentTouch2Pos.y - currentTouch1Pos.y, currentTouch2Pos.x - currentTouch1Pos.x) * Mathf.Rad2Deg;

                            // Calculate the rotation difference
                            float deltaRotation = currentRotationAngle - initialRotationAngle;

                            // Apply the rotation to the target object
                            selected.transform.Rotate(Vector3.forward, deltaRotation);

                            // Update initial rotation angle for the next frame
                            initialRotationAngle = currentRotationAngle;

                            // Update initial touch positions for the next frame
                            initialTouch1Pos = currentTouch1Pos;
                            initialTouch2Pos = currentTouch2Pos;
                        }
                        else
                        {
                            isRotating = false;
                        }
                    }
                }

            }

        }
    }

    private void SpawnChair()
    {
        Debug.Log("Chair Button clicked");
        // Instantiate the chairPrefab at the spawnPoint position and rotation
        GameObject newChair = Instantiate(chairPrefab, placementIndicator.transform.position, placementIndicator.transform.rotation);
        addedPrefabs.Add(newChair);
        Debug.Log("Number of objects in the list: " + addedPrefabs.Count);
    }

    private void SpawnTable()
    {
        Debug.Log("Table Button clicked");
        // Instantiate the chairPrefab at the spawnPoint position and rotation
        GameObject newChair = Instantiate(tablePrefab, placementIndicator.transform.position, placementIndicator.transform.rotation);
        addedPrefabs.Add(newChair);
        Debug.Log("Number of objects in the list: " + addedPrefabs.Count);
    }

    private void SpawnWall()
    {
        Debug.Log("Wall Button clicked");
        // Instantiate the chairPrefab at the spawnPoint position and rotation
        GameObject newChair = Instantiate(wallPrefab, placementIndicator.transform.position, placementIndicator.transform.rotation);
        addedPrefabs.Add(newChair);
        Debug.Log("Name of object: "+wallPrefab.name);
        Debug.Log("Number of objects in the list: " + addedPrefabs.Count);
    }

    private void movement()
    {
        if (doneButton.interactable == true)
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
                    }
                }
            }
        }
    }

    private void ObjectHighlighting()
    {

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
                // Store the previously selected material.
                previouslySelected = selected;
                previouslySelectedMaterial = originalMaterial;

                Debug.Log("Previous material of object to remember : " + originalMaterial);
                selected.GetComponent<MeshRenderer>().material = boundingMaterial;
                Debug.Log("selected object material after: " + selected.GetComponent<MeshRenderer>().material);

                // check if the selected object is a wall (assuming wall index is 2)
                if (selected.name == "pCube1")
                {
                    Debug.Log("i am wall");
                    // assuming slider_panel is your panel object reference
                    if (slider_panel != null)
                    {
                        slider_panel.SetActive(true);
                        Debug.Log("slider panel set active");
                        float scaleX = selected.transform.parent.localScale.y / 2f;
                        float scaleZ = selected.transform.parent.localScale.x / 2f;
                        scaleSliderY.value = scaleX;
                        scaleSliderZ.value = scaleZ;
                    }
                }
                else
                {
                    if (slider_panel != null)
                    {
                        slider_panel.SetActive(false);
                    }
                }
            }

            move_freely = true;
        }
    }

    public void DeleteObject()
    {
        if (doneButton.interactable == true)
        {
            if (selected != null && selectedIndex != -1)
            {
                // Remove the selected object from the array
                addedPrefabs.RemoveAt(selectedIndex);

                // Destroy the selected object
                Destroy(selected);

                // Deselect the object and disable the buttons
                selected = null;
                selectedIndex = -1;
                move_freely = false;

                // Reset the meshRenderer and materials
                meshRenderer = null;
                previouslySelectedMaterial = null;

                // Remove any null elements from the array
                addedPrefabs.RemoveAll(item => item == null);

                for (int i = 0; i < addedPrefabs.Count; i++)
                {
                    if (addedPrefabs[i] != null)
                    {
                        Debug.Log("Added Prefab at index " + i + ": " + addedPrefabs[i].name);
                    }
                }

                // Disable the buttons
                DisableButtons();

            }
        }
    }

    public void ScaleChangedY(float newValue)
    {

        if (selected != null)
        {
            GameObject parentObject = selected.transform.parent.gameObject;
            Debug.Log("parent object is = " + parentObject);
            parentObject.transform.localScale = new Vector3(parentObject.transform.localScale.x, (2f * newValue), parentObject.transform.localScale.z);
            //selected.transform.localScale = parentObject.transform.localScale;
            ScalinginProgress = true;
            addedPrefabs[selectedIndex] = parentObject;
        }
    }

    public void ScaleChangedZ(float newValue)
    {

        if (selected != null)
        {
            GameObject parentObject = selected.transform.parent.gameObject;
            parentObject.transform.localScale = new Vector3((2f * newValue), parentObject.transform.localScale.y, parentObject.transform.localScale.z);
            //Debug.Log("NEW Z VALUE = " + newValue);
            //selected.transform.localScale = new Vector3(selected.transform.localScale.x, selected.transform.localScale.y, (2f * newValue));
            ScalinginProgress = true;
            addedPrefabs[selectedIndex] = parentObject;
        }
    }

    // Callback to indicate the end of scaling
    public void EndScaling(float newValue)
    {
        // Set scalingInProgress to false when the slider interaction ends
        ScalinginProgress = false;
        Debug.Log("scaling bool: "+ ScalinginProgress);
    }

}