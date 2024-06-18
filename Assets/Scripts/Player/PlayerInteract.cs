using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public float range;
    public bool isItemInRange;


    PlayerInputManager _playerInputManager;
    IInteractible interactibleObj = null;


    void Start()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.playerInput.Player.Interact.performed += OnPlayerInteract;
        _playerInputManager.playerInput.Player.LedgeClimb.performed += OnPlayerInteract;
    }

    void Update()
    {
        Collider2D[] collidersArray = Physics2D.OverlapCircleAll(transform.position, range);
        foreach(Collider2D collider in collidersArray)
        {
            if(collider.TryGetComponent(out IInteractible interactible))
            {
                interactibleObj = interactible;
                interactibleObj.ShowCanvas();
                isItemInRange = true;
                break;
            }
            else if(interactibleObj != null)
            {
                isItemInRange = false;
                interactibleObj.HideCanvas();
                interactibleObj = null;
            }
        }
    }

    private void OnPlayerInteract(InputAction.CallbackContext context)
    {
        if(!isItemInRange) return;
        Debug.Log("Interact!");
        interactibleObj.Interact();
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
