using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light_Flicker : MonoBehaviour
{
    public float minIntensity = 0;
    public float transitionDuration = 0.1f;
    public Vector2 flickers = new Vector2(1, 3);
    public Vector2 delayBetweenFlicker = new Vector2(0, 5);


    Light2D _light2d;
    float _maxIntensity = 1;


    void Start()
    {
        _light2d = GetComponent<Light2D>();
        _maxIntensity = _light2d.intensity == 0 ? 1 : _light2d.intensity;
        StartCoroutine(Flicker());
    }

    void OnEnable() => StartCoroutine(Flicker());

    IEnumerator Flicker()
    {
        while(true)
        {
            float delay = Random.Range(delayBetweenFlicker.x, delayBetweenFlicker.y);
            yield return new WaitForSeconds(delay);

            int amtOfFlickers = (int)Random.Range(flickers.x, flickers.y);

            for(int i = 0; i < amtOfFlickers; i++)
            {
                DOVirtual.Float(_light2d.intensity, minIntensity, transitionDuration, value => { _light2d.intensity = value; });
                yield return new WaitForSeconds(transitionDuration);
                
                DOVirtual.Float(_light2d.intensity, _maxIntensity, transitionDuration, value => { _light2d.intensity = value; });
                yield return new WaitForSeconds(transitionDuration);
            }
        }
    }
}
