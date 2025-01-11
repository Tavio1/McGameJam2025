using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BugSpawnManager : MonoBehaviour
{
    // List of spawn locations for the bugs


    [SerializeField] Transform spawnsParent;
    [SerializeField] Transform bugsParent;
    [SerializeField] RectTransform miniMap;

    Transform[] spawnLocations;

    // 0: fly, 1: ant
    [SerializeField] GameObject[] bugPrefabs;
    [SerializeField] GameObject[] bugMiniPrefabs;

    float flySpawnTimer = 0f;
    float antSpawnTimer = 0f;

    void Start(){
        spawnLocations = spawnsParent.GetComponentsInChildren<Transform>();
        spawnLocations[0] = spawnLocations[1];
    }

    void Update(){
        flySpawnTimer -= Time.deltaTime;
        antSpawnTimer -= Time.deltaTime;

        if (flySpawnTimer <= 0f){
            flySpawnTimer = 5f;

            Spawn(0, findSpawnLocation());
            // Debug.Log("fly spawned");
        }

        if (antSpawnTimer <= 0f){
            antSpawnTimer = 5f;



            // Debug.Log("ant spawned");
        }
    }


    private Transform findSpawnLocation(){

        Func<Transform, bool> isOffscreen = (Transform t) =>
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(t.position);
            return screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1 || screenPoint.z < 0;
        };

        // Filtered list
        Transform[] offscreenTransforms = spawnLocations.Where(isOffscreen).ToArray();

        if (offscreenTransforms.Length == 0)
            return null;

        return offscreenTransforms[UnityEngine.Random.Range(0, offscreenTransforms.Length)];
    }

    // Spawn Function
    private void Spawn(int i, Transform location){
        GameObject bugPrefab = bugPrefabs[i];

        Debug.Log($"Bug Type: {bugPrefab} Location: {location.gameObject}");

        GameObject bug = Instantiate(bugPrefab, location.position, Quaternion.identity, bugsParent);

        // Create an icon on minimap
        Instantiate(bugMiniPrefabs[i], miniMap).GetComponent<BugMinimap>().Initialize(bug.transform);
    }

}
