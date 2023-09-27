using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class logic : MonoBehaviour
{
    private ARRaycastManager raycastmanager;
    private GameObject obj;
    private Touch touch;

    // Start is called before the first frame update
    void Start()
    {
        raycastmanager = FindAnyObjectByType<ARRaycastManager>();
        obj = transform.GetChild(0).gameObject;

        obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastmanager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);


        if (hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
            }
        }
    }
}
