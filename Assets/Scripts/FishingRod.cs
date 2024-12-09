using System;
using System.Collections;
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

    public SpriteRenderer bait;


    private void Start()
    {
        bait.color = Color.gray;
    }

    public void Interact()
    {
        if (_miniGame) return;
        
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
        bait.color = Color.white;

    }

    private void TryCatch()
    {
        if(_hooked)
            StartCatchMiniGame();
        else
        {
            _released = false;
            bait.color = Color.gray;
        }
    }

    public void StartCatchMiniGame()
    {
        
    }

    
    void FixedUpdate()
    {
        if (_pauseFishingLogic || _miniGame) return;
        
        if (_released && !_hooked)
        {
            if (RandomInt(60 * averagePullTime) != 0) return;
            
            StartCoroutine(RandomInt(averageFakePulls) == 0 ? RealPull() : FakePull());
        }
    }

    private IEnumerator FakePull()
    {
        Debug.Log("Fake");
        bait.color = Color.yellow;
        
        StartCoroutine(LogicPause(fakePullDuration));
        yield return new WaitForSeconds(fakePullDuration);
        
        bait.color = Color.white;

    }
    private IEnumerator RealPull()
    {
        Debug.Log("Real");
        bait.color = Color.green;
        _hooked = true;
        
        StartCoroutine(LogicPause(realPullDuration));
        yield return new WaitForSeconds(realPullDuration);
        
        bait.color = Color.white;
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
