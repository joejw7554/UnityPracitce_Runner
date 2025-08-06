using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    static SpawnManager instance;
    public static SpawnManager Instance 
    {  
       get => instance;
    }

    PlayerController playerControllerScript;

    [SerializeField]
    GameObject[] obstaclePrefabs;

    [SerializeField]
    float MinDelay = 2f;

    [SerializeField]
    float MaxDelay = 4f;

    Coroutine spawnCoroutine;

    Dictionary<int, ObjectPool<GameObject>> ObstaclePools = new Dictionary<int, ObjectPool<GameObject>>();


    private void Awake()
    {
        instance = this; //싱글톤
        //Prefabs 갯수만큼 풀갯수 생성
        CreatePools();
    }

    private void CreatePools()
    {
        for (int i = 0; i < obstaclePrefabs.Length; i++)
        {
            //ObjectPool 초기화 설정
            int index = i;
            ObstaclePools[index] = new ObjectPool<GameObject>
                (
                createFunc: () => Instantiate(obstaclePrefabs[index]),
                actionOnGet: (obj) => obj.SetActive(true),
                actionOnRelease: (obj) => obj.SetActive(false),
                actionOnDestroy: (obj) => Destroy(obj),
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 20
                );


            //실제 gameObject Instance Pool에서 Get해서 N개 생성후 Release
            List<GameObject> temp = new();

            for (int j = 0; j < 10; j++)
            {
                var obj = ObstaclePools[index].Get();
                temp.Add(obj);
            }

            foreach (var instance in temp)
            {
                ObstaclePools[index].Release(instance);
            }
        }
    }

    public GameObject SpawnPoolObject(int index)
    {
        var poolObj = ObstaclePools[index].Get();

        var outofBoundsScript = poolObj.GetComponent<OutofBounds>();
        if (outofBoundsScript)
        {
            outofBoundsScript.KeyIndexNumber = index;
        }
        poolObj.transform.position = new Vector3(30f, 0, 0);

        return poolObj;
    }

    public void RetrieveObj(GameObject obj, int index)
    {
        if (ObstaclePools.ContainsKey(index))
            ObstaclePools[index].Release(obj);
        else
            Destroy(obj);
    }

    void Start()
    {
        spawnCoroutine = StartCoroutine(SpawnObstacleCoroutine());
        playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerControllerScript.OnGameOver += StopObstacleCoroutine;
    }

    IEnumerator SpawnObstacleCoroutine()
    {
        while (true)
        {
            float randomWaitTime = Random.Range(MinDelay, MaxDelay);
            Debug.Log(randomWaitTime);
            int randomIndex = Random.Range(0, ObstaclePools.Count);
            SpawnPoolObject(randomIndex);
            yield return new WaitForSeconds(randomWaitTime);
        }
    }

    void StopObstacleCoroutine()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        Debug.Log("Stop Spawning");
    }

    void Update()
    {

    }

    private void OnDestroy()
    {
        playerControllerScript.OnGameOver -= StopObstacleCoroutine;
    }

}
