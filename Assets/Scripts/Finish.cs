using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public Button retryButton, nextButton;

    void Start()
    {
        //retryButton.GetComponent<Button>().onClick.AddListener(Retry);
        //nextButton.GetComponent<Button>().onClick.AddListener(Next);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Win();
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
    public void Next(string sceneName)
    {
        // Next scene
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
