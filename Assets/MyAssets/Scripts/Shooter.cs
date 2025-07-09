using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform target;

    [SerializeField] private float projectileMoveSpeed;
    [SerializeField] private float shootRate;
    private float shootTimer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            shootTimer = shootRate;
            ProjectileController projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<ProjectileController>();
            projectile.InitializeProjectile(target, projectileMoveSpeed);
        }
    }
}
