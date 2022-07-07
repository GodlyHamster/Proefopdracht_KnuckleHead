using System.Collections;
using interfaces;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private Quaternion startRotation;
    [SerializeField] private Vector3 endRotation;

    private bool _isActive;
    
    private void Start()
    {
        startRotation = transform.rotation;
        endRotation += startRotation.eulerAngles;
    }
    
    public void Interact()
    {
        _isActive = !_isActive;
        SetDoorState(!_isActive);
        OnLeverStateChange(!_isActive, _isActive);
    }

    private void SetDoorState(bool newState)
    {
        _isActive = newState;
    }

    private void OnLeverStateChange(bool oldState, bool newState)
    {
        if (newState)
        {
            StopAllCoroutines();
            StartCoroutine(SwitchLever(gameObject, startRotation));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(SwitchLever(gameObject, Quaternion.Euler(endRotation)));
        }
    }
    
    //temporary script animation
    private IEnumerator SwitchLever(GameObject target, Quaternion targetRot)
    {
        float percent = 0f;
        Quaternion startRot = target.transform.rotation;

        while (percent < 1)
        {
            percent += Time.deltaTime;
            if (percent > 1) percent = 1;

            target.transform.rotation = Quaternion.Lerp(startRot, targetRot, percent);

            yield return null;
        }
    }
}
