/* --------------------------------------- PlayerController.cs --------------------------------------- \
 * Contains main operations for Player GameObject
 * 
 * Written By: Jacob Dockter
 * Last Edited: 12/17/2017
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    // ----------------------------------------------------------------- PROPERTIES ----------------------------------------------------------------------------------

    public float walkSpeed = 0; // Handle Animation with movement -- -1 for Left, 1 for Right, 0 for no Movement
    bool grounded = true; // Whether or not the player is touching the ground
    bool crouched = false; // Whether or not the player is crouched 
    bool idleBlink = true; // Whether or ot the player is blinking
    bool shooting = false; // Whether or not the player is shooting
    bool powerShot = false; // Whether or not the player is doing a heavy shot
    bool punch = false; // Whether or not the player is punching

    public GameObject Player; // Contains a reference to the Parent GameObject
    public GameObject Bullet;

    Animator anim; // Contains a reference to the Player Animator
    
    // Jumping
    public float jumpSpeed = 1.2f;
    Vector3 moveDirection = Vector3.zero;
    public float gravity = 3;
    public float maxHeight = 5.0f;

    float yBulletPosition = 4f; // Y Position of where the bullet fires from

    // -------------------------------------------------------------------- AWAKE/START/UPDATE/FIXEDUPDATE -------------------------------------------------------------------

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        StartCoroutine(RandomWait());
    }

    void Update()
    {
        // Set up animation connections
        anim.SetFloat("Speed", walkSpeed);
        anim.SetBool("Grounded", grounded);
        anim.SetBool("Crouched", crouched);
        anim.SetBool("IdleBlink", idleBlink);
        anim.SetBool("Shooting", shooting);
        anim.SetBool("BigShot", powerShot);
        anim.SetBool("Punch", punch);

        // Blink randomly
        if (idleBlink && walkSpeed == 0)
        {
            StartCoroutine(RandomWait());
        }

        // Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            yBulletPosition = 2.8f;
            Player.GetComponent<PlayerGround>().maxSpeed = 0;
            crouched = true;
            //Change the Hitbox
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.74f, 4f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-.2f, 2f);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            yBulletPosition = 4f;
            Player.GetComponent<PlayerGround>().maxSpeed = 10;
            crouched = false;
            // Change the hitbox
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.5f, 5.5f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 2.71f);
        }

        // Shooting
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shooting = true;
            float xOffset = 2.7f;
            if (Player.transform.localScale.x == -1)
            {
                xOffset = -xOffset;
            }
            else
            {
                xOffset = 2.7f;
            }

            Instantiate(Bullet, new Vector3(transform.position.x + xOffset, transform.position.y + yBulletPosition, 0), Quaternion.identity);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            shooting = false;
        }

        // Punch
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(PunchWait());
        }

        // Heavy Shot
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            // TODO: Lock behind a power limiter
            StartCoroutine(HeavyShot());
        }

    }

    void FixedUpdate()
    {
        // TODO: Handle Player Hit

        // JUMPING
        if (grounded)
        {
            if (Input.GetButton("Jump"))
            {
                grounded = false;
                Player.GetComponent<SpriteRenderer>().enabled = true;
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = 0.0f;
            }
        }
        else
        {
            // Psuedo Gravity
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move based on position
        transform.Translate(moveDirection);

        // If the player hits the floor, ground them
        if(transform.position.y < Player.transform.position.y)
        {
            transform.position = Player.transform.position;
            grounded = true;
            Player.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // ----------------------------------------------------------------------------- COROUTINES ----------------------------------------------------------------------------

    // Randomly wait a certain time before blink animation
    public IEnumerator RandomWait()
    {
        idleBlink = false;
        int randomTime = Random.Range(0, 10);
        yield return new WaitForSeconds(randomTime);
        idleBlink = true;

    }

    // Run the punch animation then wait a short time before allowing the player to do it again
    public IEnumerator PunchWait()
    {
        punch = true;
        Player.transform.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        Player.transform.GetChild(2).gameObject.SetActive(false);
        punch = false;
    }

    // Heavy Shot
    public IEnumerator HeavyShot()
    {
        powerShot = true;
        Player.GetComponent<PlayerGround>().maxSpeed = 0;
        yield return new WaitForSeconds(.8f);
        Player.GetComponent<PlayerGround>().maxSpeed = 10;
        powerShot = false;
    }

}
