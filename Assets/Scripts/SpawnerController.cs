using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SpawnerController : MonoBehaviour
{
    public static SpawnerController Instance { get; private set; }

    [SerializeField] private BoxCollider2D spawn01;
    [SerializeField] private BoxCollider2D spawn02;
    [SerializeField] private BoxCollider2D spawn03;
    [SerializeField] private BoxCollider2D spawn04;

    [SerializeField] private GameObject pistolEnemy;
    [SerializeField] private GameObject knifeEnemy;
    [SerializeField] private GameObject sniperEnemy;

    [SerializeField] private Transform playerRef;

    [SerializeField] private Transform waveBar;

    [SerializeField] private GameObject crosshair;

    private List<GameObject> enemies = new List<GameObject>();
    private int waveIndex = 0;
    public int waveLevelIndex = 1;

    public int extraHP = 0;
    public float extraSpeed = 0;

    [SerializeField] private float timeBetweenWaves = 2f;
    [SerializeField] private float countDown = 2f;

    private bool choseUpgrade;

    private bool enemysStillSpawning = false;

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    public SpawnState state = SpawnState.COUNTING;

    private Camera cam;

    public Hashtable enemyData = new Hashtable();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        cam = Camera.main;
        playerRef = GameObject.Find("Player").transform;
    }

    private void Start()
    {
        StartCoroutine(RunSpawner());
    }


    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            waveIndex = 10;
        }*/

/*        if (Input.GetKeyDown(KeyCode.Return) && playerRef == null)
        {
            SceneManager.LoadScene(0);
        }*/
    }

    private void ChooseBox()
    {
        int num = Random.Range(1, 5);
        switch (num)
        {
            case 1:
                RandomEnemy(spawn01);
                break;
            case 2:
                RandomEnemy(spawn02);
                break;
            case 3:
                RandomEnemy(spawn03);
                break;
            case 4:
                RandomEnemy(spawn04);
                break;
            default:
                break;
        }
    }

    private void RandomPoint(BoxCollider2D box, GameObject enemy)
    {
        float xMin = box.bounds.min.x;
        float xMax = box.bounds.max.x;
        float yMin = box.bounds.min.y;
        float yMax = box.bounds.max.y;

        Vector2 randomPoint = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));

        Vector3 viewPos = cam.WorldToViewportPoint(randomPoint);

        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
            enemies.Add(Instantiate(enemy, randomPoint, Quaternion.identity));
        }
        else
        {
            ChooseBox();
        }
    }

    private void RandomEnemy(BoxCollider2D local)
    {
        int num = Random.Range(1, 4);
        switch (num)
        {
            case 1:
                RandomPoint(local, pistolEnemy);
                break;
            case 2:
                RandomPoint(local, knifeEnemy);
                break;
            case 3:
                RandomPoint(local, sniperEnemy);
                break;
            default:
                break;
        }


    }

    private IEnumerator RunSpawner()
    {
        yield return new WaitForSeconds(countDown);

        while (true)
        {
            state = SpawnState.SPAWNING;

            yield return SpawnWave();

            state = SpawnState.WAITING;

            yield return new WaitWhile(EnemyisAlive);


            state = SpawnState.COUNTING;

            if (waveIndex >= 11)
            {
                waveLevelIndex++;
                waveIndex = 0;
                extraHP += 30;
                extraSpeed += 0.1f;
                playerRef.GetComponent<PlayerHealth>().Heal();
                if(waveLevelIndex < 4)
                {
                    Invoke("InitiateUpgrade", 1.0f);
                }
                else
                {
                    GlobalMaterial.Instance.ColorChange();
                    ChoseUpgrade();
                }
                yield return new WaitWhile(DelayWhileChoose);
                yield return new WaitForSeconds(timeBetweenWaves);
            }
            else
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }

        }
    }

    private bool EnemyisAlive()
    {
        // Filter out null entries
        enemies = enemies.Where(e => e != null).ToList();

        return (enemies.Count > 0) && enemysStillSpawning;
    }

    private bool DelayWhileChoose()
    {
        return !choseUpgrade;
    }

    public void ChoseUpgrade()
    {
        choseUpgrade = true;
        Invoke("ChoseUpgradeReset", 0.1f);
    }

    private void ChoseUpgradeReset()
    {
        choseUpgrade = false;
    }

        private IEnumerator SpawnWave()
    {
        waveIndex++;

        enemysStillSpawning = false;

        //Debug.Log("Wave: " + waveIndex);

        waveBar.localScale = new Vector3((((float)waveIndex - 1) / 10) * 173.3333f, 2.85f, 1);

        for (int i = 0; i < waveIndex + waveLevelIndex; i++)
        {
            ChooseBox();
            yield return new WaitForSeconds(1.0f);
        }

        enemysStillSpawning = true;
    }

    private void InitiateUpgrade()
    {
        UpgradeCanvas.Instance.gameObject.SetActive(true);
        crosshair.SetActive(false);
        Cursor.visible = true;
        Time.timeScale = 0;
    }
}

