using System;
using System.Collections.Generic;
using UnityEngine;

public class PartSpawner : MonoBehaviour
{
    //NB: early prototype for this script: https://repl.it/@PixelZerg/FastDifficultTranslation

    public Transform player;
    public GameObject[] parts;

    //public float sigma = 3f; // sigma for Normal dist
    //public float partProbWidth = 5;

    public float aheadDist = 100; // spawn parts up till this distance ahead
    public float behindDist = 30; // despawn parts once they are this distance behind

    public float offset = 25; // initial z offset
    private float curOff; // current z offset (z-coord of the far edge of the plane)
    private float curPartNo = 0;

    private List<GameObject> activeParts = new List<GameObject>(); // list of parts currently in the world
    private float curActiveOff; // z coordinate of the foremost edge of the first activePart

    void Start()
    {
        curOff = offset; // initially, curOff = initial offset
        curActiveOff = curOff;
    }

    void Update()
    {
        while(curOff < player.position.z + aheadDist)
        {
            SpawnPart();
        }
        while (curActiveOff < player.position.z - behindDist)
        {
            DespawnPart();
        }
    }

    private float GetPartWidth(GameObject part)
    {
        foreach (Transform child in part.transform)
        {
            if (child.tag == "Floor")
            {
                return child.localScale.z;
            }
        }
        return 0;
    }
    private void SpawnPart()
    {
        // part selection
        //double r = Math.Max(0, RandNorm(curPartNo, sigma)); // sample from sliding normal distribution
        //double partIndex = Math.Floor(r / partProbWidth); // sample -> index, incoorporating partProbWidth
        //GameObject newPart = parts[(int)Math.Min(parts.Length - 1, partIndex)]; // constrain index to parts array, and select from array
        int partIndex = UnityEngine.Random.Range(0, parts.Length);
        GameObject newPart = parts[partIndex];

        // part instantiation
        float newPartWidth = GetPartWidth(newPart);
        activeParts.Add(Instantiate(newPart, new Vector3(0, 0, curOff + newPartWidth / 2), Quaternion.identity));

        // debug
        Debug.Log(String.Format("Spawned: PartNo: {0}, PartIndex: {1}", curPartNo, partIndex));

        // state update
        curPartNo += 1;
        curOff += newPartWidth;
    }

    private void DespawnPart()
    {
        if (activeParts.Count > 0)
        {
            GameObject frontPart = activeParts[0];
            curActiveOff += GetPartWidth(frontPart);

            Destroy(frontPart);
            activeParts.RemoveAt(0);
        }
    }
}
