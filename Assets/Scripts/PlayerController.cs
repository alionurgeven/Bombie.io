using UnityEngine;
using CnControls;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IKillable
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private GameObject bombPrefab;

    [SerializeField]
    private GameObject piecePrefab;

    private Rigidbody2D rb2d;
    private DropBehaviour dropBehaviour;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        dropBehaviour = GetComponent<DropBehaviour>();
    }

    private GameObject activeBomb;
    private float bombAreaRadiusToSend, pieceDropTime;
    private void Update()
    {
        pieceDropTime += Time.deltaTime;
        float horizontal = CnInputManager.GetAxisRaw("Horizontal");
        float vertical = CnInputManager.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical);
        direction.Normalize();
        direction *= movementSpeed;

        rb2d.velocity = direction;

        if (CnInputManager.GetButtonDown("Jump"))
        {
            // bomba bırakıldığı zaman player'ın score'una göre oluşucak circle collider'ın radiusunu belirler 

            // TODO - Ali player scoreları geldiği zaman ona göre ayarlı olan bir radius denklemine göre dengele
            bombAreaRadiusToSend = Random.Range(1f,10f);

            //bırakılacak bombayı oluşturur
            activeBomb = dropBehaviour.DropItem(bombPrefab, transform.position, Quaternion.identity);

            //bombanın radiusunu set eder
            activeBomb.GetComponent<BombBehaviour>().SetColliderSize(bombAreaRadiusToSend);
        }
        // TODO - Ali boost should be down to a limit of total score
        else if (CnInputManager.GetButton("Fire2") && pieceDropTime > 0.3f && rb2d.velocity != Vector2.zero )
        {
            pieceDropTime = 0f;
            dropBehaviour.DropItem(piecePrefab, transform.position, Quaternion.identity);
        }
        if (CnInputManager.GetButtonDown("Fire2"))
        {
            movementSpeed *= 2f;
        }
        if (CnInputManager.GetButtonUp("Fire2"))
        {
            movementSpeed /= 2f;
        }
    }

    public void Die()
    {
        dropBehaviour.DropItem(piecePrefab, transform.position, Quaternion.identity);
        // TODO(Anil): PoolManager integration
        Destroy(gameObject);
    }
}
