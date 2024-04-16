using UnityEngine;
using DG.Tweening;

public class LightSwitch : MonoBehaviour
{
    public GameObject[] lights;
    public float range;
    public bool isOn;

    public AudioSource audioSource;
    public AudioClip interactClip;
    public AudioClip backgroundClip;


    CanvasGroup _canvasGroup;
    Transform _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _canvasGroup = GetComponentInChildren<CanvasGroup>();

        if(isOn)
        {
            for(int i = 0; i < lights.Length; i++) lights[i].SetActive(true);
        }
        else if(!isOn)
        {
            for(int i = 0; i < lights.Length; i++) lights[i].SetActive(false);
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(_player.position, transform.position);

        if(distance <= range)
        {
            _canvasGroup.DOFade(1, 1);

            if(Input.GetKeyDown(KeyCode.E) && !isOn)
            {
                isOn = true;
                for(int i = 0; i < lights.Length; i++) lights[i].SetActive(true);
                PlaySound();
            }
            else if(Input.GetKeyDown(KeyCode.E) && isOn)
            {
                isOn = false;
                for(int i = 0; i < lights.Length; i++) lights[i].SetActive(false);
                PlaySound();
            }
        }
        else
        {
            _canvasGroup.DOFade(0, 2);
        }

        if(isOn && backgroundClip != null) DOVirtual.Float(audioSource.volume, 1, 1, value => { audioSource.volume = value; });
        else if(!isOn && backgroundClip != null) DOVirtual.Float(audioSource.volume, 0, 1, value => { audioSource.volume = value; });
    }

    public void PlaySound()
    {
        audioSource.volume = Random.Range(0.8f, 1.2f);
        audioSource.pitch = Random.Range(0.8f, 1.2f);

        audioSource.PlayOneShot(interactClip);
    }
}
