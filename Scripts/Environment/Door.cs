using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Transform doorPivot;
    private Vector3 defaultRotation;
    private Vector3 targetRotation = new Vector3(0f, 90f, 0f);
    [SerializeField]
    private bool isOpen = false;
    private float duration = 1f;
    private GameState _state;
    private SoundController _sound;
    
    void Start()
    {
        doorPivot = gameObject.transform.parent.GetComponent<Transform>();
        defaultRotation = doorPivot.eulerAngles;
        _state = GameObject.Find("GameState").GetComponent<GameState>();
        _sound = GameObject.Find("Player").GetComponent<SoundController>();
    }       
    public void OpenDoor()
    {
        if (isOpen == true) return;
        if(_state.CanOpenDoor() == false || tag == "Closed")
        {
            Debug.Log("SIEMA");
            _sound.PlayDoorBlockingSound();
            return;
        }
        
        isOpen = true;
        _state.ChangeDoorState(isOpen);
        _sound.PlayDoorOpeningSound();
        StartCoroutine("DoorRotation", defaultRotation + targetRotation);        
    }
    public void CloseDoor()
    {
        if (isOpen == false) return;
        
        isOpen = false;
        _state.ChangeDoorState(isOpen);
        _sound.PlayDoorClosingSound();
        StartCoroutine("DoorRotation", defaultRotation);
    }
    private IEnumerator DoorRotation(Vector3 newRotation)
    {
        float counter = 0;
        Vector3 defaultAngles = doorPivot.eulerAngles;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            doorPivot.eulerAngles = Vector3.Lerp(defaultAngles, newRotation, counter / duration);
            yield return null;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;        
       _state.PlayerSafe(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        _state.PlayerSafe(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        _state.PlayerSafe(false);
    }    
    public bool IsOpen() => isOpen;
}
