using UnityEngine;

public class Level : MonoBehaviour
{
    //private Player player;
    private Camera segmentCamera;
    public Sprite checkpointActive;
    public Sprite checkpointUnactive;
    private readonly Vector3[] segmentCameraPositions =
    {
     //Segment 1
     new Vector3(0.0f,0.0f,-10f),
     //Segment 2
     new Vector3(-18f,0.0f,-10f),
    //Segment 3
     new Vector3(-18f,-10.0f,-10f)
    };

    private void Start()
    {
      //  player = GameObject.Find("Player").GetComponent<Player>();
        segmentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }
    //Detects other obstacles
    public void PlayerCollision(Player player,Collision2D other)
    {
        switch (other.gameObject.name)
        {
            case "Ravine":
                player.Die();
                Debug.Log("I touched a ravine");
                break;
            case "Spike":
                player.Die();
                Debug.Log("I touched a spike");
                break;
            case "Spike2":
                player.Die();
                Debug.Log("I touched a spike");
                break;
        }
    }
    //Checks for trigger to change the camera position
    public void PlayerCollider(Player player,Collider2D other)
    {
        switch (other.gameObject.name)
        {
            case "Checkpoint1":
                Debug.Log("Touching checkpoint");
                SetCheckpoint("setCheckpoint1");
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
                player.Rotate("Up");
                break;

            case "GravityDown":
                Physics2D.gravity = new Vector2(0f, -9.81f);
                player.Rotate("Down");
                break;

            case "GravityRight":
                Physics2D.gravity = new Vector2(9.81f, 0f);
                player.Rotate("Right");
                break;

            case "GravityLeft":
                Physics2D.gravity = new Vector2(-9.81f, 0f);
                player.Rotate("Left");
                break;
        }
    }
    public void SetCheckpoint(string checkpoint)
    {
        if(checkpoint == "setCheckpoint1")
        {
            GameObject.Find("Checkpoint1").GetComponent<SpriteRenderer>().sprite = checkpointActive;
        }
    }
}
