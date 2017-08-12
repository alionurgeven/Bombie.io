using UnityEngine;
using CnControls;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    private Rigidbody2D rb2d;
    private DropBehaviour dropBehaviour;
    private Player playerRef;
    private IScoreBehavior playerScoreBehaviour;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        dropBehaviour = GetComponent<DropBehaviour>();
        playerRef = GetComponent<Player>();
        playerScoreBehaviour = GetComponent<IScoreBehavior>();
    }

    private void OnEnable()
    {
        movementSpeed = 7f;
        canFire = true;
        canBoost = false;
        isBoosting = false;
    }

    public bool canFire, canBoost, isBoosting;
    private GameObject activeBomb;
    public Image boostButtonImage;
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


        if (CnInputManager.GetButtonDown("Jump") && canFire)
        {
            // bomba bırakıldığı zaman player'ın score'una göre oluşucak circle collider'ın radiusunu belirler 

            // TODO - Ali player scoreları geldiği zaman ona göre ayarlı olan bir radius denklemine göre dengele
            bombAreaRadiusToSend = CalculateBombRadius();

            //bırakılacak bombayı oluşturur
            activeBomb = dropBehaviour.DropItem("Bomb", transform.position, Quaternion.identity, bombAreaRadiusToSend);
            StartCoroutine(FireCooldown());
            
        }
        else if (CnInputManager.GetButton("Fire2") && pieceDropTime > 0.3f && rb2d.velocity != Vector2.zero && canBoost)
        {
            pieceDropTime = 0f;
            dropBehaviour.DropItem("Piece", transform.position, Quaternion.identity);
            playerScoreBehaviour.DropScore(4);
            
        }
        else if (CnInputManager.GetButton("Fire2") && isBoosting && !canBoost)
        {
            movementSpeed /= 2f;
            isBoosting = false;
        }
        if (CnInputManager.GetButtonDown("Fire2") && canBoost)
        {
            isBoosting = true;
            movementSpeed *= 2f;
        }

        if (CnInputManager.GetButtonUp("Fire2"))
        {
            if (isBoosting)
            {
                movementSpeed /= 2f;
            }
        }

        if (playerRef.GetScore() > 20)
        {
            canBoost = true;
            boostButtonImage.color = new Color(255f, 255f, 255f, 1f);
        }
        else
        {
            canBoost = false;
            boostButtonImage.color = new Color(255f, 255f, 255f, 0.2f);
        }
    }

    
    private int scoreRef;
    private float CalculateBombRadius()
    {
        scoreRef = playerRef.GetScore();
        if (scoreRef < 50)
        {
            return 2f;
        }
        else if (scoreRef >= 50 && scoreRef <= 550)
        {
            return scoreRef * (2f / 50f);
        }
        else
        {
            return 22f;
        }
    }

     private IEnumerator FireCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(3f);
        canFire = true;
    }
}
