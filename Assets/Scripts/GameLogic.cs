using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [Header ("Shapes")]
    [SerializeField] private Shape[] _shapes;
    [SerializeField] private Color[] _shapeColors;
    [SerializeField] private GameObject _shapeSpawnPoint;

    public float dropTime = 1.0f;
    public float quickDropTime = 0.05f;
    public int width = 15, height = 30;
    public Transform[,] grid;
    public static GameLogic Instance;
    public bool IsLose { get; set; } = false;

    private Shape _activeShape;
    private int _сomplicationСriterion = 10;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        grid = new Transform[height, width];
        SpawnShape();
    }

    private void Update()
    {
        if(!_activeShape.IsMove && !IsLose)
        {
            SpawnShape();
        }

        CheckLines();
    }

    private void SpawnShape()
    {
        _activeShape = Instantiate(_shapes[Random.Range(0, _shapes.Length-1)]); 
        _activeShape.SetColor(_shapeColors[Random.Range(0, _shapeColors.Length - 1)]);
        _activeShape.transform.position = _shapeSpawnPoint.transform.position;
        GameManager.Instance.ActiveShape = _activeShape;
    }

    public void CheckLines()
    {
        for(int y = 0; y < height; y++)
        {
            if(IsLineComplete(y))
            {
                DestroyLine(y);
                MoveLines(y);
            }
        }
    }

    private void DestroyLine(int numLine)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[numLine, x].gameObject);
        }
        UpdateScore();
        AddDifficultyGame();
    }

    private void UpdateScore()
    {
        GameManager.Instance.Score += 10;
        if (GameManager.Instance.Score > GameManager.Instance.Record)
        {
            GameManager.Instance.UpdateRecord();
        }
        GameManager.Instance.UpdateScoreView();
    }

    private void AddDifficultyGame()
    {
        if(GameManager.Instance.Score == _сomplicationСriterion)
        {
            if (dropTime > 0.5f)
            {
                dropTime -= 0.2f;
            }
            else if (dropTime > 0.1f)
            {
                dropTime -= 0.1f;
            }
            _сomplicationСriterion += _сomplicationСriterion;
        }
    }

    private void MoveLines(int numLine)
    {
        for (int y = numLine; y < height - 1; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[y + 1, x] != null && y + 1 < height)
                {
                    grid[y, x] = grid[y + 1, x];
                    grid[y, x].transform.position -= new Vector3(0, 1, 0);
                    grid[y + 1, x] = null;
                }
            }
        }
    }

    private bool IsLineComplete(int numLine)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[numLine, x] == null)
            {
                return false;
            }
        }
        return true;
    }
}
