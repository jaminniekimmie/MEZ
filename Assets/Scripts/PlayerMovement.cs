using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    SpriteRenderer sr;
    Animator anim;
    CharacterController controller;
    public float moveSpeed = 2f;
    public float gravity = 1f;

    private bool isWalking = false;
    private bool isJumping = false;
    private float degree = 0f;

    float jumpForce = 680f;
    float h = 0f;
    
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update () {
        h = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            anim.SetTrigger("Jump");
            isJumping = true;
        }

        float moveFactor = moveSpeed * Time.deltaTime * h;

        anim.SetBool("isWalking", isWalking);
        anim.SetInteger("h", (int)h);

    }

    void FixedUpdate()
    {
        Vector3 trans = Vector3.zero;
        //controller.Move(h * Time.fixedDeltaTime, false, false);
        //transform.Translate(h * Time.deltaTime * moveSpeed, 0, 0);
        trans = new Vector3(h * Time.deltaTime * moveSpeed, -gravity * moveSpeed, 0f);

        controller.SimpleMove(trans);

        if (h > 0.1f)
        {
            sr.flipX = false;
            if (!isJumping) isWalking = true;
        }
        else if (h < -0.1f)
        {
            sr.flipX = true;
            if (!isJumping) isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, degree, 0), 8 * Time.deltaTime);
    }
}
