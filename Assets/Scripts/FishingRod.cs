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
    
    private bool _released;
    private bool _hooked;
    private bool _miniGame;
    private bool _pauseFishingLogic;
    private bool _closePackOnNextInteract;
    
    public FishingLine fishingLine;
    public SpriteRenderer baitSpriteRenderer;
    private GameObject _baitGameObject;

    private Vector3 _baitTargetPosition;
    private Vector3 _baitStartPosition;

    private bool _inAnimation;

    private float _baitWiggleTimer;
    private void Start()
    {
        baitSpriteRenderer.color = Color.gray;
        _baitGameObject = baitSpriteRenderer.gameObject;
        _baitStartPosition = _baitGameObject.transform.position;
        _baitTargetPosition = _baitStartPosition;
        _baitGameObject.transform.position = _fishingRodTipGameObject.transform.position;
    }
    void FixedUpdate()
    {
        if (_pauseFishingLogic || _miniGame || _inAnimation) return;
        
        if (_released && !_hooked)
        {
            _baitWiggleTimer += Time.fixedDeltaTime;
            _baitTargetPosition = _baitStartPosition + new Vector3(Mathf.Sin(_baitWiggleTimer / 5) * 0.5f, Mathf.Sin((Mathf.PI / 2 + _baitWiggleTimer) / 5) * 0.5f, 0);
            _baitGameObject.transform.position = Vector3.Lerp(_baitGameObject.transform.position, _baitTargetPosition, 0.1f);
            
            if (RandomInt(60 * averagePullTime) != 0) return;
            
            StartCoroutine(RandomInt(averageFakePulls) == 0 ? RealPull() : FakePull());
        }
    }
    public void Interact()
    {
        if (_inAnimation) return;
        
        if (_closePackOnNextInteract)
        {
            GameManager.Instance.StashPack();
            _closePackOnNextInteract = false;
            return;
        }
        if (_miniGame)
        {
            if (GameManager.Instance.OpenPack())
            {
                ResetFishingRod();
                _closePackOnNextInteract = true;
            }
            return;
        }
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
    
    private void ThrowBait()
    {
        fishingLine.ThrowOut();
        _inAnimation = true;
        _baitGameObject.transform.DOMove(_baitTargetPosition, 1).OnComplete(ThrowBaitCompleted);
        
        _pauseFishingLogic = false;
        _released = true;
        _hooked = false;
        _miniGame = false;
        baitSpriteRenderer.color = Color.white;

    }

    void ThrowBaitCompleted()
    {
        _inAnimation = false;
        fishingLine.LetLoose();
    }
    
    private void ResetFishingRod()
    {
        _pauseFishingLogic = false;
        _released = false;
        _hooked = false;
        _miniGame = false;
        baitSpriteRenderer.color = Color.gray;
        
        _inAnimation = true;
        fishingLine.PullIn();
        _baitGameObject.transform.DOMove(_fishingRodTipGameObject.transform.position, 1).OnComplete(ReelInCompleted);
        
    }

    void ReelInCompleted()
    {
        _inAnimation = false;
    }
    
    private void TryCatch()
    {
        if(_hooked)
            StartCatchMiniGame();
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
        
        baitSpriteRenderer.color = Color.yellow;
        
        StartCoroutine(LogicPause(fakePullDuration));
        yield return new WaitForSeconds(fakePullDuration);
        
        baitSpriteRenderer.color = Color.white;

    }

    private IEnumerator RealPull()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(_baitGameObject.transform.DOMove(_baitTargetPosition +  new Vector3(0, -0.5f, 0), fakePullDuration / 2).SetEase(Ease.OutCubic));
        sequence.Append(_baitGameObject.transform.DOMove(_baitTargetPosition, fakePullDuration / 2).SetEase(Ease.OutCubic));
        
        baitSpriteRenderer.color = Color.green;
        _hooked = true;
        
        StartCoroutine(LogicPause(realPullDuration));
        yield return new WaitForSeconds(realPullDuration);
        
        baitSpriteRenderer.color = Color.white;
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
