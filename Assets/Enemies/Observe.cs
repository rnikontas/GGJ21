using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Observe : MonoBehaviour
{
    public float enemySpeed;
    GameObject player;
    Camera cam;
    NavMeshAgent navMeshAgent;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        cam = player.GetComponent<Camera>();
        navMeshAgent = transform.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (IsInView(player, this.gameObject))
        {
            navMeshAgent.speed = 0;
        }
        else
        {
            navMeshAgent.speed = enemySpeed;
        }
    }

    private bool IsInView(GameObject origin, GameObject toCheck)
    {
        Vector3 pointOnScreen = cam.WorldToScreenPoint(toCheck.GetComponentInChildren<Renderer>().bounds.center);

        //Is in front
        if (pointOnScreen.z < 0)
        {
            //Debug.Log(toCheck.name + "is in front");
            return false;
        }

        //Is in FOV
        if ((pointOnScreen.x < 0) || (pointOnScreen.x > Screen.width) ||
                (pointOnScreen.y < 0) || (pointOnScreen.y > Screen.height))
        {
            //Debug.Log(toCheck.name + "is visible");
            return false;
        }

        RaycastHit hit;
        Vector3 heading = toCheck.transform.position - origin.transform.position;
        Vector3 direction = heading.normalized;// / heading.magnitude;

        if (Physics.Linecast(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, out hit))
        {
            if (hit.transform.name != toCheck.name)
            {
                /* -->
                Debug.DrawLine(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, Color.red);
                Debug.LogError(toCheck.name + " occluded by " + hit.transform.name);
                */
                //Debug.Log(toCheck.name + " occluded by " + hit.transform.name);
                return false;
            }
        }
        return true;
    }
}
