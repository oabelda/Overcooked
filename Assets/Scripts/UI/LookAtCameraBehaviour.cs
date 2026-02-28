using UnityEngine;

public class LookAtCameraBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.forward = Camera.main.transform.forward;
    }
}
