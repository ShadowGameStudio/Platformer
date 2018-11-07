using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WorldGenerator : MonoBehaviour {

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    [Header("Size")]
    [SerializeField] private int MaxLength = 20;
    [SerializeField] private int MinLength = 1;

    [Header("Enemies")]
    [SerializeField] private Transform[] Enemies;
    [SerializeField] private int EnemyFrequency = 0;
    [SerializeField] private Transform PatrolPoint;

    [SerializeField] private int PatrolPointsFrequency = 0;

    [Header("Platforms")]
    [SerializeField] private GameObject[] Platforms;
    [SerializeField] private int PlatformFrequency = 0;
    [SerializeField] private GameObject PlatformPoint;

    [Header("Objects")]
    [SerializeField] private GameObject[] Objects;
    [SerializeField] private int ObjectFrequency = 0;


    private int xSize = 0;
    private int ySize = 0;

	// Use this for initialization
	void Start () {

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        SetRandomSize();
        CreateShape();
        UpdateMesh();

        SpawnEnemies();
        SpawnPlatforms();

	}

    //Creates the base floor
    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        int i = 0;
        for (int y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x, y, 0);
                i++;
            }
        }

        triangles = new int[xSize * ySize * 6];
        int vert = 0;
        int tris = 0;

        for (int y = 0; y < ySize; y++)
        {
            for (int j = 0; j < xSize; j++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

    }
	
    //Updates the mesh upon changes
    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        GetComponent<BoxCollider2D>().size = new Vector3(xSize, ySize, 0);
        GetComponent<BoxCollider2D>().offset = new Vector2(xSize / 2f, ySize / 2f);
    }

    //Sets a random size to the world
    void SetRandomSize()
    {

        //Set the random sizes based on the inputted values
        xSize = Random.Range(MinLength, MaxLength);
        ySize = 12;
    }

    //Spawns the level enemies
    void SpawnEnemies()
    {
        //Get the average distance between enemies and get a random distance from that
        float averageEnemyDistance = (xSize - 10) / EnemyFrequency;

        Vector3 startPos = new Vector3(0, 12.5f, 0);

        //Create all the enemies
        for (int i = 0; i < EnemyFrequency; i++)
        {

            //Float to hold the random dist
            float randomDist;

            //Get if it should be less than or greater than less than the average distance
            if (Random.Range(0, 1) == 1)
            {
                randomDist = Random.Range(averageEnemyDistance, averageEnemyDistance + 5f);
            }
            else
            {
                randomDist = Random.Range(averageEnemyDistance - 5f, averageEnemyDistance);
            }
            
            startPos.x += randomDist;

            //Get random enemy type to instantiate and instantiate it
            int randomEnemy = Random.Range(0, Enemies.Length - 1);
            GameObject enemy = (GameObject)Instantiate(Enemies[randomEnemy].gameObject, startPos, Quaternion.identity);

            //Bool to check if the enemy should have patrol points or not and check it
            bool shouldHavePatrolPoints = false;

            if (Random.Range(1, 10) == 1)
            {
                shouldHavePatrolPoints = false;
            }
            else
            {
                shouldHavePatrolPoints = true;
            }

            //If the enemy should have patrol points
            if (shouldHavePatrolPoints)
            {
                EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();

                //Spawn the first point and add it to the enemys list
                Vector3 pointOnePos = new Vector3(startPos.x + Random.Range(3f, 7f), startPos.y, startPos.z);
                GameObject pointOne = (GameObject)Instantiate(PatrolPoint.gameObject, pointOnePos, Quaternion.identity);

                enemyBase.PatrolPoints.Clear();
                enemyBase.PatrolPoints.Add(pointOne.transform);

                //Spawn the second point and add it to the list
                Vector3 pointTwoPos = new Vector3(startPos.x - Random.Range(3f, 7f), startPos.y, startPos.z);
                GameObject pointTwo = (GameObject)Instantiate(PatrolPoint.gameObject, pointTwoPos, Quaternion.identity);

                enemyBase.PatrolPoints.Add(pointTwo.transform);

                //Set the current patrol point
                enemyBase.CurrentPatrolPoint = enemyBase.PatrolPoints[0];

            }
            else
            {
                //Instantiate the start point and add it to the enemy's patrolpoint array
                GameObject startPoint = (GameObject)Instantiate(PatrolPoint.gameObject, startPos, Quaternion.identity);
                EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();

                //Clear the list, add the patrol point and set the it to the current one
                enemyBase.PatrolPoints.Clear();
                enemyBase.PatrolPoints.Add(startPoint.transform);
                enemyBase.CurrentPatrolPoint = enemyBase.PatrolPoints[0];
            }

        }

    }

    //Spawns the level objects
    void SpawnObjects()
    {

    }

    //Spawns the level moving platforms
    void SpawnPlatforms()
    {
        float averageDistance = ((float)xSize - 10f) / (float)PlatformFrequency;
        Vector3 spawnPos = new Vector3(0f, 16f, 0f);

        for (int i = 0; i < PlatformFrequency; i++)
        {
            //If the random number becomes one set the distance to between average and average + 10
            //Otherwise do the opposite
            if (Random.Range(0, 1) == 1)
            {
                spawnPos.x += Random.Range(averageDistance, averageDistance + 10f);
            }
            else
            {
                spawnPos.x += Random.Range(averageDistance - 10f, averageDistance);
            }

            //Get a random platform and instantiate it
            int randomPlatform = Random.Range(0, Platforms.Length);
            GameObject platform = (GameObject)Instantiate(Platforms[randomPlatform], spawnPos, Quaternion.identity);

            //Create the points that the platform should travel between
            //Spawn pos of the first point
            Vector3 spawnPosOne = new Vector3(spawnPos.x + Random.Range(5f, 15f), spawnPos.y, spawnPos.z);
            GameObject pointOne = (GameObject)Instantiate(PlatformPoint, spawnPosOne, Quaternion.identity);

            Vector3 spawnPosTwo = new Vector3(spawnPos.x - Random.Range(5f, 15f), spawnPos.y, spawnPos.z);
            GameObject pointTwo = (GameObject)Instantiate(PlatformPoint, spawnPosTwo, Quaternion.identity);

            //Add the point to the list of points in the platform
            MovingPlatform movingPlatform = platform.GetComponent<MovingPlatform>();

            //Clear the list and add the points 
            movingPlatform.MovingPoints.Clear();
            movingPlatform.MovingPoints.Add(pointOne.transform);
            movingPlatform.MovingPoints.Add(pointTwo.transform);

            
            //Set the first element to the current move point
            movingPlatform.CurrentMovePoint = movingPlatform.MovingPoints[0];

        }
    }

}
