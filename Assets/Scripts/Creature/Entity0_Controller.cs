using System.Collections;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Entity0_Controller : MonoBehaviour
{
    public float maxDelay, minDelay;

    [Space(20)]
    public bool allowEntityToSpawn;
    public Transform player;
    public PlayerJumpscare playerJumpscare;
    public DetectLight detectLight;
    public SpriteRenderer entitySprite;
    public float range;

    [Space(20)]
    public AudioClip screamClip;
    public float sanityLossAmt;

    [Space(20)]
    public float activationDelay;
    public float followSpeed;
    public Vector2 entityOffset;

    [Space(20)]
    public bool allowCountdown;
    public float maxStareTime;


    float _currStareTime;
    float _delay;
    float _currTime;



    void Start()
    {
        detectLight.LightDetectionResult += LightDetectionResult;
        _delay = UnityEngine.Random.Range(minDelay, maxDelay);

        entitySprite.enabled = false;
    }

    void Update()
    {
        if(!allowEntityToSpawn)
        {
            _currTime += Time.deltaTime;
            if(_currTime >= _delay)
            {
                allowEntityToSpawn = true;
                _currTime = 0;
            }
            return;
        }

        // get distance
        float distance = Vector2.Distance(player.position, transform.position);

        // calculate offset
        if(player.localScale.x > 0) entityOffset.x = -math.abs(entityOffset.x);
        else if(player.localScale.x < 0) entityOffset.x = math.abs(entityOffset.x);

        // get close to player if too far
        if(distance > range)
        {
            Vector2 newPos = new Vector2(player.position.x + entityOffset.x, player.position.y + entityOffset.y);
            transform.DOLocalMove(newPos, followSpeed).SetEase(Ease.InOutSine);
        }

        // check if player is staring at the entity with his light on
        if(allowCountdown)
        {
            _currStareTime += Time.deltaTime;
            playerJumpscare.sanity.sanity -= Time.deltaTime * (playerJumpscare.sanity.sanityGainSpeedMp + 1);

            if(_currStareTime >= maxStareTime)
            {
                allowEntityToSpawn = false;
                allowCountdown = false;
                Debug.Log("JUMPSCARE");
                transform.DOLocalMove(player.position, .3f).SetEase(Ease.InExpo).OnComplete(JumpScarePlayer);
            }
            
        }
        else if(_currStareTime > 0)
        {
            allowEntityToSpawn = false;
            entitySprite.enabled = false;
            _currStareTime = 0;
        }
        Mathf.Clamp(_currStareTime, 0, maxStareTime);
    }

    public void JumpScarePlayer()
    {
        playerJumpscare.PlayJumpScare(screamClip, sanityLossAmt);
        entitySprite.enabled = false;
    }

    public void LightDetectionResult(object sender, DetectLight.LightDetectionResultArgs e)
    {
        if(!allowEntityToSpawn) return;


        if(!e.isInLight) StartCoroutine(EnableSprite());
        else if(e.isInLight && LayerMask.LayerToName(e.lightLayer) == "Light")
        {
            entitySprite.enabled = false;
            allowCountdown = false;
            StopAllCoroutines();
        }
        else if(entitySprite.enabled && e.isInLight && LayerMask.LayerToName(e.lightLayer) == "Player_Light")
        {
            allowCountdown = true;
            StopAllCoroutines();
        }
    }

    IEnumerator EnableSprite()
    {
        yield return new WaitForSeconds(activationDelay);

        entitySprite.enabled = true;
        allowCountdown = false;
    }
}
