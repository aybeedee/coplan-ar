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
    private GameObject placedPrefab;

    private ARRaycastManager arRaycastManager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private List<GameObject> addedPrefabs = new List<GameObject>();

    private Vector2 touchPosition = default;

    [SerializeField]
    private Button rotateButton;

    public GameObject PlacedPrefab {

        get { 

            return placedPrefab;
        }

        set {

            placedPrefab = value;
        }
    }

    void Awake() {

        arRaycastManager = GetComponent<ARRaycastManager>();
        rotateButton.onClick.AddListener(RotatePrefab);
    }

    private void RotatePrefab() {

        GameObject lastAddedPrefab = addedPrefabs[addedPrefabs.Count - 1];
        lastAddedPrefab.transform.Rotate(0, 10, 0);
    }

    void Update() {

        if (Input.touchCount > 0) { 
        
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {

                touchPosition = touch.position;

                if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)) {

                    var hitPose = hits[0].pose;

                    if (addedPrefabs.Count < 2) {

                        GameObject addedPrefab = Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                        addedPrefabs.Add(addedPrefab);
                    }
                }
            }

            if (touch.phase == TouchPhase.Moved) {

                touchPosition = touch.position;

                if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)) {

                    var hitPose = hits[0].pose;

                    if (addedPrefabs.Count > 0) {

                        GameObject lastAddedPrefab = addedPrefabs[addedPrefabs.Count - 1];
                        lastAddedPrefab.transform.position = hitPose.position;
                        lastAddedPrefab.transform.rotation = hitPose.rotation;
                    }
                }
            }
        }
    }
}
