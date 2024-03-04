using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private const float GAME_LENGTH = 219;
    private float[] eventTimeStamps = new float[] { 10, 35, 49, 75, 82, 102, 120, 154, 185, 216, 240 };

    private const float START_POS_LEFT = -10;
    private const float START_POS_RIGHT = 10;
    private const float MAX_POS_TOP = 0.5f;
    private const float MIN_POS_BOT = -3f;

    [Header("Fish")]
    [SerializeField] private GameObject normalFish;
    [SerializeField] private GameObject rocketFish;
    [SerializeField] private GameObject rockFish;

    [Header("Enemies")]
    [SerializeField] private GameObject asteroid;
    [SerializeField] private GameObject jellyfish;

    [Header("Tutorial")]
    [SerializeField] private GameObject tutorial;

    private float timer = 0;
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

        CheckTimer();
        SpawnEntities();
    }

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
                Debug.Log("Enter Stage 6 (spawns rock tutorial)");
                SpawnRockFishTutorial();
                currentStage++;
            }
            else if (currentStage == 5)
            {
                Debug.Log("Enter Stage 6 (spawn rocks for tutorial )");
                SpawnRockFish();
                SpawnAsteroid();
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
                SceneController.Instance.NextScene("Results"); //load final scene        
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
    private Coroutine CheckToSpawnAsteroid()
    {
        if (spawningAsteroid)
            return co_spawningAsteroid;

        float currentTime = Time.time;

        co_spawningAsteroid = StartCoroutine(WaitToSpawnAsteroid(currentTime));

        return co_spawningAsteroid;
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
        enemyAsteroid.Initialize(moveLeft, AsteroidValues.moveSpeed, AsteroidValues.waveStrength, AsteroidValues.waveSpeed);
    }
    #endregion

    #region JELLYFISH SPAWN
    private Coroutine CheckToSpawnJellyfish()
    {
        if (spawningJellyfish)
            return co_spawningJellyfish;

        float currentTime = Time.time;

        co_spawningJellyfish = StartCoroutine(WaitToSpawnJellyfish(currentTime));

        return co_spawningJellyfish;
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
        jelly.Initialize(moveLeft, JellyfishValues.moveSpeed, JellyfishValues.waveStrength, JellyfishValues.waveSpeed);
    }
    #endregion

    #region NORMAL FISH SPAWN
    private Coroutine CheckToSpawnNormalFish()
    {
        if (spawningNormalFish)
            return co_spawningNormalFish;

        float currentTime = Time.time;

        co_spawningNormalFish = StartCoroutine(WaitToSpawnNormalFish(currentTime));

        return co_spawningNormalFish;
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
        baseFish.Initialize(moveLeft, DefaultFishValues.moveSpeed, DefaultFishValues.waveStrength, DefaultFishValues.waveSpeed);
    }
    #endregion

    #region ROCKET FISH SPAWN
    private Coroutine CheckToSpawnRocketFish()
    {
        if (spawningRocketFish)
            return co_spawningRocketFish;

        float currentTime = Time.time;

        co_spawningRocketFish = StartCoroutine(WaitToSpawnRocketFish(currentTime));

        return co_spawningRocketFish;
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
        baseFish.Initialize(moveLeft, RocketFishValues.moveSpeed, RocketFishValues.waveStrength, RocketFishValues.waveSpeed);
    }
    #endregion

    #region ROCK FISH SPAWN
    private Coroutine CheckToSpawnRockFish()
    {
        if (spawningRockFish)
            return co_spawningRockFish;

        float currentTime = Time.time;

        co_spawningRockFish = StartCoroutine(WaitToSpawnRockFish(currentTime));

        return co_spawningRockFish;
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
        float yPos = Random.Range(MAX_POS_TOP, MIN_POS_BOT); //choose y position to spawn

        GameObject obj = Instantiate(rockFish);
        obj.transform.position = new(moveLeft ? START_POS_RIGHT : START_POS_LEFT, yPos);
        BaseFish baseFish = obj.GetComponent<BaseFish>();
        baseFish.Initialize(moveLeft, RockFishValues.moveSpeed, RockFishValues.waveStrength, RockFishValues.waveSpeed);
    }
    #endregion

    private void SpawnRockFishTutorial()
    {
        GameObject obj = Instantiate(tutorial);
        obj.transform.position = new(13.94f , - 1.15f);
    }

    private enum FishType { normal, rocket, rock }

    private enum EnemyType { asteroid, jellyfish }

    #region ENTITY VALUES 
    public static class DefaultFishValues
    {
        public static float moveSpeed = 0.4f;

        public static float waveStrength = 0.3f;
        public static float waveSpeed = 3;
    }

    private static class RocketFishValues
    {
        public static float moveSpeed = 0.4f;

        public static float waveStrength = 0.3f;
        public static float waveSpeed = 1;

        public static float entryTime = 1;
        public static float rocketSpeed = 10;
    }

    public static class RockFishValues
    {
        public static float moveSpeed = 0.13f;

        public static float waveStrength = 0.2f;
        public static float waveSpeed = 2;
    }

    public static class AsteroidValues
    {
        public static float moveSpeed = 0.2f;

        public static float waveStrength = 0.3f;
        public static float waveSpeed = 5;
    }

    public static class JellyfishValues
    {
        public static float moveSpeed = 0.3f;

        public static float waveStrength = 0.3f;
        public static float waveSpeed = 1;
    }
    #endregion
}
