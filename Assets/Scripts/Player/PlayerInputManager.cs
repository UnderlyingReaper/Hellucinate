using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public PlayerInput playerInput;
    
    void Awake()
    {
        if(playerInput == null)
        {
            playerInput = new PlayerInput();
        }

        playerInput.Player.Enable();
    }
}
