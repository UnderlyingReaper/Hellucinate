
using UnityEngine.InputSystem;

public interface IInteractible
{
    public void Interact(InputAction.CallbackContext context);
    public void HideCanvas();
    public void ShowCanvas();

    public void OnInteractKeyUp();
    public void OnInteractKeyDown();
}
