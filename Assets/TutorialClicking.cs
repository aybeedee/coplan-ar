using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialClicking : MonoBehaviour
{
    public GameObject ObjPlacement;
    public GameObject Objselect;
    public GameObject Objmove;
    public GameObject Objrotate;
    public GameObject Objscale;

    bool cp = false;
    bool cs = false;
    bool cm = false;
    bool cr = false;
    bool ws = false;

    void Update()
    {
        if (cp)
        {
            ObjPlacement.SetActive(false);
            cp = false;
        }

        if (cs)
        {

        }

        if (cm)
        {

        }

        if (cr)
        {

        }

        if (ws)
        {

        }



    }

    //functionality of objPlacement

    public void Helpclicking()
    {
        ObjPlacement.SetActive(true);
    }

    public void PlaceToselect()
    {
        ObjPlacement.SetActive(false);
        Objselect.SetActive(true);
    }

    public void closePlacement()
    {
        cp = true;
    }

    //functionality of objSelect

    public void selectToPlace()
    {
        Objselect.SetActive(false);
        ObjPlacement.SetActive(true);   
    }

    public void closeSelect()
    {
        Objselect.SetActive(false);
    }

    public void selectTomove()
    {
        Objselect.SetActive(false);
        Objmove.SetActive(true);
    }

    //functionality of objMove

    public void moveToselect()
    {
        Objmove.SetActive(false);
        Objselect.SetActive(true);
    }

    public void closeMove()
    {
        Objmove.SetActive(false);
    }

    public void moveTorotate()
    {
        Objmove.SetActive(false);
        Objrotate.SetActive(true);
    }

    //functionality of objRotate

    public void rotateTomove()
    {
        Objrotate.SetActive(false);
        Objmove.SetActive(true);
    }

    public void closeRotate()
    {
        Objrotate.SetActive(false);
    }

    public void rotateToscale()
    {
        Objrotate.SetActive(false);
        Objscale.SetActive(true);
    }

    //functionality of objscale
    public void scaletorotate()
    {
        Objscale.SetActive(false);
        Objrotate.SetActive(true);
    }

    public void closeScale()
    {
        Objscale.SetActive(false);
    }

}
