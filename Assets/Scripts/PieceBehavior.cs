using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceBehavior : MonoBehaviour {

    private CircleCollider2D coll;

    private void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
    }
    private void OnEnable()
    {
        StartCoroutine(OpenColliderAfterDelay());
    }

    private IEnumerator OpenColliderAfterDelay()
    {
        coll.enabled = false;
        yield return new WaitForSeconds(0.2f);
        coll.enabled = true;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().AddScore(2);
            PoolManager.Instance.Despawn(this.gameObject);
        }
    }
}
