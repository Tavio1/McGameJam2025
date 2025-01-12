using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using cakeslice;
using UnityEngine.UI;

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


    [Header("Materials")]
    [SerializeField] Material goldMaterial;
    [SerializeField] Material[] Outlines;

    [SerializeField] float respawnTimerFly = 90.0f;
    [SerializeField] float respawnTimerAnt = 90.0f;

    float flySpawnTimer = 5f;
    float antSpawnTimer = 5f;

    void Start(){
        spawnLocations = spawnsParent.GetComponentsInChildren<Transform>();
        spawnLocations[0] = spawnLocations[1];

        foreach (Transform t in spawnLocations){
            Debug.Log(t.position);
        }
    }

    void Update(){
        flySpawnTimer -= Time.deltaTime;
        antSpawnTimer -= Time.deltaTime;

        if (flySpawnTimer <= 0f){
            flySpawnTimer = respawnTimerFly;

            Spawn(0, findSpawnLocation());
            // Debug.Log("fly spawned");
        }

        if (antSpawnTimer <= 0f){
            antSpawnTimer = respawnTimerAnt;

            Spawn(1,findSpawnLocation());
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
            return spawnLocations[1];

        return offscreenTransforms[UnityEngine.Random.Range(0, offscreenTransforms.Length)];
    }



    // Spawn Function
    private void Spawn(int i, Transform location){
        Debug.Log($"Location picked: {location.position}");

        GameObject bugPrefab = bugPrefabs[i];
        GameObject bug = Instantiate(bugPrefab, location.position, Quaternion.identity, bugsParent);

        Debug.Log($"the bug's position is {bug.transform.position} and location is {location.position} and bug is {bug}");
        GameObject minimapIcon = Instantiate(bugMiniPrefabs[i], miniMap); 
        minimapIcon.GetComponent<BugMinimap>().Initialize(bug.transform);

        // Check for spawning a golden insect
        if (UnityEngine.Random.value <= 0.1){

            Renderer[] renderers = bug.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers){
                rend.material = goldMaterial;
            }

            bug.GetComponent<BugStats>().isGolden = true;

            minimapIcon.GetComponent<Image>().color = new Color(0.93f, 0.71f, 0.29f);
            return;
        }


        // Check for spawning one with power up
        if (UnityEngine.Random.value <= 0.1){
            // cakeslice.Outline outline = bug.AddComponent(typeof(cakeslice.Outline)) as cakeslice.Outline;

            // Color: 0=red, 1=green, 2=blue
            int type = UnityEngine.Random.Range(0, 2);

            SkinnedMeshRenderer[] renderers = bug.GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach (SkinnedMeshRenderer r in renderers){
                Material[] materials = new Material[2];
                materials[0] = r.materials[0];
                materials[1] = Outlines[0];
                r.materials = materials;
            }
            return;
        }

        

        
        // Create an icon on minimap
    }

}
