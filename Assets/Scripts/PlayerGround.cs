/* ------------------------- PlayerGround.cs -------------------------
 * Handles the ground position and X directional movement of the Player. Also handles flipping of the sprite. 
 * 
 * Written By: Jacob Dockter
 * Last Edited: 12/16/2017
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGround : MonoBehaviour
{

    // ----------------------------------------------------------------- PROPERTIES ----------------------------------------------------------------------------------

    public float maxSpeed = 10f; // max speed of the sprite
    public GameObject PlayerController; // Reference to the Player Handler GameObject

    // -------------------------------------------------------------------- UPDATE/FIXEDUPDATE -------------------------------------------------------------------

    void Update()
    {
        // Change direction of sprite if necessary
        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal") * Time.deltaTime * maxSpeed;
        float v = Input.GetAxisRaw("Vertical") * Time.deltaTime * maxSpeed;

        Vector2 direction = Quaternion.AngleAxis(45, Vector2.up) * new Vector3(h, v);
        
        // Set the walk speed of the player for animations
        PlayerController.GetComponent<PlayerController>().walkSpeed = Mathf.Abs(h) + Mathf.Abs(v);

        transform.Translate(direction);

        // VERTICAL CLAMPING
        if (gameObject.transform.position.y >= -9.8f)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, -9.8f);
        }

        if (gameObject.transform.position.y <= -15.8f)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, -15.8f);
        }

        // HORIZONTAL CLAMPING
        if (gameObject.transform.position.x <= -14f)
        {
            gameObject.transform.position = new Vector2(-14f, gameObject.transform.position.y);
        }

    }
}
