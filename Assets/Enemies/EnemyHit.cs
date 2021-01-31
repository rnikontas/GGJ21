using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public int hitDmg;
    public float bounceForce;
    float damageTimer = 0.5f;
    float timer = 0f;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<Player>().reduceHealth(hitDmg);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        timer += Time.deltaTime;
        if (collider.gameObject.tag == "Player")
        {
            if (timer >= damageTimer)
            {
                collider.gameObject.GetComponent<Player>().reduceHealth(hitDmg);
                timer = 0f;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        timer = 0f;
    }


}
