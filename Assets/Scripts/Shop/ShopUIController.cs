using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] private GameObject Item;

    [Header("UI")]
    [SerializeField] private GameObject ShopPanel;
    [SerializeField] private GameObject BuyPanel;
    [SerializeField] private GameObject SellPanel;
    [SerializeField] private TMP_Text LowerText;
    [SerializeField] private TMP_Text PlayerMunnyText;

    private PlayerInventory playerBuying;

    [SerializeField] private List<ItemController> ItemsOnShopToBuy = new List<ItemController>();
    [SerializeField] private List<ItemController> ItemsOnShopToSell = new List<ItemController>();
    
    public void Open(List<ClothingPiece> AvailableItems, PlayerInventory playerInventory)
    {
        if (Item == null) return;
        if (ShopPanel == null) return;
        if (BuyPanel == null) return;
        if (SellPanel == null) return;

        playerBuying = playerInventory;

        PlayerMunnyText.text = playerBuying.munny.ToString() + "$";

        GenerateItems(AvailableItems);
        GenerateItems(playerBuying.boughtPieces);        

        if(LowerText != null)
            LowerText.text = WELCOME_TEXT;

        InputManager.Instance.inputActions.UI.Back.started += CloseWindow;

        InputManager.Instance.SwitchToUI();

        ShopPanel?.SetActive(true);
        BuyPanel?.SetActive(true);
        SellPanel?.SetActive(false);
    }

    private void GenerateItems(List<ClothingPiece> pieces)
    {
        ItemsOnShopToBuy = new List<ItemController>();

        foreach (ClothingPiece cp in pieces)
        {
            var itemGO = Instantiate(Item);

            var item = itemGO.GetComponent<ItemController>();

            if (playerBuying.boughtPieces.Exists(x => x.piece == cp))
                item.bought = true;

            item.CreateItem(cp, true);
            
            itemGO.transform.SetParent(BuyPanel.transform, false);
            ItemsOnShopToBuy.Add(item);

            var button = itemGO.GetComponent<Button>();
            button.onClick.AddListener(delegate { BuyItem(item); });
        }
    }

    private void GenerateItems(List<ClothingInInventory> pieces)
    {
        ItemsOnShopToSell = new List<ItemController>();

        foreach (ClothingInInventory cp in pieces)
        {
            var itemGO = Instantiate(Item);

            var item = itemGO.GetComponent<ItemController>();
            item.CreateItem(cp.piece, false);

            itemGO.transform.SetParent(SellPanel.transform, false);
            ItemsOnShopToSell.Add(item);

            var button = itemGO.GetComponent<Button>();
            button.onClick.AddListener(delegate { SellItem(item); });
        }
    }

    private void BuyItem(ItemController itemC)
    {
        if (itemC.bought) 
        {
            if (LowerText != null)
                LowerText.text = ALREADY_BOUGHT_TEXT;

            return;
        }        

        if (playerBuying.BuyItemToInventory(itemC.clothingPiece))
        {
            itemC.BoughtItem();

            var itemGO = Instantiate(Item);

            var item = itemGO.GetComponent<ItemController>();
            item.CreateItem(itemC.clothingPiece, false);

            itemGO.transform.SetParent(SellPanel.transform, false);
            ItemsOnShopToSell.Add(item);

            var button = itemGO.GetComponent<Button>();
            button.onClick.AddListener(delegate { SellItem(item); });

            PlayerMunnyText.text = playerBuying.munny.ToString() + "$";

            if (LowerText != null)
                LowerText.text = BOUGHT_TEXT;
        }
        else
        {
            if (LowerText != null)
                LowerText.text = NO_MUNNY_TEXT;
        }
    }

    private void SellItem(ItemController itemC)
    {
        if(ItemsOnShopToSell == null) return;
        if(ItemsOnShopToSell.Count == 0) return;

        if (playerBuying.IsItemEquiped(itemC.clothingPiece))
        {
            if (LowerText != null)
                LowerText.text = EQUIPED_TEXT;

            return;
        }

        playerBuying.SellItemToInventory(itemC.clothingPiece);

        foreach (var item in ItemsOnShopToBuy)
        {
            if (item.clothingPiece == itemC.clothingPiece)
            {
                item.SellItem();

                var destroyGO = itemC.ItemGO;

                var index = ItemsOnShopToSell.IndexOf(item);

                ItemsOnShopToSell.Remove(item);
                Destroy(destroyGO);

                ItemsOnShopToSell = ItemsOnShopToSell.Where(x => x != null).ToList();

                index = Mathf.Clamp(index, 0, ItemsOnShopToSell.Count - 1);

                PlayerMunnyText.text = playerBuying.munny.ToString() + "$";

                if (LowerText != null)
                    LowerText.text = SOLD_TEXT;

                break;
            }
        };
    }

    private void CloseWindow(InputAction.CallbackContext obj)
    {
        CloseShop();
    }

    private void CloseShop()
    {
        ShopPanel?.SetActive(false);
        BuyPanel?.SetActive(false);
        SellPanel?.SetActive(false);

        foreach (var item in ItemsOnShopToBuy)
            Destroy(item.gameObject);
        
        foreach (var item in ItemsOnShopToSell)
            if(item != null)
                Destroy(item.gameObject);        

        InputManager.Instance.inputActions.UI.Back.started -= CloseWindow;

        InputManager.Instance.SwitchToGameplay();
    }

    #region TextForShop
    private const string WELCOME_TEXT = "Got some new clothes on sale, stranger!";
    private const string ALREADY_BOUGHT_TEXT = "You already bought this, stranger!";
    private const string BOUGHT_TEXT = "Nice purchase, stranger!";
    private const string SOLD_TEXT = "Thank you very much, stranger!";
    private const string NO_MUNNY_TEXT = "You don't have enough munny for that, stranger!";
    private const string EQUIPED_TEXT = "You have that piece equipped, stranger!";
    #endregion
}
