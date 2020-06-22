using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Grass
{
    public GameObject prefab;
    public int n; // num times to spawn
    public float minSize;
    public float maxSize;

    public float yCoord;
    public float minXRot;
    public float maxXRot;
    public float minYRot;
    public float maxYRot;
    public float minZRot;
    public float maxZRot;
}

public class GrassSpawner : MonoBehaviour
{
    public Grass[] grasses;
    
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer mesh = this.GetComponent<MeshRenderer>();
        float minX = mesh.bounds.min.x;
        float maxX = mesh.bounds.max.x;
        float minZ = mesh.bounds.min.z;
        float maxZ = mesh.bounds.max.z;

        foreach (Grass grass in grasses)
        {
            for(int i = 0; i < grass.n; i++)
            {
                float randX = UnityEngine.Random.Range(minX, maxX);
                float randZ = UnityEngine.Random.Range(minZ, maxZ);

                GameObject newObject = Instantiate(
                    grass.prefab,
                    new Vector3(randX, grass.yCoord, randZ), 
                    Quaternion.Euler(UnityEngine.Random.Range(grass.minXRot, grass.maxXRot), UnityEngine.Random.Range(grass.minYRot, grass.maxYRot), UnityEngine.Random.Range(grass.minZRot, grass.maxZRot)),
                    this.transform.parent);

                float randSize = UnityEngine.Random.Range(grass.minSize, grass.maxSize);
                newObject.transform.localScale = new Vector3(randSize, randSize, randSize);
            }
        }
    }

}
