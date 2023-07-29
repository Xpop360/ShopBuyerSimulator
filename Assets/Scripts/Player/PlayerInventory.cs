using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{ 
    [SerializeField] internal int munny;

    [Header("Inventory UI")]
    [SerializeField] private GameObject InventoryItem;
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private GameObject ItemPanel;

    [Header("Clothes")]
    [SerializeField] private SpriteRenderer clothesSprite;
    [SerializeField] private Animator clothesAnimator;

    [Header("Player UI")]
    [SerializeField] private Image PlayerClothesViewer;

    internal List<ClothingInInventory> boughtPieces = new List<ClothingInInventory>();

    private List<ItemController> ItemsCreated = new List<ItemController>();
    private ClothingInInventory currentlyEquiped = null;

    public void OpenInventory()
    {
        if (InventoryPanel == null) return;

        ItemsCreated = new List<ItemController>();

        foreach (ClothingInInventory clothes in boughtPieces)
        {
            var itemGO = Instantiate(InventoryItem);
            
            itemGO.transform.SetParent(ItemPanel.transform, false);           

            var item = itemGO.GetComponent<ItemController>();
            item.CreateItem(clothes.piece, false);

            ItemsCreated.Add(item);

            var button = itemGO.GetComponent<Button>();
            button.onClick.AddListener(delegate { EquipItem(clothes); });
        }

        InputManager.Instance.inputActions.UI.Back.started += CloseWindow;
        InputManager.Instance.inputActions.UI.CloseInventory.started += CloseWindow;

        InputManager.Instance.SwitchToUI();

        InventoryPanel.SetActive(true);
    }

    private void EquipItem(ClothingInInventory clothing)
    { 
        if (clothing.equiped)
        { 
            clothesSprite.enabled = false;
            clothesAnimator.runtimeAnimatorController = null;

            PlayerClothesViewer.sprite = null;
            PlayerClothesViewer.gameObject.SetActive(false);

            clothing.equiped = false;
        }
        else
        {
            if (currentlyEquiped != null)
            {
                if (currentlyEquiped.equiped)
                {
                    currentlyEquiped.equiped = false;
                }
            }                

            clothing.equiped = true;

            clothesSprite.enabled = true;
            clothesSprite.sprite = clothing.piece.IconToShow;
            clothesAnimator.runtimeAnimatorController = clothing.piece.ClothingAnimator;

            PlayerClothesViewer.sprite = clothing.piece.IconToShow;
            PlayerClothesViewer.gameObject.SetActive(true);

            currentlyEquiped = clothing;
        }         
    }

    private void CloseWindow(InputAction.CallbackContext obj)
    {
        CloseInventory();
    }

    private void CloseInventory()
    {
        InventoryPanel?.SetActive(false);

        if (ItemsCreated.Count > 0)
        {
            foreach (var item in ItemsCreated)
                Destroy(item.gameObject);
        }

        InputManager.Instance.inputActions.UI.Back.started -= CloseWindow;
        InputManager.Instance.inputActions.UI.CloseInventory.started -= CloseWindow;

        InputManager.Instance.SwitchToGameplay();
    }

    public bool IsItemEquiped(ClothingPiece piece)
    {
        foreach (var c in boughtPieces)
        {
            if (c.piece == piece)
            {
                return c.equiped;
            }
        }

        return false;
    }

    public bool BuyItemToInventory(ClothingPiece piece)
    {
        if(munny >= piece.PriceToBuy)
        {
            boughtPieces.Add(new ClothingInInventory(piece));
            munny -= piece.PriceToBuy;

            return true;
        }

        return false;
    }

    public void SellItemToInventory(ClothingPiece piece)
    {
        foreach(var c in boughtPieces)
        {
            if(c.piece == piece)
            {
                boughtPieces.Remove(c);

                munny += piece.PriceToSell;

                break;
            }
        }
    }
}

[Serializable]
public class ClothingInInventory
{
    public ClothingPiece piece;

    public bool equiped;

    public ClothingInInventory(ClothingPiece cp)
    {
        piece = cp;
    }
}
