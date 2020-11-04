using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreenScript : MonoBehaviour
{
    public Text lostText;
    public Text roundText;
    public Text moneyText;

    public Button menuButton;

    private void Start()
    {
        //lostText.text = "";
        //roundText.text = "";
        //moneyText.text = "";

        //menuButton.gameObject.SetActive(false);
        menuButton.onClick.AddListener(delegate { GoToMenu(); });
    }

    public void Display(int round, int moneyEarned, int moneySpent)
    {
        Debug.Log("Display lose screen");

        lostText.text = "GAME OVER";
        roundText.text = "You made it to Round " + round;
        moneyText.text = "You earned $" + moneyEarned + " and spent $" + moneySpent;

        menuButton.gameObject.SetActive(true);
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
