/*******************************************************************************
filename			: PlayerMove.cs
team name	: Team Ark
author			: Jihwan Oh
email				: dkdkdkdudrn@gmail.com, officialteamark@gmail.com
date				: 03.02.2020

description:
  Script for player's movement

@This file was written by team-ark ( ark-official.com )
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed; // max speed of player
    public float jumpPower; // Power of Jump
    Rigidbody2D rigid; // Rigidbody of Player
    SpriteRenderer sprRend;
    Animator anim;
    Vector2 playerCol;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprRend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerCol = GetComponent<CapsuleCollider2D>().size;
    }

    void Update()
    {
        Vector2 rigVel = rigid.velocity;
        //// MOVEMENT
        // Flip the Sprite
        if(rigVel.x < -0.3f)
        {
             sprRend.flipX = true;
        }
        else if (rigVel.x > 0.3f)
        {
             sprRend.flipX = false;
        }
        // Animation
        if (Mathf.Abs(rigVel.x) < 0.3f)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }

        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigVel.normalized.x * 0.5f, rigVel.y);
        }

        //// Jump
        if (Input.GetButtonDown("Jump") && 
                !(anim.GetBool("isJumpUp") || anim.GetBool("isJumpDown"))) // Only can jump when land
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        
    }

    void FixedUpdate()
    {
        Vector2 rigVel = rigid.velocity;
        //// Movement
        // Direction of horizontal movement
        float dir = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * dir, ForceMode2D.Impulse);

        // Set maximum velocity of player
        if(rigVel.x > maxSpeed) // Right max speed
        {
            rigid.velocity = new Vector2(maxSpeed, rigVel.y);
        }
        else if (rigVel.x <= (maxSpeed * -1.0f)) // Left max speed
        {
            rigid.velocity = new Vector2(-maxSpeed, rigVel.y);
        }

        ////JUMP
        // ANIMATION - Use Y-velocity
        if (rigVel.y > 0.0f) // Right max speed
        {
            anim.SetBool("isJumpUp", true);
            anim.SetBool("isJumpDown", false);
        }
        else if (rigVel.y <= 0.0f) // Left max speed
        {
            anim.SetBool("isJumpUp", false);
            anim.SetBool("isJumpDown", true);
        }

        // LANDING - Use ray cast to check 
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0.0f, 1.0f, 0.0f));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 2.0f, LayerMask.GetMask("Platform"));
        if (rayHit.collider != null)
        {
            if (rayHit.distance < playerCol.y)
            {
                anim.SetBool("isJumpUp", false);
                anim.SetBool("isJumpDown", false);
                //Debug.Log(rayHit.collider.name);
            }
        }

    }
}
