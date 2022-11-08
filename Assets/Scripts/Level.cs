using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
public class Level : MonoBehaviour
{
    public Camera segmentCamera;
    public Sprite checkpointActive;
    public Sprite checkpointUnactive;
    public Vector3[] segmentCameraPositions =
    {
        //Segment 1
        new Vector3(0f,0f,-10f),
        //Segment 2
        new Vector3(-18f,0.0f,-10f),
        //Segment 3
        new Vector3(-18f,-10f,-10f),
        //Segment 4
        new Vector3(-36f,-10f,-10f),
        //Segment 5
        new Vector3(-36f,-31f,-10f),
        //Segment 6
        new Vector3(-36f,-21f,-10f),
        //Segment 7
        new Vector3(-18f,-21f,-10f),
        //Segment 8
        new Vector3(0f,-21f,-10f),
        //Segment 9
        new Vector3(0f,-11f,-10f)
    };

    public int currentActiveCheckpoint; //0 = spawn, 1 = checkpoint1, 2 = checkpoint2
    public bool gameWon = false;
    private void Start()
    {
        segmentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        AudioManager.instance.Play("GameTheme1");
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
            case "ToSegment4":
                segmentCamera.transform.position = segmentCameraPositions[3];
                break;
            case "ToSegment5":
                segmentCamera.transform.position = segmentCameraPositions[4];
                break;
            case "ToSegment6":
                segmentCamera.transform.position = segmentCameraPositions[5];
                break;
            case "ToSegment7":
                segmentCamera.transform.position = segmentCameraPositions[6];
                break;
            case "ToSegment8":
                segmentCamera.transform.position = segmentCameraPositions[7];
                break;
            case "ToSegment9":
                segmentCamera.transform.position = segmentCameraPositions[8];
                break;
            case "Victory":
                StartCoroutine(GameWon());
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
        if (checkpoint == 0 && currentActiveCheckpoint != 1)
        {
            StopAllSoundtracks();
            AudioManager.instance.Play("GameTheme2");
            AudioManager.instance.Play("Checkpoint");
            currentActiveCheckpoint = 1;
            InactiveAllCheckpoints();
            GameObject.Find("Checkpoint1").GetComponent<SpriteRenderer>().sprite = checkpointActive;
            
        }
        else if (checkpoint == 1 && currentActiveCheckpoint != 2)
        {
            StopAllSoundtracks();
            AudioManager.instance.Play("GameTheme3");
            AudioManager.instance.Play("Checkpoint");
            currentActiveCheckpoint = 2;
            InactiveAllCheckpoints();
            GameObject.Find("Checkpoint2").GetComponent<SpriteRenderer>().sprite = checkpointActive;
        }
    }
    private void StopAllSoundtracks() 
    {
        AudioManager.instance.Stop("GameTheme1");
        AudioManager.instance.Stop("GameTheme2");
        AudioManager.instance.Stop("GameTheme3");
    }
    //Sets all checkpoints inactive by default
    private void InactiveAllCheckpoints()
    {
        GameObject.Find("Checkpoint1").GetComponent<SpriteRenderer>().sprite = checkpointUnactive;
        GameObject.Find("Checkpoint2").GetComponent<SpriteRenderer>().sprite = checkpointUnactive;
    }
    public IEnumerator GameWon()
    {
        if (gameWon == false)
        {
            Debug.Log("Congratulations, you won the game");
            StopAllSoundtracks();
            AudioManager.instance.Play("Victory");
            gameWon = true;
            yield return new WaitForSeconds(10f);
            SceneManager.LoadScene(0);
        }
    }
}