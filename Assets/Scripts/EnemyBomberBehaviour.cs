using UnityEngine;


public class EnemyBomberBehaviour : BomberBehaviour
{


    PieceBehavior piece;
    public override void Die()
    {
        piece = 
            dropBehaviour.
            DropItem("Piece", transform.position, Quaternion.identity).
            GetComponent<PieceBehavior>();

        piece.PopPiece(Vector2.up * Random.Range(0, 10) + Vector2.right * Random.Range(0, 10));
    }
}
