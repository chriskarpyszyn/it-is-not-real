using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public GameObject player;
    public GameObject ghostPrefab;
    public GameObject musicManagerPrefab;

    public GameObject platformPrefab;
    public GameObject portalPrefab;
    public GameObject spikiesPrefab;

    private static GameObject musicManager;

    private bool gameHasEnded = false;
    private Vector3 lastPortalPosition;
    private bool isDreaming = false;


    private List<GameObject> ghosts = new List<GameObject>();
    private List<GameObject> platforms = new List<GameObject>();
    private List<GameObject> spikies = new List<GameObject>();
    private List<GameObject> portals = new List<GameObject>();

    void Start()
    {
        if (!GameObject.FindGameObjectWithTag("MusicManager"))
        {
            musicManager = Instantiate(musicManagerPrefab, transform.position, Quaternion.identity);
            musicManager.name = musicManagerPrefab.name;
            DontDestroyOnLoad(musicManager);
        }

        //load level procedurally

        //create platform
        float platformXDistance = Mathf.Ceil(Random.Range(6f, 10f));
        GameObject firstPlatform = Instantiate(platformPrefab, new Vector3(0,0,0), Quaternion.identity);
        platforms.Add(firstPlatform);

        int numOfPlatforms = 50;
        for (int i=0; i<= numOfPlatforms; i++)
        {
            SpawnAnotherPlatform();
            SpawnAPortal();
        }
    }

    private void SpawnAnotherPlatform()
    {
        float platformXDistance2 = Mathf.Ceil(Random.Range(6f, 10f));
        float platformMaxY = 2f;
        float platformMinY = -2f;
        float platformRandomY = Random.Range(platformMinY, platformMaxY);

        GameObject anotherPlatform = Instantiate(platformPrefab, new Vector3(platformXDistance2 + platforms[platforms.Count - 1].transform.position.x, platformRandomY, 0), Quaternion.identity);
        platforms.Add(anotherPlatform);
        SpawnSpikes(anotherPlatform.transform);
    }

    private void SpawnSpikes(Transform platform)
    {
        float farLeftX = platform.position.x - 2.23f;
        float farRightX = platform.position.x + 1.89f;
        int maxNumberOfSpikes = 4;
        int minNumberOfSpikes = 0;

        //spawn between 0 and 3 spikes
        int numberOfSpikes = Random.Range(minNumberOfSpikes, maxNumberOfSpikes);

        for (int i = 0; i<=numberOfSpikes; i++)
        {
            GameObject spike1 = Instantiate(spikiesPrefab, new Vector3(Random.Range(farLeftX, farRightX), platform.position.y + 0.29f, 0), Quaternion.identity);
            spikies.Add(spike1);
        }
    }

    private void SpawnAPortal()
    {
        //null check on list
        Transform leftPlatform = platforms[platforms.Count - 2].transform;
        Transform rightPlatform = platforms[platforms.Count - 1].transform;

        float centerX = (rightPlatform.position.x - leftPlatform.position.x) / 2; //?

        float midpointY = (Mathf.Abs(leftPlatform.position.y) + Mathf.Abs(rightPlatform.position.y))/2;
        float centerY;
        if (leftPlatform.position.y > rightPlatform.position.y)
        {
            centerY = leftPlatform.position.y - midpointY;
        } else
        {
            centerY = rightPlatform.position.y - midpointY;
        }
        

        GameObject aPortal;
        if (platforms.Count == 2)
        {
            aPortal = Instantiate(portalPrefab, new Vector3(centerX, centerY, 0), Quaternion.identity); //todo-ck can i ignore quaternion prop
        } else
        {
            aPortal = Instantiate(portalPrefab, new Vector3(centerX + leftPlatform.position.x, centerY, 0), Quaternion.identity); //todo-ck can i ignore quaternion prop

        }
        portals.Add(aPortal);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void toggleMusicPitch()
    {
        if (isDreaming)
        {
            musicManager.GetComponent<AudioSource>().pitch = 1;
        } else
        {
            musicManager.GetComponent<AudioSource>().pitch = -0.7f;
        }
        
    }
    private void resetMusicPitch()
    {
        if (musicManager.GetComponent<AudioSource>().pitch < 0)
        {
            musicManager.GetComponent<AudioSource>().pitch = 1;
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
