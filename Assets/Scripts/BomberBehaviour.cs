using UnityEngine;

using System.Collections;

[RequireComponent(typeof(Bomber))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DropBehaviour))]
public class BomberBehaviour : MonoBehaviour, IKillable
{
    [SerializeField]
    protected float movementSpeed;

    protected Bomber bomber;
    protected Rigidbody2D rb2d;
    protected DropBehaviour dropBehaviour;

    protected bool canFire, canBoost, isBoosting;

    protected void Awake()
    {
        bomber = GetComponent<Bomber>();
        rb2d = GetComponent<Rigidbody2D>();
        dropBehaviour = GetComponent<DropBehaviour>();
    }

    protected void OnEnable()
    {
        movementSpeed = 7.0f;

        canFire = true;
        canBoost = false;
        isBoosting = false;
    }

    protected int score;
    protected float CalculateBombRadius()
    {
        score = bomber.GetScore();
        if (score < 50)
        {
            return 2f;
        }
        else if (score >= 50 && score <= 200)
        {
            return score * (2f / 50f);
        }
        else
        {
            return 8f;
        }
    }

    protected IEnumerator FireCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(3f);
        canFire = true;
    }

    public virtual void Die ()
    {
        return;
    }
}
