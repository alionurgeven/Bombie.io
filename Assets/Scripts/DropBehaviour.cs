using UnityEngine;

public class DropBehaviour : MonoBehaviour
{
    public void DropItem(GameObject itemToDrop, Vector3 position, Quaternion rotation)
    {
        Instantiate(itemToDrop, position, rotation);
    }
}
