using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseController : MonoBehaviour
{
    [SerializeField]
    private AIBehaviour agent;

    public GameObject noiseEvent = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (noiseEvent == null) return;

        if(AgentInRange())
        {
            agent.targetObject = noiseEvent;
            agent._state = AIBehaviour.AgentState.Chase;
        }
        noiseEvent = null;
    }

    bool AgentInRange ()
    {
        if(noiseEvent != null)
        {
            return Vector3.Distance(noiseEvent.transform.position, agent.transform.position) <= agent.noiseRange;
        }
      
        return false;
    }
}
