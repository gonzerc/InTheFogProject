using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyObj;       //enemy gameobject prefab
    public Vector3 size;            //size of spawner
    public List<GameObject> spawnLocations= new List<GameObject>();

    public IEnumerator SpawnEnemies(int numEnemies)
    {
        for (int i = 0; i < numEnemies; i++)
        {
            //Debug.Log("Spawning " + i);
            SpawnEnemy();
            yield return new WaitForSeconds(1.0f);
        }
        //GameController.roundRunning = true;
    }

    public void SpawnEnemy()
    {
        //choose random spawn location
        int rand = Random.Range(0, spawnLocations.Count);
        GameObject spawnLoc = spawnLocations[rand];

        //set random point in chosen spawn location
        float x = Random.Range( spawnLoc.transform.position.x - (size.x / 2), spawnLoc.transform.position.x + (size.x / 2));
        float z = Random.Range(spawnLoc.transform.position.z - (size.z / 2), spawnLoc.transform.position.z + (size.z / 2));

        Vector3 newPos = new Vector3(x, 1.0f, z);

        //Debug.Log(newPos);
        Instantiate(enemyObj, newPos, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        foreach(GameObject spawner in spawnLocations)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(spawner.transform.position, size);
        }
    }
}
