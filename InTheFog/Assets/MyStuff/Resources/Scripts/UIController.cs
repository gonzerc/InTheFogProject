using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public RawImage healthBorderImage;
    public RawImage deathImage;
    public Text roundNumberText;
    public Text enemiesText;
    public Slider healthSlider;
    public Image healthFill;
    public Slider staminaSlider;
    public Text currencyText;
    public Text gameMessageText;
    public RawImage minimap;

    //in game only elements
    public Image crossHairsImage;
    public Image hitMarkerImage;
    public Text bulletsText;
    public Slider reloadBar;
    public Text reloadHintText;
    public Text detectionText;
    public GameObject interactText;

    //pause screen elements
    public Image pauseBackground;
    public Button pauseText;
    public Button continueButton;
    public Button controlsButton;
    public Button quitButton;
    public GameObject controlsPanel;
    public Button closeOptionsButton;

    public Color faded;
    public Color whole;
    private Color healthColor;
    public float fadeInTime;
    private bool textFaded;
    private bool mapOpen;
    private bool crosshairOn;
    private PlayerController player;
    private ArrayList inGameElements;
    private ArrayList pauseElements;

    private MusicController musicController;
    
    void Awake()
    {
        healthBorderImage.transform.SetAsFirstSibling();
        gameMessageText.transform.SetAsLastSibling();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        healthBorderImage.color = faded;
        deathImage.color = faded;
        gameMessageText.text = "";
        gameMessageText.color = faded;
        textFaded = true;
        mapOpen = false;
        crosshairOn = true;
        reloadHintText.text = "";
        detectionText.text = "";

        inGameElements = new ArrayList();
        pauseElements = new ArrayList();

        inGameElements.Add(crossHairsImage.gameObject);
        inGameElements.Add(bulletsText.gameObject);
        inGameElements.Add(reloadHintText.gameObject);
        inGameElements.Add(detectionText.gameObject);
        pauseElements.Add(pauseText.gameObject);
        pauseElements.Add(continueButton.gameObject);
        pauseElements.Add(controlsButton.gameObject);
        pauseElements.Add(quitButton.gameObject);

        //assign buttons functions
        continueButton.onClick.AddListener(delegate { ContinueGame(); });
        controlsButton.onClick.AddListener(delegate { DisplayControls(); });
        closeOptionsButton.onClick.AddListener(delegate { CloseControls(); });
        quitButton.onClick.AddListener(delegate { QuitGame(); });
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        musicController = FindObjectOfType<MusicController>();
        interactText.gameObject.SetActive(false);
        reloadBar.gameObject.SetActive(false);
        minimap.gameObject.SetActive(false);

        //activate in game elements
        foreach(GameObject o in inGameElements)
        {
            o.SetActive(true);
        }

        //deactivate pause elements
        foreach(GameObject o in pauseElements)
        {
            o.SetActive(false);
        }

        hitMarkerImage.gameObject.SetActive(false);
        pauseBackground.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        roundNumberText.text = "Round " + GameController.roundNumber;
        enemiesText.text = "Zombies Remaining: " + ZombieController.zombies.Count;
        
        
        healthSlider.value = player.GetPlayerHealth();
        healthSlider.GetComponentInChildren<Text>().text = player.GetPlayerHealth().ToString();
        
        staminaSlider.value = player.GetPlayerStamina();
        staminaSlider.GetComponentInChildren<Text>().text = player.GetPlayerStamina().ToString();
        
        
        bulletsText.text = player.bulletsInMag + " / " + player.ammoRemaining;

        healthColor = new Color(1.0f, 1.0f, 1.0f, (100 - healthSlider.value) / 100);
        healthBorderImage.color = healthColor;

        if (Input.GetKeyDown(KeyCode.M))
        {
            TriggerMap();
        }

        if(healthSlider.value < 20)
        {
            StartCoroutine(BlinkHealthBar());
        }

        currencyText.text = "$" + player.GetPlayerCurrency();
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ChangeInGameElements(false);
        ChangePauseElements(true);
        pauseBackground.gameObject.SetActive(true);
    }

    // ========================= Button Listeners ================================
    public void ContinueGame()
    {
        musicController.ButtonClick();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        ChangeInGameElements(true);
        ChangePauseElements(false);
        pauseBackground.gameObject.SetActive(false);
        GameController.ContinueGame();
    }

    private void DisplayControls()
    {
        musicController.ButtonClick();

        ChangePauseElements(false);

        controlsPanel.SetActive(true);
    }

    private void CloseControls()
    {
        musicController.ButtonClick();

        controlsPanel.SetActive(false);

        ChangePauseElements(true);
    }

    public void QuitGame()
    {
        musicController.ButtonClick();

        FindObjectOfType<GameController>().QuitGame();
    }

    // ===============================================================
    public void DisplayMissionAssignment()
    {
        //Debug.Log("Display Mission");
        gameMessageText.text = "Survive the zombies\nRound " + GameController.roundNumber;
        StartCoroutine(FadeText(whole));
    }

    public void DisplayPrepareMessage(float timer)
    {
        gameMessageText.text = "Prepare for the next round" + "\n" + timer;
        StartCoroutine(FadeText(whole));
    }

    public void DisplayReloadMessage(string message)
    {
        reloadHintText.text = message;
    }

    public void SetPlayerDetectedText(string status)
    {
        detectionText.text = status;
    }

    private void TriggerMap()
    {
        mapOpen = !mapOpen;
        minimap.gameObject.SetActive(mapOpen);
    }

    public IEnumerator ReloadAnimation(float duration)
    {
        reloadHintText.text = "";
        reloadBar.gameObject.SetActive(true);
        reloadBar.value = 0;

        float reloadTime = 0.0f;
        while(reloadTime < duration)
        {
            reloadTime += Time.deltaTime;
            float lerpValue = reloadTime / duration;
            reloadBar.value = Mathf.Lerp(0.0f, 1.0f, lerpValue);
            yield return null;
        }
        reloadBar.gameObject.SetActive(false);
    }

    private IEnumerator FadeText(Color endColor)
    {
        float time = 0.0f;
        Color startColor = gameMessageText.color;

        while (time < fadeInTime)
        {
            gameMessageText.color = Color.Lerp(startColor, endColor, time / fadeInTime);
            time += Time.deltaTime;
            yield return null;
        }
        gameMessageText.color = endColor;
        textFaded = !textFaded;
        //Debug.Log(textFaded);

        yield return new WaitForSeconds(2.0f);

        if (!textFaded)
        {
            StartCoroutine(FadeText(faded));
        }
    }

    private IEnumerator BlinkHealthBar()
    {
        while (healthSlider.value < 25)
        {
            healthFill.color = Color.white;
            yield return new WaitForSeconds(0.25f);
            healthFill.color = Color.red;
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void DisplayInteractText()
    {
        interactText.SetActive(true);
    }
    public void HideInteractText()
    {
        interactText.SetActive(false);
    }


    public void ToggleADS()
    {
        crosshairOn = !crosshairOn;
        crossHairsImage.gameObject.SetActive(crosshairOn);
    }

    public IEnumerator DisplayHitMarker()
    {
        hitMarkerImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hitMarkerImage.gameObject.SetActive(false);
    }

    public IEnumerator FadeToDeath(Color endColor)
    {
        gameMessageText.text = "YOU DIED";
        gameMessageText.color = whole;
        float timer = 0f;
        float fadeTimer = 5f;
        Color startColor = deathImage.color;

        while(timer < fadeTimer)
        {
            deathImage.color = Color.Lerp(startColor, endColor, timer / fadeTimer);
            timer += Time.deltaTime;
            yield return null;
        }

        //StartCoroutine(FadeToDeath(Color.black));
    }

    public void ChangeInGameElements(bool b)
    {
        foreach (GameObject o in inGameElements)
        {
            o.SetActive(b);
        }
    }

    private void ChangePauseElements(bool b)
    {
        foreach (GameObject o in pauseElements)
        {
            o.SetActive(b);
        }
    }
}
