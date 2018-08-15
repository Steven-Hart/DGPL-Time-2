using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 1, lives = 5, lifetime;
    public UnityEngine.UI.Text lifeTimer, lifeCount, WinLife, WinTime, WinText;
    public GameObject winPanel, nextButton;

    private bool gameOver;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        lifetime = Time.time;
    }

    void Update()
    {
        if (gameOver)
            return;
        float lifespan = Time.time - lifetime;
        if (lifespan >= 15)
        {
            gameOver = true;
            lifetime = Time.time;
            animator.Play("Shrink");
            return;
        }
        lifeTimer.text = Mathf.Round(15 - lifespan).ToString("00");
        float inputHorizontal = Input.GetAxis("Horizontal"), inputVertical = Input.GetAxis("Vertical");
        if (inputVertical > 0)
        {
            MovePlayer(2, 0, 0);
        }
        else if (inputVertical < 0)
        {
            MovePlayer(-2, 0, 0);
        }
        else if (inputHorizontal > 0)
        {
            MovePlayer(0, 0, -2);
        }
        else if (inputHorizontal < 0)
        {
            MovePlayer(0, 0, 2);
        }
    }

    public void Win()
    {
        gameOver = true;
        winPanel.SetActive(true);
        WinText.text = "Level Completed!";
        WinLife.text = lives.ToString("Lives remaining: 0");
        WinTime.text = Mathf.Round((5 - lives) * 15 + Time.time - lifetime).ToString("0 seconds");
    }

    public void Lose()
    {
        winPanel.SetActive(true);
        WinTime.gameObject.SetActive(false);
        WinLife.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        WinText.text = "Level Failed!";
    }

    private void MovePlayer(float x, float y, float z)
    {
        Vector3 newPosition = transform.position + new Vector3(x * moveSpeed * Time.deltaTime, y * moveSpeed * Time.deltaTime, z * moveSpeed * Time.deltaTime);
        Collider[] collisions = Physics.OverlapBox(newPosition, new Vector3(0.5f, 0.5f, 0.5f));
        foreach (Collider col in collisions)
        {
            if (col.tag == "Obstacle")
            {
                return;
            }
        }
        transform.Translate(x * moveSpeed * Time.deltaTime, y * moveSpeed * Time.deltaTime, z * moveSpeed * Time.deltaTime);
    }

    private void NextLife()
    {
        if (lives <= 0)
        {
            Lose();
            return;
        }
        gameOver = false;
        transform.position = new Vector3(4, 0.5f, 0);
        animator.Play("Expand");
        lifetime = Time.time;
        lives--;
        lifeCount.text = lives.ToString();
    }
}
