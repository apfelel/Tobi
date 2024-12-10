using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoSingleton<GameManager>
{
    public List<CardScriptableObject> droppableCards;
    [Space] public CatchMiniGame catchMiniGame;
    
    //private List<Card> _collectedCards = new();
    private List<Card> _curCardPack = new();
    private CardSlot[,] _sortedCollectedCardSlots;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Transform _cardParent;
    [SerializeField] private GameObject _closedPack;
    [SerializeField] private float _randomRange;
    [SerializeField] private float _qualityComboMultiplier = 0.03f;
    
    [SerializeField] private FishingRod _fishingRod;

    public int maxUniqueCombinations = 4;
    private void Start()
    {
        CalculateRandomRange();
        _closedPack.SetActive(false);
        _sortedCollectedCardSlots = new CardSlot[droppableCards.Count, maxUniqueCombinations];
        _sortedCollectedCardSlots = DataPersistenceManager.Instance.LoadGame();

    }
    [ContextMenu("ManualCalculation")]
    private void CalculateRandomRange()
    {
        _randomRange = 0;
        droppableCards.ForEach((c) => _randomRange += c.dropChance);
    }
    public void GeneratePack(int amount)
    {
        _fishingRod.MiniGameFinished();
        
        int qualityIncrease = 0;
        if (amount >= 5)
        {
            qualityIncrease = amount - 5;
            amount = 5;
            Debug.Log(qualityIncrease);
        }
        for (int i = 0; i < amount; i++)
        {
            float randomFloat = Random.Range(0, _randomRange);
            Debug.Log(randomFloat);
            randomFloat = Mathf.Pow(randomFloat / _randomRange, Mathf.Clamp01(1 - _qualityComboMultiplier * qualityIncrease)) * _randomRange;
            Debug.Log(randomFloat);
            
            var randomCardSO = droppableCards.FirstOrDefault((c) =>
            {
                randomFloat -= c.dropChance;
                return randomFloat < 0;
            });

            if (!randomCardSO)
                randomCardSO = droppableCards.Last();

            var randomCard = new Card(randomCardSO); 
            _curCardPack.Add(randomCard);
            
            var cardSlot = _sortedCollectedCardSlots[droppableCards.IndexOf(randomCardSO), randomCard.rarityIndex] ??= new CardSlot(randomCard);
            cardSlot.CardAmount += 1;
            
            if (cardSlot.BestCard.length < randomCard.length)
                cardSlot.BestCard = randomCard;
            
            //_collectedCards.Add(new Card(randomCardSO));
        }
        ShowClosedPack();
    }

    public void StashPack()
    {
        _curCardPack.Clear();
        foreach (Transform card in _cardParent)
        {
            Destroy(card.gameObject);
        }
    }
    public bool OpenPack()
    {
        if (!_closedPack.activeInHierarchy) return false;
        _closedPack.SetActive(false);

        foreach (var card in _curCardPack)
        {
            var cardImage = Instantiate(_cardPrefab, _cardParent).GetComponent<CardImage>();
            cardImage.Initialize(card);
        }
        return true;
    }

    public void ShowClosedPack()
    {
        _closedPack.SetActive(true);
    }
    public void StartMiniGame()
    {
        catchMiniGame.gameObject.SetActive(true);
    }

    public CardSlot[,] GetSortedCollectedCardSlots()
    {
        return _sortedCollectedCardSlots;
    }
}
