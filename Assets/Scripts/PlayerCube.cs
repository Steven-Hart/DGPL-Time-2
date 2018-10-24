using UnityEngine;

public class PlayerCube : MonoBehaviour {

    private Animator animator;
    private Player player;
    private bool resetAnimation = false;
    public AudioClip sound_move;


    void Start () {
        player = transform.parent.GetComponent<Player>();
        animator = GetComponent<Animator>();
	}

    void LateUpdate()
    {
        if (resetAnimation)
        {
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.identity;
            player.TranslatePlayer();
            resetAnimation = false;
        }
    }

    public void MoveAnimation() // Play movement animation
    {
        animator.Play("MoveDown");
    }

    public void MovePlayer() // To be called by animation event move down
    {
        resetAnimation = true;
        animator.Play("Idle");
        GetComponentInParent<AudioSource>().PlayOneShot(sound_move, 1f);
    }

    private void ResetDelay()
	{
		player.moveDelay = false;
	}
}
