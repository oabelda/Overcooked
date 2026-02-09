using UnityEngine;

public class DeactiveOnLevelEndedBehaviour : MonoBehaviour
{
    [SerializeField] MonoBehaviour component;
    [SerializeField] bool fullGameObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManagerBehaviour.RegisterOnLevelEnded(Deactivate);
    }

    // Update is called once per frame
    void Deactivate()
    {
        if (fullGameObject)
        {
            component.gameObject.SetActive(false);
        }
        else
        {
            component.enabled = false;
        }
    }
}
