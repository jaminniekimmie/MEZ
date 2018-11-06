using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    SpriteRenderer sr;
    Animator anim;
    CharacterController controller;
    public float moveSpeed = 2f;
    public float gravity = 1f;
    public bool isJumping = false;

    private bool isWalking = false;
    private float degree = 0f;
    private FacingDirection facingDirection;

    float jumpHeight = 0f;
    float h = 0f;
    
    public FacingDirection CmdFacingDirection
    {
        set { facingDirection = value; }
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update () {
        //h = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.RightArrow)) h = 1;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) h = -1;

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
        //controller.Move(h * Time.fixedDeltaTime, false, false);
        //transform.Translate(h * Time.deltaTime * moveSpeed, 0, 0);

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

        float moveFactor = moveSpeed * Time.deltaTime * 10f;
        MoveCharacter(moveFactor);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, degree, 0), 8 * Time.deltaTime);
    }

    private void MoveCharacter(float moveFactor)
    {
        Vector3 trans = Vector3.zero;

        switch (facingDirection)
        {
            case FacingDirection.Front:
                trans = new Vector3(h * moveFactor, -gravity * moveFactor, 0f);
                break;
            case FacingDirection.Right:
                trans = new Vector3(0f, -gravity * moveFactor, h * moveFactor);
                break;
            case FacingDirection.Back:
                trans = new Vector3(-h * moveFactor, -gravity * moveFactor, 0f);
                break;
            case FacingDirection.Left:
                trans = new Vector3(0f, -gravity * moveFactor, -h * moveFactor);
                break;
        }
        
        //trans = new Vector3(h * Time.deltaTime * moveFactor, -gravity * moveFactor, 0f);

        if(isJumping)
            transform.Translate(Vector3.up * jumpHeight * Time.deltaTime);

        controller.SimpleMove(trans);
    }

    public void UpdateToFacingDirection(FacingDirection newDirection, float angle)
    {
        facingDirection = newDirection;
        degree = angle;
    }
}
