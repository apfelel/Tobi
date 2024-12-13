using UnityEngine;

public class Card
{
    public float length;
    public CardEnums.UniqueTypes? uniqueTypes;
    public CardScriptableObject baseCardInfo;
    public int rarityIndex;
    public Card(CardScriptableObject cardInfosRef)
    {
        Initialize(cardInfosRef, 0);
    }

    public Card(CardScriptableObject cardInfosRef, int qualityIncrease)
    {
        Initialize(cardInfosRef, qualityIncrease);
    }

    private void Initialize(CardScriptableObject cardInfosRef, int qualityIncrease)
    {
        rarityIndex = 0;
        baseCardInfo = cardInfosRef;
        uniqueTypes = CardEnums.UniqueTypes.Normal;
        var qualityMod = Mathf.Pow(1.5f, qualityIncrease);
        if (Random.Range(0, 40f) <= 1 * qualityMod)
        {
            uniqueTypes = CardEnums.UniqueTypes.Holo;
            rarityIndex = 1;
        }

        if (Random.Range(0, 40f) <= 1 * qualityMod)
        {
            uniqueTypes = CardEnums.UniqueTypes.Normal;
            uniqueTypes = CardEnums.UniqueTypes.Shiny;
            rarityIndex = 2;
        }

        if (Random.Range(0, 80f) <= 1 * qualityMod)
        {
            uniqueTypes = CardEnums.UniqueTypes.Normal;
            uniqueTypes = CardEnums.UniqueTypes.Shiny | CardEnums.UniqueTypes.Holo;
            rarityIndex = 3;
        }

        length = Random.Range(baseCardInfo.minLength, baseCardInfo.maxLength);
    }
}

public class CardSlot
{
    public CardSlot(Card card)
    {
        BestCard = card;
    }
    public Card BestCard;
    public int CardAmount;
}
