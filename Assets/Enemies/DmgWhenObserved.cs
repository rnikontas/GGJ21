using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgWhenObserved : MonoBehaviour
{
    GameObject player;
    AudioSource audioSource;
    Camera cam;
    public int hitDmg;
    public float timeBeforeDmgSeconds = 10;
    private float currentViewingTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        cam = player.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInView(player, this.gameObject))
        {
            audioSource.Play(0);
            currentViewingTime += Time.deltaTime;
            if (currentViewingTime >= timeBeforeDmgSeconds)
            {
                currentViewingTime = 0;
                //player.DoAHurt(hitDmg);
            }
        }
        else
        {
            currentViewingTime = 0;
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
