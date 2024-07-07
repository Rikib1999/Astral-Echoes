using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{

    public GameObject craftTable;


    private void OnMouseDown()
    {

        if (craftTable.activeSelf)
        {
            craftTable.SetActive(false);
        }
        else
        {
            craftTable.SetActive(true);
        }


    }


}
