using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private bool gameHasEnded = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            //todo-ckdo i need to disable movement?

            FindObjectOfType<PlayerMovement>().disablePlayerMovement();
            Invoke("Restart", 2f);
            FindObjectOfType<PlayerMovement>().resetPlayerProps();

        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
