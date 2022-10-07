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
    
    private readonly float playerMovementSpeed = 2.5f;
    private readonly float jumpPower = 4.5f;
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
        playerRB.velocity = new Vector2(horizontalInput * playerMovementSpeed, playerRB.velocity.y);
        
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
    private void Rotate(string direction)
    {
        if(direction == "Up")
        {
            transform.eulerAngles = new Vector3(0f, 0f, 180f);
            
        }
        else if(direction == "Down")
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (direction == "Right")
        {
            transform.eulerAngles = new Vector3(0, 0f, 90f);
        }
        else if (direction == "Left")
        {
            transform.eulerAngles = new Vector3(0f, 0f, -90f);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
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
        playerRB.velocity = new Vector2(playerRB.velocity.x, jumpPower) ;
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

        //Check gravity zone
        switch (other.gameObject.name)
        {
            case "GravityUp":
                Physics2D.gravity = new Vector2(0f, 9.81f);
                Rotate("Up");
                break;

            case "GravityDown":
                Physics2D.gravity = new Vector2(0f, -9.81f);
                Rotate("Down");
                
                break;

            case "GravityRight":
                Physics2D.gravity = new Vector2(9.81f, 0f);
                Rotate("Right");
                break;

            case "GravityLeft":
                Physics2D.gravity = new Vector2(-9.81f, 0f);
                Rotate("Left");
                break;
        }
    }
    //Kills the player (Will respawn player soon)
    private void Die()
    {
        Destroy(gameObject);
    }
}
