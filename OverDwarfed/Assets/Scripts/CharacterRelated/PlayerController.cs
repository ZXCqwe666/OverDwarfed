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
            if (PlayerInventory.instance.CanAdd(item.id)) // can add allows 1
            {
                PlayerInventory.instance.AddItem(item.id, item.amount);
                Destroy(collision.gameObject);
            }
        }
    }
}
