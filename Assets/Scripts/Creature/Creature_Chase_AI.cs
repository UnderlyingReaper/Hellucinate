using System;
using System.Collections.Generic;
using CameraShake;
using DG.Tweening;
using UnityEngine;

public class Creature_Chase_AI : MonoBehaviour
{
    public float speed;
    public string deathTxt;
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip growlClip;
    public AudioClip footstepClip;
    public AudioClip roarClip;


    public event EventHandler<SearchEndResultArgs> SearchEndResult;
    public class SearchEndResultArgs: EventArgs {
        public bool didCatchPlayer;
        public string deathTxt;
    }


    [Header("Shake")]
    public PerlinShake.Params perlinShake;
    public AnimationCurve amplitudeDistanceCurve;

    Vector3 _targetDestination;
    Transform _player;
    bool _chaseEnd = false;



    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, _player.position);

        // changes the amplitude of the shake, The Shorter the distance the greater the amplitude & vice versa
        float amplitude = amplitudeDistanceCurve.Evaluate(distance);
        CameraShaker.Shake(new PerlinShake(perlinShake, amplitude));

        if(_chaseEnd) return;

        // Check if the player is in light when the creature gets close, kill the player if player is in light
        if(distance <= 2)
        {
            DetectLight playerLightDetection = _player.GetComponent<DetectLight>();

            if(playerLightDetection.isInLight)
            {
                DOTween.Kill(transform);
                SearchEndResult?.Invoke(this, new SearchEndResultArgs { didCatchPlayer = true, deathTxt = deathTxt });
                PlayRoar();
                anim.SetBool("isMoving", false);
                anim.SetTrigger("PlayerCaught");
                _chaseEnd = true;
            }
        }
    }

    public void DoSearch()
    {
        transform.DOMove(_targetDestination, speed).SetEase(Ease.InOutSine).OnComplete(SearchEnd);
        anim.SetBool("isMoving", true);
    }

    public void CalculateDestination(List<Transform> cameraCornerPoints)
    {
        int startingPosIndex = UnityEngine.Random.Range(0, 2);
        int destinationIdex = (startingPosIndex + 1) % 2;

        Transform startingPos = cameraCornerPoints[startingPosIndex];
        Transform destinationPos = cameraCornerPoints[destinationIdex];

        if(startingPosIndex == 1) transform.localScale = new Vector3(1, 1, 1);
        else transform.localScale = new Vector3(-1, 1, 1);

        transform.position = new Vector3(startingPos.position.x, startingPos.position.y, transform.position.z);
        _targetDestination = new Vector3(destinationPos.position.x, destinationPos.position.y, transform.position.z);
    }

    public bool IsFacingRight()
    {
        bool isFacingRight = false; 
        if(transform.localScale == new Vector3(1, 1, 1)) isFacingRight = true;
        else if(transform.localScale == new Vector3(-1, 1, 1)) isFacingRight = false;

        return isFacingRight;
    }

    public void SearchEnd()
    {
        // Send result that player was not caught to prepare for the next round >:)
        SearchEndResult?.Invoke(this, new SearchEndResultArgs{ didCatchPlayer = false });
        anim.SetBool("isMoving", false);
        _chaseEnd = true;

        Destroy(gameObject, 0.5f);
    }


    public void RandomizeAudioSettings()
    {
        audioSource.volume = UnityEngine.Random.Range(0.5f, 0.9f);
        audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
    }
    public void PlayGrowl()
    {
        RandomizeAudioSettings();
        audioSource.PlayOneShot(growlClip);
    }
    public void PlayRoar()
    {
        RandomizeAudioSettings();
        audioSource.PlayOneShot(roarClip ? roarClip : growlClip);
    }
    public void PlayFootstep()
    {
        RandomizeAudioSettings();
        audioSource.PlayOneShot(footstepClip);
    }
}