using UnityEngine;
using DG.Tweening;
using System.Collections;

public class EyesFollow : MonoBehaviour
{
    public bool allowBlink = true;
    public GameObject player;
    public float range;
    public Transform centerEye;
    public Vector2 centerEyePos;

    public float timeDelay;
    public float timeRandomness;
    public float blinkDuration;
    public Color blinkColor;


    SpriteRenderer _spriteRenderer;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        if(allowBlink) StartCoroutine(Blink());
    }

    void Update()
    {
        if(player == null) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if(distance <= range)
        {
            centerEye.DOLocalMove(centerEyePos, 0.3f);
            EyeFollow();
        }
        else
        {
            centerEye.DOLocalMove(Vector2.zero, 0.3f);
            centerEye.DORotate(Vector2.zero, 0.3f);
        }
        
    }

    void EyeFollow()
    {
        Vector2 playerPos = player.transform.position;

        Vector2 direction = new Vector2(playerPos.x - transform.position.x, playerPos.y - transform.position.y);

        transform.up = direction;
    }

    IEnumerator Blink()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(timeDelay - timeRandomness, timeDelay + timeRandomness));
            StartCoroutine(BlinkOnce());
        }
    }
    public IEnumerator BlinkOnce()
    {
        DOVirtual.Color(_spriteRenderer.color, blinkColor, blinkDuration, value => { _spriteRenderer.color = value; });
        yield return new WaitForSeconds(blinkDuration);
        DOVirtual.Color(_spriteRenderer.color, Color.red, blinkDuration, value => { _spriteRenderer.color = value; });
    }
}
