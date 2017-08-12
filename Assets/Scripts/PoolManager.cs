using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPool
    {
		// Gameobject to pool
        public GameObject prefab;

		// Maximum instances of the gameobject
        public int maximumInstances;

		// Name of the pool
        public string poolName;

		// List to hold all instances of the object
        private List<GameObject> objectList;
        

		/// <summary>
		/// Initialize the pool with creating instances of the gameobject and a container
		/// for the hieararchy
		/// </summary>
        public void InitializePool()
        {
			// Create the list and container for the objects
            objectList = new List<GameObject>();
            GameObject pool = new GameObject("[" + poolName + "]");

			// Reference to the created instance
            GameObject clone;

            for (int i = 0; i < maximumInstances; i++)
            {
				// Create the gameobject
                clone = Instantiate(prefab);

				// Deactivate and add to the container and list
                clone.SetActive(false);
                clone.transform.parent = pool.transform;
                
                objectList.Add(clone);
            }
        }

		/// <summary>
		/// Get the next gameobject that can be spawned from the pool
		/// </summary>
		/// <returns>Next gameobject to spawn</returns>
        public GameObject GetNextObject()
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                if (!objectList[i].activeInHierarchy)
                {
                    return objectList[i];
                }
            }

            //foreach (GameObject obj in objectList)
            //{
            //    if (!obj.activeInHierarchy)
            //    {
            //        return obj;
            //    }
            //}

            return null;
        }

		// Properties
        public int MaximumInstances { get { return maximumInstances; } }
        public string PoolName { get { return poolName; } set { poolName = value; } }
    }

	// List to hold all the pools for the game
    public List<ObjectPool> pools;

	public static PoolManager Instance;

    private void Awake()
    {
		// Singleton pattern
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

        // Initialize all the pools
        for (int i = 0; i < pools.Count; i++)
        {
            pools[i].InitializePool();
        }
        //foreach (ObjectPool pool in pools)
        //{
        //    pool.InitializePool();
        //}
    }

	/// <summary>
	/// Spawn the next gameobject at its current place 
	/// </summary>
	/// <param name="poolName">Name of the pool to get the gameobject</param>
	/// <returns>Spawned gameobject</returns>
    public GameObject Spawn(string poolName)
    {
		// Get the pool with the given pool name
        ObjectPool pool = GetObjectPool(poolName);

        if (pool == null)
        {
            //Debug.LogErrorFormat("Cannot find the object pool with name %s", poolName);

            return null;
        }
		
		// Get the next object from the pool
        GameObject clone = pool.GetNextObject();

        if (clone == null)
        {
            //Debug.LogError("Scene contains maximum number of instances.");

            return null;
        }

		// Spawn the gameobject
        clone.SetActive(true);

        return clone;
    }

	/// <summary>
	/// Spawn the next gameobject from the given pool with the specified position and
	/// rotation
	/// </summary>
	/// <param name="poolName">Name of the pool</param>
	/// <param name="position">Position of the spawned gameobject</param>
	/// <param name="rotation">Rotation of the spawned gameobject</param>
	/// <returns>Spawned gameobject</returns>
    public GameObject Spawn(string poolName, Vector3 position, Quaternion rotation)
    {
        // Spawn the gameobject
        GameObject clone = Spawn(poolName);
        

		// Set its position and rotation
        if (clone != null)
        {
            clone.transform.position = position;
            clone.transform.rotation = rotation;
           
            return clone;
        }

        return null;
    }

	/// <summary>
	/// Spawn the next gameobject from the given pool to the random location between two
	/// vectors and given rotation
	/// </summary>
	/// <param name="poolName">Name of the pool</param>
	/// <param name="minVector">Minimum vector position for the spawned gameobject</param>
	/// <param name="maxVector">Maximum vector position for the spawned gameobject</param>
	/// <param name="rotation">Rotation of the spawned gameobject</param>
	/// <returns>Spawned gameobject</returns>
    public GameObject Spawn(string poolName, Vector3 minVector, Vector3 maxVector, Quaternion rotation)
    {
		// Determine the random position
        float x = Random.Range(minVector.x, maxVector.x);
        float y = Random.Range(minVector.y, maxVector.y);
        float z = Random.Range(minVector.z, maxVector.z);
        Vector3 newPosition = new Vector3(x, y, z);

		// Spawn the next gameobject
        return Spawn(poolName, newPosition, rotation);
    }

	/// <summary>
	/// Despawn the given gameobject from the scene
	/// </summary>
	/// <param name="obj">Gameobject to despawn</param>
    public void Despawn(GameObject obj)
    {
            obj.SetActive(false);
    }
    Vector3 spawnPosition;
    IEnumerator dieRoutine(GameObject gameobj)
    {
        yield return new WaitForSeconds(1f);
        gameobj.SetActive(false);
        //spawnPosition = new Vector3(Random.Range(GameManager.Instance.min_x_position + 20, GameManager.Instance.max_x_position -20), Random.Range(GameManager.Instance.min_y_position+20, GameManager.Instance.max_y_position-20), 1);
        //Spawn("JellyFishes", spawnPosition, Quaternion.identity);
        gameobj.SetActive(true);
    }
    /// <summary>
    /// Despawn the given gameobject with a delay
    /// </summary>
    /// <param name="obj">Gameobject to despawn</param>
    /// <param name="delay">Delay for the despawning</param>
    public void Despawn(GameObject obj, float delay)
    {
        Invoke("Despawn", delay);
    }

	/// <summary>
	/// Get the object pool reference from the pool list with the given pool name
	/// </summary>
	/// <param name="poolName">Name of the pool</param>
	/// <returns>ObjectPool object with the given name</returns>
    private ObjectPool GetObjectPool(string poolName)
    {
        // Find the pool with the given name
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].PoolName.Equals(poolName))
            {
                return pools[i];
            }
        }

        //foreach (ObjectPool pool in pools)
        //{
        //    if (pool.PoolName.Equals(poolName))
        //    {
        //        return pool;
        //    }
        //}

        return null;
    }
}
