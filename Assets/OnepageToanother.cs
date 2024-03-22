using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class OnepageToanother : MonoBehaviour
{

    public Canvas splashCanvas;
    public Canvas LoginPage;
    public Canvas SignUp;
    public Canvas create_room;
    public Canvas join_room;
    public Canvas arCanvas;
    public ARSession arSession;
    bool arSessionStarted = false;


    void Start()
    {
        // Activate splash canvas
        splashCanvas.gameObject.SetActive(true);
        arCanvas.gameObject.SetActive(false);

        // Delayed transition to login canvas after 2 seconds
        Invoke("ShowLoginPage", 2f);
    }

    void ShowLoginPage()
    {
        // Deactivate splash canvas
        splashCanvas.gameObject.SetActive(false);

        // Activate login canvas
        LoginPage.gameObject.SetActive(true);
    }

    //void Start()
    //{
    //    // Activate splash canvas
    //    splashCanvas.gameObject.SetActive(true);
    //    arCanvas.gameObject.SetActive(false);

    //    // Delayed transition to AR canvas after 2 seconds
    //    Invoke("ShowARCanvas", 2f);
    //}

    //void ShowARCanvas()
    //{
    //    // Deactivate splash canvas
    //    splashCanvas.gameObject.SetActive(false);

    //    // Activate AR canvas
    //    arCanvas.gameObject.SetActive(true);

    //    // Start AR session
    //    StartCoroutine(EnableARSession());
    //}

    //IEnumerator EnableARSession()
    //{
    //    // Wait for 2 seconds
    //    yield return new WaitForSeconds(2f);

    //    // Start AR session
    //    arSession.enabled = true;
    //}
}

