using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 1, lives = 5, lifetime, movesMade, startMoves=15;
    public UnityEngine.UI.Text lifeTimer, lifeCount, WinLife, WinTime, WinText;
    public GameObject winPanel, nextButton;
    public PlayerCube playerCube;
    public Perspective perpsCamera;
    public bool gameOver, ghostLife, moveDelay;
	public Vector3 startPosition = new Vector3(4, 1.2f, 0);

    private Animator animator;
    private Vector3 newPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        lifetime = Time.time; // Start of game for scoring
        ghostLife = false;
    }

    void Update()
    {
        if (gameOver)
            return;
        //float lifespan = Time.time - lifetime;
        float lifespan = startMoves - movesMade;
        if (lifespan < 0) // End of life
        {
            if (!ghostLife) // Ghost life
            {
                gameOver = true;
                lifetime = Time.time;
                animator.Play("Shrink"); // "Death" animation
                return;
            }
            ResetPos();
            GetComponent<SphereCollider>().enabled = true;
            ghostLife = false;
        }
        // lifeTimer.text = Mathf.Round(15 - lifespan).ToString("00"); // Life timer display update
        lifeTimer.text = lifespan.ToString("00");
        if (moveDelay)
            return;
        float inputHorizontal = Input.GetAxis("Horizontal"), inputVertical = Input.GetAxis("Vertical"); // Get movement input
        if (inputVertical > 0) // Up
        {
            transform.rotation = Quaternion.Euler(0,180,0);
            MovePlayer(2, 0, 0);
        }
        else if (inputVertical < 0) // Down
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            MovePlayer(-2, 0, 0);
        }
        else if (inputHorizontal > 0) // Right
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
            MovePlayer(0, 0, -2);
        }
        else if (inputHorizontal < 0) // Left
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            MovePlayer(0, 0, 2);
        }
    }

    public void Win()
    {
        gameOver = true;
        winPanel.SetActive(true); // Display end screen
        WinText.text = "Level Completed!";
        WinLife.text = lives.ToString("Lives remaining: 0");
        WinTime.text = Mathf.Round((5 - lives) * 15 + Time.time - lifetime).ToString("0 seconds");
    }

    public void Lose()
    {
        winPanel.SetActive(true); // Display end screen
        WinTime.gameObject.SetActive(false);
        WinLife.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        WinText.text = "Level Failed!";
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
        Collider[] collisions = Physics.OverlapBox(movePosition, new Vector3(0.5f, 0.5f, 0.5f)); // Check for obstacles
        foreach (Collider col in collisions)
        {
            if (col.tag == "Obstacle")
            {
                return;
            }
        }
        newPosition = new Vector3(x, y, z);
		moveDelay = true;
        playerCube.MoveAnimation(); // Play movement animation
    }

    public void TranslatePlayer()
    {
        movesMade++;
        transform.position += newPosition; // Move
        perpsCamera.TargetCameraPosition = newPosition;
        if (perpsCamera.CameraSmooth == false)
        {
            perpsCamera.CameraMove();
        }
    }

    private void NextLife() // Called by animation event at end of shrink "death" animation
    {
        if (lives <= 0)
        {
            Lose();
            return;
        }
        gameOver = false;
        ResetPos();
        lives--;
        lifeCount.text = lives.ToString();
    }
    private void ResetPos()
    {
        transform.position = startPosition; // Start position, change variable for checkpoints
        perpsCamera.CameraPositionReset();
        movesMade = 0;
        animator.Play("Expand"); // Play spawn animation
        lifetime = Time.time; // Start of new life
    }
}
