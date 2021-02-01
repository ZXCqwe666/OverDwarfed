using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 inputs;
    private readonly float speed = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        inputs.x = Input.GetAxisRaw("Horizontal");
        inputs.y = Input.GetAxisRaw("Vertical");
        inputs.Normalize();

        rb.velocity = inputs * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Item item = collision.GetComponent<Item>();
            int capacity = PlayerInventory.instance.CanAddCapacity(item.id);
            Debug.Log(capacity);

            if(capacity >= item.amount)
            {
                PlayerInventory.instance.AddItem(item.id, item.amount);
                Destroy(collision.gameObject);
            }
            else
            {
                PlayerInventory.instance.AddItem(item.id, capacity);
                item.amount -= capacity;
                item.UpdateAmountDisplay();
            }
        }
    }
}
