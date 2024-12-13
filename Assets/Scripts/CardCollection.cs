using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardCollection : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardParent;
    [SerializeField] private TextMeshProUGUI curPageDescription;

    private AudioSource _audioSource;

    public AudioClip[] pageClip;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    private int _curRarity;
    private void OnEnable()
    {
        _curRarity = 0;
        Refresh();
    }

    private void OnDisable()
    {
        _audioSource.PlayOneShot(pageClip[Random.Range(0, pageClip.Length)]);
    }

    public void Refresh()
    {
        _audioSource.PlayOneShot(pageClip[Random.Range(0, pageClip.Length)]);


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
                curPageDescription.text = "Common";
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
        var old = _curRarity;

        _curRarity += change;
        _curRarity = Mathf.Clamp(_curRarity, 0, GameManager.Instance.maxUniqueCombinations - 1);
        
        if(_curRarity != old)
            Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
