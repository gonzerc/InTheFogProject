using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Button button;
    public Text itemName;
    public Text priceText;
    public int price;

    public void Initialize(string name, int price)
    {
        this.itemName.text = name;
        this.price = price;
        this.priceText.text = "$ " + price;
    }
}
