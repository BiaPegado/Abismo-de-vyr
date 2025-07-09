using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    [SerializeField] private float speed = 5f;
    [SerializeField] private int attackDamage;

    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        moveDirection = direction.normalized;

        // Rotaciona visualmente a bala
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);

        // Aplica a velocidade
        if (rb != null)
        {
            rb.linearVelocity = moveDirection * speed;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 7f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(attackDamage, other.transform.right);
            }
            Destroy(gameObject);
        }
    }
}
