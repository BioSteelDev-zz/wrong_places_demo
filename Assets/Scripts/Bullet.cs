/* --------------------------------------- Bullet.cs --------------------------------------- \
 * Handles everything about the Bullet
 * 
 * Written By: Jacob Dockter
 * Last Edited: 12/17/2017
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    // ----------------------------------------------------------------- PROPERTIES ----------------------------------------------------------------------------------

    float speed = 30;
    float fireDirection = 1;

    // -------------------------------------------------------------------- UNITY METHODS -------------------------------------------------------------------

    void Start()
    {
        fireDirection = GameObject.Find("Player").transform.localScale.x;
    }

    void FixedUpdate()
    {
        float h = fireDirection * Time.deltaTime * speed;
        Vector2 direction = Quaternion.AngleAxis(45, Vector2.up) * new Vector3(h, 0);
        transform.Translate(direction);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
       if(col.gameObject.tag != "Bounds")
       {
            Destroy(gameObject);
       }
       if(col.gameObject.tag == "Enemy")
       {
            col.GetComponent<AIController>().hitpoints -= 1;
       }
    }

}
