using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject player;
    public GameObject ghostPrefab;

    private bool gameHasEnded = false;
    private Vector3 lastPortalPosition;
    private List<GameObject> ghosts = new List<GameObject>();

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
            Invoke("Restart", 1f);
            FindObjectOfType<PlayerMovement>().resetPlayerProps();

        }
    }

    public void CreateGhostAtPosition(Vector3 lastPortalPosition)
    {
        //this may create a bug
        this.lastPortalPosition = lastPortalPosition;
        Invoke("DelayedCreateGhost", 2);
    }

    public void HideAllGhosts()
    {
        foreach (GameObject ghost in ghosts)
        {
            ghost.SetActive(false);
        }
    }

    public void ShowAllGhosts()
    {
        foreach (GameObject ghost in ghosts)
        {
            ghost.SetActive(true);
        }
    }

    private void DelayedCreateGhost()
    {
        if (player.GetComponent<PlayerMovement>().isDreaming)
        {
            GameObject ghoulie = Instantiate(ghostPrefab, lastPortalPosition, new Quaternion(0, 0, 0, 0));
            ghosts.Add(ghoulie);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
