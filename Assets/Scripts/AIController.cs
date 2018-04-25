/* --------------------------------------- AIController.cs --------------------------------------- \
 * Handles basically Enemy AI tracking script
 * 
 * Written By: Jacob Dockter
 * Last Edited: 12/16/2017
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIController : MonoBehaviour {

    // ----------------------------------------------------------------- PROPERTIES ----------------------------------------------------------------------------------

    Transform Player; // Reference to Player Object Transform
    public int MoveSpeed = 4; // Enemy movement speed
    public int MaxDist = 10; // If the enemy is at least this close to the Player, perform action
    public int MinDist = 5; // As long as the enemy is more than this distance away from the Player, track to him/her
    bool attack = false; // Attack animation active or not
    Animator anim; // Reference to Animator Component
    bool attacking = false; // whether or not the enemy is attacking
    public int hitpoints = 1; // health of enemy
    public bool takingHit = false; // Whether or not object was hit

    // -------------------------------------------------------------------- UPDATE/START -------------------------------------------------------------------
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    
    void Update () {

        // Set animation properties
        anim.SetBool("Attack", attack);
        anim.SetBool("Hit", takingHit);

        // Set the enemy to look in the direction of the player
        if (Player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Track to the player
        if (!takingHit)
        {
            if (Vector2.Distance(transform.position, Player.position) >= MinDist)
            {

                transform.position = Vector2.MoveTowards(transform.position, Player.position, MoveSpeed * Time.deltaTime);
            }
            if (Vector2.Distance(transform.position, Player.position) <= MaxDist)
            {
                // Attack Function
                if (!attacking)
                    StartCoroutine(AttackWait());
            }
        }

        if(hitpoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Run the attack animation and set the attack collider to true for a time, then reset
    IEnumerator AttackWait()
    {
        attacking = true;
        attack = true;
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        transform.GetChild(0).gameObject.SetActive(false);
        attack = false;
        attacking = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            takingHit = true;
            StartCoroutine(Hit());
        }
    }

    IEnumerator Hit()
    {
        transform.position = new Vector3(transform.position.x - .2f, transform.position.y, 0);
        yield return new WaitForSeconds(.5f);
        takingHit = false;
    }
}
