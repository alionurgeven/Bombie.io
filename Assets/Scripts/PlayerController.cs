using UnityEngine;
using CnControls;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private GameObject bombPrefab;

    private Rigidbody2D rb2d;
    private DropBehaviour dropBehaviour;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        dropBehaviour = GetComponent<DropBehaviour>();
    }

    private void Update()
    {
        float horizontal = CnInputManager.GetAxisRaw("Horizontal");
        float vertical = CnInputManager.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical);
        direction.Normalize();
        direction *= movementSpeed;

        rb2d.velocity = direction;

        if (CnInputManager.GetButtonDown("Jump"))
        {
            dropBehaviour.DropItem(bombPrefab, transform.position, Quaternion.identity);
        }
    }


}
