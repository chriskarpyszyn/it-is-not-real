using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{

    public GameObject player;
    public GameObject itIsRealText;
    public GameObject itIsNotRealText;
    public GameObject ghostPrefab;
    public GameObject musicManagerPrefab;
    public Light envLight;
    public Light envNightLight;

    public GameObject platformPrefab;
    public GameObject portalPrefab;
    public GameObject spikiesPrefab;
    public GameObject detectionZonePrefab;
    public GameObject pickupPrefab;
    public GameObject shieldPrefab;

    public Material bluePortalMat;
    public Material redPortalMat;

    public Material dayPlatformMat;
    public Material nightPlatformMat;

    public Material skyboxDay;
    public Material skyboxNight;

    private float timeToDestruction = 60f;

    private static GameObject musicManager;

    private bool gameHasEnded = false;
    private Vector3 lastPortalPosition;
    private bool isDreaming = false;
    private float ghostSpawnDelay = 1.98f;


    private List<GameObject> ghosts = new List<GameObject>();
    private List<GameObject> platforms = new List<GameObject>();
    private List<GameObject> spikies = new List<GameObject>();
    private List<GameObject> portals = new List<GameObject>();
    private List<GameObject> detectionZones = new List<GameObject>();
    private List<GameObject> pickups = new List<GameObject>();
    private List<GameObject> shields = new List<GameObject>();

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
        SpawnPlatformAndPortal(false);

        //int numOfPlatforms = 50;
        //for (int i=0; i<= numOfPlatforms; i++)
        //{
        //    SpawnAnotherPlatform();
        //    SpawnAPortal();
        //}
    }

    public void SpawnPlatformAndPortal(bool destroy)
    {
        SpawnAnotherPlatform(destroy);
        SpawnAPortal(destroy);
    }
    private void SpawnAnotherPlatform(bool destroy)
    {
        float platformXDistance2 = Mathf.Ceil(Random.Range(6f, 10f));
        float platformMaxY = 2f;
        float platformMinY = -2f;
        float platformRandomY = Random.Range(platformMinY, platformMaxY);

        GameObject anotherPlatform = Instantiate(platformPrefab, new Vector3(platformXDistance2 + platforms[platforms.Count - 1].transform.position.x, platformRandomY, 0), Quaternion.identity);
        platforms.Add(anotherPlatform);

        if (destroy)
            Destroy(anotherPlatform, timeToDestruction);

        SpawnSpikes(anotherPlatform.transform, destroy);
        SpawnPickups(anotherPlatform.transform, destroy);

        GameObject detectionZone = Instantiate(detectionZonePrefab, new Vector3(anotherPlatform.transform.position.x+2.5f, 0, 0), Quaternion.identity);
        detectionZones.Add(detectionZone);
    }

    private void SpawnSpikes(Transform platform, bool destroy)
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
            if (destroy)
                Destroy(spike1, timeToDestruction);
        }
    }

    private void SpawnPickups(Transform platform, bool destroy)
    {
        float farLeftX = platform.position.x - 3f;
        float farRightX = platform.position.x + 3f;
        float minY = platform.position.y + 1f;
        float maxY = platform.position.y + 4f;
        int maxNumberOfPickups = 1;
        int minNumberOfPickups = -3;

        int numberOfPickups = Random.Range(minNumberOfPickups, maxNumberOfPickups);

        for (int i = 0; i<=numberOfPickups; i++)
        {
            GameObject pickup = Instantiate(pickupPrefab, new Vector3(Random.Range(farLeftX, farRightX), Random.Range(minY, maxY), 0), Quaternion.identity);
            pickups.Add(pickup);
            if (destroy)
                Destroy(pickup, timeToDestruction);
        }
    }

    private void SpawnAPortal(bool destroy)
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
        if (destroy)
            Destroy(aPortal, timeToDestruction);

    }

    public void SpawnShield()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject shield = Instantiate(shieldPrefab, player.transform.position, Quaternion.identity);
        shield.GetComponent<FollowPlayer>().player = player.transform;
        shields.Add(shield);
    }

    // Update is called once per frame
    void Update()
    {
        //cheats
        if (Input.GetKeyDown(KeyCode.M))
        {
            SpawnShield();
        }
    }

    private void toggleMusicPitch()
    {
        if (isDreaming)
        {
            musicManager.GetComponent<AudioSource>().pitch = 1;
        } else
        {
            musicManager.GetComponent<AudioSource>().pitch = 0.5f;
        }
        
    }
    private void resetMusicPitch()
    {
        if (musicManager.GetComponent<AudioSource>().pitch < 1)
        {
            musicManager.GetComponent<AudioSource>().pitch = 1;
        }
    }

    public void CheckSheidOrDie()
    {
        if (shields.Count > 0)
        {
            //kill a ghost
            GameObject ghostToKill = ghosts[ghosts.Count-1];
            Destroy(ghostToKill);
            ghosts.RemoveAt(ghosts.Count - 1);

            //remove a shield
            GameObject shieldToRemove = shields[shields.Count-1];
            Destroy(shieldToRemove);
            shields.RemoveAt(shields.Count - 1);

            
        } else
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;

            FindObjectOfType<PlayerMovement>().disablePlayerMovement();
            Invoke("Restart", 1f);
        }
    }
    private void Restart()
    {
        FindObjectOfType<PlayerMovement>().resetPlayerProps();
        resetMusicPitch();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CreateGhostAtPosition(Vector3 lastPortalPosition)
    {
        //this may create a bug
        this.lastPortalPosition = lastPortalPosition;
        //NOTE: MUST MAKE SURE IS DREAMING IS TOGGLED BEFORE CALLING
        //CREATE GHOST
        
        if (isDreaming)
        {
            Invoke("DelayedCreateGhost", ghostSpawnDelay);
        }
        
    }
    private void DelayedCreateGhost()
    {
        if(isDreaming)
        {
            GameObject ghoulie = Instantiate(ghostPrefab, lastPortalPosition, new Quaternion(0, 0, 0, 0));
            ghosts.Add(ghoulie);
        }
            
    }

    public void ToggleDreamMode()
    {
        toggleMusicPitch();
        if (isDreaming)
        {
            isDreaming = false;
            FindObjectOfType<GameManager>().HideAllGhosts();
            setCanvasTitleText(isDreaming);

            //modify lightning
            Light light = envLight.GetComponent<Light>();
            light.enabled = true;
            Light nightLight = envNightLight.GetComponent<Light>();
            nightLight.enabled = false;

            //modify skybox
            RenderSettings.skybox = skyboxDay;

            //modify portal color
            foreach (GameObject portal in portals)
            {
                portal.GetComponent<MeshRenderer>().material = bluePortalMat;
            }

            //modify platform color
            foreach (GameObject platform in platforms)
            {
                platform.GetComponent<MeshRenderer>().material = dayPlatformMat;
            }

        }
        else
        {
            isDreaming = true;
            FindObjectOfType<GameManager>().ShowAllGhosts();
            setCanvasTitleText(isDreaming);

            //modify lightning
            Light light = envLight.GetComponent<Light>();
            light.enabled = false;
            Light nightLight = envNightLight.GetComponent<Light>();
            nightLight.enabled = true;

            //modify skybox
            RenderSettings.skybox = skyboxNight;

            //modify portal color
            foreach (GameObject portal in portals)
            {
                portal.GetComponent<MeshRenderer>().material = redPortalMat;
            }

            //modify platform color
            foreach (GameObject platform in platforms)
            {
                platform.GetComponent<MeshRenderer>().material = nightPlatformMat;
            }
        }
    }

    private void setCanvasTitleText(bool isDreaming)
    {
        if (isDreaming)
        {
            itIsNotRealText.SetActive(true);
            itIsRealText.SetActive(false);
        } else
        {
            itIsNotRealText.SetActive(false);
            itIsRealText.SetActive(true);
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


}
