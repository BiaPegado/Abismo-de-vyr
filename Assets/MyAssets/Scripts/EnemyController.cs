using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator anim;
    public int maxHealth = 100;
    int currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        anim.SetTrigger("Hurt");

        if (currentHealth < 0)
        {
            Die();
        }
    }

    void Die()
        {
        anim.SetBool("isDead", true);
        gameObject.layer = LayerMask.NameToLayer("Default");
        this.enabled = false;
        }
}
