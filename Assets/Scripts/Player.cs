using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float playerMovementSpeed = 225f;
    private float jumpPower = 500f;
    private float horizontalInput;
    private Rigidbody2D playerRB;
    private bool rightFace = true;
    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //Makes a float with the input "Horizontal" (Horizontal contains: A and LeftArrow for negative x movement; D and RightArrow for positive x movement)
        horizontalInput = Input.GetAxisRaw("Horizontal");
        playerRB.velocity = new Vector2(horizontalInput * playerMovementSpeed * Time.deltaTime, playerRB.velocity.y);

        //Makes the play jump with the input: spacebar and W
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            playerRB.velocity = Vector2.up * jumpPower * Time.deltaTime;
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

    //Detects other obstacles
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Trap")
        {
            Destroy(gameObject);
            Debug.Log("I touch a spike");
        }
    }


}
