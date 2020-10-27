using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalUIController : MonoBehaviour
{
    public Text currencyText;

    public ShopItem healthRegen;
    public ShopItem ammoRefill;
    public ShopItem itemOne;
    public ShopItem itemTwo;
    public ShopItem itemThree;

    public Button closeTerminalButton;

    private TerminalScript terminalScript;
    private PlayerController player;
    private ArrayList items;


    void Awake()
    {
        terminalScript = FindObjectOfType<TerminalScript>();
        player = FindObjectOfType<PlayerController>();

        healthRegen.Initialize("Health Regen", 500);
        ammoRefill.Initialize("Buy Ammo", 300);
        itemOne.Initialize("TBD One", 10000);
        itemTwo.Initialize("TBD Two", 10000);
        itemThree.Initialize("TBD Three", 10000);

        items = new ArrayList();

        items.Add(healthRegen);
        items.Add(ammoRefill);
        items.Add(itemOne);
        items.Add(itemTwo);
        items.Add(itemThree);

        healthRegen.button.onClick.AddListener(delegate { terminalScript.RegenerateHealth(healthRegen.price); });
        ammoRefill.button.onClick.AddListener(delegate { terminalScript.RefillAmmo(ammoRefill.price); });
        closeTerminalButton.onClick.AddListener(delegate { CloseTerminal(); });
    }

    // Update is called once per frame
    void Update()
    {
        currencyText.text = "$ " + player.GetPlayerCurrency();
    }


    public void UpdatePricing()
    {

        foreach(ShopItem item in items)
        {
            if(player.GetPlayerCurrency() < item.price)
            {
                item.button.interactable = false;
            }
            else
            {
                item.button.interactable = true;
            }
        }

        if(player.GetPlayerHealth() == 100)
        {
            healthRegen.button.interactable = false;
        }
    }

    private void CloseTerminal()
    {
        terminalScript.CloseTerminal();
    }
}
