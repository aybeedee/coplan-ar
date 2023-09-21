using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

//automatically adds ray cast manager when script is added to a game object
[RequireComponent(typeof(ARRaycastManager))]

public class PlacementController : MonoBehaviour
{

    [SerializeField]
    private GameObject placementModel;

    [SerializeField]
    private Camera arCamera;

    [SerializeField]
    private Button rotateButton;

    private PlacedPrefab[] placedPrefabs;

    private Vector2 touchPosition = default;

    private ARRaycastManager arRaycastManager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private PlacedPrefab lastSelectedPrefab;

    private List<GameObject> addedPrefabs = new List<GameObject>(); 

    public GameObject PlacementModel {

        get { 

            return placementModel;
        }

        set {

            placementModel = value;
        }
    }

    void Awake() {

        arRaycastManager = GetComponent<ARRaycastManager>();
        rotateButton.onClick.AddListener(RotatePrefab);
    }

    private void RotatePrefab() {

        Debug.Log("HELLO");
        //GameObject lastAddedPrefab = addedPrefabs[addedPrefabs.Count - 1];
        //lastAddedPrefab.transform.Rotate(0, 10, 0);
    }

    void Update() {

        if (Input.touchCount > 0) { 
        
            Touch touch = Input.GetTouch(0);
            
            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began) {

                Ray ray = arCamera.ScreenPointToRay(touchPosition);

                RaycastHit hitObject;

                if (Physics.Raycast(ray, out hitObject)) { 
                    
                    lastSelectedPrefab = hitObject.transform.GetComponent<PlacedPrefab>();

                    if (lastSelectedPrefab != null) {

                        PlacedPrefab[] allPlacedPrefabs = FindObjectsOfType<PlacedPrefab>();

                        foreach (PlacedPrefab placedPref in allPlacedPrefabs) {

                            if (placedPref != lastSelectedPrefab)
                            {

                                placedPref.Selected = false;
                            }

                            else {

                                placedPref.Selected = true; 
                            }
                        }
                    }
                }

            }

            if (touch.phase == TouchPhase.Ended) {

                lastSelectedPrefab.Selected = false;
            }
        }

        if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)) {

            Pose hitPose = hits[0].pose;

            if (lastSelectedPrefab == null) {

                lastSelectedPrefab = Instantiate(placementModel, hitPose.position, hitPose.rotation).GetComponent<PlacedPrefab>();
            }

            else if (lastSelectedPrefab.Selected) {

                lastSelectedPrefab.transform.position = hitPose.position;
                lastSelectedPrefab.transform.rotation = hitPose.rotation;
            }
        }
    }
}
