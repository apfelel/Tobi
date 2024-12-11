using System.Collections;
using UnityEngine;

public class SplashHelper : MonoBehaviour
{
    private float _alpha;

    private Material splashMat;

    public int iterations;

    public bool StartSequence;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<MeshRenderer>().material = new Material(GetComponent<MeshRenderer>().material);
        splashMat = GetComponent<MeshRenderer>().material;
        splashMat.SetFloat("_Transparent", 1);
    }

    public void ShowSplash()
    {
        StartCoroutine(SmoothFadeIn());
    }

    public void HideSplash()
    {
        StartCoroutine(SmoothFadeOut());
    }

    IEnumerator SmoothFadeIn()
    {
        for (int i = 0; i < iterations; i++)
        {
            splashMat.SetFloat("_Transparent", _alpha);
            _alpha = 1- (i / 50f);
            yield return new WaitForFixedUpdate();
        }
        _alpha = 0;

        if (StartSequence)
            StartCoroutine(SmoothFadeOut());
    }
    IEnumerator SmoothFadeOut()
    {
        for (int i = 0; i < iterations; i++)
        {
            splashMat.SetFloat("_Transparent", _alpha);
            _alpha = i / 50f;
            yield return new WaitForFixedUpdate();
        }
        _alpha = 1;
        if(StartSequence)
            Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
