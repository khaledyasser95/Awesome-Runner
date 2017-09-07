using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    [SerializeField]
    //How Many Platforms
    private int levelLength;

    [SerializeField]
    //Start : 5 Platform beside eachother
    //End: Same as start but end of Level
    private int startPlatformLength=5, endPlatformLength = 5;

    [SerializeField]
    private int distance_between_platforms;

    [SerializeField]
    //Parent for gameobject parent
    private Transform platformPrefab,platform_parent;

    [SerializeField]
    private Transform monster, monster_parent;

    [SerializeField]
    private Transform health_Collectable, healthCollectable_parent;

    [SerializeField]
    private float platformPosition_MinY =0f, platformPosition_MaxY = 10f;

    [SerializeField]
    //How many platform put together to form platform
    private int platformPosition_Min = 1, platformPosition_Max = 4;

    [SerializeField]
    private float chanceForMonsterExistance = 0.25f, chanceForCollecatableExistance = 0.1f;

    [SerializeField]
    //Should be Below or above platform
    private float healthCollectable_MinY = 1f, healthCollectable_MaxY = 3f;

    private float platformLastPositionX;

    private enum PlatformType
    {
        None,
        Flat

    }

    private class PlatformPositionInfo
    {
        public PlatformType platformType;
        public float positionY;
        public bool hasMonster;
        public bool hasHealthCollectable;

        public PlatformPositionInfo(PlatformType type ,float  posY , bool has_monster , bool has_HealthCollectable)
        {
            platformType = type;
            positionY = posY;
            hasMonster = has_monster;
            hasHealthCollectable = has_HealthCollectable;
        }

    }// Class PlatformPosition

    void Start()
    {
        GenerateLevel(true);
    }
    void FillOutPositionInfo(PlatformPositionInfo[] platformInfo)
    {
        int currentPlatformIndex = 0;
        for (int i = 0; i < startPlatformLength; i++)
        {
            //Initializing START OF GAME
            platformInfo[currentPlatformIndex].platformType = PlatformType.Flat;
            platformInfo[currentPlatformIndex].positionY = 0f;

            currentPlatformIndex++;
        }
        //INGAME
        while(currentPlatformIndex< levelLength - endPlatformLength)
        {
            
            if (platformInfo[currentPlatformIndex-1].platformType != PlatformType.None)
            {
                currentPlatformIndex++;
                continue;
            }
            float platformPositionY = Random.Range(platformPosition_MinY, platformPosition_MaxY);
            int platformLength = Random.Range(platformPosition_Min, platformPosition_Max);
            for (int i = 0; i < platformLength; i++)
            {
                bool has_Monster = (Random.Range(0f, 1f) < chanceForMonsterExistance);
                bool has_HealthCollectable = (Random.Range(0f, 1f) < chanceForCollecatableExistance);
                platformInfo[currentPlatformIndex].platformType = PlatformType.Flat;
                platformInfo[currentPlatformIndex].positionY = platformPositionY;
                platformInfo[currentPlatformIndex].hasMonster = has_Monster;
                platformInfo[currentPlatformIndex].hasHealthCollectable = has_HealthCollectable;
                currentPlatformIndex++;
                //END
                if (currentPlatformIndex>(levelLength - endPlatformLength))
                {
                    currentPlatformIndex = levelLength - endPlatformLength;
                    break;
                }
            }//for loop

            //END OF GAME
            for(int i = 0;i < endPlatformLength; i++){
                platformInfo[currentPlatformIndex].platformType = PlatformType.Flat;
                platformInfo[currentPlatformIndex].positionY = 0f;

                currentPlatformIndex++;
            }

        }//while loop

    }
    void CreatePlatformFromPositionInfo(PlatformPositionInfo[] platformpositioninfo,bool gameStarted)
    {
        for (int i=0; i< platformpositioninfo.Length; i++)
        {
            PlatformPositionInfo positionInfo = platformpositioninfo[i];
            if (positionInfo.platformType == PlatformType.None)
                continue;
            //Position for platform
            Vector3 platformPosition;//x y z 
            //check game started or NOT
            if (gameStarted)
                platformPosition = new Vector3(distance_between_platforms * i, positionInfo.positionY, 0);
            else
                platformPosition= new Vector3(distance_between_platforms +platformLastPositionX, positionInfo.positionY, 0);
            // Save platform position for later use
            platformLastPositionX = platformPosition.x;
            //create new game object prefab
            //Quat = Rotation
            Transform creatblock = (Transform)Instantiate(platformPrefab,platformPosition,Quaternion.identity);

            creatblock.parent = platform_parent;

            if (positionInfo.hasMonster)
            {
                if (gameStarted)
                {
                    platformPosition = new Vector3(distance_between_platforms * i, positionInfo.positionY + 0.1f, 0);
                }
                else
                {
                    platformPosition = new Vector3(distance_between_platforms + platformLastPositionX, positionInfo.positionY+0.1f, 0);
                }

                Transform createMonster = (Transform)Instantiate(monster,platformPosition,Quaternion.Euler(0,-90,0));
                createMonster.parent = monster_parent;  
            }
            if (positionInfo.hasHealthCollectable)
            {
                if (gameStarted)
                {
                    platformPosition = new Vector3(distance_between_platforms * i, positionInfo.positionY+Random.Range(healthCollectable_MinY,healthCollectable_MaxY), 0);

                }else
                {
                    platformPosition = new Vector3(distance_between_platforms + platformLastPositionX, positionInfo.positionY + Random.Range(healthCollectable_MinY, healthCollectable_MaxY), 0);

                }
                Transform createHealth = (Transform)Instantiate(health_Collectable, platformPosition, Quaternion.Euler(0, -90, 0));
                createHealth.parent = healthCollectable_parent;
            }
        }//for loop
    }
    public void GenerateLevel(bool gameStarted)
    {
        PlatformPositionInfo[] platforminfo = new PlatformPositionInfo[levelLength];
        for (int i=0; i < platforminfo.Length; i++)
        {
            platforminfo[i] = new PlatformPositionInfo(PlatformType.None,-1f,false,false);
        }
        FillOutPositionInfo(platforminfo);
        CreatePlatformFromPositionInfo(platforminfo,gameStarted);
    }

}//class
