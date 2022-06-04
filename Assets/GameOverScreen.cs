using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    GameObject gameObject;

    public void Setup(){
        Debug.Log("GameOver");
        gameObject.SetActive(true);
        
    }

    public void RestartGame(){
        SceneManager.LoadScene(0);
    }

}
