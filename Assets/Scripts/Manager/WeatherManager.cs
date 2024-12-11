using System.Collections;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public ParticleSystem FogParticleSystem;
    
    public enum Weathers
    {
        Normal, Rainy, Foggy
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeWeather(Weathers.Foggy);
    }

    public void ChangeWeather(Weathers newWeather)
    {
        StartCoroutine(ChangeWeatherDelayed(newWeather));
    }

    public IEnumerator ChangeWeatherDelayed(Weathers newWeather)
    {
        switch (newWeather)
        {
            case Weathers.Foggy:
                yield return new WaitForFixedUpdate();
                ParticleSystem.EmissionModule module = FogParticleSystem.emission;
                module.rateOverTime = 5;
                break;
        }
       
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
