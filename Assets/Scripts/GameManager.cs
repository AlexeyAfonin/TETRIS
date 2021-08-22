using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI mine game")]
    [SerializeField] private Text _scoreView;

    [Header ("UI info panel")]
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private Text _recordView;

    public int Score { get; set; } = 0;
    public int Record { get; set; } = 0;
    public Shape ActiveShape { get; set; } = null;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ReadRecord();
        UpdateScoreView();
    }

    private void ReadRecord()
    {
        Record = PlayerPrefs.GetInt("Record", 0);
    }

    public void WriteRecord()
    {
        PlayerPrefs.SetInt("Record", Record);
    }

    public void UpdateScoreView() => _scoreView.text = $"Score: {Score} | Record: {Record}";
    public void UpdateRecord() => Record = Score;
    public void MoveShapeToTheSide(int newPosX) => ActiveShape.Move(newPosX, 0);
    public void MoveShapeToTheDown() => ActiveShape.MoveDown();
    public void RotateShape() => ActiveShape.Rotate();

    public void ShowInfoPanel()
    {
        _infoPanel.SetActive(true);
        _recordView.text = Record.ToString();
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        WriteRecord();
    }
}
