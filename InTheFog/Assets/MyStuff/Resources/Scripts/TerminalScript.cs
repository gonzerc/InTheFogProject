using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalScript : MonoBehaviour
{
    public Canvas mainCanvas;
    public Canvas terminalCanvas;
    public Camera mapCamera;

    private UIController uiController;
    private MusicController musicController;
    private PlayerController player;
    private bool canInteract;


    // Start is called before the first frame update
    void Start()
    {
        uiController = FindObjectOfType<UIController>();
        musicController = FindObjectOfType<MusicController>();
        player = FindObjectOfType<PlayerController>();
        terminalCanvas.gameObject.SetActive(false);
        canInteract = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleTerminal();
        }
    }


    private void ToggleTerminal()
    {
        if(canInteract && !GameController.terminalOpen)
        {
            OpenTerminal();
        }
        else if (GameController.terminalOpen)
        {
            CloseTerminal();
        }
    }

    private void OpenTerminal()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        mapCamera.cullingMask = (1 << LayerMask.NameToLayer("Level")) | (1 << LayerMask.NameToLayer("Minimap"));

        GameController.terminalOpen = true;

        uiController.ChangeInGameElements(false);
        terminalCanvas.gameObject.SetActive(true);
        terminalCanvas.GetComponent<TerminalUIController>().UpdatePricing();
    }

    public void CloseTerminal()
    {
        terminalCanvas.gameObject.SetActive(false);
        uiController.ChangeInGameElements(true);

        GameController.terminalOpen = false;

        mapCamera.cullingMask = (1 << LayerMask.NameToLayer("Level"));

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            uiController.DisplayInteractText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            uiController.HideInteractText();
        }
    }



    public void RegenerateHealth(int price)
    {
        player.HealPlayer(20);
        player.RemoveCurrency(price);
        musicController.PlayCash();
        terminalCanvas.GetComponent<TerminalUIController>().UpdatePricing();
    }

    public void RefillAmmo(int price)
    {
        player.RefillAmmo(20);
        player.RemoveCurrency(price);
        musicController.PlayCash();
        terminalCanvas.GetComponent<TerminalUIController>().UpdatePricing();
    }
}
