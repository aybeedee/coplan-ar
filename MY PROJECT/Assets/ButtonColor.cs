using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour
{
    private Button button;
    private ColorBlock originalColors;

    private void Start()
    {
        button = GetComponent<Button>();
        originalColors = button.colors;
        button.onClick.AddListener(ChangeButtonColor);
    }

    private void ChangeButtonColor()
    {
        // Revert the color of the previously clicked button
        ChangeButtonColorToOriginal();

        // Change the color of the clicked button to green
        ColorBlock colors = button.colors;
        colors.normalColor = Color.green;
        button.colors = colors;
    }

    private void ChangeButtonColorToOriginal()
    {
        // Revert the color of the previously clicked button to its original color
        button.colors = originalColors;
    }
}