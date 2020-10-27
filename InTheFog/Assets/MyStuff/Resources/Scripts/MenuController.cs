using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Text gameTitle;
    public Button playButton;
    public Button controlsButton;
    public Button quitButton;

    public GameObject controlsPanel;
    public Button closeOptionsButton;

    public Button forestButton;
    public Button townButton;
    public Button thirdButton;
    public Button menuButton;

    private ArrayList mainElements;     //elements of canvas for main menu
    private ArrayList levelElements;    //elements of canvas for level select

    private MusicController musicController;

    void Awake()
    {
        mainElements = new ArrayList();
        mainElements.Add(gameTitle.gameObject);
        mainElements.Add(playButton.gameObject);
        mainElements.Add(controlsButton.gameObject);
        mainElements.Add(quitButton.gameObject);

        levelElements = new ArrayList();
        levelElements.Add(forestButton.gameObject);
        levelElements.Add(townButton.gameObject);
        levelElements.Add(thirdButton.gameObject);
        levelElements.Add(menuButton.gameObject);

        playButton.onClick.AddListener(delegate { PlayGame(); });
        controlsButton.onClick.AddListener(delegate { DisplayControls(); });
        quitButton.onClick.AddListener(delegate { QuitGame(); });
        closeOptionsButton.onClick.AddListener(delegate { CloseControls(); });
        forestButton.onClick.AddListener(delegate { LoadForest(); });
        townButton.onClick.AddListener(delegate { LoadTown(); });
        thirdButton.onClick.AddListener(delegate { LoadThird(); });
        menuButton.onClick.AddListener(delegate { ReturnToMenu(); });
    }

    private void Start()
    {
        musicController = FindObjectOfType<MusicController>();
        foreach (GameObject o in levelElements)
        {
            o.SetActive(false);
        }

        foreach (GameObject o in mainElements)
        {
            o.SetActive(true);
        }

        controlsPanel.SetActive(false);
    }

    void PlayGame()
    {
        musicController.ButtonClick();

        foreach(GameObject o in mainElements)
        {
            o.SetActive(false);
        }

        foreach(GameObject o in levelElements)
        {
            o.SetActive(true);
        }
    }

    void DisplayControls()
    {
        musicController.ButtonClick();

        foreach (GameObject o in mainElements)
        {
            o.SetActive(false);
        }

        controlsPanel.SetActive(true);
    }

    void CloseControls()
    {
        musicController.ButtonClick();

        controlsPanel.SetActive(false);

        foreach(GameObject o in mainElements)
        {
            o.SetActive(true);
        }
    }

    void QuitGame()
    {
        musicController.ButtonClick();

        Debug.Log("Quit");
    }

    void LoadForest()
    {
        musicController.ButtonClick();

        SceneManager.LoadScene("ForestScene");
    }

    void LoadTown()
    {
        musicController.ButtonClick();

        Debug.Log("Not out yet");
    }

    void LoadThird()
    {
        musicController.ButtonClick();

        Debug.Log("Not out yet");
    }

    void ReturnToMenu()
    {
        musicController.ButtonClick();

        foreach (GameObject o in levelElements)
        {
            o.SetActive(false);
        }

        foreach (GameObject o in mainElements)
        {
            o.SetActive(true);
        }
    }
}
