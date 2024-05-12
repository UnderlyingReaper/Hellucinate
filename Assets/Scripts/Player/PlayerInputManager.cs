using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public PlayerInput playerInput;
    
    void Start()
    {
        if(playerInput == null)
        {
            playerInput = new PlayerInput();
        }

        playerInput.Player.Enable();
    }
}
