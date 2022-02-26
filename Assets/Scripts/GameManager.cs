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

    public Transform ghostKillEffect;

    private static GameObject musicManager;

    static private bool gameHasEnded = false;
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

    static PlayerAudio playerAudio;

    void Start()
    {
        if (!gameHasEnded)
        {
            List<string> test = new List<string>();

            if (!GameObject.FindGameObjectWithTag("MusicManager"))
            {
                musicManager = Instantiate(musicManagerPrefab, transform.position, Quaternion.identity);
                musicManager.name = musicManagerPrefab.name;
                DontDestroyOnLoad(musicManager);
            }

            //load level procedurally

            //create platform
            float platformXDistance = Mathf.Ceil(Random.Range(6f, 10f));
            //GameObject firstPlatform = Instantiate(platformPrefab, new Vector3(0,0,0), Quaternion.identity);
            GameObject firstPlatform = GameObject.Find("Platform");
            platforms.Add(firstPlatform);
            SpawnPlatformAndPortal();
            SpawnPlatformAndPortal();
            SpawnPlatformAndPortal();

            //int numOfPlatforms = 50;
            //for (int i=0; i<= numOfPlatforms; i++)
            //{
            //    SpawnAnotherPlatform();
            //    SpawnAPortal();
            //}

            playerAudio = player.GetComponent<PlayerAudio>();
        }

    }

    public void SpawnPlatformAndPortal()
    {
        SpawnAnotherPlatform();
        SpawnAPortal();
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
        SpawnPickups(anotherPlatform.transform);

        GameObject detectionZone = Instantiate(detectionZonePrefab, new Vector3(anotherPlatform.transform.position.x+2.5f, 0, 0), Quaternion.identity);
        detectionZones.Add(detectionZone);
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

    private void SpawnPickups(Transform platform)
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

    public void SpawnShield()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject shield = Instantiate(shieldPrefab, player.transform.position, Quaternion.identity);
        shield.GetComponent<FollowPlayer>().player = player.transform;
        shields.Add(shield);
    }

    //************************
    //******garbage collections
    //************************
    private void destroyObjects(List<GameObject> gameObjectList, int listCount, int numToRemove)
    {
        if (gameObjectList.Count >= listCount)
        {
            for (int i = 0; i < numToRemove; i++)
            {
                GameObject go = gameObjectList[i];
                gameObjectList.RemoveAt(i);
                Destroy(go);
            }
        }
    }
    public void doDestroyObjects()
    {
        destroyObjects(platforms, 8, 1);
        destroyObjects(spikies, 25, 3);
        destroyObjects(pickups, 10, 1);
        destroyObjects(portals, 8, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //cheats
        if (Input.GetKeyDown(KeyCode.M))
        {
            SpawnShield();
        }

        if (Input.anyKeyDown && gameHasEnded
            && !(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
            gameHasEnded = false;
            SceneManager.LoadScene(0); 
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

    /*
     * When a ghost hits the player, check if they have a shielf
     */
    public void CheckSheidOrDie()
    {
        if (shields.Count > 0)
        {
            //kill a ghost
            GameObject ghostToKill = ghosts[ghosts.Count-1];
            Transform effect = Instantiate(ghostKillEffect, ghostToKill.transform.position, ghostToKill.transform.rotation);
            Destroy(ghostToKill);
            Destroy(effect.gameObject, 3);
            ghosts.RemoveAt(ghosts.Count - 1);

            //play a kill ghost sound
            GameObject.FindObjectOfType<PlayerAudio>().playGhostDeathSound();


            //remove a shield
            GameObject shieldToRemove = shields[shields.Count-1];
            shields.RemoveAt(shields.Count - 1);
            Destroy(shieldToRemove.gameObject);
            

            
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
            playerAudio.playDeathSound();

            FindObjectOfType<PlayerMovement>().disablePlayerMovement();
            Invoke("Restart", 1f);
        }
    }
    private void Restart()
    {
        FindObjectOfType<PlayerMovement>().resetPlayerProps();
        resetMusicPitch();
        SceneManager.LoadScene(1);
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
            //ghoulie.GetComponent<GhoseFade>().FadeInObject();

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
            ghost.GetComponent<GhoseFade>().FadeOutObject();
            ghost.SetActive(false);
        }
    }

    private void ShowAllGhosts()
    {
        foreach (GameObject ghost in ghosts)
        {
            ghost.SetActive(true);
            ghost.GetComponent<GhoseFade>().FadeInObject();

        }
    }

    public bool getIsDreaming()
    {
        return isDreaming;
    }

    public bool HasGameEnded()
    {
        return gameHasEnded;
    }


}
