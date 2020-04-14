using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisFigure : MonoBehaviour
{
    private float PreviousTime;
    private float FallTime=0.8f;
    private static int FieldWidth=10;
    private static int FieldHeight=20;
    [SerializeField] private Vector3 RotationPoint;
    private static Transform [,] grid = new Transform[FieldWidth,FieldHeight];
    private static int Score = 0;


    private void Start()
    {
        //если нельзя заспаунить фигуру -игра окончена
        if (!ValidMove())
        {
            print($"Your score: {Score}");
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position+=new Vector3(-1,0);
            if (!ValidMove()) transform.position-=new Vector3(-1,0);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position+=new Vector3(1,0);
            if (!ValidMove()) transform.position-=new Vector3(1,0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0,0,1), 90);
            if (!ValidMove()) transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0,0,1), -90);
        }
        
        if (Time.time - PreviousTime > (Input.GetKeyDown(KeyCode.DownArrow)? FallTime*0.1 : FallTime))
        {
            transform.position+=new Vector3(0,-1);
            PreviousTime = Time.time;
            if (!ValidMove())
            {
                transform.position-=new Vector3(0,-1);
                AddToGrid();
                CheckForLines();
                this.enabled = false;
                FindObjectOfType<Spawner>().SpawnRandomTetromino();
            }
        }
    }

    //проверяет может ли фигура занимать текущую позицию
    bool ValidMove()
    {
        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.position.x);
            int y = Mathf.RoundToInt(child.position.y);
            if (x < 0 || x >= FieldWidth || y < 0 || y >= FieldHeight) return false;
            if (grid[x, y] != null) return false;

        }
        return true;
    }

    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.transform.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y);
            grid[x, y] = child;
        }
    }

    void CheckForLines()
    {
        int linesCleared=0;
        for (int i = FieldHeight-1; i >=0; i--)
        {
            if (Hasline(i))
            {
                DeleteLine(i);
                RowDown(i);
                linesCleared++;
            }
        }
        
        Score += linesCleared * linesCleared * FieldWidth; // больше очков за несколько линий
    }

    //Проверка на заполненный ряд
    bool Hasline(int i)
    {
        for (int j = 0; j < FieldWidth; j++)
        {
            if (grid[j, i] == null) return false;
        }

        return true;
    }

    //Удаление блоков в указанной строке
    void DeleteLine(int i)
    {
        for (int j = 0; j < FieldWidth; j++)
        {
            Destroy(grid[j,i].gameObject);
            grid[j, i] = null;
            
        }
    }

    //Смещение блоков над указанным рядом
    void RowDown(int i)
    {
        for (int j = i; j < FieldHeight; j++)
        {
            for (int k = 0; k < FieldWidth; k++)
            {
                if (grid[k, j] != null)
                {
                    grid[k, j - 1] = grid[k, j];
                    grid[k, j] = null;
                    grid[k, j - 1].transform.position+=new Vector3(0,-1);  
                }
            }
        }
    }
}
