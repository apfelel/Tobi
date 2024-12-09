using System;
using TMPro;
using UnityEngine;

public class CardCollection : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardParent;
    [SerializeField] private TextMeshProUGUI curPageDescription;

    
    private int _curRarity;
    private void OnEnable()
    {
        _curRarity = 0;
        Refresh();
    }

    public void Refresh()
    {
        foreach (Transform card in cardParent)
        {
            Destroy(card.gameObject);
        }
        var cardSlots = GameManager.Instance.GetSortedCollectedCardSlots();

        for (int i = 0; i < cardSlots.GetLongLength(0); i++)
        {
            var cardUI = Instantiate(cardPrefab, cardParent).GetComponent<CardImage>();
            cardUI.Initialize(cardSlots[i, _curRarity]);
        }

        switch (_curRarity)
        {
            case 0:
                curPageDescription.text = "Normal";
                break;
            case 1:
                curPageDescription.text = "Holo";
                break;
            case 2:
                curPageDescription.text = "Shiny";
                break;
            case 3:
                curPageDescription.text = "Holo + Shiny";
                break;
        }
    }

    public void ChangeRarity(int change)
    {
        _curRarity += change;
        Debug.Log(_curRarity);
        _curRarity = Mathf.Clamp(_curRarity, 0, GameManager.Instance.maxUniqueCombinations - 1);
        Debug.Log(_curRarity);
        Refresh();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
