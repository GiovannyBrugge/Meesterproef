using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public Level level;
    private Rigidbody2D playerRB;
    private Transform groundCheck;

    public LayerMask groundLayer;

    const float checkCollisionRadius = 0.2f;
    private readonly float playerMovementSpeed = 2.5f;
    private readonly float jumpPower = 4.5f;
    private float horizontalInput;

    private bool rightFace = true;
    private bool isGrounded = false;
    
    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        level = GameObject.Find("Level").GetComponent<Level>();
        groundCheck = GameObject.Find("GroundCheck").GetComponent<Transform>();
    }
    private void Update()
    {
        //Check if the player is touching the ground
        GroundCheck();

        //Makes a float with the input "Horizontal" (Horizontal contains: A and LeftArrow for negative x movement; D and RightArrow for positive x movement)
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (Physics2D.gravity == new Vector2(9.81f, 0f)) 
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, horizontalInput * playerMovementSpeed);
        }
        else if (Physics2D.gravity == new Vector2(-9.81f, 0f))
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, horizontalInput * -playerMovementSpeed);
        }
        else
        {
            playerRB.velocity = new Vector2(horizontalInput * playerMovementSpeed, playerRB.velocity.y);
        }

        //Makes the play jump with the input: spacebar and W
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded == true )
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
    //Checks if the invisible circle underneath the player is touching a layer named "Ground"
    private void GroundCheck()
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, checkCollisionRadius, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
        }
    }
    //Rotates the play in a specific direction
    public void Rotate(string direction)
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
   //Let's the play jump under gravity direction circumstances
    private void Jump()
    {

        if (Physics2D.gravity == new Vector2(9.81f, 0f))
        {
            playerRB.velocity = new Vector2(-jumpPower, playerRB.velocity.y );
        }
        else if (Physics2D.gravity == new Vector2(-9.81f, 0f))
        {
            playerRB.velocity = new Vector2(jumpPower, playerRB.velocity.y);
        }
        else if (Physics2D.gravity == new Vector2(0f, 9.81f))
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, -jumpPower);
        }
        else { 
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpPower);
        }
    }
    //Detects other obstacles
    void OnCollisionEnter2D(Collision2D other)
    {
        level.PlayerCollision(this,other);

        switch (other.gameObject.tag)
        {
            case "Ground":
                break;
        }
    }
   //Checks for trigger to change the camera position
    void OnTriggerEnter2D(Collider2D other)
    {
        level.PlayerCollider(this,other);
    }

    //Kills the player (Will respawn player soon)
    public void Die()
    {
        ResetPlayer();
        Debug.Log("The player has died");
        SceneManager.LoadScene("Main_Menu");
    }
    //Sets gravity and player rotation to default
    private void ResetPlayer()
    {
        Physics2D.gravity = new Vector2(0f, -9.81f);
    }
}
