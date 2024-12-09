using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Card", menuName = "New Card", order = 1)]
public class CardScriptableObject : ScriptableObject
{
    public string cardName;
    public CardEnums.Rarities rarity;
    public float dropChance;

    public Sprite background;
    public Sprite icon;

    public float minLength, maxLength;
    
}
