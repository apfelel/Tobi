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
    [SerializeField] private AudioClip _introClip, _throwOutClip, _reelInClip, _journalClip, _hm, _naw, _nice, _wow, _a;

    public SplashHelper _splashFelper;

    private AudioSource _audioSource;
    
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
        _audioSource = GetComponent<AudioSource>();
        
        SetFrame(IBotFrames.Idle);

        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
        {
            GiveTutorial();
        }
        else
        {
            Destroy(gameObject);
            Destroy(_tutorialText.gameObject);
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

        IEnumerator DelayedIntro()
        {
            yield return new WaitForSeconds(5);
            if (_tutorialindex == 0)
            {
                _audioSource.clip = _throwOutClip;
                _audioSource.Play();
                _tutorialText.text = "Press E on your keyboard or X on your controller to cast your fishing rod";
                _tutorialindex++;
            }
        }
        switch (_tutorialindex)
        {
            case 0:
                _audioSource.clip = _introClip;
                _audioSource.Play();
                _tutorialText.text = "Hi, I'm I-Bot, your personal fishing companion";
                StartCoroutine(DelayedIntro());
                break;
            case 1:
                _audioSource.clip = _reelInClip;
                _audioSource.Play();
                _tutorialText.text = "When you hear the bell, press E or X again to reel in";
                _tutorialindex++;
                break;
            case 2:
                _audioSource.clip = _journalClip;
                _audioSource.Play();
                
                _tutorialText.text = "Good work! You can see all your journal by pressing TAB or the Select button on your controller";
                _tutorialindex++;
                break;
            case 3:
                _tutorialText.gameObject.SetActive(false);
                _tutorialindex++;
                break;
            case 4:
                _tutorialText.gameObject.SetActive(true);
                if (Random.Range(0, 5) != 0)
                {
                    SetFrame(IBotFrames.Explaining);
                    sequence.Append(transform.DOMove(transform.position + new Vector3(0, 0.3f, 0), 0.2f));
                    sequence.Append(transform.DOMove(transform.position, 0.2f));
                    sequence.Append(transform.DOMove(transform.position, 0.4f));
                    sequence.OnComplete(() => SetFrame(IBotFrames.Idle));
                    
                    switch (Random.Range(0, 4))
                    {
                        case 0:
                            _audioSource.clip = _wow;
                            _audioSource.Play();
                            _tutorialText.text = "Wow";
                            break;
                        case 1:
                            _audioSource.clip = _nice;
                            _audioSource.Play();
                            _tutorialText.text = "Nice";
                            break;
                        case 2:
                            _audioSource.clip = _hm;
                            _audioSource.Play();
                            _tutorialText.text = "Hmm";
                            break;
                        case 3:
                            _audioSource.clip = _naw;
                            _audioSource.Play();
                            _tutorialText.text = "Nah";
                            break;
                    }

                    _tutorialindex++;
                    return;
                }
                _audioSource.clip = _a;
                _audioSource.Play();
                _tutorialText.text = "AAAAAA";
                _splashFelper.ShowSplash();
                SetFrame(IBotFrames.Fall);
                PlayerPrefs.SetInt("Tutorial", 1);
                sequence.Append(transform.DOMove(_fallTarget.transform.position, 1.3f).OnComplete(() =>
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
                if(!_fishingRod._released && !UIManager.Instance.inBoosterView)
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
