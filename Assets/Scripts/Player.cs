using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float moveSpeed = 1, lives = 5, lifetime, movesMade, startMoves=15;
    //public UnityEngine.UI.Text lifeTimer, lifeCount, WinLife, WinTime, WinText;
    public LifeController lifeLine;
    //public GameObject winPanel, nextButton;
    public PlayerCube playerCube;
    //public Perspective perpsCamera;
    public bool gameOver, ghostLife, moveDelay;
	public Vector3 startPosition = new Vector3(4, 1.2f, 0);
    public float scaledMoveDistance = 1;
    public Respawn respawn;
    public List<Enemy> enemyList;
    public AudioClip sound_death;
    public AudioClip sound_edge;
    public AudioClip sound_obsticalbump;
    public AudioClip sound_finish;
    public AudioClip sound_start;
	public AudioClip sound_lose;
	public Direction currentDirection=Direction.Up;

    private AudioSource source;
    //private float volLowRange = 0.5f;
    //private float volHighRange = 1.0f;

    private Animator animator;
    private Vector3 newPosition;

    void Start()
	{
        //lifeLine.LinkMoves();
		lifeLine.StartingMoves = Mathf.RoundToInt(startMoves);
		lifeLine.SetStartingMoves();
		source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        //perpsCamera = Camera.main.GetComponent<Perspective>();
        //lifetime = Time.time; // Start of game for scoring
        ghostLife = false;
    }

    void Update()
    {
		if (Input.GetAxis("Restart") > 0)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		}
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
                LoseLife();
                return;
            }
            GetComponent<SphereCollider>().enabled = true;
            ghostLife = false;
        }
        //lifeTimer.text = Mathf.Round(15 - lifespan).ToString("00"); // Life timer display update
        //lifeTimer.text = lifespan.ToString("00");
        if (moveDelay)
            return;
        float inputHorizontal = Input.GetAxis("Horizontal"), inputVertical = Input.GetAxis("Vertical"); // Get movement input
        if (inputVertical > 0) // Up
        {
            transform.rotation = Quaternion.Euler(0,-90,0);
			// You may be wondering why it says right below instead of UP, well its to match up with enemy script's directions and I'm too scared to replace enemy script code in case something breaks since we're running out of time. <3 Calvin 
			currentDirection = Direction.Right;
            MovePlayer(0, 0, -scaledMoveDistance);
        }
        else if (inputVertical < 0) // Down
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
			currentDirection = Direction.Left;
            MovePlayer(0, 0, scaledMoveDistance);
        }
        else if (inputHorizontal > 0) // Right
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
			currentDirection = Direction.Down;
            MovePlayer(-scaledMoveDistance,0, 0);
        }
        else if (inputHorizontal < 0) // Left
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
			currentDirection = Direction.Up;
            MovePlayer(scaledMoveDistance, 0, 0);
        }
    }

    public void Win()
    {
        gameOver = true;
        source.PlayOneShot(sound_finish, 1f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelFade>().ChooseFade(LevelFade.FadeState.NextLevel);
        gameOver = false;
    }

    public void Lose()
    {
        source.PlayOneShot(sound_lose, 1f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelFade>().ChooseFade(LevelFade.FadeState.RestartLevel);
    }

    private void MovePlayer(float x, float y, float z)
    {
        Vector3 movePosition = transform.position + new Vector3(x, y, z); // Change to fixed space movement later
        Vector3 relativePosition = movePosition - transform.position;
        relativePosition = relativePosition / 2;
        Collider[] collisions = Physics.OverlapBox(movePosition - relativePosition, new Vector3(0.1f, 1.1f, 0.1f)); // Check for obstacles
        foreach (Collider col in collisions)
        {
            switch (col.tag)
            {
                case "Obstacle":
                    if(!source.isPlaying)
                        source.PlayOneShot(sound_obsticalbump, 1f);
                    return;
                default:
                    continue;
            }
        }
        collisions = Physics.OverlapBox(movePosition, new Vector3(0.48f, 0.8f, 0.48f)); // Check for ground
		bool groundExists = false;
        foreach (Collider col in collisions)
        {
            switch (col.tag)
            {
                case "Ground":
					groundExists = true;
                    break;
				case "Enemy":
					Direction enemyDirection = col.gameObject.GetComponent<Enemy>().currentDirection;
					if (currentDirection == enemyDirection)
					{
						break;
					} else if ((currentDirection == Direction.Left && enemyDirection == Direction.Right)||
						(currentDirection == Direction.Right && enemyDirection == Direction.Left)||
						(currentDirection == Direction.Up && enemyDirection == Direction.Down)||
						(currentDirection == Direction.Down && enemyDirection == Direction.Up))
						{
							return;
						}
					break;
                default:
                    continue;
            }
        }
		if (groundExists)
		{
			groundExists = true;
			newPosition = new Vector3(x, y, z);
			moveDelay = true;
			lifeLine.MinusMove();
			foreach (Enemy e in enemyList)
			{
				e.ChooseDirection();
			}
			playerCube.MoveAnimation(); // Play movement animation
		}
    }

    public void TranslatePlayer()
    {
        movesMade++;
        transform.position += newPosition; // Move
    }

    public void LoseLife()
    {
        gameOver = true;
        source.PlayOneShot(sound_death, 1f);
        animator.Play("Shrink"); // "Death" animation
        lives--;
        lifeLine.MinusLife();
    }

    public void NextLife() // Called by animation event at end of shrink "death" animation
    {
		// To stop lose being triggered after winning on last move
        if (lives <= 0)
        {
			if (gameOver)
				Lose();
            return;
		}
		animator.Play("Expand"); // Play spawn animation
		gameOver = false;
		ResetPos();
    }
    private void ResetPos()
    {
        Vector3 respawnPosition = respawn.transform.position;
        transform.position = new Vector3(respawnPosition.x, 1.5f, respawnPosition.z);
        movesMade = 0;
		lifeLine.MovesReset();
        source.PlayOneShot(sound_start, 1f);
    }
}
