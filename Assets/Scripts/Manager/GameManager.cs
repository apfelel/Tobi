using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoSingleton<GameManager>
{
    public List<CardScriptableObject> droppableCards;
    [Space] public CatchMiniGame catchMiniGame;

    [SerializeField] private float _randomRange;
    private void Start()
    {
        
    }
    [ContextMenu("ManualCalculation")]
    private void CalculateRandomRange()
    {
        _randomRange = 0;
        droppableCards.ForEach((c) => _randomRange += c.dropChance);
    }
    public List<CardScriptableObject> GetCards(int amount)
    {
        List<CardScriptableObject> cardPack = new();
        
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
            
            cardPack.Add(randomCard);
        }

        return cardPack;
    }

    public void StartMiniGame()
    {
        catchMiniGame.gameObject.SetActive(true);
    }
}
