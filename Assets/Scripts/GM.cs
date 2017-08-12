using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {

    public static GM Instance;
    public GameObject Player;
    public GameObject MainMenu, HUD;

    [SerializeField]
    private int MAX_NUM_NATURALPIECES = 150;

	// Use this for initialization
	private void Start () {
        SpawnNaturalPieces();
        SpawnIdleAI();
	}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private Vector3 MAX_MAP_BOUNDARY = new Vector3(50f,50f,0);
    private Vector3 spawnPosition;
    private float x, y;
    private void SpawnNaturalPieces()
    {
        for (int i = 0; i < 150; i++)
        {
            x = Random.Range(-MAX_MAP_BOUNDARY.x, MAX_MAP_BOUNDARY.x);
            y = Random.Range(-MAX_MAP_BOUNDARY.y, MAX_MAP_BOUNDARY.y);
            spawnPosition = Vector3.right * x + Vector3.up * y;
            PoolManager.Instance.Spawn("NaturalPieces", spawnPosition,Quaternion.identity);
        }
    }
    private void SpawnIdleAI()
    {
        for (int i = 0; i < 100; i++)
        {
            x = Random.Range(-MAX_MAP_BOUNDARY.x, MAX_MAP_BOUNDARY.x);
            y = Random.Range(-MAX_MAP_BOUNDARY.y, MAX_MAP_BOUNDARY.y);
            spawnPosition = Vector3.right * x + Vector3.up * y;
            PoolManager.Instance.Spawn("IdleAI", spawnPosition, Quaternion.identity);
        }
    }

    public void StartGame()
    {
        Player.SetActive(true);
        MainMenu.SetActive(false);
        HUD.SetActive(true);
    }
    
    public void EndGame()
    {
        Player.SetActive(false);
        MainMenu.SetActive(true);
        HUD.SetActive(false);
    }
}
