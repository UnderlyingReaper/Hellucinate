using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialTexts : MonoBehaviour, IInteractible
{
    public bool allow = true;
    public string actionName;
    CanvasGroup _canvas;


    void Start()
    {
        _canvas = GetComponent<CanvasGroup>();
        HideCanvas();
    }

    public void HideCanvas()
    {
        if(allow) _canvas.DOFade(0, 1);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(context.action.name == actionName)
        {
            allow = false;
            _canvas.DOFade(0, 1);
            Destroy(gameObject, 1.1f);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void OnInteractKeyDown()
    {

    }

    public void OnInteractKeyUp()
    {

    }

    public void ShowCanvas()
    {
        if(allow) _canvas.DOFade(1, 1);
    }
}
