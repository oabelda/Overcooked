using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ProgressBarBehaviour : MonoBehaviour
{
    IProcessor processor;
    Slider progressSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        processor = GetComponentInParent<IProcessor>();
        progressSlider = GetComponent<Slider>();
        progressSlider.gameObject.SetActive(false);

        processor.RegisterOnItemPlaced(OnItemPlaced);
        processor.RegisterOnItemProcessed(UpdateSlider);
    }

    public void UpdateSlider(float value)
    {
        progressSlider.value = value;

        progressSlider.gameObject.SetActive(value != 0);
    }

    public void OnItemPlaced(IKitchenProcessing newItem)
    {
        if (newItem == null)
        {
            progressSlider.gameObject.SetActive(false);
        }
        else
        {
            // Ver el valor al que tiene que ponerse el slider
            float progress = newItem.GetProcess();

            UpdateSlider(progress);
        }
    }


}
