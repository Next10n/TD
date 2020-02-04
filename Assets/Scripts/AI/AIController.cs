using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform _target;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(_target.position);
    }

    void Update()
    {
       
    }
}