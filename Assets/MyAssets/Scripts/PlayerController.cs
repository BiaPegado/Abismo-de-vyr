using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0f;

    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField]  private float jumpForce;

    [Header("Collision Check")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask isGround;
    private bool groundDetected;

    private bool isFacingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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

        rb.linearVelocity = new Vector2(h * speed, rb.linearVelocity.y); // Para jogos de plataforma

        if (Input.GetKeyDown(KeyCode.Z)) Jump();

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