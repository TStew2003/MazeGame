using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //singleton part
    private static GameManager _instance = null;


    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }


    }

    public static GameManager instance()
    {
        return _instance;
    }

    public EnemyMovement[] enemies;
    public GridMovement player;
    public GameObject grid;
    public Text heartCount;
    private int activeIndex;
    private int childCount;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        childCount = grid.transform.childCount;
        score = 0;

        for (int i = 1; i < childCount; i++)
        {
            GameObject maze = grid.transform.GetChild(i).gameObject;
            maze.SetActive(false);
        }
        activeIndex = Random.Range(1, (childCount));

        (grid.transform.GetChild(activeIndex)).gameObject.SetActive(true);
    }

    public void SwitchMaze()
    {
        //Debug.Log("switch mazes!");
        int newActiveIndex = activeIndex;
        while (newActiveIndex == activeIndex)
        {
            newActiveIndex = Random.Range(1, childCount);
        }

        (grid.transform.GetChild(activeIndex)).gameObject.SetActive(false);
        (grid.transform.GetChild(newActiveIndex)).gameObject.SetActive(true);

        activeIndex = newActiveIndex;

        UpdateHeartText();
        ResetEnemies();
    }

    public void UpdateHeartText()
    {
        score++;
        heartCount.text = "x" + score;
    }

    public void ResetEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].Reset();
        }
    }



}
