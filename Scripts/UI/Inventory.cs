using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject[] items;
    [SerializeField]
    private GameObject projector;    
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(items[0].active == true)
            {
                DeactivateOtherLetters(0);
                projector.transform.GetChild(0).gameObject.SetActive(true);
                projector.SetActive(true);
                return;
            }          
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            if (items[0].active == true)
            {
                projector.transform.GetChild(0).gameObject.SetActive(false);
                projector.SetActive(false);
                return;
            }
        }        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {            
            if (items[1].active == true)
            {
                DeactivateOtherLetters(1);
                projector.transform.GetChild(1).gameObject.SetActive(true);
                projector.SetActive(true);
                return;
            }
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            if (items[1].active == true)
            {
                projector.transform.GetChild(1).gameObject.SetActive(false);
                projector.SetActive(false);
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (items[2].active == true)
            {
                DeactivateOtherLetters(2);
                projector.transform.GetChild(2).gameObject.SetActive(true);
                projector.SetActive(true);
                return;
            }
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            if (items[2].active == true)
            {
                projector.transform.GetChild(2).gameObject.SetActive(false);
                projector.SetActive(false);
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (items[3].active == true)
            {
                DeactivateOtherLetters(3);
                projector.transform.GetChild(3).gameObject.SetActive(true);
                projector.SetActive(true);
                return;
            }
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            if (items[3].active == true)
            {
                projector.transform.GetChild(3).gameObject.SetActive(false);
                projector.SetActive(false);
                return;
            }

        }
    }
    public void DeactivateOtherLetters(int index)
    {
        for(int i = 0; i < 4; i++)
        {
            if (i != index)
                projector.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void DisplayNewObject(string objectName)
    {
        if (objectName == null) return;
        foreach(var item in items)
        {
            if(item.name == objectName)
            {
                item.SetActive(true);
            }
        }
    }
}
