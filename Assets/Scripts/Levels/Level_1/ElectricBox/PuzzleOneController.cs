using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PuzzleOneController : MonoBehaviour
{
    public Sockets[] sockets;
    public int socketsConnected = 0;
    public GameObject wires;
    public bool allWiresConnected = false;
    public Light2D redLight, greenLight;
    public EventHandler OnPuzzleComplete;

    public Transform lever;

    [Header("Sounds")]
    public AudioSource electricBuzzing_Source;
    public AudioClip breaker_Clip;
    public AudioSource audioSource;

    PlayerTextDisplay _playerTextDisplay;


    void Start()
    {

        wires.SetActive(true);
        electricBuzzing_Source.volume = 0;

        for(int i = 0; i < sockets.Length; i++)
        {
            sockets[i].OnConnected += OnSocketConnected;
        }
    }

    public void OnSocketConnected(object sender, EventArgs e)
    {
        socketsConnected++;
        if(socketsConnected == sockets.Length) allWiresConnected = true;
    }

    bool _allowLeverInteraction = true;
    void OnMouseOver()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && _allowLeverInteraction) StartCoroutine(PlaySwitchAnimation());
    }

    public IEnumerator PlaySwitchAnimation()
    {
        _allowLeverInteraction = false;
        lever.DOScaleY(-2, 0.5f);
        PlaySound(breaker_Clip);

        if(allWiresConnected)
        {
            DOVirtual.Float(redLight.intensity, 0, 0.5f, value => { redLight.intensity = value; });
            DOVirtual.Float(greenLight.intensity, 0.1f, 0.5f, value => { greenLight.intensity = value; });
            DOVirtual.Float(electricBuzzing_Source.volume, 1, 1, value => { electricBuzzing_Source.volume = value; });
            OnPuzzleComplete?.Invoke(this, EventArgs.Empty);
            yield break;
        }

        yield return new WaitForSeconds(1);

        lever.DOScaleY(2, 0.5f);
        _allowLeverInteraction = true;
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.volume = UnityEngine.Random.Range(0.3f, 0.7f);
        audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);

        audioSource.PlayOneShot(clip);
    }
}
