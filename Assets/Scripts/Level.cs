using UnityEngine;

public class Level : MonoBehaviour
{
    //private Player player;
    public Camera segmentCamera;
    public Sprite checkpointActive;
    public Sprite checkpointUnactive;
    public Vector3[] segmentCameraPositions =
    {
        //Segment 1
        new Vector3(0.0f,0.0f,-10f),
        //Segment 2
        new Vector3(-18f,0.0f,-10f),
        //Segment 3
        new Vector3(-18f,-10.0f,-10f)
    };

    public int currentActiveCheckpoint; //0 = spawn, 1 = checkpoint1, 2 = checkpoint2
    private void Start()
    {
        //  player = GameObject.Find("Player").GetComponent<Player>();
        segmentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }
    //Detects other obstacles
    public void PlayerCollision(Player player, Collision2D other)
    {
        switch (other.gameObject.name)
        {
            case "Ravine":
                StartCoroutine(player.Die());
                Debug.Log("I touched a ravine");
                break;
            case "Spike":
                StartCoroutine(player.Die());
                Debug.Log("I touched a spike");
                break;
            case "Spike2":
                StartCoroutine(player.Die());
                Debug.Log("I touched a spike");
                break;
        }
    }
    //Checks for trigger to change the camera position
    public void PlayerCollider(Player player, Collider2D other)
    {
        switch (other.gameObject.name)
        {
            case "Checkpoint1":
                Debug.Log("Touching checkpoint");
                SetCheckpoint(0);
                break;
            case "Checkpoint2":
                Debug.Log("Touching checkpoint");
                SetCheckpoint(1);
                break;
            case "ToSegment1":
                segmentCamera.transform.position = segmentCameraPositions[0];
                break;
            case "ToSegment2":
                segmentCamera.transform.position = segmentCameraPositions[1];
                break;
            case "ToSegment3":
                segmentCamera.transform.position = segmentCameraPositions[2];
                break;
        }
        //Check gravity zone
        switch (other.gameObject.name)
        {
            case "GravityUp":
                Physics2D.gravity = new Vector2(0f, 9.81f);
                player.Rotate(Player.RotateDirection.Up);
                break;

            case "GravityDown":
                Physics2D.gravity = new Vector2(0f, -9.81f);
                player.Rotate(Player.RotateDirection.Down);
                break;

            case "GravityRight":
                Physics2D.gravity = new Vector2(9.81f, 0f);
                player.Rotate(Player.RotateDirection.Right);
                break;

            case "GravityLeft":
                Physics2D.gravity = new Vector2(-9.81f, 0f);
                player.Rotate(Player.RotateDirection.Left);
                break;
        }
    }
    public void SetCheckpoint(int checkpoint)
    {
        if (checkpoint == 0)
        {
            currentActiveCheckpoint = 1;
            InactiveAllCheckpoints();
            GameObject.Find("Checkpoint1").GetComponent<SpriteRenderer>().sprite = checkpointActive;
          
        }
        else if (checkpoint == 1)
        {
            currentActiveCheckpoint = 2;
            InactiveAllCheckpoints();
            GameObject.Find("Checkpoint2").GetComponent<SpriteRenderer>().sprite = checkpointActive;
           
        }
    }
    //Sets all checkpoints inactive by default
    private void InactiveAllCheckpoints()
    {
        GameObject.Find("Checkpoint1").GetComponent<SpriteRenderer>().sprite = checkpointUnactive;
        GameObject.Find("Checkpoint2").GetComponent<SpriteRenderer>().sprite = checkpointUnactive;
    }
}