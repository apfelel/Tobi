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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CalculatePosition();
    }
    [ContextMenu("Calculate Position")]
    private void CalculatePosition()
    {
        playerKnot.Position = _startTransform.position;
        _splineContainer.Spline.SetKnot(0, playerKnot);

        baitKnot.Position = _endTransform.position;
        _splineContainer.Spline.SetKnot(2, baitKnot);

        inBetweenKnot.Position = ((playerKnot.Position * 0.5f + baitKnot.Position * 2f) / 2) + new float3(Mathf.Sin(Time.time / 5) * 0.5f, -2, 0);
        _splineContainer.Spline.SetKnot(1, inBetweenKnot);
    }
}
