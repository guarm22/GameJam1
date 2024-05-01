using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    
    public static GameSystem Instance;

    public bool isPaused;

    public void PauseGame() {
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        isPaused = false;
    }

    void Start() {
        Instance = this;
        isPaused = false;
    }

    // Update is called once per frame
    void Update() {
        /*if(Input.GetKeyDown(KeyCode.Q)) {
            if(isPaused) {
                ResumeGame();
            }
            else {
                PauseGame();
            }
        }*/
    }
}
