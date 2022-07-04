using interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class Interact : MonoBehaviour
    {
        private Transform cameraTransform;
        [SerializeField] private InputActionAsset playerInput;

        private IInteractable currentInteractable;

        private void Start()
        {
            cameraTransform = Camera.main.transform;
            
            var interact = playerInput["Interact"];
            interact.performed += InteractWithObject;
        }

        private void Update()
        {
            GameObject hitObject;
            RaycastHit hit;
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit))
            {
                hitObject = hit.transform.gameObject;

                if (hitObject.GetComponent(typeof(IInteractable)))
                {
                    currentInteractable = hitObject.GetComponent<IInteractable>();
                }
            }
            else
            {
                currentInteractable = null;
            }
        }
        
        private void InteractWithObject(InputAction.CallbackContext obj)
        {
            if (currentInteractable == null) return;
            
            currentInteractable.Interact();
        }
    }
}