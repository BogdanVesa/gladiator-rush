using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

<<<<<<< Updated upstream
	public int maxHealth = 100;
=======
	public GameObject GameOverScreen;
	//public GameObject Play;
	public int maxHealth = 2147483647;
>>>>>>> Stashed changes
	public int currentHealth;

	public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			TakeDamage(20);
		}
    }

	void TakeDamage(int damage)
	{
		if (currentHealth > 0){
			currentHealth -= damage;
			healthBar.SetHealth(currentHealth);
		}
		else {
			//Debug.Log("Game Over");
			//GameOverScreen.Setup();
			StartCoroutine(DisplaygameOver());
		}
	}

	IEnumerator DisplaygameOver(){
		yield return new WaitForSeconds(0.25f);
		GameOverScreen.SetActive(true);
	}

}