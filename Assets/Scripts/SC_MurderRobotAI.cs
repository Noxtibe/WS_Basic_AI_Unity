using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SC_MurderRobotAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    //[SerializeField] private Transform _targets; // For a single point
    [SerializeField] private List<Transform> _targets; // For multiple points

    private int current = 0;
    public int delay = 3;
    public float distanceTreshold = 1.0f;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(_targets[current].position);
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, _targets[current].position) <= distanceTreshold)
        {
             current = (current +1)%(_targets.Count);
            Invoke("MoveToNextTarget", delay);
            //_agent.isStopped = true; // Stop the AI
        }
    }

    void MoveToNextTarget()
    {
        _agent.SetDestination(_targets[current].position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _agent.isStopped = true; // Stop the AI
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _agent.isStopped = false;
    }
}
