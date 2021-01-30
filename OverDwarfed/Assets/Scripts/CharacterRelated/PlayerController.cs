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
            int itemId = collision.GetComponent<Item>().id;
            if (PlayerInventory.instance.CanAdd(itemId))
            {
                PlayerInventory.instance.AddItem(itemId, 1);
                Destroy(collision.gameObject);
            }
        }
    }
}
