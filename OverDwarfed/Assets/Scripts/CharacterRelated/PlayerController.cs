using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int id = 0;
    private Rigidbody2D rb;
    private Vector2 inputs;
    private readonly float speed = 5f;
    private PlayerHealth health;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<PlayerHealth>();
    }
    void Update()
    {
        inputs.x = Input.GetAxisRaw("Horizontal");
        inputs.y = Input.GetAxisRaw("Vertical");
        inputs.Normalize();

        rb.velocity = inputs * speed;
    }
    private void FixedUpdate()
    {
        PlayersPositions.instance.playerPositions[id] = new Vector3(transform.position.x, transform.position.y, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            ItemSpawned itemSpawned = collision.GetComponent<ItemSpawned>();
            int capacity = PlayerInventory.instance.CanAddCapacity(itemSpawned.item);

            if(capacity >= itemSpawned.amount)
            {
                PlayerInventory.instance.AddItem(itemSpawned.item, itemSpawned.amount);
                Destroy(collision.gameObject);
            }
            else
            {
                PlayerInventory.instance.AddItem(itemSpawned.item, capacity);
                itemSpawned.amount -= capacity;
                itemSpawned.UpdateAmountDisplay();
            }
        }
    }
}

