using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject AttackHitbox;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask enemyLayers;

    //public Transform attackPoint;
    //[SerializeField] private float attackRange = 0.5f;
    [Header("Combat Stats")]
    [SerializeField] private int attackDamage = 40;
    [SerializeField] private float knockbackPlayer = 10f;
    [SerializeField] private float attackCooldown = 2;
    private int attackDirection;
    private bool canAttack = true;

    private void Start()
    {
        DisableHitBox();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.X)) && canAttack)
        {
            StartCoroutine(AttackController());
        }
    }

    IEnumerator AttackController()
    {
        canAttack = false;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetTrigger("attackUp");
            attackDirection = 0;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            anim.SetTrigger("attackDown");
            attackDirection = -1;
        }
        else
        {
            anim.SetTrigger("attack");
            attackDirection = 0;
        }
        /*  
            *  Attack using attackPoint collider
            
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage);

        }
        */
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage, transform.right);
            Vector2 knockDirection = other.transform.position - transform.position;
            other.gameObject.GetComponent<EnemyController>().Knockback(knockDirection);
            if (attackDirection == -1)
            {
                player.GetComponent<PlayerController>().Knockback(knockbackPlayer, Vector2.up);
            }
        }
    }

    /*
     * Attack using attackPoint collider
     * Draws the attackPoint on the screen
    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawSphere(attackPoint.position, attackRange);

    }
    */

    public void EnableHitBox()
    {
        AttackHitbox.SetActive(true);
    }

    public void DisableHitBox()
    {
        AttackHitbox.SetActive(false);
    }
}
