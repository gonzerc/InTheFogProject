using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int numEnemies;
    public EnemySpawner enemySpawner;

    public static int roundNumber;
    public static bool gamePaused;
    public static bool terminalOpen;

    public Camera mainCam;
    public Vector3 deathCamPos;

    public Camera deathCam;

    public static bool gameOver;
    private Boolean roundRunning;
    private UIController uiController;

    void Awake()
    {
        uiController = FindObjectOfType<UIController>();

        roundNumber = 1;
        roundRunning = false;
        gamePaused = false;
        terminalOpen = false;

        gameOver = false;
    }


    private void Start()
    {
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        //while game is running
        if (!gameOver)
        {
            //start of round
            if (ZombieController.zombies.Count > 0)
            {
                roundRunning = true;
            }

            //end of round
            if (roundRunning && ZombieController.zombies.Count == 0)
            {
                //Debug.Log("Round over");
                roundRunning = false;
                roundNumber++;
                numEnemies = numEnemies + (numEnemies / 2);
                StartCoroutine(DelayBetweenRounds());
            }

            //Pause Game
            if (Input.GetKeyDown(KeyCode.Escape) && !terminalOpen)
            {
                PauseGame();
            }
        }
    }

    private IEnumerator StartGame()
    {
        uiController.DisplayMissionAssignment();
        yield return new WaitForSeconds(5f);
        StartCoroutine(enemySpawner.SpawnEnemies(numEnemies));
    }

    private void StartNextRound()
    {

        uiController.DisplayMissionAssignment();
        StartCoroutine(enemySpawner.SpawnEnemies(numEnemies));
    }

    private void PauseGame()
    {
        gamePaused = true;
        uiController.PauseGame();
        Enemy.PauseGame();
        Time.timeScale = 0;
        //Debug.Log("Paused: " + Time.timeScale);
    }

    public static void ContinueGame()
    {
        Time.timeScale = 1f;
        //Debug.Log("Continue: " + Time.timeScale);
        gamePaused = false;
        Enemy.ContinueGame();
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        StartCoroutine(EndGame(false));
        //SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator DelayBetweenRounds()
    {
        float timer = 30f;
        while (timer > 0f)
        {
            uiController.DisplayPrepareMessage(timer);
            yield return new WaitForSeconds(1f);
            timer -= 1;
        }
        StartNextRound();
    }

    public IEnumerator EndGame(bool playerDeath)
    {
        Debug.Log("Game Ended.");
        gameOver = true;

        ZombieController.QuitGame();
        FindObjectOfType<PlayerController>().enabled = false;
        FindObjectOfType<MusicController>().PlayGameOver();

        if (playerDeath)
        {
            StartCoroutine(uiController.FadeToDeath(new Color(0.1f, 0.0f, 0.0f)));

            yield return new WaitForSeconds(5f);

        }
        uiController.gameObject.SetActive(false);

        deathCam.gameObject.SetActive(true);
        mainCam.GetComponent<AudioListener>().enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        deathCam.GetComponentInChildren<LoseScreenScript>().Display(roundNumber, FindObjectOfType<PlayerController>().GetMoneyEarned(), FindObjectOfType<PlayerController>().GetMoneySpent());

    }
}
