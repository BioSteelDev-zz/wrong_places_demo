/* --------------------------------------- EnemyAttack.cs --------------------------------------- \
 * Handles the Attack collider on an enemy
 * 
 * Written By: Jacob Dockter
 * Last Edited: 12/17/2017
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerHealth>().TakeDamage();
        }
    }
}
