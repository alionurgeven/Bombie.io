using UnityEngine;

public class DropBehaviour : MonoBehaviour
{
    public GameObject DropItem(string itemToDrop, Vector3 position, Quaternion rotation)
    {
        if (itemToDrop == "Bomb")
        {
            return PoolManager.Instance.Spawn("Bombs", position, rotation);
        }
        else if (itemToDrop == "Piece")
        {
            return PoolManager.Instance.Spawn("DroppedPieces", position, rotation);
        }
        else
        {
            return null;
        }
    }

    private BombBehaviour bomb;

    public GameObject DropItem(string itemToDrop, Vector3 position, Quaternion rotation, float radius)
    {
        bomb = PoolManager.Instance.Spawn("Bombs", position, rotation).GetComponent<BombBehaviour>();
        bomb.SetColliderSize(radius);
        return bomb.gameObject;
    }
}
