using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ItemData itemData;

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item - " + itemData.name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.Instance.AddItem(itemData);
            Destroy(this.gameObject);
        }
    }
}