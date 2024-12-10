using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoSingleton<DataPersistenceManager>
{
   private string dataDirPath, dataFileName;
   private void Start()
   {
      dataDirPath = Application.persistentDataPath;
      dataFileName = "SaveFile.game";
      Debug.Log(dataDirPath);
   }

   private void OnApplicationQuit()
   {
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

               data[i, j].CardAmount = PlayerPrefs.GetInt(i + "-" + j + "_Amount");
               data[i, j].BestCard.length = PlayerPrefs.GetFloat(i + "-" + j + "_Length");
               data[i, j].BestCard.rarityIndex = PlayerPrefs.GetInt(i + "-" + j + "_RarityIndex");
               data[i, j].BestCard.uniqueTypes =Enum.Parse<CardEnums.UniqueTypes>(PlayerPrefs.GetString(i + "-" + j + "_UniqueTypes"));
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
      Debug.Log("SaveGame");
      int cardCount = GameManager.Instance.droppableCards.Count;
      int uniqueCount = GameManager.Instance.maxUniqueCombinations;

      var data = GameManager.Instance.GetSortedCollectedCardSlots();
      
      for (int i = 0; i < cardCount; i++)
      {
         for (int j = 0; j < uniqueCount; j++)
         {
            Debug.Log("SaveGame");

            PlayerPrefs.SetInt(i + "-" + j + "_Amount", data[i, j].CardAmount);
            PlayerPrefs.SetFloat(i + "-" + j + "_Length", data[i, j].BestCard.length);
            PlayerPrefs.SetInt(i + "-" + j + "_RarityIndex", data[i, j].BestCard.rarityIndex);
            PlayerPrefs.SetString(i + "-" + j + "_UniqueTypes", data[i, j].BestCard.uniqueTypes.ToString());
         }
      }
   }
}
