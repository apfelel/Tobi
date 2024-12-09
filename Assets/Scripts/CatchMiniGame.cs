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
    private int _buttonAmount;
    
    private List<Directions> InputActions = new();
    private int _curActionIndex;
    
    private void OnEnable()
    {
        InitializeMiniGame();
    }

    void InitializeMiniGame()
    {
        //TODO debug 
        _buttonAmount = 4;
        Generate(4);
    }

    private void Generate(int buttons)
    {
        for (int i = 0; i < buttons; i++)
        {
            int randomInt = Mathf.RoundToInt(Random.Range(0, 3));
            InputActions.Add((Directions)randomInt);
            var arrow = Instantiate(arrowGameobject, arrowParent);
            arrow.transform.Rotate(new Vector3(0, 0, 90 * randomInt));
        }
    }

    private void TrySolve(Vector2 input)
    {
        if (input.magnitude < 0.5f) return;
        Directions inputDirection;
        
        if (input.x >= 0.5f) inputDirection = Directions.Right;
        else if (input.x <= -0.5f) inputDirection = Directions.Left;
        else if (input.y >= 0.5f) inputDirection = Directions.Up;
        else inputDirection = Directions.Down;
        
        if(InputActions[_curActionIndex] == inputDirection)
        {
            arrowParent.GetChild(_curActionIndex).GetComponent<Image>().color = Color.green;
            _curActionIndex++;
        }
        else
        {
            Generate(4);
        }
    }
}
