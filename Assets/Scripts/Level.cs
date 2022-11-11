using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Level : MonoBehaviour
{
    public Camera segmentCamera;
    //Checkpoint active sprite
    public Sprite checkpointActive;
    //Checkpoint inactive sprite
    public Sprite checkpointInactive;
    //List of segment camera positions
    public GameObject muteButton;
    public GameObject unmuteButton;
    public readonly Vector3[] segmentCameraPositions =
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
    public static GameObject Canvas;
    public int currentActiveCheckpoint; //0 = spawn, 1 = checkpoint1, 2 = checkpoint2
    //Checks if the game is beaten
    private bool gameWon = false;
    //Counts the amount of deaths
    public float DeathCounter;
    //The text for the death counter
    public Text deathCounterText;

    private void Start()
    {
        DeathCounter = 0f;
        deathCounterText = GameObject.Find("DeathCounterTxt").GetComponent<Text>();
        segmentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        ResetPlayer();
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
        //When first checkpoint is activated (1 time use, unless other checkpoint got activated)
        if (checkpoint == 0 && currentActiveCheckpoint != 1)
        {
            StopAllSoundtracks();
            AudioManager.instance.Play("GameTheme2");
            AudioManager.instance.Play("Checkpoint");
            currentActiveCheckpoint = 1;
            InactiveAllCheckpoints();
            GameObject.Find("Checkpoint1").GetComponent<SpriteRenderer>().sprite = checkpointActive;
            
        }
        //When second checkpoint is activated (1 time use, unless other checkpoint got activated)
        else if (checkpoint == 1 && currentActiveCheckpoint != 2)
        {
            StopAllSoundtracks();
            AudioManager.instance.Play("Checkpoint");
            currentActiveCheckpoint = 2;
            InactiveAllCheckpoints();
            GameObject.Find("Checkpoint2").GetComponent<SpriteRenderer>().sprite = checkpointActive;
            AudioManager.instance.Play("GameTheme3"); 
        }
    }
    //Stops all game themes
    private void StopAllSoundtracks() 
    {
        AudioManager.instance.Stop("GameTheme1");
        AudioManager.instance.Stop("GameTheme2");
        AudioManager.instance.Stop("GameTheme3");
    }
    //Mute all sounds
    public void MuteSoundtrack()
    {
        AudioListener.volume = 0;
        muteButton.SetActive(false);
        unmuteButton.SetActive(true);
    }
    //Unmute all sounds
    public void UnmuteSoundtrack()
    {
        AudioListener.volume = 1;
        unmuteButton.SetActive(false);
        muteButton.SetActive(true);
    }
    //Go back to main menu
    public void GoToMainMenu()
    {
        StopAllSoundtracks();
        SceneManager.LoadScene(0);
    }
    //Sets all checkpoints inactive by default
    private void InactiveAllCheckpoints()
    {
        GameObject.Find("Checkpoint1").GetComponent<SpriteRenderer>().sprite = checkpointInactive;
        GameObject.Find("Checkpoint2").GetComponent<SpriteRenderer>().sprite = checkpointInactive;
    }
    //When you finish the last segment
    public IEnumerator GameWon()
    {
        switch (gameWon)
        {
            case false:
                Debug.Log("Congratulations, you won the game");
                StopAllSoundtracks();
                AudioManager.instance.Play("Victory");
                gameWon = true;
                yield return new WaitForSeconds(10f);
                SceneManager.LoadScene(0);
                gameWon = false;
                break;
        }
    }
    //Resets all player info
    private void ResetPlayer()
    {
        segmentCamera.transform.position = segmentCameraPositions[0];
        Physics2D.gravity = new Vector2(0f, -9.81f);
        AudioListener.volume = 1;
    }
}