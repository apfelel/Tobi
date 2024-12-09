using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoSingleton<GameManager>
{
    public List<CardScriptableObject> droppableCards;
    [Space] public CatchMiniGame catchMiniGame;
    
    private List<CardScriptableObject> _catchedFish = new();
    private List<CardScriptableObject> _curCardPack = new();

    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Transform _cardParent;
    [SerializeField] private GameObject _closedPack;
    
    [SerializeField] private float _randomRange;
    private void Start()
    {
        CalculateRandomRange();
        _closedPack.SetActive(false);
    }
    [ContextMenu("ManualCalculation")]
    private void CalculateRandomRange()
    {
        _randomRange = 0;
        droppableCards.ForEach((c) => _randomRange += c.dropChance);
    }
    public void GeneratePack(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            float randomFloat = Random.Range(0, _randomRange);
            var randomCard = droppableCards.FirstOrDefault((c) =>
            {
                randomFloat -= c.dropChance;
                return randomFloat < 0;
            });

            if (!randomCard)
                randomCard = droppableCards.Last();
            
            _curCardPack.Add(randomCard);
            _catchedFish.Add(randomCard);
        }
        ShowClosedPack();
    }

    public void OpenPack()
    {
        _closedPack.SetActive(false);

        foreach (var card in _curCardPack)
        {
            var Card = Instantiate(_cardPrefab, _cardParent).GetComponent<CardImage>();
            Card.Initialize(card);
        }
    }

    public void ShowClosedPack()
    {
        _closedPack.SetActive(true);
    }
    public void StartMiniGame()
    {
        catchMiniGame.gameObject.SetActive(true);
    }
}
