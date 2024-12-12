using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
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

    private HorizontalLayoutGroup _cardParentGroup;
    
    private bool _inOpenAnim;
    
    public int maxUniqueCombinations = 4;

    [ContextMenu("SortCards")]
    public void SortCardList()
    {
        droppableCards = droppableCards.OrderByDescending((x) => x.dropChance).ToList();
    }
    private void Start()
    {
        CalculateRandomRange();
        _closedPack.SetActive(false);
        _sortedCollectedCardSlots = new CardSlot[droppableCards.Count, maxUniqueCombinations];
        _sortedCollectedCardSlots = DataPersistenceManager.Instance.LoadGame();
        _cardParentGroup = _cardParent.GetComponent<HorizontalLayoutGroup>();

    }
    [ContextMenu("ManualCalculation")]
    private void CalculateRandomRange()
    {
        _randomRange = 0;
        droppableCards.ForEach((c) => _randomRange += c.dropChance);
    }
    public void GeneratePack(int amount)
    {
        if (amount == 0)
        {
            _fishingRod.ResetFishingRod();
            StashPack();
            return;
        }

        UIManager.Instance.inBoosterView = true;
        
        _fishingRod.MiniGameFinished();
        
        int qualityIncrease = 0;
        if (amount >= 5)
        {
            qualityIncrease = amount - 5;
            amount = 5;
        }
        for (int i = 0; i < amount; i++)
        {
            float randomFloat = Random.Range(0, _randomRange);
            randomFloat = Mathf.Pow(randomFloat / _randomRange, Mathf.Clamp01(1 - _qualityComboMultiplier * qualityIncrease)) * _randomRange;
            
            var randomCardSO = droppableCards.FirstOrDefault((c) =>
            {
                randomFloat -= c.dropChance;
                return randomFloat < 0;
            });

            if (!randomCardSO)
                randomCardSO = droppableCards.Last();

            var randomCard = new Card(randomCardSO, qualityIncrease); 
            _curCardPack.Add(randomCard);
            
            var cardSlot = _sortedCollectedCardSlots[droppableCards.IndexOf(randomCard.baseCardInfo), randomCard.rarityIndex] ??= new CardSlot(randomCard);
            if (cardSlot.BestCard.length < randomCard.length)
                cardSlot.BestCard = randomCard;
        }
        ShowClosedPack();
    }

    public void StashPack()
    {
        UIManager.Instance.inBoosterView = false;
        _curCardPack.Clear();
        foreach (Transform card in _cardParent)
        {
            Destroy(card.gameObject);
        }
    }
    public bool OpenPack()
    {
        UIManager.Instance.inBoosterView = true;

        if (_inOpenAnim) return false;
        if (!_closedPack.activeInHierarchy) return false;

        _cardParentGroup.spacing = -1920;
        StartCoroutine(SmoothShow());

        float delay = 1.1f;
        //SpawnPack
        foreach (var card in _curCardPack)
        {
            delay += 0.2f;
            var cardImage = Instantiate(_cardPrefab, _cardParent).GetComponent<CardImage>();
            var cardSlot = _sortedCollectedCardSlots[droppableCards.IndexOf(card.baseCardInfo), card.rarityIndex] ??= new CardSlot(card);
            bool isNew = cardSlot.CardAmount == 0;
            cardSlot.CardAmount += 1;

            bool isRecord = cardSlot.BestCard == card &! isNew;

            StartCoroutine(DelayedCardShow(delay, cardImage, card, isNew, isRecord));
        }

        StartCoroutine(EndAnim(delay));
        _inOpenAnim = true;
        _closedPack.SetActive(false);
        return true;
    }

    private IEnumerator SmoothShow()
    {
        for (int i = 0; i < 40; i++)
        {
            yield return new WaitForFixedUpdate();
            _cardParentGroup.spacing = -1920 * (1 - Mathf.Sqrt(i / 40f));
        }
    }
    private IEnumerator EndAnim(float time)
    {
        yield return new WaitForSeconds(time);
        OnOpeningAnimEnd();
    }
    private IEnumerator DelayedCardShow(float time, CardImage cardImage,Card card, bool isNew,bool isRecord)
    {
        yield return new WaitForSeconds(time);
        cardImage.Initialize(card, isNew, isRecord);
    }
    public void OnOpeningAnimEnd()
    {
        _inOpenAnim = false;
        _fishingRod.closePackOnNextInteract = true;
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
