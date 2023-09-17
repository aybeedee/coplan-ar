using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

//automatically adds ray cast manager when script is added to a game object
[RequireComponent(typeof(ARRaycastManager))]

public class PlacementController : MonoBehaviour
{

    [SerializeField]
    private GameObject placedPrefab;

    private ARRaycastManager arRaycastManager;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

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
    }

    bool TryGetTouchPosition(out Vector2 touchPosition) {

        if (Input.touchCount > 0) {

            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;

        return false;
    }

    void Update() {

        if (!TryGetTouchPosition(out Vector2 touchPosition)) {

            return;
        }

        if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)) {

            var hitPose = hits[0].pose;

            Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
        }
    }
}
