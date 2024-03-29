using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    private readonly float[] eventTimeStamps = new float[] { 1, 35, 49, 75, 82, 100, 112, 154, 185, 216, 240 };

    private const float START_POS_LEFT = -10;
    private const float START_POS_RIGHT = 10;
    private const float MAX_POS_TOP = 0.5f;
    private const float MIN_POS_BOT = -3f;

    private const float ROCKFISH_MAX_POS_TOP = -2f;

    [Header("Fish")]
    [SerializeField] private GameObject normalFish;
    [SerializeField] private GameObject rocketFish;
    [SerializeField] private GameObject rockFish;

    [Header("Enemies")]
    [SerializeField] private GameObject asteroid;
    [SerializeField] private GameObject jellyfish;

    [Header("Tutorial")]
    [SerializeField] private GameObject tutorial;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;

    private float timer = 0;
    private float countDown = 240;
    private int currentStage = 0;

    private bool normalFishEnabled;
    private bool rocketFishEnabled;
    private bool rockFishEnabled;
    private bool asteroidEnabled;
    private bool jellyfishEnabled;

    private float normalFishSpawnInterval = 3;
    private float rocketFishSpawnInterval = 7;
    private float rockFishSpawnInterval = 12;
    private float asteroidSpawnInterval = 6; 
    private float jellyfishSpawnInterval = 10; 

    private Coroutine co_spawningNormalFish = null;
    private Coroutine co_spawningRocketFish = null;
    private Coroutine co_spawningRockFish = null;
    private Coroutine co_spawningAsteroid = null;
    private Coroutine co_spawningJellyfish = null;
    
    private bool spawningNormalFish => co_spawningNormalFish != null;
    private bool spawningRocketFish => co_spawningRocketFish != null;
    private bool spawningRockFish => co_spawningRockFish != null;
    private bool spawningAsteroid => co_spawningAsteroid != null;
    private bool spawningJellyfish => co_spawningJellyfish != null;


    private void Update()
    {
        timer += Time.deltaTime;
        countDown -= Time.deltaTime;

        CheckTimer();
        SpawnEntities();
        UpdateTimer();
    }

    private void UpdateTimer() => timerText.text = ((int)countDown).ToString();

    private void CheckTimer()
    {
        if (timer > eventTimeStamps[currentStage])
        {
            if (currentStage == 0)
            {
                Debug.Log("Enter Stage 1");
                normalFishEnabled = true;
                asteroidEnabled = true;
                currentStage++;
            }
            else if (currentStage == 1)
            {
                Debug.Log("Enter Stage 2 (add rocket)");
                SpawnRocketFish();
                rocketFishEnabled = true;
                currentStage++;
            }
            else if (currentStage == 2)
            {
                Debug.Log("Enter Stage 3 (speed up)");
                normalFishSpawnInterval = 2.5f;
                asteroidSpawnInterval = 5f;             
                currentStage++;
            }
            else if (currentStage == 3)
            {
                Debug.Log("Enter Stage 4 (stop for tutorial)");
                
                normalFishEnabled = false;
                asteroidEnabled = false;
                rocketFishEnabled = false;
                currentStage++;
            }
            else if (currentStage == 4)
            {
                Debug.Log("Enter Stage 5 (spawns rock tutorial)");
                SpawnRockFishTutorial();
                currentStage++;
            }
            else if (currentStage == 5)
            {
                Debug.Log("Enter Stage 6 (spawn rocks for tutorial )");
                SpawnRockFish();
                rockFishEnabled = true;
                asteroidEnabled = true;
                rockFishSpawnInterval = 5;
                asteroidSpawnInterval = 5;
                currentStage++;
            }
            else if (currentStage == 6)
            {
                Debug.Log("Enter Stage 6 (spawn everyone again)");
                normalFishSpawnInterval = 4;
                asteroidSpawnInterval = 4.5f;
                rockFishSpawnInterval = 9;
                normalFishEnabled = true;
                asteroidEnabled = true;
                rocketFishEnabled = true;
                currentStage++;
            }
            else if (currentStage == 7)
            {
                Debug.Log("Enter Stage 7 (add jelly)");
                SpawnJellyfish();
                normalFishSpawnInterval = 3;
                asteroidSpawnInterval = 4f;
                rocketFishSpawnInterval = 7;
                rockFishSpawnInterval = 8;
                jellyfishEnabled = true;
                currentStage++;
            }
            else if (currentStage == 8)
            {
                Debug.Log("Enter Stage 8 (final speed up)");
                normalFishSpawnInterval = 2.5f;
                asteroidSpawnInterval = 3.5f;
                rocketFishSpawnInterval = 5;
                rockFishSpawnInterval = 7;
                jellyfishSpawnInterval = 5;
                currentStage++;
            }
            else if (currentStage == 9)
            {
                Debug.Log("Enter Stage 9 (stop end)");
                normalFishEnabled = false;
                asteroidEnabled = false;
                rocketFishEnabled = false;
                rockFishEnabled = false;
                jellyfishEnabled = false;
                currentStage++;               
            }
            else if (currentStage == 10)
            {
                SceneController.Instance.SwitchScene("Results"); //load final scene        
            }
        }
    }

    private void SpawnEntities()
    {
        if (normalFishEnabled)
            CheckToSpawnNormalFish();
        if (rocketFishEnabled)
            CheckToSpawnRocketFish();
        if (rockFishEnabled)
            CheckToSpawnRockFish();
        if (asteroidEnabled)
            CheckToSpawnAsteroid();
        if (jellyfishEnabled)
            CheckToSpawnJellyfish();
    }


    #region ASTEROID SPAWN
    private void CheckToSpawnAsteroid()
    {
        if (spawningAsteroid)
            return;

        float currentTime = Time.time;

        co_spawningAsteroid = StartCoroutine(WaitToSpawnAsteroid(currentTime));
    }

    private IEnumerator WaitToSpawnAsteroid(float lastTime)
    {
        while (Time.time < lastTime + asteroidSpawnInterval)
            yield return null;

        SpawnAsteroid();

        co_spawningAsteroid = null;
    }

    private void SpawnAsteroid()
    {
        bool moveLeft = Random.Range(0, 2) == 0; //50/50 chance to move left or right
        float yPos = Random.Range(MAX_POS_TOP, MIN_POS_BOT); //choose y position to spawn

        GameObject obj = Instantiate(asteroid);
        obj.transform.position = new(moveLeft ? START_POS_RIGHT : START_POS_LEFT, yPos);
        EnemyAsteroid enemyAsteroid = obj.GetComponent<EnemyAsteroid>();
        enemyAsteroid.InitializeDirection(moveLeft);
    }
    #endregion

    #region JELLYFISH SPAWN
    private void CheckToSpawnJellyfish()
    {
        if (spawningJellyfish)
            return;

        float currentTime = Time.time;

        co_spawningJellyfish = StartCoroutine(WaitToSpawnJellyfish(currentTime));
    }

    private IEnumerator WaitToSpawnJellyfish(float lastTime)
    {
        while (Time.time < lastTime + jellyfishSpawnInterval)
            yield return null;

        SpawnJellyfish();

        co_spawningJellyfish = null;
    }

    private void SpawnJellyfish()
    {
        bool moveLeft = Random.Range(0, 2) == 0; //50/50 chance to move left or right
        float yPos = Random.Range(MAX_POS_TOP, MIN_POS_BOT); //choose y position to spawn

        GameObject obj = Instantiate(jellyfish);
        obj.transform.position = new(moveLeft ? START_POS_RIGHT : START_POS_LEFT, yPos);
        Jellyfish jelly = obj.GetComponent<Jellyfish>();
        jelly.InitializeDirection(moveLeft);
    }
    #endregion

    #region NORMAL FISH SPAWN
    private void CheckToSpawnNormalFish()
    {
        if (spawningNormalFish)
            return;

        float currentTime = Time.time;

        co_spawningNormalFish = StartCoroutine(WaitToSpawnNormalFish(currentTime));
    }

    private IEnumerator WaitToSpawnNormalFish(float lastTime)
    {
        while (Time.time < lastTime + normalFishSpawnInterval)
            yield return null;

        SpawnNormalFish();

        co_spawningNormalFish = null;
    }

    private void SpawnNormalFish()
    {
        bool moveLeft = Random.Range(0, 2) == 0; //50/50 chance to move left or right
        float yPos = Random.Range(MAX_POS_TOP, MIN_POS_BOT); //choose y position to spawn

        GameObject obj = Instantiate(normalFish);
        obj.transform.position = new(moveLeft ? START_POS_RIGHT : START_POS_LEFT, yPos);
        BaseFish baseFish = obj.GetComponent<BaseFish>();
        baseFish.Initialize(moveLeft);
    }
    #endregion

    #region ROCKET FISH SPAWN
    private void CheckToSpawnRocketFish()
    {
        if (spawningRocketFish)
            return;

        float currentTime = Time.time;

        co_spawningRocketFish = StartCoroutine(WaitToSpawnRocketFish(currentTime));
    }

    private IEnumerator WaitToSpawnRocketFish(float lastTime)
    {
        while (Time.time < lastTime + rocketFishSpawnInterval)
            yield return null;

        SpawnRocketFish();

        co_spawningRocketFish = null;
    }

    private void SpawnRocketFish()
    {
        bool moveLeft = Random.Range(0, 2) == 0; //50/50 chance to move left or right
        float yPos = Random.Range(MAX_POS_TOP, MIN_POS_BOT); //choose y position to spawn

        GameObject obj = Instantiate(rocketFish);
        obj.transform.position = new(moveLeft ? START_POS_RIGHT : START_POS_LEFT, yPos);
        BaseFish baseFish = obj.GetComponent<BaseFish>();
        baseFish.Initialize(moveLeft);
    }
    #endregion

    #region ROCK FISH SPAWN
    private void CheckToSpawnRockFish()
    {
        if (spawningRockFish)
            return;

        float currentTime = Time.time;

        co_spawningRockFish = StartCoroutine(WaitToSpawnRockFish(currentTime));
    }

    private IEnumerator WaitToSpawnRockFish(float lastTime)
    {
        while (Time.time < lastTime + rockFishSpawnInterval)
            yield return null;

        SpawnRockFish();

        co_spawningRockFish = null;
    }

    private void SpawnRockFish()
    {
        bool moveLeft = Random.Range(0, 2) == 0; //50/50 chance to move left or right
        float yPos = Random.Range(ROCKFISH_MAX_POS_TOP, MIN_POS_BOT); //choose y position to spawn

        GameObject obj = Instantiate(rockFish);
        obj.transform.position = new(moveLeft ? START_POS_RIGHT : START_POS_LEFT, yPos);
        BaseFish baseFish = obj.GetComponent<BaseFish>();
        baseFish.Initialize(moveLeft);
    }
    #endregion

    private void SpawnRockFishTutorial()
    {
        GameObject obj = Instantiate(tutorial);
        obj.transform.position = new(13.94f , - 1.15f);
    }
}
