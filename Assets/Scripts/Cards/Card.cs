using UnityEngine;

[System.Serializable]
public class Card
{
    public float length;
    public CardEnums.UniqueTypes? uniqueTypes;
    public CardScriptableObject baseCardInfo;
    public int rarityIndex;
    public Card(CardScriptableObject cardInfosRef)
    {
        rarityIndex = 0;
        baseCardInfo = cardInfosRef;
        uniqueTypes = null;
        if (Random.Range(0, 10) == 0)
        {
            uniqueTypes = CardEnums.UniqueTypes.Holo;
            rarityIndex = 1;
        }

        if (Random.Range(0, 10) == 0)
        {
            uniqueTypes = CardEnums.UniqueTypes.Shiny;
            rarityIndex = 2;
        }

        if (Random.Range(0, 10) == 0)
        {
            uniqueTypes = CardEnums.UniqueTypes.Shiny | CardEnums.UniqueTypes.Holo;
            rarityIndex = 3;
        }

        length = Random.Range(baseCardInfo.minLength, baseCardInfo.maxLength);
    }
}

[System.Serializable]
public class CardSlot
{
    public CardSlot(Card card)
    {
        BestCard = card;
    }
    public Card BestCard;
    public int CardAmount;
}
