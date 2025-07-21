using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPos;
    private GameObject player;
    public int damage;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SingleBlast()
    {
        FireInDirection(0f); // Sem offset
    }

    public void ShotgunBlast()
    {
        float[] angles = { -33f, 0f, 33f }; // Espalhamento

        foreach (float angle in angles)
        {
            FireInDirection(angle);
        }
    }

    private void FireInDirection(float angleOffset)
    {
        // Direção base para o player
        Vector3 baseDirection = player.transform.position - bulletPos.position;

        // Rotaciona a direção base
        Vector3 rotatedDirection = Quaternion.Euler(0, 0, angleOffset) * baseDirection;

        // Calcula o target final da bala
        Vector3 targetPosition = bulletPos.position + rotatedDirection.normalized * 10f; // 10f = distância "longa"

        // Instancia a bala
        GameObject newBullet = Instantiate(bullet, bulletPos.position, Quaternion.identity);

        // Passa o target para a bala
        EnemyBulletController bulletController = newBullet.GetComponent<EnemyBulletController>();
        if (bulletController != null)
        {
            bulletController.attackDamage = damage;
            bulletController.SetTarget(targetPosition);
        }
    }
    
}
