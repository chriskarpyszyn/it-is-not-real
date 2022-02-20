using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject player;
    public GameObject ghostPrefab;
    public GameObject musicManagerPrefab;
    private static GameObject musicManager;

    private bool gameHasEnded = false;
    private Vector3 lastPortalPosition;
    private List<GameObject> ghosts = new List<GameObject>();
    private bool isDreaming = false;

    void Start()
    {
        if (!GameObject.FindGameObjectWithTag("MusicManager"))
        {
            musicManager = Instantiate(musicManagerPrefab, transform.position, Quaternion.identity);
            musicManager.name = musicManagerPrefab.name;
            DontDestroyOnLoad(musicManager);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void toggleMusicPitch()
    {
        musicManager.GetComponent<AudioSource>().pitch *= -1;
    }
    private void resetMusicPitch()
    {
        if (musicManager.GetComponent<AudioSource>().pitch < 0)
        {
            musicManager.GetComponent<AudioSource>().pitch *= -1;
        }
    }

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;

            FindObjectOfType<PlayerMovement>().disablePlayerMovement();
            FindObjectOfType<PlayerMovement>().resetPlayerProps();
            resetMusicPitch();
            Invoke("Restart", 1f);
        }
    }
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CreateGhostAtPosition(Vector3 lastPortalPosition)
    {
        //this may create a bug
        this.lastPortalPosition = lastPortalPosition;
        Invoke("DelayedCreateGhost", 2);
    }

    public void ToggleDreamMode()
    {
        toggleMusicPitch();
        if (isDreaming)
        {
            isDreaming = false;
            FindObjectOfType<GameManager>().HideAllGhosts();
        }
        else
        {
            isDreaming = true;
            FindObjectOfType<GameManager>().ShowAllGhosts();
        }
    }

    private void HideAllGhosts()
    {
        foreach (GameObject ghost in ghosts)
        {
            ghost.SetActive(false);
        }
    }

    private void ShowAllGhosts()
    {
        foreach (GameObject ghost in ghosts)
        {
            ghost.SetActive(true);
        }
    }

    public bool getIsDreaming()
    {
        return isDreaming;
    }



    private void DelayedCreateGhost()
    {
        if (isDreaming)
        {
            GameObject ghoulie = Instantiate(ghostPrefab, lastPortalPosition, new Quaternion(0, 0, 0, 0));
            ghosts.Add(ghoulie);
        }
    }


}
