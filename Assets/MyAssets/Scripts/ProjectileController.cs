using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Transform target;
    float moveSpeed;

    private float distanceToDestroy = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirNormalized = (target.position - transform.position).normalized;

        transform.position += moveDirNormalized * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < distanceToDestroy)
        {
            Destroy(gameObject);
        }
    }

    public void InitializeProjectile(Transform target, float moveSpeed)
    {
        this.target = target;
        this.moveSpeed = moveSpeed;

    }
}
