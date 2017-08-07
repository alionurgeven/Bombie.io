using UnityEngine;

public class DropBehaviour : MonoBehaviour
{
    public GameObject DropItem(GameObject itemToDrop, Vector3 position, Quaternion rotation)
    {
        return Instantiate(itemToDrop, position, rotation);
    }
}
