using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public SceneChanger sceneChanger;
    public GameScene gameScene;
    public GameObject tutorHand;
    public GameObject cookiePrefab;
    #region Game status
    private Level currentLevelData;
    private bool isGameWin = false;
    private bool isGameLose = false;
    private bool isGamePause = false;
    private bool isGameStart = false;

    private float startCountDown = 1.5f;
    [SerializeField]
    public int achivement = 0;
    private const int MAX_ACHIVE = 3;
    #endregion

    private void OnEnable()
    {
        currentLevelData = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex);
        GameObject map = Instantiate(currentLevelData.map);
        GridCellManager.instance.SetTileMap(map.transform.GetChild(0).GetChild(0).GetComponent<Tilemap>());
        MovementManager.instance.SetContainer(map.transform.GetChild(1));
        foreach(CookieData data in currentLevelData.cookies)
        {
            GameObject cookie = Instantiate(cookiePrefab, map.transform.GetChild(1));
            cookie.GetComponent<Cookie>().SetCookieType(data.type);
            cookie.transform.position = GridCellManager.instance.PositonToMove(data.cellPos);
            cookie.GetComponent<Cookie>().SetDefaultPos(cookie.transform.position);
            GridCellManager.instance.PlacedCell(data.cellPos);
        }

        MovementManager.instance.DevideTheCake();
        Time.timeScale = 1;
    }

    private void Update()
    {
        if(!isGameStart)
        {
            if(startCountDown > 0)
            {
                startCountDown -= Time.deltaTime;
            }
            else
            {
                isGameStart = true;
            }
        }
    }

    public void Win()
    {
        if (LevelManager.instance.levelData.GetLevels().Count > LevelManager.instance.currentLevelIndex + 1)
        {
            if (LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex + 1).isPlayable == false)
            {
                LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex + 1, true, false);
            }
        }

        isGameWin = true;
        StartCoroutine(DelayWin());
        //LevelManager.instance.levelData.SaveDataJSON();
    }


    private IEnumerator DelayWin()
    {
        yield return new WaitForSecondsRealtime(.7f);
        gameScene.ShowWinPanel();
    }

    private void SetAchivement()
    {

    }

    public void DisableHand()
    {
        if (tutorHand != null && tutorHand.activeSelf)
        {
            tutorHand.SetActive(false);
        }
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }

    public bool IsGameLose()
    {
        return isGameLose;
    }

    public bool IsGameStart()
    {
        return isGameStart;
    }

    public void PauseGame()
    {
        isGamePause = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        isGamePause = false;
        Time.timeScale = 1;
    }

    public bool IsGamePause()
    {
        return isGamePause;
    }
}

