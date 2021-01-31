using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    GameObject destination;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destination = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(destination.transform.position);
    }
}
