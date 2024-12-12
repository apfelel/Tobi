using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeatherManager : MonoBehaviour
{
    public ParticleSystem FogParticleSystem, RainParticleSystem, GodRayParticleSystem, ShinyParticleSystem;

    public Light sun;
    public Color nightCol, rainColMult;

    private float _timeToChangeWeather = 60, _curTimer, _dayCycleTimer, _dayCycleDuration = 120;
    
    
    private static readonly int Rainy = Shader.PropertyToID("_Rainy");
    private static readonly int Night = Shader.PropertyToID("_Night");

    public enum Weathers
    {
        Normal, Rainy, Foggy
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Shader.SetGlobalFloat("_Rainy", 0);

        ChangeWeather(Weathers.Rainy);
    }

    public void ChangeWeather(Weathers newWeather)
    {
        StartCoroutine(ChangeWeatherDelayed(newWeather));
    }

    public IEnumerator ChangeWeatherDelayed(Weathers newWeather)
    {
        ParticleSystem.EmissionModule fogModule = FogParticleSystem.emission;
        ParticleSystem.EmissionModule rainModule = RainParticleSystem.emission;

        switch (newWeather)
        {
            case Weathers.Foggy:
                if (Shader.GetGlobalFloat("_Rainy") > 0.9f)
                {
                    for (int i = 0; i < 120; i++)
                    {
                        yield return new WaitForFixedUpdate();
                        Shader.SetGlobalFloat("_Rainy", 1 - i / 120f);
                    }
                }
                
                
                fogModule.rateOverTime = 5;
                rainModule.rateOverTime = 0;
                break;
            case Weathers.Rainy:
                if (Shader.GetGlobalFloat("_Rainy") < 0.1f)
                {
                    for (int i = 0; i < 120; i++)
                    {
                        yield return new WaitForFixedUpdate();
                        Shader.SetGlobalFloat("_Rainy", i / 120f);
                    }
                }
                fogModule.rateOverTime = 0;
                rainModule.rateOverTime = 60;
                break;
            case Weathers.Normal:
                if (Shader.GetGlobalFloat("_Rainy") > 0.9f)
                {
                    for (int i = 0; i < 120; i++)
                    {
                        yield return new WaitForFixedUpdate();
                        Shader.SetGlobalFloat("_Rainy", 1 - i / 120f);
                    }
                }
                fogModule.rateOverTime = 0;
                rainModule.rateOverTime = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newWeather), newWeather, null);
        }
       
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        float night =
            Mathf.Clamp01(Mathf.Sin((_dayCycleTimer / _dayCycleDuration) * Mathf.PI * 2 + (Mathf.PI / 2)) * 0.5f + 0.5f);
        Shader.SetGlobalFloat(Night, night);

        var godrayEmission = GodRayParticleSystem.emission;
        godrayEmission.rateOverTime = night * 10 - 6;

        var ShinyParticleSystemEmission = this.ShinyParticleSystem.emission;
        ShinyParticleSystemEmission.rateOverTime = 1 - night * 3; 
        
        _curTimer += Time.fixedDeltaTime;
        _dayCycleTimer += Time.fixedDeltaTime;
        
        if (_curTimer > _timeToChangeWeather)
        {
            ChangeWeather((Weathers)Random.Range(0, 3));
            _timeToChangeWeather = Random.Range(10, 20);
            _curTimer = 0;
        }
        sun.color = Color.white * Color.Lerp(nightCol,  Color.white, Mathf.Clamp01(Mathf.Sin((_dayCycleTimer / _dayCycleDuration) * Mathf.PI * 2 + (Mathf.PI / 2)) * 0.5f + 0.5f));
        sun.color *= Color.Lerp(Color.white, rainColMult, Shader.GetGlobalFloat(Rainy));
        if (_dayCycleTimer > _dayCycleDuration)
        {
            _dayCycleTimer = 0;
        }
    }
}
