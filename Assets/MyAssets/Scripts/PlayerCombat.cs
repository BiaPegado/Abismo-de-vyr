using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator anim;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;

    // Update is called once per frame
    void Update()
    {
        AttackController();
    }

    public void AttackController()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetTrigger("attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage);

            }
        }

    }

    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawSphere(attackPoint.position, attackRange);

    }
}
