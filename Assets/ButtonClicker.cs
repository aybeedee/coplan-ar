using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
// using Firebase;
// using Firebase.Auth;
// using Firebase.Database;
using TMPro;
// using Firebase.Extensions;


public class ButtonClicker : MonoBehaviour
{
    public Canvas LoginPage;
    public Canvas SignUp;
    public Canvas dashboard;
    public Canvas arCanvas;
    public ARSession arSession;
    public GameObject colocalizationObject;
    public ARMeshManager _arMeshManager;


    public void OpenRoomPage()
    {
        LoginPage.gameObject.SetActive(false);
        dashboard.gameObject.SetActive(true);        
    }

    

    public void OpenSignUpPage()
    {
        LoginPage.gameObject.SetActive(false);
        SignUp.gameObject.SetActive(true);
    }

    public void OpenLoginPage()
    {
        SignUp.gameObject.SetActive(false);
        LoginPage.gameObject.SetActive(true);
    }

    //public void CreateRoom()
    //{
    //    create_room.gameObject.SetActive(false);
    //    SignUp.gameObject.SetActive(false);
    //    LoginPage.gameObject.SetActive(false);
    //    join_room.gameObject.SetActive(true);
    //}

    public void JoinRoom()
    {
        SignUp.gameObject.SetActive(false);
        dashboard.gameObject.SetActive(false);
        LoginPage.gameObject.SetActive(false);

        // Activate AR canvas
        arCanvas.gameObject.SetActive(true);

        _arMeshManager.enabled = true;

        // Start AR session
        // arSession.enabled = true;

        // Activate colocalization script
        // colocalizationObject.SetActive(true);
    }

    //public void JoinRoom2()
    //{
    //    SignUp.gameObject.SetActive(false);
    //    join_room.gameObject.SetActive(false);
    //    LoginPage.gameObject.SetActive(false);


    //    // Activate AR canvas
    //    arCanvas.gameObject.SetActive(true);

    //    // Start AR session
    //    arSession.enabled = true;
    //}
}
