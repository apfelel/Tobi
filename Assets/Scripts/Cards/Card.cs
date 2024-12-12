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
        uniqueTypes = null;
        var qualityMod = Mathf.Pow(1.5f, qualityIncrease);
        Debug.Log(qualityMod);
        if (Random.Range(0, 20f) <= 1 * qualityMod)
        {
            uniqueTypes = CardEnums.UniqueTypes.Holo;
            rarityIndex = 1;
        }

        if (Random.Range(0, 20f) <= 1 * qualityMod)
        {
            uniqueTypes = null;
            uniqueTypes = CardEnums.UniqueTypes.Shiny;
            rarityIndex = 2;
        }

        if (Random.Range(0, 10) <= 1 * qualityMod)
        {
            uniqueTypes = null;
            uniqueTypes = CardEnums.UniqueTypes.Shiny | CardEnums.UniqueTypes.Holo;
            Debug.Log(uniqueTypes.ToString());
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
