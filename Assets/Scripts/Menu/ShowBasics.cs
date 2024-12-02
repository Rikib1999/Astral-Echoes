using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBasics : MonoBehaviour
{
    public GameObject MenuCanvas;

    public GameObject BasicsCanvas;


    public void ShowBasicControls()
    {
        
        BasicsCanvas.SetActive(true);
        MenuCanvas.SetActive(false);


    }

    public void ShowMenuFromBasics()
    {

        BasicsCanvas.SetActive(false);
        MenuCanvas.SetActive(true);

    }

}
