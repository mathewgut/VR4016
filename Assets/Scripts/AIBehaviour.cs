using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class AIMovement : MonoBehaviour
{
    enum AgentState
    {
        Patrol,
        Chase,
        Wander
    }

    [SerializeField]
    private AgentState _state = AgentState.Patrol;

    private NavMeshAgent agent;
    
    [SerializeField]
    private float baseSpeed;

    [SerializeField]
    private float chaseSpeedMult = 1.25f;

    public bool followPatrol = true;

    public List<GameObject> patrolPoints;

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

        if (patrolPoints.Count == 0 || !followPatrol) return;



        if (_state == AgentState.Patrol)
        {
            FollowPatrol();
        }
    }

    // -1 means all layers
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    void FollowPatrol()
    {
        if(targetPoint == null || Vector3.Distance(transform.position, targetPoint) < 2f)
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

}
