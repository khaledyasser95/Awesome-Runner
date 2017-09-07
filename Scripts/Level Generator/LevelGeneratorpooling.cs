using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorpooling : MonoBehaviour {
    [SerializeField]
    private Transform Platform, platform_parent;
    [SerializeField]
    private Transform Monster, monster_parent;
    [SerializeField]
    private Transform health, Health_parent;
    [SerializeField]
    private int levelLength = 100;
    [SerializeField]
    private float distance_between_platforms = 15f;
    [SerializeField]
    private float MIN_Position_Y = 0f, MAX_Position_Y = 7f;
    [SerializeField]
    private float chanceForMonsterExistance = 0.25f, changeForCollectableExsitance = 0.1f;
    [SerializeField]
    private float healthCollectable_MinY = 1f, healthCollectable_MaxY = 3f;

    private float platformLastPositionX;
    private Transform[] platform_Array;


   
    // Use this for initialization
    void Start () {
        CreatPlatforms();
    }
	
	void CreatPlatforms()
    {
        platform_Array = new Transform[levelLength];
        for (int i=0; i < platform_Array.Length; i++)
        {
            Transform newPlatform = (Transform)Instantiate(Platform, Vector3.zero, Quaternion.identity);
            platform_Array[i] = newPlatform;

        }
        for (int i = 0; i < platform_Array.Length; i++)
        {
            float platformPosY = Random.Range(MIN_Position_Y, MAX_Position_Y);
            Vector3 platformPosition;
            if (i < 2)
                platformPosY = 0f;
            platformPosition = new Vector3(distance_between_platforms * i, platformPosY, 0);
            platformLastPositionX = platformPosition.x;
            platform_Array[i].position = platformPosition;
            platform_Array[i].parent = platform_parent;
            //SPAWN MONSTERS
            SpawnHealthAndMonsters(platformPosition, i, true);

        }
    }
    public void PoolingPlatforms()
    {
        for (int i=0; i < platform_Array.Length; i++)
        {
            if (!platform_Array[i].gameObject.activeInHierarchy)
            {
                platform_Array[i].gameObject.SetActive(true);
                float platformPositionY = Random.Range(MIN_Position_Y, MAX_Position_Y);
                Vector3 platformPosition = new Vector3(distance_between_platforms + platformLastPositionX, platformPositionY, 0);

                platform_Array[i].position = platformPosition;
                platformLastPositionX = platformPosition.x;
                //SPAWN HEALTH AND MONSTERS
                SpawnHealthAndMonsters(platformPosition,i,false);
            }
        }
    }

    void SpawnHealthAndMonsters(Vector3 platformPosition,int i,bool gameStarted)
    {
        if (i > 2)
        {
            if (Random.Range(0f, 1f) < chanceForMonsterExistance)
            {
                if (gameStarted)
                    platformPosition = new Vector3(distance_between_platforms * i, platformPosition.y+0.1f, 0);
                else
                    platformPosition = new Vector3(distance_between_platforms +platformLastPositionX, platformPosition.y + 0.1f, 0);
                Transform createMonster = (Transform)Instantiate(Monster, platformPosition, Quaternion.Euler(0, -90, 0));
                createMonster.parent = monster_parent;

            }//if for monster

            if (Random.Range(0f, 1f) < changeForCollectableExsitance)
            {
                if (gameStarted)
                    platformPosition = new Vector3(distance_between_platforms * i, platformPosition.y + Random.Range(healthCollectable_MinY, healthCollectable_MaxY), 0);
                else
                    platformPosition = new Vector3(distance_between_platforms + platformLastPositionX, platformPosition.y + Random.Range(healthCollectable_MinY, healthCollectable_MaxY), 0);
                Transform createHealth = (Transform)Instantiate(health, platformPosition, Quaternion.identity);
                createHealth.parent = Health_parent;

            }//if for health


        }
    }




}//class










