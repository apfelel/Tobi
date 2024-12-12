using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishingRod : MonoBehaviour
{
    [SerializeField] private int averagePullTime = 1;
    [SerializeField] private int averageFakePulls = 5;
    [SerializeField] private float fakePullDuration = 1;
    [SerializeField] private float realPullDuration = 2;
    
    [SerializeField] private GameObject _fishingRodTipGameObject;
    
    [HideInInspector] public bool _released;

    private Player _player;
    
    private bool _hooked;
    private bool _miniGame;
    private bool _pauseFishingLogic;
    public  bool closePackOnNextInteract;
    private bool _miniGameEnd;
    public FishingLine fishingLine;
    [SerializeField] private GameObject _baitGameObject;

    private Vector3 _baitTargetPosition;
    private Vector3 _baitStartPosition;

    private bool _inAnimation;

    private float _baitWiggleTimer;
    [Space] [Header("SoundStuff")] [SerializeField] private AudioClip _throwOutAudio;
    [SerializeField] private AudioClip _reelInAudio, _bigWaterSplashAudio, _realBiteAudio, _fakeBiteAudio;
    private AudioSource _audioSource;

    public SplashHelper _splashHelper;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _baitStartPosition = _baitGameObject.transform.position;
        _baitTargetPosition = _baitStartPosition;
        _baitGameObject.transform.position = _fishingRodTipGameObject.transform.position;
        _player = GetComponent<Player>();
    }
    void FixedUpdate()
    {
        if (_pauseFishingLogic || _miniGame || _inAnimation) return;
        
        if (_released && !_hooked)
        {
            _baitWiggleTimer += Time.fixedDeltaTime;
            _baitTargetPosition = _baitStartPosition + new Vector3(Mathf.Sin(_baitWiggleTimer / 5) * 0.5f, 0, Mathf.Sin((Mathf.PI / 2 + _baitWiggleTimer) / 5) * 0.5f);
            _baitGameObject.transform.position = Vector3.Lerp(_baitGameObject.transform.position, _baitTargetPosition, 0.1f);
            
            if (RandomInt(60 * averagePullTime) != 0) return;
            
            StartCoroutine(RandomInt(averageFakePulls) == 0 ? RealPull() : FakePull());
        }
    }
    public void Interact()
    {
        if (_miniGameEnd)
        {
            if (closePackOnNextInteract)
            {
                GameManager.Instance.StashPack();
                closePackOnNextInteract = false;
                _miniGameEnd = false;
                return;
            }

            GameManager.Instance.OpenPack();
            return;
        }
        
        if (_inAnimation) return;
        
        if (_released)
        {
            TryCatch();
        }
        else
        {
            ThrowBait();
        }
        StopAllCoroutines();
    }

    public void MiniGameFinished()
    {
        ResetFishingRod();
        _miniGameEnd = true;
    }
    private void ThrowBait()
    {
        _player.ChangeToThrowSprite();
        _baitGameObject.SetActive(false);
        var sequence = DOTween.Sequence();
        sequence.AppendInterval(0.5f);
        sequence.OnComplete(ThrowBaitDelayed);
        
        _pauseFishingLogic = false;
        _hooked = false;
        _miniGame = false;
    }

    private void ThrowBaitDelayed()
    {
        _baitGameObject.SetActive(true);

        _released = true;

        _player.ChangeToIdleSprite();
        _baitGameObject.transform.SetParent(null);
        
        fishingLine.ThrowOut();
        _inAnimation = true;
        _baitGameObject.transform.DOMove(_baitTargetPosition, 1).OnComplete(ThrowBaitCompleted);
        
        _audioSource.PlayOneShot(_throwOutAudio);

    }
    void ThrowBaitCompleted()
    {
        _inAnimation = false;
        fishingLine.LetLoose();
        _audioSource.PlayOneShot(_bigWaterSplashAudio);
        _splashHelper.ShowSplash();
    }
    
    public void ResetFishingRod()
    {
        _splashHelper.HideSplash();
        _pauseFishingLogic = false;
        _released = false;
        _hooked = false;
        _miniGame = false;
        
        _inAnimation = true;
        fishingLine.PullIn();
        _player.ChangeToReelInSprite();
        _baitGameObject.transform.DOMove(_fishingRodTipGameObject.transform.position, 1).OnComplete(ReelInCompleted);
        _audioSource.PlayOneShot(_reelInAudio);
    }

    void ReelInCompleted()
    {
        _inAnimation = false;
        _baitGameObject.transform.SetParent(_fishingRodTipGameObject.transform);
        _player.ChangeToIdleSprite();
    }
    
    private void TryCatch()
    {
        if (_hooked)
        {
            StartCatchMiniGame();
            fishingLine.PullIn();
        }
        else
        {
            ResetFishingRod();
        }
    }

    private void StartCatchMiniGame()
    {
        GameManager.Instance.StartMiniGame();
        _miniGame = true;
    }

    private IEnumerator FakePull()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(_baitGameObject.transform.DOMove(_baitTargetPosition +  new Vector3(0, -0.1f, 0), fakePullDuration / 2).SetEase(Ease.OutCubic));
        sequence.Append(_baitGameObject.transform.DOMove( _baitTargetPosition, fakePullDuration / 2).SetEase(Ease.OutCubic));
        
        _audioSource.PlayOneShot(_fakeBiteAudio, 0.3f);

        StartCoroutine(LogicPause(fakePullDuration));
        yield return new WaitForSeconds(fakePullDuration);
    }

    private IEnumerator RealPull()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(_baitGameObject.transform.DOMove(_baitTargetPosition +  new Vector3(0, -0.5f, 0), fakePullDuration / 2).SetEase(Ease.OutCubic));
        sequence.Append(_baitGameObject.transform.DOMove(_baitTargetPosition, fakePullDuration / 2).SetEase(Ease.OutCubic));
        
        _audioSource.PlayOneShot(_realBiteAudio);
        _audioSource.PlayOneShot(_fakeBiteAudio, 0.5f);
        _hooked = true;
        
        StartCoroutine(LogicPause(realPullDuration));
        yield return new WaitForSeconds(realPullDuration);
        
        _hooked = false;
    }
    private IEnumerator LogicPause(float time)
    {
        _pauseFishingLogic = true;
        yield return new WaitForSeconds(time);
        _pauseFishingLogic = false;
    }

    private int RandomInt(int max)
    {
        return Mathf.RoundToInt(Random.Range(0, max));
    }
}
