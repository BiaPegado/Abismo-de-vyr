using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Player Stats")]
    [SerializeField] private float jumpForce;
    [SerializeField] private int maxHealth = 200;
    [SerializeField] private float speed = 0f;
    private int currentHealth;

    [Header("Collision Check")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask isGround;
    private bool groundDetected;

    [Header("Particles")]
    [SerializeField] private ParticleSystem damageParticles;
    private ParticleSystem damageParticlesInstance;

    private bool isFacingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float jump = Input.GetAxis("Jump");

        AnimationControls();
        FlipController();
        CollisionChecks();

        rb.linearVelocity = new Vector2(h * speed, rb.linearVelocity.y); 

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Period)) Jump();
        if (Input.GetKeyUp(KeyCode.Z) && rb.linearVelocity.y > 0) rb.linearVelocity = new Vector2(rb.linearVelocity.x, (rb.linearVelocity.y * 0.5f));

    }

    private void AnimationControls()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", groundDetected);
    }

    private void Jump()
    {
        if (groundDetected)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void FlipController()
    {
        if (rb.linearVelocity.x < 0 && isFacingRight)
        {
            Flip();
        } else if (rb.linearVelocity.x > 0 && !isFacingRight)
        {
            Flip();
        }

    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void CollisionChecks()
    {
        groundDetected = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGround);
    }
    public void TakeDamage(int damage, Vector2 attackDirection)
    {
        currentHealth -= damage;
        anim.SetTrigger("Hurt");
        SpawnDamageParticles(attackDirection);
        if (currentHealth < 0)
        {
            Die();
        }
    }

    public void Knockback(float force, Vector2 direction)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    void Die()
    {
        anim.SetTrigger("isDead");
        gameObject.layer = LayerMask.NameToLayer("Ui");
        this.enabled = false;
        Debug.Log("Game over!");
    }

    private void SpawnDamageParticles(Vector2 attackDirection)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector2.left, attackDirection);
        damageParticlesInstance = Instantiate(damageParticles, transform.position, rotation);
    }

}




/*
rb.AddForce(new Vector2(rb.linearVelocity.x, jump * 5)); // Para jogos de plataforma
*/

//rb.linearVelocity = new Vector2(h*speed, v*speed); //Desligar gravidade

/* 
 * Adiciona uma "força" no player
rb.AddForce(new Vector2(h, v));
*/
/*
if (h != 0){
    transform.position = new Vector3(transform.position.x + h*speed,
        transform.position.y, transform.position.z);
}
if ( v != 0) {
    transform.position = new Vector3(transform.position.x, transform.position.y + v*speed, transform.position.z);
}
*/