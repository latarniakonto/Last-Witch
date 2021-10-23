using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float gravity = 9.81f;   
    private GameState _state;
    private void Start()
    {
        _state = GameObject.Find("GameState").GetComponent<GameState>();        
    }
    public void Move(CharacterController controller, NavMeshAgent agent, float speed)
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
                
        Vector3 move = transform.right * x + transform.forward * z;
        move.y -= gravity;

        agent.SetDestination(move);
        controller.Move(move * speed * Time.deltaTime);
    }
    public void MoveTo(CharacterController controller, NavMeshAgent agent, Vector3 position, float speed)
    {
        agent.speed = speed;
        agent.SetDestination(position);        
    }
}
