using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CatchMiniGame : MonoBehaviour
{
    public enum Directions
    {
        Left, Up, Right, Down
    }
    public GameObject arrowGameobject;
    public Transform arrowParent;
    public Image fishingRodFillImage;
    
    [SerializeField] private float _timeToSolve;
    [SerializeField] private float _timePenalty;

    private float _timer;
    
    private int _actionAmount;
    private List<Directions> _inputActions = new();
    private int _curActionIndex;
    private int _solvedAmount;
    
    private bool _initialized;
    
    private void OnEnable()
    {
        InitializeMiniGame();
    }

    private void OnDisable()
    {
        _inputActions.Clear();
        _curActionIndex = 0;
        _initialized = false;
    }

    void InitializeMiniGame()
    {
        //TODO debug 
        _initialized = true;
        _solvedAmount = 0;
        _actionAmount = 4;
        Generate(4);
    }

    private void Generate(int buttons)
    {
        _inputActions.Clear();
        _curActionIndex = 0;

        foreach (Transform arrow in arrowParent)
        {
            Destroy(arrow.gameObject);
        }
        
        for (int i = 0; i < buttons; i++)
        {
            int randomInt = Mathf.RoundToInt(Random.Range(0, 3));
            _inputActions.Add((Directions)randomInt);
            var arrow = Instantiate(arrowGameobject, arrowParent);
            arrow.transform.Rotate(new Vector3(0, 0, -90 * randomInt));
        }
    }

    public void TrySolve(Vector2 input)
    {
        if (!_initialized) return;
        if (input.magnitude < 0.9f) return;
        Directions inputDirection;
        
        if (input.x >= 0.8f) inputDirection = Directions.Right;
        else if (input.x <= -0.8f) inputDirection = Directions.Left;
        else if (input.y >= 0.8f) inputDirection = Directions.Up;
        else inputDirection = Directions.Down;
        
        if(_inputActions[_curActionIndex] == inputDirection)
        {
            arrowParent.GetChild(_curActionIndex).GetComponent<Image>().color = Color.green;
            _curActionIndex++;
            if (_curActionIndex == _actionAmount)
            {
                Generate(4);
                _solvedAmount++;
            }
        }
        else
        {
            _timer += _timePenalty;
            Generate(4);
        }
    }

    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;
        fishingRodFillImage.fillAmount = 1 - (_timer / _timeToSolve);
        if (_timer > _timeToSolve)
        {
            GameManager.Instance.GeneratePack(_solvedAmount);
            gameObject.SetActive(false);
        }
    }
}
