using UnityEngine;

using System.Collections.Generic;

[RequireComponent(typeof(DropBehaviour))]
public class IdleAIBehaviour : MonoBehaviour
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
        bomberTransforms.Add(GameObject.Find("Player").transform);

        InvokeRepeating("GetClosestBomber", 0, 0.1f);
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

            // TODO(Anil): Kill bomber
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
}
