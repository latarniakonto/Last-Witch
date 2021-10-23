using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDoor : MonoBehaviour
{
    private GameState _state;
    private void Start()
    {
        _state = GameObject.Find("GameState").GetComponent<GameState>();
    }
    private void OnTriggerEnter(Collider other)
    {
        _state.BlockOpeningDoor(false);
    }
    private void OnTriggerStay(Collider other)
    {
        _state.BlockOpeningDoor(false);
    }
    private void OnTriggerExit(Collider other)
    {
        _state.BlockOpeningDoor(true);
    }
}
