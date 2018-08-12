using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public GameObject obstacleContainer;
    public GameObject mainLine;
    public GameObject playerPrefab;
    public GameObject gameOverCanvas;
    public GameObject dialogueCanvas;
    public GameObject HUDCanvas;
    public GameObject stars;

    public int numLivesStart;
    public int numPowerupsNeeded;
    public Image[] liveSprites;
    public Vector3 playerSpawnLoc;

    ObstacleSpawner mainObstacleSpawner;
    LineController mainLineController;
    GameObject curPlayer;
    Text normalDialogue, importantDialogue;
    GameObject nextButton, goodButton;
    ParticleSystem starsPS;
    Slider powerupSlider, progressSlider;

    int numLives;
    int numPowerUps;
    
    private void Awake()
    {
        instance = this;
        mainObstacleSpawner = obstacleContainer.GetComponent<ObstacleSpawner>();
        mainLineController = mainLine.GetComponent<LineController>();
        starsPS = stars.GetComponent<ParticleSystem>();

        powerupSlider = HUDCanvas.transform.Find("PowerupSlider").GetComponent<Slider>();
        progressSlider = HUDCanvas.transform.Find("ProgressSlider").GetComponent<Slider>();
    }

    private void Start()
    {
        numLives = numLivesStart;
        gameOverCanvas.SetActive(false);
        dialogueCanvas.SetActive(false);
        powerupSlider.maxValue = numPowerupsNeeded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            BeginFirstPhase();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnPlayer();
        }
    }

    public void BeginFirstPhase()
    {
        mainObstacleSpawner.BeginSpawningObstacles();
    }

    public void CollectPowerUp()
    {
        numPowerUps++;
        if(numPowerUps > numPowerupsNeeded) { numPowerUps = numPowerupsNeeded; }
        powerupSlider.value = numPowerUps;
    }

    public void LifeLost()
    {
        numLives--;
        if(numLives < 0)
        {
            //EndGame
            Destroy(curPlayer);
            gameOverCanvas.SetActive(true);
            return;
        }

        liveSprites[numLives].color = Color.clear;
        Destroy(curPlayer);
        //Maybe play a death effect here
        Invoke("SpawnPlayer", 3f);
    }

    void SpawnPlayer()
    {
        Vector3 newSpawnLoc = mainLine.transform.position + playerSpawnLoc;
        curPlayer = Instantiate(playerPrefab, newSpawnLoc, Quaternion.identity);
    }

    public void StartOver()
    {
        //Fade screen
        SceneManager.LoadScene("SpaceWalk");
    }

    public void Continue()
    {
        numLives = numLivesStart;
        for(int i = 0; i < numLives; i++)
        {
            liveSprites[i].color = Color.white;
        }
        gameOverCanvas.SetActive(false);
        mainLineController.StartLine(1);
        SpawnPlayer();
    }

    void OnButtonNext()
    {

    }

    void OnButtonGood()
    {

    }
    
}
