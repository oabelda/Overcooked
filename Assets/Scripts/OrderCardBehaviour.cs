using UnityEngine;
using UnityEngine.UI;

public class OrderCardBehaviour : MonoBehaviour
{
    Order order;
    Slider timeSlider;
    [SerializeField] Image recipe;
    [SerializeField] Image[] toppings;

    void Start()
    {
        timeSlider = GetComponentInChildren<Slider>();

        gameObject.SetActive(order != null);
    }

    public void SetOrder(Order order)
    {
        if (this.order != null)
        {
            this.order.OnOrderFailed -= Order_OrderFailed;
            this.order.OnOrderDelivered -= Order_OnOrderDelivered;
        }

        this.order = order;

        if (this.order != null)
        {
            // Visual set the order
            SetVisuals();

            // Link the events
            this.order.OnOrderFailed += Order_OrderFailed;
            this.order.OnOrderDelivered += Order_OnOrderDelivered;

            // Make sure is the last of the active ones
            transform.SetAsLastSibling();

            // Show the card
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void SetVisuals()
    {
        recipe.sprite = order.GetSprite();

        var toppingsSprites = order.GetToppingsSprites();
        int toppingCount = toppingsSprites?.Length ?? 0;

        for (int i = 0; i < this.toppings.Length; ++i)
        {
            bool hasTopping = i < toppingCount;
            this.toppings[i].gameObject.SetActive(hasTopping);

            if (hasTopping)
            {
                this.toppings[i].sprite = toppingsSprites[i];
                
            }
        }
    }

    private void Order_OnOrderDelivered(Order order)
    {
        SetOrder(null);
    }

    private void Order_OrderFailed(Order order)
    {
        Debug.Log("Se ha fallado este pedido: " + order.GetNameString());
    }

    void Update()
    {
        UpdateTimeSlider(order.GetProgress());
    }

    public void UpdateTimeSlider(float time)
    {
        if (!timeSlider) return;
        
        timeSlider.value = time;
        timeSlider.fillRect.GetComponent<Image>().color =
            Color.Lerp(Color.green, Color.red, time);
    }

}
