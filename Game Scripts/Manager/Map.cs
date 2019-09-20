using UnityEngine;
using System.Collections;

public class Map : Singleton<Map>
{
    [Header("Object Reference")]
    public GameObject plane;

    [Header("Player Movement")]
    public int divideDistance;
    [HideInInspector] public int mapLimit;
    [HideInInspector] public Vector3[] movePoint;

    [Header("Dynamic Setting")]
    public const int speedLimit = 150;
    [Range(10, speedLimit)] public float initialSpeed;
    public float speed;
    public Vector3 direction;
    public float distance = 0;

    [Header("Spawner Setting")]
    public float spawnDistance;                       //플레이어로 부터 얼마나 떨어져 있을지를 정합니다.
    public float lastSpawn;                            //장애물이 마지막으로 스폰되고부터 경과한 시간
    public Transform spawnedObjectParent;     //Parent
    public const float minSpawnDelay = 0.45f;   //Safe time, humanize

    [Header("Timer")]
    [SerializeField] private float sixtyTimer = 0;

    [System.Serializable]
    public struct Spawner
    {
        //Identity
        public string name;
        public bool quizItem;

        //Public:
        [Range(minSpawnDelay, 20f)]
        public float delay;
        public GameObject[] target;

        //Private:
        public Coroutine loop;

        //Method
        public GameObject GetRandomTarget()
        {
            return target[Random.Range(0, target.Length)];
        }
    }

    public Spawner[] spawners;

    //Coroutines
    private Coroutine changeSpeed;
    private Coroutine changeInitialSpeed;

    private void OnEnable()
    {
        GameManager.EventGameOver += GameOver;
    }
    private void OnDisable()
    {
        GameManager.EventGameOver -= GameOver;
    }
    public void Initialize()
    {
        //Intialize variable
        speed = initialSpeed;

        //점진적 속도 증가
        ChangeInitialSpeed(initialSpeed + 50f, 300f);

        Calculation();

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].loop = StartCoroutine(SpawningLoop(spawners[i]));
        }
    }
    private void Update()
    {
        //Timer Check
        if (sixtyTimer >= 60)
        {
            sixtyTimer = 0;
        }

        //Timer Increase
        lastSpawn += Time.deltaTime;
        sixtyTimer += Time.deltaTime;

        //Calculate Distance
        distance += speed * Time.deltaTime;
    }

    #region Map Generation

    private IEnumerator SpawningLoop(Spawner spawner)
    {
        float timer = 0;
        int spawned = 0;
        for (; ; )
        {
            if (timer > spawner.delay && lastSpawn >= minSpawnDelay)
            {
                timer = 0;

                if (spawner.quizItem)
                {
                    for (int i = 0; i < mapLimit; i++)
                    {
                        SpawnObject(spawner.GetRandomTarget(), i);
                    }
                }
                else if (CanSpawnObstalce())
                {
                    for (int i = 0; i < mapLimit; i++)
                    {
                        if (spawned > mapLimit - 2)
                        {
                            spawned = 0;
                            continue;
                        }
                        else if (Random.Range(0, 2) == 1)
                        {
                            spawned++;
                            SpawnObject(spawner.GetRandomTarget(), i);
                        }
                    }
                }
            }
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public bool CanSpawnObstalce()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            if (spawners[i].quizItem)
            {
                if (sixtyTimer % spawners[i].delay < minSpawnDelay)
                {
                    //print("Yield --> Sixty : " + sixtyTimer + ", Calc : " + sixtyTimer % spawners[i].delay);
                    return false;
                }
            }
        }
        return true;
    }

    public void SpawnObject(GameObject obj, int index)
    {
        lastSpawn = 0;
        Vector3 startPos = movePoint[index];
        startPos.y = obj.transform.position.y;
        startPos.z += spawnDistance;
        ISpawnable spawned = Instantiate(obj, startPos, obj.transform.rotation, spawnedObjectParent).GetComponent<ISpawnable>();
        spawned.Initialize(direction, index);
    }

    public void Calculation()
    {
        float horizontalLength = plane.transform.localScale.x;
        int cnt = (int)(horizontalLength / divideDistance);
        float rest = (horizontalLength % divideDistance) / 2;
        float startXpos = plane.transform.localPosition.x - plane.transform.localScale.x / 2 + divideDistance / 2;

        if (divideDistance % 2 != 0)
        {
            cnt--;
            movePoint = new Vector3[cnt];
            for (int i = 0; i < cnt; i++)
            {
                movePoint[i].x = startXpos + rest + divideDistance * (i + 1);
            }
        }
        else
        {
            movePoint = new Vector3[cnt];
            for (int i = 0; i < cnt; i++)
            {
                movePoint[i].x = startXpos + rest + divideDistance * i;
            }
        }

        //완료, 원하는 결과를 얻었습니다.
        mapLimit = cnt;
    }

    #endregion

    #region Game Event
    private void GameOver()
    {
        //We need to stop the map
        if (changeSpeed != null) StopCoroutine(changeSpeed);
        if (changeInitialSpeed != null) StopCoroutine(changeInitialSpeed);
        ChangeSpeed(0, 0.1f);

        //Stop instantiating
        for (int i = 0; i < spawners.Length; i++)
        {
            StopCoroutine(spawners[i].loop);
        }
    }
    #endregion

    #region Speed Change
    public void ChangeSpeed(float value, float elapsed, System.Action done = null)
    {
        if (value > speedLimit)
        {
            return;
        }
        if (changeSpeed != null)
        {
            StopCoroutine(changeSpeed);
        }
        changeSpeed = StartCoroutine(ChangeSpeedCoroutine(value, elapsed, done));
    }
    private IEnumerator ChangeSpeedCoroutine(float value, float elapsed, System.Action callback)
    {
        float timer = 0;
        float start = speed;

        while (elapsed >= timer)
        {
            timer += Time.deltaTime;
            speed = Mathf.Lerp(start, value, timer / elapsed);
            yield return null;
        }
        callback?.Invoke(); //콜백 호출
        yield break;
    }

    public void ChangeInitialSpeed(float value, float elapsed, System.Action done = null)
    {
        if (changeInitialSpeed != null)
        {
            StopCoroutine(changeSpeed);
        }
        changeInitialSpeed = StartCoroutine(ChangeInitialSpeedCoroutine(value, elapsed, done));
    }
    private IEnumerator ChangeInitialSpeedCoroutine(float value, float elapsed, System.Action callback)
    {
        float timer = 0;
        float start = initialSpeed;

        while (elapsed >= timer)
        {
            timer += Time.deltaTime;
            initialSpeed = Mathf.Lerp(start, value, timer / elapsed);
            if (speed < initialSpeed)
            {
                speed = initialSpeed;
            }
            yield return null;
        }
        callback?.Invoke(); //콜백 호출
        yield break;
    }
    #endregion
}
