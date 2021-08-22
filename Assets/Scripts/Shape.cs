using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] private GameObject _rig;

    public bool IsMove { get; private set; } = true;

    private float _timer;

    private void Update()
    {
        if (IsMove)
        {
            Drop();
            Controller();
        }
    }

    private void Drop()
    {
        _timer += 1 * Time.deltaTime;
        if (_timer >= GameLogic.dropTime)
        {
            Move(0, -1);
            _timer = 0;
        }
    }

    private void Controller()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && _timer >= GameLogic.quickDropTime)
        {
            Move(0, -1);
            _timer = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(-1, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(1, 0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate();
        }
    }

    public void MoveDown()
    {
        if (_timer >= GameLogic.quickDropTime)
        {
            Move(0, -1);
            _timer = 0;
        }
    }

    public void Move(float newPosX, float newPosY)
    {
        transform.position += new Vector3(newPosX, newPosY, 0);

        if(newPosY != 0)
        {
            _timer = 0;
        }

        if (!CheckValid())
        { 
            transform.position -= new Vector3(newPosX, newPosY, 0);

            if (newPosY != 0)
            {
                IsMove = false;
                RegisterShape();
            }
        }
    }

    public void Rotate()
    {
        _rig.transform.eulerAngles -= new Vector3(0, 0, 90);

        if (!CheckValid())
        {
            _rig.transform.eulerAngles += new Vector3(0, 0, 90);
        }
    }

    private bool CheckValid()
    {
        foreach(Transform cube in _rig.transform)
        {
            if (cube.transform.position.x >= GameLogic.width ||
                cube.transform.position.x < 0 ||
                cube.transform.position.y < 0)
            {
                return false;
            }
            if (cube.position.y < GameLogic.height &&
                GameLogic.Instance.grid[Mathf.FloorToInt(cube.position.y), Mathf.FloorToInt(cube.position.x)] != null)
            {
                return false;
            }
        } 
        return true;
    }

    private void RegisterShape()
    {
        foreach (Transform cube in _rig.transform)
        {
            if (cube.transform.position.y > GameLogic.height)
            {
                GameLogic.Instance.IsLose = true;
                GameManager.Instance.WriteRecord();
                GameManager.Instance.ShowInfoPanel();
            }
            else
            {
                GameLogic.Instance.grid[Mathf.FloorToInt(cube.position.y), Mathf.FloorToInt(cube.position.x)] = cube;
            }
        }
    }

    public void SetColor(Color colorCubes)
    {
        foreach (Transform cube in _rig.transform)
        {
            cube.GetComponent<SpriteRenderer>().color = colorCubes;
        }
    }
}
