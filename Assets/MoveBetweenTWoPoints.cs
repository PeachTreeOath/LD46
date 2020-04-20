using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveBetweenTWoPoints : MonoBehaviour
{
 
    public void StartGame()
    {
        AudioManager.instance.StartGameSounds();
        SceneManager.LoadScene("Game");
    }
}
