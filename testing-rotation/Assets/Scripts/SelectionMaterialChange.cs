using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMaterialChange : MonoBehaviour
{

    private PlacedPrefab placedPref;
    private GameObject placementObject;
    private GameObject nestedDeskObject;
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Material originalMaterial;

    [SerializeField]
    private Material selectedMaterial;

    void Awake() {
        placedPref = GetComponent<PlacedPrefab>();
        placementObject = gameObject;

        if (placementObject == null)
        {
            Debug.LogError("game object reference is not set!");
            return;
        }

        nestedDeskObject = gameObject.transform.Find("desk").gameObject;

        if (nestedDeskObject == null)
        {
            Debug.LogError("nested game object not found!");
            return;
        }

        meshRenderer = nestedDeskObject.GetComponent<MeshRenderer>();

        if (meshRenderer == null)
        {
            Debug.LogError("Mesh Renderer not found on the nested game object!");
            return;
        }

        Debug.Log(placementObject.name);
        Debug.Log(nestedDeskObject.name);
    }

    void Update() {

        if (placedPref.Selected) {

            meshRenderer.material = selectedMaterial;
        }

        else {

            meshRenderer.material = originalMaterial;
        }
    }
}
