using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public GameObject obstacleContainer;
    public GameObject powerupContainer;
    public GameObject mainLine;
    public GameObject playerPrefab;
    public GameObject gameOverCanvas;
    public GameObject dialogueCanvas;
    public GameObject HUDCanvas;
    public GameObject stars;
    public GameObject staticMajor;
    public VideoPlayer generalEnding;

    public int numLivesStart;
    public int numPowerupsNeeded;
    public float phase1Dist, phase2Dist;
    public float powerupTime;
    public string[] introDialogue, halfwayDialogue;
    public bool[] introDialogueImportance, halfwayDialogueImportance;
    public Image[] liveSprites, checkpointFlags;
    public Vector3 playerSpawnLoc;
    public Sprite inactiveCheckpoint, activeCheckpoint;
    public Gradient secondPhaseColor;
    
    ObstacleSpawner mainObstacleSpawner;
    PowerupSpawner mainPowerupSpawner;
    LineController mainLineController;
    GameObject curPlayer;
    GameObject dialoguePanel;
    Text normalDialogue, importantDialogue;
    GameObject nextButton, goodButton;
    ParticleSystem starsPS;
    Slider powerupSlider, progressSlider;
    Text pressSpaceText;

    string[] curDialogue;
    bool[] curDialogueImportance;

    int numLives;
    int numPowerUps;
    int curCheckpoint;
    int curPhase = 0;
    int dialogueIndex;
    bool isProgressing = false;
    bool betweenPhases = false;
    float curProgress;
    float curPhaseDist;
    float distLastFrame;

    private void Awake()
    {
        instance = this;
        mainObstacleSpawner = obstacleContainer.GetComponent<ObstacleSpawner>();
        mainPowerupSpawner = powerupContainer.GetComponent<PowerupSpawner>();
        mainLineController = mainLine.GetComponent<LineController>();
        starsPS = stars.GetComponent<ParticleSystem>();

        powerupSlider = HUDCanvas.transform.Find("PowerupSlider").GetComponent<Slider>();
        progressSlider = HUDCanvas.transform.Find("ProgressSlider").GetComponent<Slider>();
        pressSpaceText = powerupSlider.transform.Find("PressSpaceText").GetComponent<Text>();
        pressSpaceText.text = "";

        dialoguePanel = dialogueCanvas.transform.Find("DialoguePanel").gameObject;
        normalDialogue = dialoguePanel.transform.Find("NormalDialogue").GetComponent<Text>();
        importantDialogue = dialoguePanel.transform.Find("ImportantDialogue").GetComponent<Text>();
        nextButton = dialoguePanel.transform.Find("NextButton").gameObject;
        goodButton = dialoguePanel.transform.Find("GoodButton").gameObject;
    }

    private void Start()
    {
        numLives = numLivesStart;
        gameOverCanvas.SetActive(false);
        dialogueCanvas.SetActive(false);
        HUDCanvas.SetActive(false);
        staticMajor.SetActive(false);
        powerupSlider.maxValue = numPowerupsNeeded;
        SpawnInitialPlayer();
        BeginIntroDialogue();
        //generalEnding.url = "https://youtu.be/KS3KwGXJD_8";
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.N))
        {
            BeginFirstPhase();
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnInitialPlayer();
            BeginIntroDialogue();
        }
        */

        if(Input.GetKeyDown(KeyCode.Space) && !betweenPhases && powerupSlider.value >= numPowerupsNeeded && curPlayer != null)
        {
            Debug.Log("Using Powerup!", gameObject);
            pressSpaceText.text = "";
            curPlayer.GetComponent<PlayerController>().UsePowerup(powerupTime);
            mainLineController.UsePowerup(powerupTime);
            numPowerUps = 0;
            powerupSlider.value = 0;
        }
        if (isProgressing)
        {
            float progressDelta = mainLine.transform.position.x - distLastFrame;
            curProgress += progressDelta;
            distLastFrame = mainLine.transform.position.x;
            float progressPct = curProgress / curPhaseDist;
            progressSlider.value = progressPct;
            if(curCheckpoint == 0 && progressPct >= 0.33f)
            {
                Debug.Log("first checkpoint reached", gameObject);
                curCheckpoint = 1;
                checkpointFlags[0].sprite = activeCheckpoint;
            }
            else if(curCheckpoint == 1 && progressPct >= 0.66f)
            {
                Debug.Log("second checkpoint reached", gameObject);
                curCheckpoint = 2;
                checkpointFlags[1].sprite = activeCheckpoint;
            }
            else if(curCheckpoint == 2 && progressPct >= 1f)
            {

                Debug.Log("end reached", gameObject);
                if(curPhase == 1)
                {
                    EndFirstPhase();
                }
                else if(curPhase == 2)
                {
                    EndSecondPhase();
                }
                else
                {
                    Debug.LogError("Error ending unknown phase!");
                }
            }
        }
    }

    void BeginIntroDialogue()
    {
        Debug.Log("Beginning Intro Dialogue", gameObject);
        dialogueCanvas.SetActive(true);
        curDialogue = introDialogue;
        curDialogueImportance = introDialogueImportance;
        dialogueIndex = 0;
        normalDialogue.text = curDialogue[dialogueIndex];
        importantDialogue.text = "";
        goodButton.SetActive(false);
    }

    public void BeginFirstPhase()
    {
        Debug.Log("Beginning first phase", gameObject);
        mainObstacleSpawner.BeginSpawningObstacles();
        mainPowerupSpawner.BeginSpawningPowerups();
        mainLineController.GivePlayerControl();
        isProgressing = true;
        curPhaseDist = phase1Dist;
        distLastFrame = mainLine.transform.position.x;
        curCheckpoint = 0;
        curPhase = 1;
        HUDCanvas.SetActive(true);
        dialogueCanvas.SetActive(false);
    }

    void EndFirstPhase()
    {
        Debug.Log("First Phase Complete!", gameObject);
        isProgressing = false;
        mainObstacleSpawner.StopSpawningObstacles();
        mainPowerupSpawner.StopSpawningPowerups();
        
        betweenPhases = true;
        Invoke("BeginHalfwayDialogue", 5f);
    }

    void BeginHalfwayDialogue()
    {
        Debug.Log("Beginning Halfway Dialogue", gameObject);
        mainLineController.MoveLineTo(0f);
        HUDCanvas.SetActive(false);
        dialogueCanvas.SetActive(true);
        curDialogue = halfwayDialogue;
        curDialogueImportance = halfwayDialogueImportance;
        dialogueIndex = 0;
        normalDialogue.text = curDialogue[dialogueIndex];
        importantDialogue.text = "";
        goodButton.SetActive(false);
        nextButton.SetActive(true);
    }

    public void BeginSecondPhase()
    {
        Debug.Log("Beginning Second Phase");
        mainObstacleSpawner.BeginSpawningObstacles();
        mainPowerupSpawner.BeginSpawningPowerups();
        mainLineController.GivePlayerControl();
        checkpointFlags[0].sprite = inactiveCheckpoint;
        checkpointFlags[1].sprite = inactiveCheckpoint;
        isProgressing = true;
        betweenPhases = false;
        curPhaseDist = phase2Dist;
        distLastFrame = mainLine.transform.position.x;
        curCheckpoint = 0;
        curPhase = 2;
        curProgress = 0;
        HUDCanvas.SetActive(true);
        dialogueCanvas.SetActive(false);
        mainLineController.StartLine(2);
        mainLine.GetComponent<TrailRenderer>().colorGradient = secondPhaseColor;
    }

    public void EndSecondPhase()
    {
        Debug.Log("Ending Second Phase", gameObject);
        isProgressing = false;
        staticMajor.SetActive(true);
        curPlayer.GetComponent<PlayerController>().MakeInvincible(100f);
        Invoke("ShowEnding", 5f);
    }

    void ShowEnding()
    {
        generalEnding.Play();
        HUDCanvas.SetActive(false);
        stars.SetActive(false);
        mainLine.SetActive(false);
        staticMajor.SetActive(false);
        curPlayer.SetActive(false);
        obstacleContainer.SetActive(false);
        powerupContainer.SetActive(false);
        generalEnding.loopPointReached += VideoOver;
    }

    void VideoOver(VideoPlayer source)
    {
        Debug.Log("video over!");
        SceneManager.LoadScene("MainMenu");
    }

    public void CollectPowerUp()
    {
        numPowerUps++;
        if(numPowerUps >= numPowerupsNeeded) {
            numPowerUps = numPowerupsNeeded;
            pressSpaceText.text = "PRESS SPACE BAR!";
        }
        powerupSlider.value = numPowerUps;
    }

    public void LifeLost()
    {
        numLives--;
        isProgressing = false;
        if (numLives < 0)
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
        if (!betweenPhases)
        {
            Invoke("ResumeProgress", 3f);
        }
    }

    void SpawnPlayer()
    {
        Vector3 newSpawnLoc = mainLine.transform.position + playerSpawnLoc;
        curPlayer = Instantiate(playerPrefab, newSpawnLoc, Quaternion.identity);
        curPlayer.GetComponent<PlayerController>().MakeInvincible(4f);
    }

    void SpawnInitialPlayer()
    {
        Vector3 newSpawnLoc = mainLine.transform.position + playerSpawnLoc;
        curPlayer = Instantiate(playerPrefab, newSpawnLoc, Quaternion.identity);
    }

    void ResumeProgress()
    {
        isProgressing = true;
        distLastFrame = mainLine.transform.position.x;
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
        numPowerUps = 0;
        powerupSlider.value = 0;
        pressSpaceText.text = "";
        gameOverCanvas.SetActive(false);
        //mainLineController.StartLine(1);
        curProgress = (curCheckpoint / 3f) * curPhaseDist;
        Debug.Log("continuing at: " + curProgress + " when curCheckpoint is: " + curCheckpoint + " and curPD is: " + curPhaseDist);
        progressSlider.value = curProgress / curPhaseDist;
        SpawnPlayer();
        ResumeProgress();
    }

    public void OnButtonNext()
    {
        dialogueIndex++;
        if(dialogueIndex >= curDialogue.Length - 1)
        {
            nextButton.SetActive(false);
            goodButton.SetActive(true);
        }
        if (curDialogueImportance[dialogueIndex])
        {
            importantDialogue.text = curDialogue[dialogueIndex];
            normalDialogue.text = "";
        }
        else
        {
            normalDialogue.text = curDialogue[dialogueIndex];
            importantDialogue.text = "";
        }
    }

    public void OnButtonGood()
    {
        if (curPhase == 0)
        {
            BeginFirstPhase();
        }
        else if(curPhase == 1)
        {
            BeginSecondPhase();
        }
        else
        {
            Debug.LogError("Error invalid phase", gameObject);
        }
    }
    
}
