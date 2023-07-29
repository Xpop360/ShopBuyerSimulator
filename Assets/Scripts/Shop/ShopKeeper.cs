using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    [SerializeField] private List<ClothingPiece> AvailableItems = new List<ClothingPiece>();

    [SerializeField] private ShopUIController _shopUIController;

    public void OpenShop(PlayerInventory inventory)
    {
        if (_shopUIController == null) return;
        if (AvailableItems == null) return;
        if (AvailableItems.Count == 0) return;

        _shopUIController.Open(AvailableItems, inventory);
    }
}
