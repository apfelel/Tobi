using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardImage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name, _rarity;
    [SerializeField] private Image _icon, _background;
    
    public void Initialize(CardScriptableObject card)
    {
        _name.text = card.cardName;
        _rarity.text = card.rarity.ToString();
        _icon.sprite = card.icon;
        _background.sprite = card.background;
    }
}
