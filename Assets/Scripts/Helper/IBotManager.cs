using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class IBotManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro _tutorialText;
    [SerializeField] private FishingRod _fishingRod;
    [SerializeField] private GameObject _fallTarget;

    public SplashHelper _splashFelper;
    
    private int _tutorialindex;
    public enum IBotFrames
    {
        Idle, Explaining, Fall
    }

    public void SetFrame(IBotFrames frame)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        
        transform.GetChild((int)frame).gameObject.SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetFrame(IBotFrames.Idle);

        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
        {
            GiveTutorial();
        }
        else
        {
            Destroy(gameObject);            
        }
    }

    public void GiveTutorial()
    {
        var sequence = DOTween.Sequence();
        if (_tutorialindex != 4 && _tutorialindex != 5)
        {
            SetFrame(IBotFrames.Explaining);
            sequence.Append(transform.DOMove(transform.position + new Vector3(0, 0.3f, 0), 0.2f));
            sequence.Append(transform.DOMove(transform.position, 0.2f));
            sequence.Append(transform.DOMove(transform.position, 0.4f));
            sequence.OnComplete(() => SetFrame(IBotFrames.Idle));
        }

        switch (_tutorialindex)
        {
            case 0:
                _tutorialText.text = "Oi, press E | X button";
                _tutorialindex++;
                break;
            case 1:
                _tutorialText.text = "Oi, press E | X button when fish bites and then WASD | D-Pad";
                _tutorialindex++;
                break;
            case 2:
                _tutorialText.text = "Oi, press TAB | Select for cards";
                _tutorialindex++;
                break;
            case 3:
                _tutorialindex++;
                break;
            case 4:

                if (Random.Range(0, 5) != 0)
                {
                    SetFrame(IBotFrames.Explaining);
                    sequence.Append(transform.DOMove(transform.position + new Vector3(0, 0.3f, 0), 0.2f));
                    sequence.Append(transform.DOMove(transform.position, 0.2f));
                    sequence.Append(transform.DOMove(transform.position, 0.4f));
                    sequence.OnComplete(() => SetFrame(IBotFrames.Idle));
                    
                    Debug.Log("Test");
                    _tutorialText.text = Random.Range(0, 5) switch
                    {
                        0 => "Wow",
                        1 => "Woah",
                        2 => "Nice",
                        3 => "Hmm",
                        4 => "Nah",
                        _ => _tutorialText.text
                    };
                    _tutorialindex++;
                    return;
                }
                _tutorialText.text = "AAAAAA";
                _splashFelper.ShowSplash();
                SetFrame(IBotFrames.Fall);
                sequence.Append(transform.DOMove(_fallTarget.transform.position, 0.5f).OnComplete(() =>
                {
                    sequence.Complete();
                    Destroy(_tutorialText.gameObject);
                    Destroy(gameObject);
                }));
                _tutorialindex++;
                break;
            case 5:
                _tutorialindex--;
                break;
        }
        
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        switch (_tutorialindex)
        {
            case 1:
                if(_fishingRod._released)
                    GiveTutorial();
                break;
            case 2:
                if(!_fishingRod._released)
                    GiveTutorial();
                break;
            case 3:
                if (UIManager.Instance.inCollection)
                {
                    GiveTutorial();
                    _tutorialText.text = "";
                }
                break;
            case 4:
                if(_fishingRod._released)
                    GiveTutorial();
                break;
            case 5:
                if(!_fishingRod._released)
                    GiveTutorial();
                break;
        }
    }
}
