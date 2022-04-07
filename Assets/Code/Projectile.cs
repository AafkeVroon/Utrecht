using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("How much damage the bullet does")]
    [SerializeField] private float damage = 1;
    [Tooltip("How fast the bullet goes")]
    [SerializeField] private float bulletSpeed = 5;

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    private void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("NotHittable"))
            Destroy(gameObject);
    }
}
