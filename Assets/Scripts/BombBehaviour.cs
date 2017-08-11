using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BombBehaviour : MonoBehaviour
{
    private CircleCollider2D bombCollider;
    private Transform bombAreaTransform;

    public void SetColliderSize(float colliderSize)
    {
        bombCollider.radius = colliderSize;
        maximumScale = bombAreaTransform.localScale * bombCollider.radius * 2;
        minimumScale = bombAreaTransform.localScale / bombCollider.radius * 2;
        StartCoroutine(BombAreaBehavior());
    }

    private void Awake()
    {
        bombCollider = GetComponent<CircleCollider2D>();
        bombAreaTransform = transform.GetChild(1);
    }

    private void OnEnable()
    {
        Invoke("OpenExplosionCollider", 2.8f);
        Destroy(gameObject, 3.0f);
    }

    private void OpenExplosionCollider()
    {
        bombCollider.enabled = true;
    }

    // Bombanın patlamadan önce yanıp sönme animasyonu

    Vector3 maximumScale, minimumScale;
    private IEnumerator BombAreaBehavior()
    {
        bombAreaTransform.DOScale(maximumScale, 0.5f);
        yield return new WaitForSeconds(0.5f);
        bombAreaTransform.DOScale(minimumScale, 0.5f);
        yield return new WaitForSeconds(0.5f);
        bombAreaTransform.DOScale(maximumScale, 0.5f);
        yield return new WaitForSeconds(0.5f);
        bombAreaTransform.DOScale(minimumScale, 0.5f);
        yield return new WaitForSeconds(0.5f);
        bombAreaTransform.DOScale(maximumScale, 0.5f);
        yield return new WaitForSeconds(1f);
        bombAreaTransform.DOScale(minimumScale,0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with: " + other.name);

        if (other.CompareTag("IdleAI"))
        {
            IKillable killable = other.GetComponent<IKillable>();
            killable.Die();
        }
    }
}
