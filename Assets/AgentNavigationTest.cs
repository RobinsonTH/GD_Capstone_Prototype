using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentNavigationTest : MonoBehaviour
{
    public Transform target; // Assign your target object in the Inspector
    private NavMeshAgent agent;

    

    private Rigidbody2D rb;
    private Character character;

    private Vector3 direction;
    private Quaternion lookDirection;

    //private NavMeshHit hit;

    //[SerializeField] Transform shieldTransform;
    [SerializeField] float maxSpeed;
    [SerializeField] float maxAngleSpeed;

    private void Awake()
    {
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.updatePosition = true;
        // Sample a position on the NavMesh near a spawn point
        //NavMeshHit hit;
        /*if (NavMesh.SamplePosition(transform.position, out hit, 1000f, 1))
        {
            agent.Warp(hit.position);
            Debug.Log("Agent placed on NavMesh at " + hit.position);
        }
        else
        {
            Debug.LogError("Failed to place agent on NavMesh!");
        }*/
    }

    void Update()
    {
        agent.updatePosition = (character == null || character.GetControl());
        if (target != null)
        {
            agent.SetDestination(target.position);

            
            //lookDirection
            if (agent.hasPath)
            {
                //transform.rotation
                //direction = (agent.steeringTarget - transform.position).normalized;
                //direction = (agent.nextPosition - transform.position).normalized;
                //lookDirection = Quaternion.LookRotation(Vector3.forward, direction);
                //transform.rotation = Quaternion.Lerp(transform.rotation, lookDirection, 0.5f * Time.deltaTime);
                /*if(Vector3.Angle(transform.up, direction) > 30f)
                {
                    //agent.updatePosition = false;
                    //rb.velocity = Vector3.zero;
                    agent.speed = 0f;
                }
                else
                {
                    //agent.updatePosition = true;
                    agent.speed = 1f;
                    //rb.velocity = agent.desiredVelocity;
                }*/
                //shieldTransform.rotation = Quaternion.LookRotation(Vector3.forward, agent.velocity);
                //rb.velocity = agent.desiredVelocity;


                //Slowly turn towards where the agent wants to go.
                direction = agent.velocity;
                direction.z = 0f;
                lookDirection = Quaternion.LookRotation(Vector3.forward, direction.normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDirection, maxAngleSpeed * Time.deltaTime);
                
               
                //Scale agent movement speed with how aligned its movement and the object's heading are.
                //Creates a nice effect where the agent will only really move forward or mostly forward, but spins to align itself when it gets off the path
                agent.speed = Vector2.Dot(direction.normalized, transform.up) * maxSpeed;
                if(agent.speed <= 0f)
                {
                    //Agent wigs out if speed is ever 0 or less so just prevent it from happening
                    agent.speed = 0.01f;
                }
            }

            //agent.Raycast(transform.position + (0.6f * transform.up), out hit);

            //if (hit.mask != NavMesh.GetAreaFromName("Not Walkable"))
            //NavMesh.Raycast(transform.position, transform.position + (0.1f * transform.up.normalized), out hit, NavMesh.GetAreaFromName("Not Walkable"));
            /*if (NavMesh.Raycast(transform.position, transform.position + (0.4f * transform.up.normalized), out hit, NavMesh.GetAreaFromName("Obstacle")))
            {
                rb.velocity = Vector3.zero;
                agent.ResetPath();
                agent.SetDestination(target.position);
                Debug.Log("Distance to hit: " + hit.distance.ToString());
            }
            else
            {

                rb.velocity = transform.up.normalized;
            }*/
            //rb.velocity = transform.up;
            
        }
    }
}
