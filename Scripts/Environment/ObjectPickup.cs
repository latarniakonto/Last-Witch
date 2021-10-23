using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    [SerializeField]
    private string objectName = "List";
    private Inventory _inventory;    
    private GameState _state;
    private SoundController _sound;
    
    private void Start()
    {
        _state = GameObject.Find("GameState").GetComponent<GameState>();        
        _inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        _sound = GameObject.Find("Player").GetComponent<SoundController>();
    }
    public string Pickup()
    {        
        gameObject.SetActive(false);
        _state.ObjectWasPickedup(objectName);
        _inventory.DisplayNewObject(objectName);
        _sound.PlayPickupSound(objectName); 
        return objectName;
    }
}
