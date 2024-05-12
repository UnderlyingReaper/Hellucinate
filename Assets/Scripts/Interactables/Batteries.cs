using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Batteries : MonoBehaviour
{
    public FlashLight flashLight;
    public float batteryAmt = 1;
    public float range = 1;
    public AudioSource source;
    public CanvasGroup canvas;


    Transform _player;
    PlayerInputManager _pInputManager;

    bool _isDissolving = false;
    Material _material;
    float _distance;



    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _material = GetComponent<SpriteRenderer>().material;

        _pInputManager = _player.GetComponent<PlayerInputManager>();
        _pInputManager.playerInput.Player.Interact.performed += TryPickBattery;

        if(canvas != null) canvas.alpha = 0;
    }

    void Update()
    {
        if(_isDissolving) return;

        _distance = Vector2.Distance(_player.position, transform.position);

        if(_distance <= range)
        {
            if(canvas != null) canvas.DOFade(1, 1);
        }
        else
        {
            if(canvas != null) canvas.DOFade(0, 1);
        }
    }

    public void TryPickBattery(InputAction.CallbackContext context)
    {
        if(_distance <= range) PickupBattery();
    }

    public void PickupBattery()
    {
        flashLight.currBatteries += batteryAmt;
        PlayPickupSound(); // play sound
        
        // Start desolving the shader
        _isDissolving = true;
        if(_material != null) DOVirtual.Float(_material.GetFloat("_Fade"), 0, 2, value => {_material.SetFloat("_Fade", value); });

        // Hide canvas
        canvas.DOFade(0, 1f);
        
        // Destroy the item
        Destroy(gameObject, 2.1f);
    }

    public void PlayPickupSound()
    {
        source.volume = UnityEngine.Random.Range(0.8f, 1.2f);
        source.pitch = UnityEngine.Random.Range(0.8f, 1.2f);

        source.PlayOneShot(source.clip);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
