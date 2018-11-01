using UnityEngine;

public class EnemyCube : MonoBehaviour
{

    private Animator animator;
    private Enemy enemy;
    private bool resetAnimation = false;

    void Start()
    {
        enemy = transform.parent.GetComponent<Enemy>();
        animator = GetComponent<Animator>();
    }

    void LateUpdate()
    {
        if (resetAnimation)
        {
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.identity;
            enemy.TranslateEnemy();
			enemy.ChooseDirectionOnly();
            resetAnimation = false;
        }
    }   

    public void MoveAnimation() // Play movement animation
    {
        animator.Play("MoveDown Enemy");
    }

    public void MoveEnemy() // To be called by animation event move down
    {
        resetAnimation = true;
        animator.Play("Idle Enemy");
    }

    private void ResetDelay()
    {
		if(enemy != null)
		{
	    	enemy.moveDelay = false;
		}
	}
}
