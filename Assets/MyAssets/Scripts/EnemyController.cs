using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] protected GameObject player;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask playerLayer;

    protected Rigidbody2D rb;

    [Header("Enemy Stats")]
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected float fov = 4f;
    [SerializeField] protected float speed;
    [SerializeField] protected int attackDamage = 10;
    [SerializeField] protected float attackRange = 0.5f;
    [SerializeField] protected float attackCooldown = 2;
    [SerializeField] protected float knockbackEnemy = 0.5f;
    [SerializeField] protected Transform[] patrolPoints;
    protected int patrolDestination = 0;
    protected int currentHealth;
    protected bool isDead;

    [Header("Particles")]
    [SerializeField] protected ParticleSystem damageParticles;
    protected ParticleSystem damageParticlesInstance;

    protected bool canAttack = true;
    protected bool isFacingLeft = true;
    protected float isMoving = 0;
    protected float direction = 0;
    protected float distance;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        currentHealth = maxHealth;
        isDead = false;

        Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        Collider2D enemyCollider = GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(playerCollider, enemyCollider);
    }

    protected virtual void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        MovementController();
    }

    protected virtual void MovementController() 
    {
        if (distance < fov)
        {
            isMoving = 1;
            anim.SetFloat("isMoving", isMoving);
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            direction = player.transform.position.x - transform.position.x;

        }
        else if (patrolDestination == 0)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, patrolPoints[0].position, speed * Time.deltaTime);
            isMoving = 1;
            anim.SetFloat("isMoving", isMoving);
            direction = patrolPoints[0].position.x - transform.position.x;

            if (Vector2.Distance(transform.position, patrolPoints[0].position) < .5f)
            {
                patrolDestination = 1;
            }
        }
        else if (patrolDestination == 1)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, patrolPoints[1].position, speed * Time.deltaTime);
            isMoving = 1;
            anim.SetFloat("isMoving", isMoving);
            direction = patrolPoints[1].position.x - transform.position.x;
            if (Vector2.Distance(transform.position, patrolPoints[1].position) < .5f)
            {
                patrolDestination = 0;
            }
        }
        else if (isMoving == 1)
        {
            isMoving = 0;
            anim.SetFloat("isMoving", isMoving);
        }

        FixDirection();
    }

    protected void FixDirection()
    {
        if (direction < 0 && !isFacingLeft) Flip();
        if (direction > 0 && isFacingLeft) Flip();
    }
    protected void Flip()
    {
        isFacingLeft = !isFacingLeft;
        transform.Rotate(0, 180, 0);
    }
    public void TakeDamage(int damage, Vector2 attackDirection)
    {
        currentHealth -= damage;
        anim.SetTrigger("Hurt");
        SpawnDamageParticles(attackDirection);

        if (currentHealth < 0)
        {
            Die();
            isDead = true;
        }
    }

    public void Knockback(Vector2 direction)
    {
        if (currentHealth > 0)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction.normalized * knockbackEnemy, ForceMode2D.Impulse);
        }
    }

    void Die()
        {
        anim.SetBool("isDead", true);
        gameObject.layer = LayerMask.NameToLayer("UI");
        rb.bodyType = RigidbodyType2D.Static;
        this.enabled = false;
        }

    protected void SpawnDamageParticles(Vector2 attackDirection)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, attackDirection);
        damageParticlesInstance = Instantiate(damageParticles, transform.position, rotation);
    }

    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawSphere(attackPoint.position, attackRange);

    }

    protected virtual IEnumerator AttackController()
    {
        canAttack = false;

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Attack(other);
        }
    }

    protected void Attack(Collider2D other)
    {
        if (isDead || !canAttack) return;

        other.GetComponent<PlayerController>().TakeDamage(attackDamage, transform.right);
        StartCoroutine(AttackController());
    }

}
