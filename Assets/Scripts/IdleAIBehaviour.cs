using UnityEngine;

using System.Collections.Generic;

interface IKillable
{
    void Die();
}

[RequireComponent(typeof(DropBehaviour))]
public class IdleAIBehaviour : MonoBehaviour, IKillable
{
    [SerializeField]
    private float maxDistanceSqrFromBombers;

    private List<Transform> bomberTransforms;
    private Transform bomberToFollow;
    private DropBehaviour dropBehaviour;

    private void Awake()
    {
        dropBehaviour = GetComponent<DropBehaviour>();
    }

    private void Start()
    {
        // TODO(Anil): Get all bombers from GameManager or something
        bomberTransforms = new List<Transform>();
        bomberTransforms.Add(GM.Instance.Player.transform);

        InvokeRepeating("GetClosestBomber", 0, 0.05f);
    }

    private void FixedUpdate()
    {
        if (bomberToFollow)
        {
            transform.Translate((bomberToFollow.position - transform.position) * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bomber"))
        {
            // TODO(Anil): PoolManager integration
            Destroy(gameObject);

            collision.GetComponent<IKillable>().Die();
        }
    }

    private void GetClosestBomber()
    {
        if (!bomberToFollow)
        {
            float closestDistanceSqr = float.MaxValue;
            Transform closestTransform = null;

            for (int i = 0; i < bomberTransforms.Count; i++)
            {
                float currentDistanceSqr = Vector3.SqrMagnitude(transform.position - bomberTransforms[i].position);
                if (currentDistanceSqr <= closestDistanceSqr)
                {
                    closestDistanceSqr = currentDistanceSqr;
                    closestTransform = bomberTransforms[i];
                }
            }

            if (closestDistanceSqr <= maxDistanceSqrFromBombers)
            {
                bomberToFollow = closestTransform;
                Debug.Log("Bomber entered to the idle ai range.");
            }
        }
    }

    void IKillable.Die()
    {
        Debug.Log("asdasd");
        Destroy(this.gameObject);
        for (int i = 0; i < Random.Range(1,6); i++)
        {
            PoolManager.Instance.Spawn("DroppedPieces", transform.position, Quaternion.identity);
        }
    }
}
