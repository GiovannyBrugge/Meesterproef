using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [HideInInspector]
    //Level class
    public Level level;
    private Rigidbody2D playerRB;
    private SpriteRenderer playerSprite;
    //Player ground detecting
    private Transform groundCheck;
    public LayerMask groundLayer;
    const float checkCollisionRadius = 0.2f;
    //Player movement
    private readonly float playerMovementSpeed = 2.5f;
    private readonly float jumpPower = 4.5f;
    private float horizontalInput;
    //Player check facing direction
    private bool rightFace = true;
    //Player check touching ground
    private bool isGrounded = false;
    //Checks if the play can jump after dying
    private bool hasJumped = false;
    //Rotation directions
    public enum RotateDirection {Up, Down, Right, Left }
    //Player's current position
    private Transform currentPlayerPosition;
    //Spawn positions
    private readonly Vector3[] playerSpawnPosition =
    {
        //Start
        new Vector3(-7.5f,-3.5f,0f),
        //Checkpoint 1
         new Vector3(0f,0f,0f),
        //Checkpoint 2
        new Vector3(0f,0f,0f)
    };

    //Animator of the player
    public Animator animator;

    private void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        playerRB = GetComponent<Rigidbody2D>();
        level = GameObject.Find("Level").GetComponent<Level>();
        groundCheck = GameObject.Find("GroundCheck").GetComponent<Transform>();
        currentPlayerPosition = GameObject.Find("Player").GetComponent<Transform>();
        //Set checkpoint1 position
        Vector3 cp1 = GameObject.Find("Checkpoint1").GetComponent<Transform>().position;
        playerSpawnPosition[1] = new Vector3(cp1.x, cp1.y, 0f);
        //Set checkpoint2 position
        Vector3 cp2 = GameObject.Find("Checkpoint2").GetComponent<Transform>().position;
        playerSpawnPosition[2] = new Vector3(cp2.x, cp2.y, 0f);
        //Start position
        currentPlayerPosition.transform.position = playerSpawnPosition[0];
    }
    private void Update()
    {
        //Check if the player is touching the ground
        GroundCheck();

        //Makes a float with the input "Horizontal" (Horizontal contains: A and LeftArrow for negative x movement; D and RightArrow for positive x movement)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("PlayerSpeed", Mathf.Abs(horizontalInput));
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
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded == true && hasJumped == false)
        {
            Jump();
            animator.SetTrigger("IsLaunching");
            AudioManager.instance.Play("PlayerJump");
        }
        if (isGrounded == true)
        {
            animator.SetBool("IsJumping", false);
        }
        else
        {
            animator.SetBool("IsJumping", true);
        }
        if (rightFace == false && horizontalInput > 0)
        {
            Mirror();
        }
        else if (rightFace == true && horizontalInput < 0)
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
    public RotateDirection Rotate(RotateDirection direction)
    {
        if (direction == RotateDirection.Up)
        {
            transform.eulerAngles = new Vector3(180f, 0f, 0f);
        }
        else if (direction == RotateDirection.Down)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (direction == RotateDirection.Right)
        {
            transform.eulerAngles = new Vector3(0, 0f, 90f);
        }
        else if (direction == RotateDirection.Left)
        {
            transform.eulerAngles = new Vector3(0f, 0f, -90f);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }
        return direction;

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
            playerRB.velocity = new Vector2(-jumpPower, playerRB.velocity.y);
        }
        else if (Physics2D.gravity == new Vector2(-9.81f, 0f))
        {
            playerRB.velocity = new Vector2(jumpPower, playerRB.velocity.y);
        }
        else if (Physics2D.gravity == new Vector2(0f, 9.81f))
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, -jumpPower);
        }
        else
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpPower);
        }
    }
    //Detects other obstacles
    void OnCollisionEnter2D(Collision2D other)
    {
        level.PlayerCollision(this, other);

        switch (other.gameObject.tag)
        {
            case "Ground":
                break;
        }
    }
    //Checks for trigger to change the camera position
    void OnTriggerEnter2D(Collider2D other)
    {
        level.PlayerCollider(this, other);
    }

    //Kills the player (Will respawn player soon)
    public IEnumerator Die()
    {
        //During dying
        Debug.Log("The player has died");
        AudioManager.instance.Play("PlayerDying");
        hasJumped = true;
        playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
        playerSprite.color = Color.red;
        level.deathCounter += 1f;
        level.deathCounterText.text = "Deaths: " + level.deathCounter;
        yield return new WaitForSeconds(0.7f);
        //After dying
        hasJumped = false;
        playerSprite.color = Color.white;
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        LoadLastCheckpoint();
    }
    public void LoadLastCheckpoint()
    {
        switch (level.currentActiveCheckpoint)
        {
            case 0:
                Rotate(RotateDirection.Down);
                Physics2D.gravity = new Vector2(0f, -9.81f);
                currentPlayerPosition.transform.position = playerSpawnPosition[0];
                level.segmentCamera.transform.position = level.segmentCameraPositions[0];
                level.TurnOffTutorial();
                level.tutorialCanvas.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 1:
                Rotate(RotateDirection.Up);
                Physics2D.gravity = new Vector2(0f, 9.81f);
                currentPlayerPosition.transform.position = playerSpawnPosition[1];
                level.segmentCamera.transform.position = level.segmentCameraPositions[3];
                break;
            case 2:
                Rotate(RotateDirection.Down);
                Physics2D.gravity = new Vector2(0f, -9.81f);
                currentPlayerPosition.transform.position = playerSpawnPosition[2];
                level.segmentCamera.transform.position = level.segmentCameraPositions[6];
                break;
        }
    }
}