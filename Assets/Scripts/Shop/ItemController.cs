using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemPrice;

    internal bool bought = false;

    internal ClothingPiece clothingPiece;

    internal GameObject ItemGO;

    public void CreateItem(ClothingPiece cp, bool ToBuy)
    {
        clothingPiece = cp;

        itemImage.sprite = cp.IconToShow;
        itemName.text = cp.ClothingName;

        ItemGO = gameObject;

        if(itemPrice != null)
        {
            if (bought)
                itemPrice.text = "SOLD!";
            else
                itemPrice.text = ToBuy ? cp.PriceToBuy.ToString() : cp.PriceToSell.ToString();
        }        
    }

    public void BoughtItem()
    {
        bought = true;

        itemPrice.text = "SOLD!";
    }

    public void SellItem()
    {
        bought = false;

        itemPrice.text = clothingPiece.PriceToBuy.ToString();
    }
}
