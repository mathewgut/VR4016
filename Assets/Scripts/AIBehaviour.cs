using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIBehaviour : MonoBehaviour
{
    public enum AgentState
    {
        Patrol,
        Chase,
        Wander
    }
    public enum Lights
    {
        Chase,
        Seen,
        Passive
    }

    [SerializeField] GameObject passiveLight;
    [SerializeField] GameObject seenLight;
    [SerializeField] GameObject chaseLight;

    List<GameObject> lights = new List<GameObject>();

    public AgentState _state = AgentState.Patrol;
    public Lights activeLight = Lights.Passive;

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
        if(chaseLight && seenLight && passiveLight)
        {
            lights.Add(chaseLight);
            lights.Add(passiveLight);
            lights.Add(seenLight);
        }
        ActivateLight(passiveLight, Lights.Passive);

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
            if (activeLight != Lights.Passive) ActivateLight(passiveLight, Lights.Passive);
        }
        else if (_state == AgentState.Chase)
        {
            followPatrol = false;
            agent.speed = baseSpeed * chaseSpeedMult;
            ChaseTarget();
            if (activeLight != Lights.Chase) ActivateLight(chaseLight, Lights.Chase);
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

    void ActivateLight(GameObject lightRef, Lights type)
    {
        foreach (GameObject lightObj in lights)
        {
            if (lightObj == lightRef) lightObj.SetActive(true);
            else lightObj.SetActive(false);
        }

        activeLight = type;
    }

}
