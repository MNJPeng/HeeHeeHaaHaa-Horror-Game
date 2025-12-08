using UnityEngine;

// anything that needs to be interacted with can inherit from this class
public abstract class Interactable : MonoBehaviour
{
    protected bool canInteract = true;

    public abstract string GetInteractTip();
    public abstract void Interact();
    public abstract bool CheckIsInteractable();
}
