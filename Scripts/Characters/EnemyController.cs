using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private CharacterController _controller;    
    private NavMeshAgent _agent;
    private Movement _movement;
    private SphereCollider _sphere;
    private GameState _state;
    private SoundController _sound;    
    private int nextWaypoint = -1;
    private float timeSinceLastSawPlayer = Mathf.Infinity;
    private float dwellTime = Mathf.Infinity;
    private bool atPike = false;
    private bool isAttacking = false;    
    [SerializeField]
    private Waypoint[] waypoints;
    [SerializeField]
    private Waypoint[] letterWaypoints;
    [SerializeField]
    private Waypoint[] waterWaypoints;
    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float attackSpeed = 1f;
    [SerializeField]
    private float chaseRange = 8f;
    [SerializeField]
    private float pikeChaseRange = 12f;
    [SerializeField]
    private float suspicionTime = 0f;
    [SerializeField]
    private int pikeWaypoint = 0;
    [SerializeField]
    private Transform placeOfDeath;
    
    void Start()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _controller = gameObject.GetComponent<CharacterController>();
        _movement = gameObject.GetComponent<Movement>();
        _sphere = gameObject.GetComponent<SphereCollider>();
        _state = GameObject.Find("GameState").GetComponent<GameState>();
        _sound = GameObject.Find("Player").GetComponent<SoundController>();
        if (waypoints.Length != 0) 
            nextWaypoint = 0;
        if (_sphere != null)
            _sphere.radius = chaseRange;
              
    }
    
    void Update()
    {
        if(_state.IsEnemyDead() == false)
        {
            if (timeSinceLastSawPlayer < suspicionTime || dwellTime < suspicionTime)
            {
                if (timeSinceLastSawPlayer < suspicionTime)
                    timeSinceLastSawPlayer += Time.deltaTime;
                if (dwellTime < suspicionTime)
                    dwellTime += Time.deltaTime;
                return;
            }
            if (waypoints.Length == 0) return;
            if (isAttacking == true) return;
            if(_state.WasLetterPickedUp())
            {
                atPike = false;
                pikeWaypoint = 0;                
                waypoints = letterWaypoints;
            }
            if(_state.WasWaterPickedUp())
            {
                atPike = false;
                pikeWaypoint = 0;
                waypoints = waterWaypoints;
            }
            if (_state.IsPlayerDead() == true) return;

            PatrollingBehaviour();
        }else
        {
            StartCoroutine(Death(_state.EnemyFinishedDeathSequence));
        }       
    }
    private void PatrollingBehaviour()
    {
        Vector3 targetPosition = waypoints[nextWaypoint].transform.position;
        if (Vector3.Distance(targetPosition, gameObject.transform.position) > 5f)
        {
            _movement.MoveTo(_controller, _agent, targetPosition, speed);
        }
        else
        {
            dwellTime = 0f;
            if (nextWaypoint == pikeWaypoint)
            {
                if (_sphere != null)
                    _sphere.radius = pikeChaseRange;
                chaseRange = pikeChaseRange;
                atPike = true;
            }
            nextWaypoint = (nextWaypoint + 1) % waypoints.Length;
            if (nextWaypoint == 0) nextWaypoint += pikeWaypoint;
        }
    }
    private IEnumerator Death(Action OnComplete)
    {
        _movement.MoveTo(_controller, _agent, _state.TargetPlayer().position, attackSpeed);
        yield return new WaitForSeconds(0.4f);        
        transform.position = placeOfDeath.position;
        yield return new WaitForSeconds(2.5f);
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        gameObject.GetComponentInChildren<Light>().enabled = false;
        
        yield return new WaitForSeconds(0.4f);        
        OnComplete();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        if (_state.IsEnemyDead() == true) return;        
        if (_state.IsPlayerSafe() == true)
        {
            isAttacking = false;            
            return;
        }

        if (_state.IsHoldingShift() == true && atPike == true) return;                        
        isAttacking = true;
        Vector3 targetPosition = other.transform.position;
        _movement.MoveTo(_controller, _agent, targetPosition, attackSpeed);
        if (Vector3.Distance(targetPosition, gameObject.transform.position) < 3f)
        {
            _sound.PlayScreamingSound();
            transform.LookAt(other.transform);
            _movement.MoveTo(_controller, _agent, transform.position, attackSpeed);
            _state.PlayerCaught();            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;
        if (_state.IsEnemyDead() == true) return;
        
        if (_state.IsPlayerSafe() == true)
        {
            isAttacking = false;            
            return;
        }
        if (_state.IsPlayerDead() == true) return;
        if (_state.IsHoldingShift() == true && atPike == true) return;        
        Transform target = other.transform;
        Vector3 targetPosition = other.transform.position;
        _movement.MoveTo(_controller, _agent, targetPosition, attackSpeed);
        if (Vector3.Distance(targetPosition, gameObject.transform.position) < 3f)
        {
            _sound.PlayScreamingSound();
            _state.PlayerCaught();
            transform.LookAt(target);
            _movement.MoveTo(_controller, _agent, transform.position, attackSpeed);
        }
    }    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        if (_state.IsEnemyDead() == true) return;
        
        isAttacking = false;
        timeSinceLastSawPlayer = 0f;        
        _movement.MoveTo(_controller, _agent, transform.position, attackSpeed);        
    }
}
