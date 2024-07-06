using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public float range;
    public bool isItemInRange;


    PlayerInputManager _playerInputManager;
    IInteractible interactibleObj = null;
    GameObject _object = null;


    void Start()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.playerInput.Player.Interact.performed += OnPlayerInteract;
        _playerInputManager.playerInput.Player.Interact.canceled += OnPlayerInteractKeyUp;
        _playerInputManager.playerInput.Player.Interact.started += OnPlayerInteractKeyDown;
    }

    void Update()
    {
        Collider2D[] collidersArray = Physics2D.OverlapCircleAll(transform.position, range);

        Collider2D[] triggerCollidersArray = collidersArray.Where(collider => collider.isTrigger).ToArray();
        if(triggerCollidersArray.Length == 0)
        {
            if(interactibleObj != null)
            {
                isItemInRange = false;
                interactibleObj.HideCanvas();
                interactibleObj = null;
            }
            return;
        }

        foreach(Collider2D collider in triggerCollidersArray)
        {
            if(collider.TryGetComponent(out IInteractible interactible))
            {
                _object = collider.gameObject;
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
                _object = null;
            }
        }
    }

    private void OnPlayerInteract(InputAction.CallbackContext context)
    {
        if(!isItemInRange) return;
        Debug.Log("Interact with " + _object.name);
        interactibleObj.Interact(context);
    }
    private void OnPlayerInteractKeyUp(InputAction.CallbackContext context)
    {
        if(!isItemInRange) return;
        interactibleObj.OnInteractKeyUp();
    }
    private void OnPlayerInteractKeyDown(InputAction.CallbackContext context)
    {
        if(!isItemInRange) return;
        interactibleObj.OnInteractKeyDown();
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
