using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class FishingLine : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;
    [SerializeField] private Transform _endTransform;

    [SerializeField] private SplineContainer _splineContainer;

    private BezierKnot playerKnot, inBetweenKnot, baitKnot;

    private float _targetYMult = 1, _currentYMult = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PullIn()
    {
        _targetYMult = 0;
    }

    public void LetLoose()
    {
        _targetYMult = 1;
    }

    public void ThrowOut()
    {
        _targetYMult = -0.8f;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CalculatePosition();
    }
    [ContextMenu("Calculate Position")]
    private void CalculatePosition()
    {
        _currentYMult = Mathf.Lerp(_currentYMult, _targetYMult, 0.1f);
        
        playerKnot.Position = _startTransform.position;
        _splineContainer.Spline.SetKnot(0, playerKnot);

        baitKnot.Position = _endTransform.position;
        _splineContainer.Spline.SetKnot(2, baitKnot);

        inBetweenKnot.Position = 
            (baitKnot.Position + (playerKnot.Position - baitKnot.Position) / 2) + new float3(Mathf.Sin(Time.time / 5) * 0.5f,
                (-Vector3.Magnitude(playerKnot.Position - baitKnot.Position) / 15) * _currentYMult,
                0);
        _splineContainer.Spline.SetKnot(1, inBetweenKnot);
    }
}
