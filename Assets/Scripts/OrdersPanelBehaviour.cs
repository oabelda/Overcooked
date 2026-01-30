using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OrdersPanelBehaviour : MonoBehaviour
{
    [SerializeField] GameObject[] orderPanels;
    [SerializeField] Slider pressureSlider;

    void Start()
    {
        for (int i = 0; i < orderPanels.Length; ++i)
        {
            orderPanels[i].SetActive(false);
        }

        GameManagerBehaviour.RegisterOnOrderRemoved(RemoveOrder);
        GameManagerBehaviour.RegisterOnOrderAdded(AddOrder);
    }

    public void AddOrder(Order newOrder)
    {
        for (int i = 0; i < orderPanels.Length; ++i)
        {
            if (!orderPanels[i].activeSelf)
            {
                orderPanels[i].SetActive(true);
                orderPanels[i].GetComponentInChildren<Text>()
                    .text = newOrder.GetNameString();
                return;
            }
        }
    }

    public void RemoveOrder(int index)
    {
        int auxIndex = index;

        while (auxIndex+1 < orderPanels.Length && 
            orderPanels[auxIndex+1].activeSelf) {

            orderPanels[auxIndex].GetComponentInChildren<Text>().text =
                orderPanels[auxIndex + 1].GetComponentInChildren<Text>().text;

            ++auxIndex;
        }

        orderPanels[auxIndex].SetActive(false);
    }

    public void UpdatePressureSlider(float pressure)
    {
        if (!pressureSlider) return;

        pressureSlider.value = pressure;
        pressureSlider.fillRect.GetComponent<Image>().color =
            Color.Lerp(Color.green, Color.red, pressure);
    }

    public void Test()
    {
        Debug.Log("Estamos probando el delegado");
    }
}
