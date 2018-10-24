using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float moveSpeed = 1, lives = 5, lifetime, movesMade, startMoves=15;
    public UnityEngine.UI.Text lifeTimer, lifeCount, WinLife, WinTime, WinText;
    public LifeController lifeLine;
    public GameObject winPanel, nextButton;
    public PlayerCube playerCube;
    //public Perspective perpsCamera;
    public bool gameOver, ghostLife, moveDelay;
	public Vector3 startPosition = new Vector3(4, 1.2f, 0);
    public float scaledMoveDistance = 1;
    public Respawn respawn;
    public List<Enemy> enemyList;
    public AudioClip sound_move;
    public AudioClip sound_death;
    public AudioClip sound_edge;
    public AudioClip sound_obsticalbump;
    public AudioClip sound_finish;
    public AudioClip sound_start;

    private AudioSource source;
    //private float volLowRange = 0.5f;
    //private float volHighRange = 1.0f;

    private Animator animator;
    private Vector3 newPosition;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        //perpsCamera = Camera.main.GetComponent<Perspective>();
        //lifetime = Time.time; // Start of game for scoring
        ghostLife = false;
    }

    void Update()
    {
        if (gameOver)
            return;
        //float lifespan = Time.time - lifetime;
        float lifespan = startMoves - movesMade;
        if (lifespan <= 0) // End of life
        {
            if (!ghostLife) // Ghost life
            {
                gameOver = true;
                //lifetime = Time.time;
                source.PlayOneShot(sound_death, 1f);
                animator.Play("Shrink"); // "Death" animation
                
                return;
            }
            GetComponent<SphereCollider>().enabled = true;
            ghostLife = false;
        }
        lifeTimer.text = Mathf.Round(15 - lifespan).ToString("00"); // Life timer display update
        lifeTimer.text = lifespan.ToString("00");
        if (moveDelay)
            return;
        float inputHorizontal = Input.GetAxis("Horizontal"), inputVertical = Input.GetAxis("Vertical"); // Get movement input
        if (inputVertical > 0) // Up
        {
            transform.rotation = Quaternion.Euler(0,-90,0);
            MovePlayer(0, 0, -scaledMoveDistance);
        }
        else if (inputVertical < 0) // Down
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            MovePlayer(0, 0, scaledMoveDistance);
        }
        else if (inputHorizontal > 0) // Right
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            MovePlayer(-scaledMoveDistance,0, 0);
        }
        else if (inputHorizontal < 0) // Left
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            MovePlayer(scaledMoveDistance, 0, 0);
        }
    }

    public void Win()
    {
        gameOver = true;
        winPanel.SetActive(true); // Display end screen
        WinText.text = "Level Completed!";
        WinLife.text = lives.ToString("Lives remaining: 0");
        WinTime.text = Mathf.Round((5 - lives) * 15 + Time.time - lifetime).ToString("0 seconds");
        source.PlayOneShot(sound_finish, 1f);
    }

    public void Lose()
    {
        winPanel.SetActive(true); // Display end screen
        WinTime.gameObject.SetActive(false);
        WinLife.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        WinText.text = "Level Failed!";
        source.PlayOneShot(sound_finish, 1f);
        while (source.isPlaying) { }
        gameObject.SetActive(false);
    }

    private void MovePlayer(float x, float y, float z)
    {
        /*
        Vector3 newPosition = transform.position + new Vector3(x * moveSpeed * Time.deltaTime, y * moveSpeed * Time.deltaTime, z * moveSpeed * Time.deltaTime); // Change to fixed space movement later
        Collider[] collisions = Physics.OverlapBox(newPosition, new Vector3(0.5f, 0.5f, 0.5f)); // Check for obstacles
        foreach (Collider col in collisions)
        {
            if (col.tag == "Obstacle")
            {
                return;
            }
        }
        transform.Translate(x * moveSpeed * Time.deltaTime, y * moveSpeed * Time.deltaTime, z * moveSpeed * Time.deltaTime); // Move
        */
        Vector3 movePosition = transform.position + new Vector3(x, y, z); // Change to fixed space movement later
        Vector3 relativePosition = movePosition - transform.position;
        relativePosition = relativePosition / 2;
        Collider[] collisions = Physics.OverlapBox(movePosition - relativePosition, new Vector3(0.1f, 1.1f, 0.1f)); // Check for obstacles
        foreach (Collider col in collisions)
        {
            switch (col.tag)
            {
                case "Obstacle":
					//Debug.Log("Player: Hit obstacle");
                    if(!moveDelay)
                        source.PlayOneShot(sound_obsticalbump, 1f);
                    return;
                default:
                    continue;
            }
        }
        collisions = Physics.OverlapBox(movePosition,new Vector3(0.48f, 2f, 0.48f)); // Check for ground
        foreach (Collider col in collisions)
        {
            switch (col.tag)
            {
                case "Ground":
					Debug.Log("Player: Ground detected");
                    newPosition = new Vector3(x, y, z);
                    moveDelay = true;
                    foreach (Enemy e in enemyList)
                    {
                        e.ChooseDirection();
                    }
                    playerCube.MoveAnimation(); // Play movement animation
                    source.PlayOneShot(sound_move, 1f);
                    return;
                default:
                    source.PlayOneShot(sound_edge, 1f);
                    continue;
            }
        }
    }

    public void TranslatePlayer()
    {
        movesMade++;
        transform.position += newPosition; // Move
        /* Camera Movement
        perpsCamera.TargetCameraPosition = newPosition;
        if (perpsCamera.CameraSmooth == false)
        {
            perpsCamera.CameraMove();
        }
        */
    }

    public void NextLife() // Called by animation event at end of shrink "death" animation
    {
        lives--;
        lifeLine.MinusLife();

        if (lives <= 0)
        {
            Lose();
            source.PlayOneShot(sound_finish, 1f);
            return;
        }
        gameOver = false;
        ResetPos();
        //lifeCount.text = lives.ToString();
    }
    private void ResetPos()
    {
        // transform.position = respawn.adjustedRespawn(); // Why was this needed?
        Vector3 respawnPosition = respawn.transform.position;
        transform.position = new Vector3(respawnPosition.x, 1.5f, respawnPosition.z);
        //perpsCamera.CameraPositionReset();
        movesMade = 0;
        animator.Play("Expand"); // Play spawn animation
        source.PlayOneShot(sound_start, 1f);
        //lifetime = Time.time; // Start of new life
    }
}
