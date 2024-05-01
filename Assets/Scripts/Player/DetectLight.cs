using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DetectLight : MonoBehaviour
{
    public bool isInLight;
    public LayerMask lightLayer;
    public float detectionRadius;
    public bool isFlashLightOn;

    public event EventHandler<LightDetectionResultArgs> LightDetectionResult;

    public class LightDetectionResultArgs: EventArgs {
        public bool isInLight;
    }

    Collider2D[] _lightSourcesCollider;


    void Update()
    {
        if(isInLight) LightDetectionResult?.Invoke(this, new LightDetectionResultArgs { isInLight = true });

        FindLightSources();
    }

    public void FindLightSources()
    {
        bool foundValidLightSource = false;

        // Check if flashlight is on, if its on no need to check if player is in light & increase his Sanity & return, since we know he is in light
        if(isFlashLightOn)
        {
            LightDetectionResult?.Invoke(this, new LightDetectionResultArgs { isInLight = true });
            foundValidLightSource = true;
            isInLight = true;
            return;
        }
        
        _lightSourcesCollider = Physics2D.OverlapCircleAll(transform.position, detectionRadius, lightLayer);

        // If there is no light in range, decrease Sanity & return
        if(_lightSourcesCollider.Length == 0)
        {
            Debug.Log("Losing Sanity");
            LightDetectionResult?.Invoke(this, new LightDetectionResultArgs { isInLight = false });
            foundValidLightSource = false;
            isInLight = false;
            return;
        }

        // Loop through all the lights in range
        foreach(Collider2D lightSourceCollider in _lightSourcesCollider)
        {
            Light2D lightSource = lightSourceCollider.GetComponent<Light2D>();

            // If the light is off, skip this light & move on to the next light in the list
            if(lightSource.intensity == 0 || !lightSource.gameObject.activeSelf)
            {
                foundValidLightSource = false;
                isInLight = false;
                continue; // move onto next object in the list
            }
            
            // If the player is in the range of light, proceed, else move on to the next light in the list;
            if(Vector3.Distance(transform.position, lightSource.transform.position) <= lightSource.pointLightOuterRadius)
            {
                // Transform the players position into the same coordinate system as the light source
                Vector2 playerPosRelativeToLight = lightSource.transform.InverseTransformPoint(transform.position);

                // Calculate the angle between the players position relative to the light source and the forward direction of the light
                float angle = Vector2.Angle(Vector2.up, playerPosRelativeToLight);

                // If the player is in the light angle, then he is in light & no need to check for the other light sources in the list
                if(angle <= lightSource.pointLightOuterAngle/2)
                {
                    foundValidLightSource = true;
                    isInLight = true;
                    return;
                }
            }
        }

        if(!foundValidLightSource)
        {
            Debug.Log("Losing Sanity");
            LightDetectionResult?.Invoke(this, new LightDetectionResultArgs { isInLight = false });
        }
    }

    // Litteraly the same code as above but with gizmos for visualization
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (!Application.isPlaying) return;

        if (_lightSourcesCollider.Length > 0)
        {
            foreach(Collider2D lightSourceCollider in _lightSourcesCollider)
            {
                Light2D lightSource = lightSourceCollider.GetComponent<Light2D>();

                if(Vector3.Distance(transform.position, lightSource.transform.position) <= lightSource.pointLightOuterRadius)
                {
                    // Transform the player's position into the same coordinate system as the light source
                    Vector2 playerPosRelativeToLight = lightSource.transform.InverseTransformPoint(transform.position);

                    // Calculate the angle between the player's position relative to the light source and the forward direction of the light
                    float angle = Vector2.Angle(Vector2.up, playerPosRelativeToLight);

                    if(angle <= lightSource.pointLightOuterAngle/2)
                    {
                        Gizmos.color = Color.green;
                    }
                    else
                        Gizmos.color = Color.red;

                    Gizmos.DrawLine(transform.position, lightSource.transform.position);
                }
            }
        }
    }
}
