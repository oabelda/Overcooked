using UnityEngine;

[RequireComponent(typeof(InteractableBehaviour))]
public class FocusApplianceVisual : MonoBehaviour
{
    [SerializeField] GameObject[] visuals;
    InteractableBehaviour interactable;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactable = GetComponent<InteractableBehaviour>();
        interactable.OnFocusChange += OnFocusChange;
    }

    private void OnFocusChange(bool focused)
    {
        for (int i = 0; i < visuals.Length; ++i)
            visuals[i].SetActive(focused);
    }
}
