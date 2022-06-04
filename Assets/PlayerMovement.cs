using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
	
	public Animator animator;

	public Transform attackPoint;
	public float attackRange = 0.5f;
	public int attackDamage = 50;
	public LayerMask enemyLayers;
	
	// Update is called once per frame
	void Update () {


		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			Debug.Log("is jumping");
			animator.SetBool("IsJumping",true);
		}

		if (Input.GetKeyDown(KeyCode.X))
		{
			Attack();
		}

		// if (Input.GetButtonDown("Crouch"))
		// {
		// 	crouch = true;
		// } else if (Input.GetButtonUp("Crouch"))
		// {
		// 	crouch = false;
		// }

	}

	void Attack(){
		animator.SetTrigger("Attack");

		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange,enemyLayers);

		foreach (Collider2D enemy in hitEnemies)
		{
			enemy.GetComponent<EnemyAI>().TakeDamage(attackDamage);
		}

	}

	void OnDrawGizmosSelected(){
		if(attackPoint == null)
			return;
		
		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}

	public void OnLanding(){
		animator.SetBool("IsJumping", false);
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}
}
