using UnityEngine;
using UnityEngine.UI;

public class OrdersPanelBehaviour : MonoBehaviour
{
    [SerializeField] GameObject[] orderPanels;

    public void Start()
    {
        for (int i = 0; i < orderPanels.Length; ++i)
        {
            orderPanels[i].SetActive(false);
        }
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

    }
}
