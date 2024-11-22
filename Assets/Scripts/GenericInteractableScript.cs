using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericInteractableScript : MonoBehaviour
{
    [SerializeField] private string InteractableID;
    public abstract void Interact(GameObject Player);

    public virtual string GetInteractableID(){
        return InteractableID;
    }
}
