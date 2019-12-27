using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("MainSettings")]

    public int baseHp = 10; //жизнь базы
    public float delayWaves; // задержка между акивизацией волн
    public float enemyInterval; //интервал между противниками, уменьшается с увеличением волны, присваивается в функции GameSetting()
    public int waveSize; // рандомное количество противников в текущей волне, присваивается в функции GameSetting()
    public int gold; //валюта, присваивается в скрипте Enemy.cs
    public int kills; // общее количество убитых противников
    public bool gameOver; // конец игры

    [Header("OtherSettings")]
    public GameObject enemyPrefab; //префаб противника
    public int waveNum; // номер волны
    public int factorWaveSize; //увеличивает количество противников с кажной новой волной, присваивается в функции GameSetting()
    public int factorGold; //увеличение получаемого золота, увеличивается с увеличением волны, присваивается в функции GameSetting()
    public int waveIndex; //приравнивается к waveSize, отслеживает уничтоения противников в текущей волне, присваивается в скрипте Enemy.cs
                          //когда равен нулю настраивается и запускается новая волна в функции GameSetting()

    public int countEnemy; //количество созданных противников в текущей волне, увеличивается при создании префаба противника,
                          //когда равен размеру текущей волны, создание префабов останавливается, отслеживание происходит в функции Wave()

    float enemyMaxHp = 80; // здоровье противника приравниваетя к максимальному здоровью префаба в скрипте Enemy.cs,
                           //увеличивается при увеличении волны в функции GameSetting()

    float startTime; //начало созданиея противников в волне



    bool figth; //сейчас идет бой
    float enemySpeed = 4f; // скорость противников, увеличивается с увеличением волны, в функции GameSetting()

    [Header("Nivigation")]
    public Transform[] way; //заданный путь
    public Transform spawnPoint; //точка создания противников

    [Header("UI")]
    public GameObject restartPanel; //панель для перезапуска сцены
    public Text goldTxt, killsTxt;
    public Text baseHpTxt;
    public Text waveTxt, TimerTxt;
    float time;// таймер между волнами , приравнивается к delayWaves

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartSettting();
    }

    void Update()
    {
        GameSetting(); //все насройки 
        Wave();
        Base();
    }
    //начальные настройки
    void StartSettting()
    {
        Time.timeScale = 1;
        time = delayWaves;
    }

    //создание префаба противника
    void SpawnEnemy()
    {
        countEnemy++;
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity) as GameObject;
    }

    //конфигурвции волн, настройки игры
    void GameSetting()
    {
        goldTxt.text = gold.ToString(); // количество золота в текст
        baseHpTxt.text = baseHp.ToString(); // жизни базы в текст 
        enemyPrefab.GetComponent<Enemy>().maxHp = enemyMaxHp; //присвоение максимального HP в префаб противника с каждой волной увеличивается
        enemyPrefab.GetComponent<Enemy>().speed = enemySpeed;


        // настройки новой волны
        if (waveIndex == 0 && figth)
        {
            waveTxt.enabled = false;
            factorWaveSize += 3;
            factorGold++; //присваивается в скрипте Enemy
            waveSize = UnityEngine.Random.Range(4,7)  + factorWaveSize;
            waveIndex = waveSize; 
            waveNum++;
            enemyMaxHp += 30;
            if (enemyInterval > 0.5f)
                enemyInterval -= 0.5f;
            countEnemy = 0;//
            time = delayWaves;
            TimerTxt.enabled = true;
            StartCoroutine(DelayWaves());
        }
        //таймер следующей волны
        if (time >= 0)
        {
            time -= Time.deltaTime;
            TimerTxt.text = time.ToString();
            var ts = TimeSpan.FromSeconds(time);
            TimerTxt.text = string.Format("Next Wave: " + "{1}", ts.Minutes, ts.Seconds);
        }

        //нахождение башни с помощью луча и включение кнопри улучшения
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hit = Physics.RaycastAll(ray, 100.0f);

            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.GetComponent<upgreadTurret>() != null)
                {
                    hit[i].collider.GetComponent<upgreadTurret>().OpenDialog();
                }
            }
        }
    }

    // волна
    void Wave()
    {
        if (!figth && !gameOver)
        {
            figth = true;
            InvokeRepeating("SpawnEnemy", startTime, enemyInterval);
        }
        if (countEnemy == waveSize && figth)
        {
            CancelInvoke("SpawnEnemy");
        }
    }
    
    void Base()
    {
        if(baseHp <= 0)
        {
            gameOver = true;
            killsTxt.text = "You killed " + kills.ToString();
            restartPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Restart()
    {
        
        SceneManager.LoadScene(0);
        
    }
    //задержка между волнами
    IEnumerator DelayWaves()
    {
        yield return new WaitForSeconds(delayWaves);
        TimerTxt.enabled = false;
        waveTxt.enabled = true;
        waveTxt.text = "Wave " + waveNum.ToString();
        figth = false;
    }
}
