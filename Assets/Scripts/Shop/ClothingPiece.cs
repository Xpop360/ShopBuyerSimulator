using UnityEngine;

[CreateAssetMenu(fileName = nameof(ClothingPiece),
    menuName = "ScriptableObjects/" + nameof(ClothingPiece))]
public class ClothingPiece : ScriptableObject
{
    public string ClothingName;
    public string Description;

    public int PriceToBuy;
    public int PriceToSell;

    public Sprite IconToShow;

    public RuntimeAnimatorController ClothingAnimator;
}
