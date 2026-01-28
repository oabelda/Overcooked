using UnityEngine;

public class PickableItemBehaviour : MonoBehaviour
{
    IPickableParentBehaviour parent;
    [SerializeField] new string name;
    [SerializeField] Sprite sprite;

    public string GetName()
    {
        return name;
    }
    public void SetParent(IPickableParentBehaviour parent)
    {
        if (this.parent == parent)
            return;

        if (this.parent != null)
        {
            this.parent.SetItem(null);
        }
           
        this.parent = parent;

        if (parent != null)
        {
            parent.SetItem(this);
            this.transform.parent = this.parent.GetPlaceholder();
            this.transform.localPosition = Vector3.zero;
        }
        else
        {
            this.transform.parent = null;
        }
    }

    public void ClearParent()
    {
        if (this.parent != null)
        {
            this.parent.SetItem(null);
            this.transform.parent = null;
        }
    }

    public IPickableParentBehaviour GetParent()
    {
        return parent;
    }

    public void DestroyItem()
    {
        ClearParent();
        Destroy(this.gameObject);
    }

    public bool IsInstanceOf(PickableItemBehaviour prefab)
    {
        return this.gameObject.name.Contains(prefab.gameObject.name);
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
}
