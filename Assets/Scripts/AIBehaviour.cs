using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class AIBehaviour : MonoBehaviour
{
    public enum AgentState
    {
        Patrol,
        Chase,
        Wander
    }

  
    public AgentState _state = AgentState.Patrol;

    private NavMeshAgent agent;

    [SerializeField]
    private float baseSpeed;

    [SerializeField]
    private float chaseSpeedMult = 1.25f;

    public bool followPatrol = true;

    public List<GameObject> patrolPoints;

    public readonly float noiseRange = 40f;

    public GameObject targetObject;
    Vector3 targetPoint;

    int currPatrolIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (patrolPoints.Count != 0)
        {
            targetPoint = patrolPoints[currPatrolIndex].transform.position;
            agent.SetDestination(targetPoint);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // follow an object
        if (targetObject != null && !followPatrol)
        {
            agent.SetDestination(targetObject.transform.position);
        }

        if (patrolPoints.Count == 0) return;

        if (_state == AgentState.Patrol)
        {
            followPatrol = true;
            agent.speed = baseSpeed;
            FollowPatrol();
        }
        else if (_state == AgentState.Chase)
        {
            followPatrol = false;
            agent.speed = baseSpeed * chaseSpeedMult;
            ChaseTarget();
            
        }
        else if (_state == AgentState.Wander)
        {

        }
    }

    // -1 means all layers
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    void FollowPatrol()
    {
        // if at point
        if(targetPoint == null || AtTarget())
        {
            if (currPatrolIndex == patrolPoints.Count - 1)
            {
                currPatrolIndex = 0;
            }
            else
            {
                currPatrolIndex += 1;
            }

            targetPoint = patrolPoints[currPatrolIndex].transform.position;
            agent.SetDestination(targetPoint);
        }
    }

    void ChaseTarget()
    {
        targetPoint = targetObject.transform.position;

        agent.SetDestination(targetPoint);
        if (AtTarget())
        {
            // switch to wander for time
            _state = AgentState.Patrol;
        }
    }

    bool AtTarget()
    {
        return Vector3.Distance(transform.position, targetPoint) < 2f ? true : false;
    }

}
