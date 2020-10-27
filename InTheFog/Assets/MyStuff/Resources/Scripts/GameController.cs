using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int numEnemies;
    public EnemySpawner enemySpawner;

    public static int roundNumber;
    public static bool gamePaused;
    public static bool terminalOpen;


    private Boolean roundRunning;
    private UIController uiController;


    // Start is called before the first frame update
    void Awake()
    {
        uiController = FindObjectOfType<UIController>();

        roundNumber = 1;
        roundRunning = false;
        gamePaused = false;
        terminalOpen = false;
    }


    private void Start()
    {
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        //start of round
        if (Enemy.enemies.Count > 0)
        {
            roundRunning = true;
        }

        //end of round
        if (roundRunning && Enemy.enemies.Count == 0)
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
}
