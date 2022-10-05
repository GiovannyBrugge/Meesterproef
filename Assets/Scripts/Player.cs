using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera segmentCamera;
    //index 0 = Segement 1, index 1 = segment 2 etc. etc.
    readonly Vector3[] segmentCameraPositions =
     {
     new Vector3(0.0f,0.0f,-10f),
     new Vector3(-18f,0.0f,-10f)
    };
    [HideInInspector]
    public float playerMovementSpeed = 225f;
    //[HideInInspector]
    readonly float jumpPower = 250f;
    private float horizontalInput;
    private Rigidbody2D playerRB;
    private bool rightFace = true;
    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Makes a float with the input "Horizontal" (Horizontal contains: A and LeftArrow for negative x movement; D and RightArrow for positive x movement)
        horizontalInput = Input.GetAxisRaw("Horizontal");
        playerRB.velocity = new Vector2(horizontalInput * playerMovementSpeed * Time.deltaTime, playerRB.velocity.y);

        //Makes the play jump with the input: spacebar and W
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

        if (rightFace == false && horizontalInput > 0)
        {
            Mirror();
        }
        else if(rightFace == true && horizontalInput < 0)
        {
            Mirror();
        }
    }
     //The player will face the correct direction
    private void Mirror() 
    {
        rightFace = !rightFace;
        Vector2 Size = transform.localScale;
        Size.x *= -1;
        transform.localScale = Size;
    }

    private void Jump()
    {
        playerRB.velocity = (Vector2.up * jumpPower) * Time.deltaTime;
    }
    //Detects other obstacles
    void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.name)
        {
            case "Ravine":
                Die();
                Debug.Log("I touched a ravine");
                break;

            case "Spike":
                Die();
                Debug.Log("I touched a spike");
                break;
        }
    }
    //Checks for trigger to change the camera position
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.name)
        {
            case "1to2":
                segmentCamera.transform.position = segmentCameraPositions[1];
                break;
        }
    }
    //Kills the player (Will respawn player soon)
    private void Die()
    {
        Destroy(gameObject);
    }
}
