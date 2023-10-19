using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSelection : MonoBehaviour
{
    //private GameObject selectedObject;
    //public BtnManager obj;
    //int selectedIndex=-1;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    obj = FindObjectOfType<BtnManager>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    // Check for object selection
    //    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    //    {
    //        Debug.Log("in object selection script");

    //        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
    //        RaycastHit hit;

    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            GameObject selected = hit.transform.gameObject;
    //            Debug.Log("Selected Object Name: " + selected.name);

    //            foreach (GameObject addedPrefab in obj.objectPlacement.addedPrefabs)
    //            {
    //                Debug.Log("Added Prefab Name: " + addedPrefab.transform.position.x +", "+ addedPrefab.transform.position.x + ", "+ addedPrefab.transform.position.z);
    //            }

    //            // Check if the selected object is in the array
    //            for(int i=0;i< obj.objectPlacement.addedPrefabs.Count; i++)
    //            {
    //                if(selected.transform.position.x == obj.objectPlacement.addedPrefabs[i].transform.position.x && selected.transform.position.y == obj.objectPlacement.addedPrefabs[i].transform.position.y && selected.transform.position.z == obj.objectPlacement.addedPrefabs[i].transform.position.z)
    //                {
    //                    selectedIndex = i;
    //                    break;
    //                }
    //            }

    //            Debug.Log("Selected Index: " + selectedIndex);

    //            if (selectedIndex != -1)
    //            {
    //                // Deselect the previously selected object
    //                DeselectObject();

    //                Debug.Log("Working Select Object Function");
    //                // Select the new object
    //                SelectObject(selected, selectedIndex);
    //            }
    //            selectedIndex = -1;
    //        }
    //    }


    //}
    //private void SelectObject(GameObject obj, int index)
    //{
    //    selectedObject = obj;
    //}

    //private void DeselectObject()
    //{
    //    selectedObject = null;
    //}

}
