using UnityEngine;

public class PlayerCube : MonoBehaviour {

    private Animator animator;
    private Player player;

	void Start () {
        player = transform.parent.GetComponent<Player>();
        animator = GetComponent<Animator>();
	}

    public void MoveAnimation() // Play movement animation
    {
        animator.Play("MoveDown");
    }

    public void MovePlayer() // To be called by animation event move down
    {
        player.TranslatePlayer();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        animator.Play("Idle");
    }

	private void ResetDelay()
	{
		player.moveDelay = false;
	}
}
