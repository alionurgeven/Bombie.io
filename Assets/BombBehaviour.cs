using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    private Collider2D bombCollider;

    private void Awake()
    {
        bombCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        Invoke("OpenExplosionCollider", 3.0f);
        Destroy(gameObject, 4.0f);
    }

    private void OpenExplosionCollider()
    {
        bombCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collide etti anacim");
    }
}
