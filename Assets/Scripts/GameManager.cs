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
    public Shape ActiveShape { get; set; } = null;

    public static GameManager Instance;

    private int _record;

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
        UpdateScore();
    }

    private void ReadRecord()
    {
        _record = PlayerPrefs.GetInt("Record", 0);
    }

    public void WriteRecord()
    {
        PlayerPrefs.SetInt("Record", _record);
    }

    public void UpdateScore() => _scoreView.text = $"Score: {Score} | Record: {_record}";
    public void MoveShapeToTheSide(int newPosX) => ActiveShape.Move(newPosX, 0);
    public void MoveShapeToTheDown() => ActiveShape.Move(0, -1);
    public void RotateShape() => ActiveShape.Rotate();

    public void ShowInfoPanel()
    {
        _infoPanel.SetActive(true);
        _recordView.text = _record.ToString();
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
