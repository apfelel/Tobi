using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardImage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name, _length;
    [SerializeField] private Image _icon, _background;
    [Header("SmallOnly")]
    [SerializeField] private TextMeshProUGUI _amount;
    [Header("BigOnly")]
    [SerializeField] private TextMeshProUGUI _unique;
    [SerializeField] private GameObject _isNew, _isBest ;
    [SerializeField] private GameObject _rarity;
    public void Initialize(Card card, bool isNew= false, bool isBest = false)
    {
        _name.text = card.baseCardInfo.cardName;
        _rarity.transform.GetChild((int)card.baseCardInfo.rarity).gameObject.SetActive(true);
        _icon.sprite = card.baseCardInfo.icon;
        _background.sprite = card.baseCardInfo.background;
        _unique.text = card.uniqueTypes.ToString();
        _length.text = card.length.ToString("F1");

        _background.material = new Material(_background.material);
        _icon.material = new Material(_icon.material);
        
        if (card.uniqueTypes.HasFlag(CardEnums.UniqueTypes.Holo))
        {
            _background.material.SetFloat("_Holo", 1);
        }
        else
        {
            _background.material.SetFloat("_Holo", 0);
        }

        if (card.uniqueTypes.HasFlag(CardEnums.UniqueTypes.Shiny))
        {
            _icon.material.SetFloat("_Shiny", 0);
        }
        else
        {
            _icon.material.SetFloat("_Shiny", 1);
        }

        _isNew.gameObject.SetActive(isNew);
        _isBest.gameObject.SetActive(isBest);
    }
    public void Initialize(CardSlot card)
    {
        if (card == null || card.CardAmount == 0)
        {
            _background.color = Color.black;
            _icon.color = Color.black;
            return;
        }
        _name.text = card.BestCard.baseCardInfo.cardName;
        _rarity.transform.GetChild((int)card.BestCard.baseCardInfo.rarity).gameObject.SetActive(true);
        _icon.sprite = card.BestCard.baseCardInfo.icon;
        _background.sprite = card.BestCard.baseCardInfo.background;
        _length.text = card.BestCard.length.ToString("F1");
        _amount.text = card.CardAmount.ToString();
        
        _background.material = new Material(_background.material);
        _icon.material = new Material(_icon.material);
        
        if (card.BestCard.uniqueTypes.HasFlag(CardEnums.UniqueTypes.Holo))
        {
            _background.material.SetFloat("_Holo", 1);
        }
        else
        {
            _background.material.SetFloat("_Holo", 0);
        }

        if (card.BestCard.uniqueTypes.HasFlag(CardEnums.UniqueTypes.Shiny))
        {
            _icon.material.SetFloat("_Shiny", 0);
        }
        else
        {
            _icon.material.SetFloat("_Shiny", 1);
        }
    }
}
