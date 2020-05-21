using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private List<GameObject> activeTiles = new List<GameObject>();
    
    public Transform playerTransform;
    public GameObject[] tilePrefabs;
    public GameObject[] roadsideTilePrefabs;

    public float zSpawn = 0;
    public float tileLength = 30;
    
    public int numberOfTiles = 5;
    public int numberOfRoadsideTiles = 1;
    int roadsideTilesCreated = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numberOfTiles; i++)
        {
            if (i == 0)
                SpawnTile(0);
            else SpawnTile(UnityEngine.Random.Range(0, tilePrefabs.Length));
            
            SpawnRoadsideTiles(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.z - 35 > zSpawn - (numberOfTiles * tileLength))
        {
            SpawnTile(UnityEngine.Random.Range(0, tilePrefabs.Length));
            SpawnRoadsideTiles(0);
            DeleteTiles();
        }
    }

    public void SpawnTile(int tileIndex)
    {
        GameObject go = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        activeTiles.Add(go);
        zSpawn += tileLength;
    }

    public void SpawnRoadsideTiles(int tileIndex)
    {
        GameObject left = Instantiate(roadsideTilePrefabs[tileIndex], new Vector3(0.5F + (tileLength / 4), 0, tileLength * roadsideTilesCreated), transform.rotation);
        GameObject right = Instantiate(roadsideTilePrefabs[tileIndex], new Vector3(-(0.5F + (tileLength / 4)), 0, tileLength * roadsideTilesCreated), transform.rotation);
        
        activeTiles.Add(left);
        activeTiles.Add(right);

        roadsideTilesCreated++;
    }

    //removing three tiles at once (main and two from roadsides)
    private void DeleteTiles()
    {
        for (int i = 0; i < 2; i++)
        {
            Destroy(activeTiles[i]);
            activeTiles.RemoveAt(i);
        }
    }
}
