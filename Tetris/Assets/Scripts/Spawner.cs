using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] Tetrominos;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnRandomTetromino();
    }
    
    public void SpawnRandomTetromino()
    {
        Instantiate(Tetrominos[Random.Range(0, Tetrominos.Length)], transform.position, Quaternion.identity);
    }
    
}
