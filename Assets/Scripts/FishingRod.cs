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
    
    private bool _released;
    private bool _hooked;
    private bool _miniGame;
    private bool _pauseFishingLogic;
    private bool _resetOnNextInteract;
    
    public SpriteRenderer baitSpriteRenderer;
    private GameObject _baitGameObject;

    private Vector3 _baitTargetPosition;
    private Vector3 _baitStartPosition;

    private void Start()
    {
        baitSpriteRenderer.color = Color.gray;
        _baitGameObject = baitSpriteRenderer.gameObject;
        _baitStartPosition = _baitGameObject.transform.position;
    }

    
    
    void FixedUpdate()
    {
        _baitTargetPosition = _baitStartPosition + new Vector3(Mathf.Sin(Time.time / 5) * 0.5f, Mathf.Sin((Mathf.PI / 2 + Time.time) / 5) * 0.5f, 0);
        _baitGameObject.transform.position = Vector3.Lerp(_baitGameObject.transform.position, _baitTargetPosition, 0.1f);
        
        if (_pauseFishingLogic || _miniGame) return;
        
        if (_released && !_hooked)
        {
            if (RandomInt(60 * averagePullTime) != 0) return;
            
            StartCoroutine(RandomInt(averageFakePulls) == 0 ? RealPull() : FakePull());
        }
    }
    public void Interact()
    {
        if (_miniGame)
        {
            if (_resetOnNextInteract)
            {
                ResetFishingRod();
                GameManager.Instance.StashPack();
                return;
            }
            
            if (GameManager.Instance.OpenPack())
                _resetOnNextInteract = true;
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
    }
    
    private void ThrowBait()
    {
        _pauseFishingLogic = false;
        _released = true;
        _hooked = false;
        _miniGame = false;
        baitSpriteRenderer.color = Color.white;

    }

    private void ResetFishingRod()
    {
        _pauseFishingLogic = false;
        _released = false;
        _hooked = false;
        _miniGame = false;
        _resetOnNextInteract = false;
        baitSpriteRenderer.color = Color.gray;
        
        StopAllCoroutines();
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
        Debug.Log("Fake");
        var sequence = DOTween.Sequence();
        sequence.Append(_baitGameObject.transform.DOMove(_baitTargetPosition +  new Vector3(0, -0.1f, 8), fakePullDuration / 2).SetEase(Ease.OutCubic));
        sequence.Append(_baitGameObject.transform.DOMove(_baitTargetPosition +  new Vector3(0, 0.1f, 8), fakePullDuration / 2).SetEase(Ease.OutCubic));
        
        baitSpriteRenderer.color = Color.yellow;
        
        StartCoroutine(LogicPause(fakePullDuration));
        yield return new WaitForSeconds(fakePullDuration);
        
        baitSpriteRenderer.color = Color.white;

    }
    private IEnumerator RealPull()
    {
        Debug.Log("Real");
        var sequence = DOTween.Sequence();
        sequence.Append(_baitGameObject.transform.DOMove(_baitTargetPosition +  new Vector3(0, -0.3f, 8), fakePullDuration / 2).SetEase(Ease.OutCubic));
        sequence.Append(_baitGameObject.transform.DOMove(_baitTargetPosition +  new Vector3(0, 0.3f, 8), fakePullDuration / 2).SetEase(Ease.OutCubic));
        
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
