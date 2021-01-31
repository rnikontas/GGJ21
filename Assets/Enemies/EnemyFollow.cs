using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using PhotonNetwork = Photon.Pun.PhotonNetwork;

public class EnemyFollow : MonoBehaviour
{
    GameObject destination;
    NavMeshAgent agent;

    void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destination = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            agent.SetDestination(destination.transform.position);
        }
    }
}
