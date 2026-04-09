using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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

    [SerializeField] AudioSource hornSource;
    [SerializeField] AudioSource chaseSource;

    [SerializeField] GameObject passiveLight;
    [SerializeField] GameObject seenLight;
    [SerializeField] GameObject chaseLight;

    List<GameObject> lights = new List<GameObject>();

    public AgentState _state = AgentState.Patrol;
    public Lights activeLight = Lights.Passive;

    private NavMeshAgent agent;
    public GameObject Player;
    PlayerAttributes attributes;
    GameObject playerCollider;

    [SerializeField] float viewConeAngle = 130;
    float viewDistance = 20f;

    [SerializeField]
    private float baseSpeed;

    [SerializeField]
    private float chaseSpeedMult = 1.25f;

    public bool followPatrol = true;

    public List<GameObject> patrolPoints;

    public readonly float noiseRange = 40f;

    public GameObject targetObject;
    Vector3 targetPoint;

    public bool playerVisible = false;

    int currPatrolIndex = 0;

    float wanderStartTime = -1;
    float wanderTime = 6;

    float seenPlayerStart = -1;
    float seenPlayerTime = 1.5f;

    // tracks whether the horn audio played for chase or not
    bool playedHorn = false;


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

        Player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = GameObject.FindGameObjectWithTag("PlayerCollider");

        attributes = Player.GetComponent<PlayerAttributes>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckViewCone();

        // if seen timer started
        bool isNoticingPlayer = seenPlayerStart != -1;

        if (_state == AgentState.Patrol)
        {
            playedHorn = false;
            followPatrol = true;
            agent.speed = baseSpeed;
            if (!isNoticingPlayer) FollowPatrol();
            if (activeLight != Lights.Passive && isNoticingPlayer) ActivateLight(passiveLight, Lights.Passive);
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
            playedHorn = false;
            agent.speed = baseSpeed / 2;
            followPatrol = false;
            if (wanderStartTime == -1) wanderStartTime = Time.time;

            if (Time.time - wanderStartTime >= wanderTime) { 
                _state = AgentState.Patrol;

                // can't null a vector3, so this makes the ai "at target", so can resume patrol
                targetPoint = transform.position;
            }
            else
            {
                if (activeLight != Lights.Seen) ActivateLight(seenLight, Lights.Seen);
                WanderTarget();
            }
        }

        if (_state != AgentState.Chase && chaseSource.isPlaying) chaseSource.Stop();

        if (Vector3.Distance(transform.position, Camera.main.transform.position) <= 3)
        {
            GameManager.Instance.SetGameState(GameManager.GameState.Lost);
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
        if (!hornSource.isPlaying && !playedHorn) { 
            hornSource.Play();
            chaseSource.Play();
            playedHorn = true;
            
        }
        agent.SetDestination(targetPoint);

        if (AtTarget() && !playerVisible)
        {
            _state = AgentState.Wander;
        }
    }

    void WanderTarget()
    {
        //targetPoint = RandomNavSphere(transform.position, 7f, -1);

        if (AtTarget())
        {
            targetPoint = RandomNavSphere(transform.position, 7f, -1);
        }

        agent.SetDestination(targetPoint);
    }

    bool AtTarget()
    {
        return Vector3.Distance(transform.position, targetPoint) < 3.5f ? true : false;
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

    void CheckViewCone()
    {
        // cone visuals
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-viewConeAngle, Vector3.up) * transform.forward * viewDistance, Color.yellow);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(viewConeAngle, Vector3.up) * transform.forward * viewDistance, Color.yellow);


        Vector3 npcEyes = transform.position + Vector3.up;
        Vector3 playerTarget = Camera.main.transform.position;

        Vector3 direction = playerTarget - npcEyes;
        float dist = direction.magnitude;


        Debug.DrawRay(npcEyes, direction.normalized * viewDistance, Color.green);

        if (dist <= viewDistance && Vector3.Angle(transform.forward, direction) < viewConeAngle || dist < 3)
        {
            RaycastHit hit;
            if (Physics.Raycast(npcEyes, direction.normalized, out hit, viewDistance))
            {

                bool hitPlayer = hit.transform.CompareTag("PlayerCollider") ||
                                (hit.transform.parent != null && hit.transform.parent.CompareTag("PlayerCollider"));

                if (hitPlayer && !attributes.isHidden)
                {
                    playerVisible = true;
                    if (seenPlayerStart == -1) seenPlayerStart = Time.time;


                    targetPoint = hit.point;


                    if (Time.time - seenPlayerStart >= seenPlayerTime)
                    {
                        _state = AgentState.Chase;
                    }


                    agent.SetDestination(targetPoint);

                    if (_state != AgentState.Chase) ActivateLight(seenLight, Lights.Seen);

                    return;
                }
            }
        }

        // Only reset if we actually lost sight
        playerVisible = false;
        seenPlayerStart = -1;
    }

}
