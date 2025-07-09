using UnityEngine;
using System.Collections;

public class MageController : EnemyController
{
    [SerializeField] private EnemyShooter enemyShooter;
    [SerializeField] private float maxDistance;
    [SerializeField] private float pauseAttackTime = 5;

    [Header("Collision Check")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask isGround;
    private bool groundDetected;

    private bool isFirstTime = true;
    private bool attackFinished = false;
    private bool isAttacking = false;  // Controle para garantir que o ataque não seja repetido enquanto já estiver em execução
    int currentAttack;
    protected override void Start()
    {
        base.Start();
        patrolDestination = -1;
    }

    protected override void Update()
    {
        if (isDead) return;
        if (currentHealth < maxHealth * 0.5)
        {
            Debug.Log("chegou na metade");
            speed = 4;
            attackCooldown = 2;
            Debug.Log(attackCooldown);
        }

        AnimationControlller();
        CollisionChecks();

        base.Update();
    }

    protected override void MovementController()
    {
        if (distance < maxDistance && isFirstTime)
        {
            isFirstTime = false;
            attackFinished = true;
            Debug.Log("Entrou no raio!");
        }

        if (attackFinished && !isAttacking)
        {
            Debug.Log("Iniciando ataque");
            attackFinished = false;
            currentAttack = Random.Range(0, 2);

            StartCoroutine(Attack(currentAttack));  
        } else if (currentAttack == 0)                      // Movimentação do ataque 0 (Blast)
        {
            if (patrolDestination == 0)
            {
                MoveToPatrolPoint(patrolPoints[0].position);

                if (Vector2.Distance(transform.position, patrolPoints[0].position) < .5f)
                {
                    patrolDestination = 1;
                }
            }
            else if (patrolDestination == 1)
            {
                MoveToPatrolPoint(patrolPoints[1].position);

                if (Vector2.Distance(transform.position, patrolPoints[1].position) < .5f)
                {
                    patrolDestination = 2;
                }
            }
            else if (patrolDestination == 2)
            {
                MoveToPatrolPoint(patrolPoints[2].position);

                if (Vector2.Distance(transform.position, patrolPoints[2].position) < .5f)
                {
                    patrolDestination = 0;
                }
            }
        } else if(currentAttack == 1)
        {
            if (Vector2.Distance(transform.position, patrolPoints[0].position) > .5f)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, patrolPoints[0].position, speed * Time.deltaTime);
                isMoving = 1;
                anim.SetFloat("isMoving", isMoving);
                direction = patrolPoints[0].position.x - transform.position.x;
            }

        }


            FixDirection();
    }

    private void MoveToPatrolPoint(Vector3 position)
    {
        if (attackFinished || isDead) return;
        transform.position = Vector2.MoveTowards(this.transform.position, position, speed * Time.deltaTime);
        isMoving = 1;
        anim.SetFloat("isMoving", isMoving);
        direction = position.x - transform.position.x;
    }
   

    private IEnumerator Attack(int randomAttack)
    {
        if (isDead) yield break;

        Debug.Log("entrou em attack");
        isAttacking = true;  // Marca que o inimigo está atacando

        switch (randomAttack)
        {
            case 0:
                Debug.Log("BLASSSST");
                patrolDestination = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (isDead) yield break;
                    anim.SetTrigger("blast");
                    enemyShooter.SingleBlast();
                    yield return new WaitForSeconds(attackCooldown); 
                }
                break;

            case 1:
                Debug.Log("Parado");
                isMoving = 0;
                anim.SetFloat("isMoving", isMoving);
                for (int i = 0; i < 3; i++)
                {
                    if (isDead) yield break;
                    anim.SetTrigger("blast");
                    enemyShooter.ShotgunBlast();
                    yield return new WaitForSeconds(attackCooldown);  
                }
                break;
        }
        currentAttack = -1;
        attackFinished = true;
        isAttacking = false;  
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void CollisionChecks()
    {
        groundDetected = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGround);
    }

    private void AnimationControlller()
    {
        anim.SetBool("isGrounded", groundDetected);
    }
}
