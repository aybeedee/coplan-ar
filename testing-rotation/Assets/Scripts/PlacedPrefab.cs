using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedPrefab : MonoBehaviour
{
    [SerializeField]
    private bool isSelected;

    public bool Selected {

        get {
            return this.isSelected;
        }

        set {

            isSelected = value;
        }
    }
}