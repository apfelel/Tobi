using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardImage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name, _rarity, _length;
    [SerializeField] private Image _icon, _background;
    [Header("SmallOnly")]
    [SerializeField] private TextMeshProUGUI _amount;

    [Header("BigOnly")]
    [SerializeField] private TextMeshProUGUI _unique;
    
    public void Initialize(Card card)
    {
        _name.text = card.baseCardInfo.cardName;
        _rarity.text = card.baseCardInfo.rarity.ToString();
        _icon.sprite = card.baseCardInfo.icon;
        _background.sprite = card.baseCardInfo.background;
        _unique.text = card.uniqueTypes.ToString();
        _length.text = card.length.ToString("F1");
    }
    public void Initialize(CardSlot card)
    {
        if (card == null)
        {
            _background.color = Color.black;
            return;
        }
        _name.text = card.BestCard.baseCardInfo.cardName;
        _rarity.text = card.BestCard.baseCardInfo.rarity.ToString();
        _icon.sprite = card.BestCard.baseCardInfo.icon;
        _background.sprite = card.BestCard.baseCardInfo.background;
        _length.text = card.BestCard.length.ToString("F1");
        _amount.text = card.CardAmount.ToString();
    }
}
