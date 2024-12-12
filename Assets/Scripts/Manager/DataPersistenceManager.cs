using System;
using UnityEngine;

public class DataPersistenceManager : MonoSingleton<DataPersistenceManager>
{
   private void OnApplicationQuit()
   {
      if (GameManager.Instance != null) 
         SaveGame();
   }

   private void OnDisable()
   {
      if (GameManager.Instance == null) return;
      
      SaveGame();
   }

   public CardSlot[,] LoadGame()
   {
      Debug.Log("Load");
      
      int cardCount = GameManager.Instance.droppableCards.Count;
      int uniqueCount = GameManager.Instance.maxUniqueCombinations;

      var data = GameManager.Instance.GetSortedCollectedCardSlots();
      
      for (int i = 0; i < cardCount; i++)
      {
         for (int j = 0; j < uniqueCount; j++)
         {
            try
            {
               if (data[i, j] == null)
               {
                  Card tempCard = new(GameManager.Instance.droppableCards[i]);
                  data[i, j] = new CardSlot(tempCard);
               }
               data[i, j].CardAmount = PlayerPrefs.GetInt(i + "-" + j + "_Amount", 0);
               data[i, j].BestCard.length = PlayerPrefs.GetFloat(i + "-" + j + "_Length", 0);               
               data[i, j].BestCard.rarityIndex = PlayerPrefs.GetInt(i + "-" + j + "_RarityIndex", 0);               
               try
               {
                  data[i, j].BestCard.uniqueTypes = Enum.Parse<CardEnums.UniqueTypes>(PlayerPrefs.GetString(i + "-" + j + "_UniqueTypes"));
               }
               catch (Exception e)
               {
                  PlayerPrefs.SetString(i + "-" + j + "_UniqueTypes", null);
               }
            }
            catch (Exception _)
            {
            }
         }
      }

      return data;
   }
   public void SaveGame()
   {
      int cardCount = GameManager.Instance.droppableCards.Count;
      int uniqueCount = GameManager.Instance.maxUniqueCombinations;

      var data = GameManager.Instance.GetSortedCollectedCardSlots();
      
      for (int i = 0; i < cardCount; i++)
      {
         for (int j = 0; j < uniqueCount; j++)
         {
            PlayerPrefs.SetInt(i + "-" + j + "_Amount", data[i, j].CardAmount);
            PlayerPrefs.SetFloat(i + "-" + j + "_Length", data[i, j].BestCard.length);
            PlayerPrefs.SetInt(i + "-" + j + "_RarityIndex", data[i, j].BestCard.rarityIndex);
            PlayerPrefs.SetString(i + "-" + j + "_UniqueTypes", data[i, j].BestCard.uniqueTypes.ToString());
         }
      }
   }
}
