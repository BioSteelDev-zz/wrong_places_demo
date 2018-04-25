using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    bool tookHit = false; // Whether or not the player just took Damage

    Animator anim; // Contains a reference to the Player Animator

    public bool isDead = false;

    // Health
    int startingHealth = 100;
    int currentHealth;
    public Slider healthSlider;
    public PlayerController playerMechanics;
    public PlayerGround playerMovement;

    // -------------------------------------------------------------------- AWAKE/START/UPDATE/FIXEDUPDATE -------------------------------------------------------------------
    void Awake()
    {
        currentHealth = startingHealth;
    }

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetBool("Hit", tookHit);

        if (currentHealth <= 0)
        {
            isDead = true;
        }
    }

    // Handle when the Player is hit
    public void TakeDamage()
    {
        // if the player was just hit, give invuln frame
        if(!tookHit)
            StartCoroutine(Hit());
    }

    IEnumerator Hit()
    {
        StartCoroutine(DisableMovement());
        tookHit = true;
        currentHealth = currentHealth - 10;
        healthSlider.value = currentHealth;
        yield return new WaitForSeconds(1f);
        tookHit = false;
    }

    IEnumerator DisableMovement()
    {
        playerMovement.enabled = false;
        playerMechanics.enabled = false;
        yield return new WaitForSeconds(.5f);
        playerMovement.enabled = true;
        playerMechanics.enabled = true;
    }
}
