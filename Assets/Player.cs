using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

/// <summary>
/// Represents a player (or client).  A player controls more than one character.  This class represents all characters controlled
/// by that given player.
/// </summary>
public class Player : MonoBehaviour
{

    public GameObject[] CharacterPrefabs = new GameObject[4];

    private GameObject[] characters = new GameObject[4];

    public GameObject[] Characters
    {
        get { return this.characters; }
    }

    void Start()
    {
        Vector3[] spawnPoints = getSpawnPoints();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            this.characters[i] = Instantiate(CharacterPrefabs[i], spawnPoints[i], Quaternion.identity) as GameObject;
        }
    }


    Vector3[] getSpawnPoints()
    {
        Vector3[] spawnPoints = new Vector3[4];
        spawnPoints[0] = new Vector3(15, 0, 0);
        spawnPoints[1] = new Vector3(15, 0, 15);
        spawnPoints[2] = new Vector3(0, 0, 15);
        spawnPoints[3] = new Vector3(0, 0, 0);
        return spawnPoints;
    }
}