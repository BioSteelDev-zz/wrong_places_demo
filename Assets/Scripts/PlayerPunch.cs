/* --------------------------------------- PlayerPunch.cs --------------------------------------- \
 * Handles basically Enemy AI tracking script
 * 
 * Written By: Jacob Dockter
 * Last Edited: 12/16/2017
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            if(!col.GetComponent<AIController>().takingHit)
                col.GetComponent<AIController>().hitpoints -= 1;
        }
    }
}
