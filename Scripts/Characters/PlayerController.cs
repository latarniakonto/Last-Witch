using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{   
    private CharacterController _controller;
    private NavMeshAgent _agent;
    private Movement _movement;
    private GameState _state;
    private SoundController _sound;
    private TerrainDetector _detector;
    private float liftRotationSpeed = 2f;
    private float smoothlift = 350f;
    private float liftDuration = 5f;
    private float grabbingRange = 2f;
    private float randomStepLength;
    private float distanceTravelled = 0f;
    private Vector3 lastPosition;
    [SerializeField]
    private float speed = 12f;
    [SerializeField]
    private Transform pantry;   
    [SerializeField]
    private Vector3 startingPosition;
    [SerializeField]
    private float stepDistance;

    private void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _movement = gameObject.GetComponent<Movement>();
        _sound = gameObject.GetComponent<SoundController>();
        _state = GameObject.Find("GameState").GetComponent<GameState>();
        lastPosition = transform.position;
        randomStepLength = UnityEngine.Random.Range(0, 0.5f);
        _detector = new TerrainDetector();
    }

    void Update()
    {
        if(_state.IsEnemyDead() == true)
        {
            EnemyDeathSequence();
            return;
        }
        if(_state.IsPlayerDead() == false)
        {
            Moving();
            DoorInteraction();
            ObjectPickupInteraction();
            PikeInteraction();
            ShiftKeyMechanic();
            LadderInteraction();
        }else
        {
            PlayerDeathSequence();
        }
    }

    private void PlayerDeathSequence()
    {
        gameObject.GetComponentInChildren<Looking>().enabled = false;
        StartCoroutine(Death());        
    }

    private void EnemyDeathSequence()
    {
        gameObject.GetComponentInChildren<Looking>().enabled = false;
        Vector3 newPosition = _state.TargetEnemy().position;
        newPosition.x = transform.position.x;
        newPosition.z = transform.position.z;

        Vector3 targetDirection = _state.TargetEnemy().position - transform.position;

        float singleStep = liftRotationSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
        transform.GetChild(1).transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void Moving()
    {
        int material = MaterialCheck();
        int type = _state.IsHoldingShift() == true ? 1 : 0;
        _movement.Move(_controller, _agent, speed);
        distanceTravelled += (transform.position - lastPosition).magnitude;
        if(lastPosition != transform.position && lastPosition != startingPosition)
        {            
            if(distanceTravelled >= stepDistance + randomStepLength)
            {
                _sound.PlayFootStepsSound(material, type);
                distanceTravelled = 0f;
                randomStepLength = UnityEngine.Random.Range(0f, 0.5f);
            }
        }
        lastPosition = transform.position;
    }
    private int MaterialCheck()
    {
        int material = 1; //Default material index
        material = _detector.GetActiveTerrainTextureIdx(transform.position);        
        switch (material)
        {
            case (int)TerrainTextures.Dirt:
                material = (int)FootStepsMaterials.Dirt;
                break;
            case (int)TerrainTextures.Grass:
                material = (int)FootStepsMaterials.Grass;
                break;
            case (int)TerrainTextures.Stone:
                material = (int)FootStepsMaterials.Dirt;
                break;
            default:
                break;

        }              
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, grabbingRange))
        {
            if (hit.collider.tag == "Wood")
                material = (int)FootStepsMaterials.Wood;

            if (hit.collider.tag == "Rock")
                material = (int)FootStepsMaterials.Dirt;
        }
        return material;
    }
    private void LadderInteraction()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, grabbingRange*0.3f))
            {
                if (hit.collider.tag == "Ladder")
                {                    
                    _agent.Warp(pantry.position);
                    if(GameObject.Find("BlockDoor") != null)
                        GameObject.Find("BlockDoor").SetActive(false);                    
                }
            }
        }
    }

    private void ShiftKeyMechanic()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed /= 2;
            _state.HoldsShift(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed *= 2;
            _state.HoldsShift(false);
        }
    }

    private void PikeInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, grabbingRange))
            {                
                if (hit.collider.tag == "Pike")
                {
                    if (_state.IsInventoryFull())
                    {                        
                        _state.KilledEnemy();
                        _state.UpdatePlayerPosition(transform);
                    }
                }
            }
        }
    }

    private void ObjectPickupInteraction()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, grabbingRange))
            {
                if (hit.collider.tag == "Pickable")
                {
                    var pickup = hit.collider.gameObject.GetComponentInParent<ObjectPickup>();
                    string objectName = pickup.Pickup();
                    _state.UpdateInventory(objectName);                    
                }
            }
        }
    }
    private IEnumerator Death()
    {
        float counter = 0;
        Vector3 newPosition = _state.TargetEnemy().position;
        newPosition.x = transform.position.x;
        newPosition.z = transform.position.z;

        Vector3 targetDirection = _state.TargetEnemy().position - transform.position;

        float singleStep = liftRotationSpeed * Time.deltaTime;
        
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);        

        transform.rotation = Quaternion.LookRotation(newDirection);
        transform.GetChild(1).transform.rotation = Quaternion.LookRotation(newDirection);
        
        while (counter < liftDuration)
        {
            counter += Time.deltaTime;
            if (counter >= liftDuration)
            {
                break;
            }
            transform.position = Vector3.Lerp(transform.position, newPosition, counter / smoothlift);            
            yield return null;
        }        
        _sound.PlayNeckSnappingSound();
        _state.PlayerFinishedDeathSequence();
    }   
    private void DoorInteraction()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, grabbingRange))
            {
                if (hit.collider.tag == "Door" || hit.collider.tag == "Closed")
                {                    
                    var door = hit.collider.gameObject.GetComponentInParent<Door>();
                    if (door.IsOpen() == true)
                    {
                        door.CloseDoor();
                    }
                    else
                    {
                        door.OpenDoor();
                    }                    
                }                
            }
        }
    }
}
