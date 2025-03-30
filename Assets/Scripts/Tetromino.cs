using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    #region FIELDS
    static Transform[,] grid = new Transform[WIDTH, HEIGHT];
    
    const int WIDTH = 10;
    const int HEIGHT = 22;

    const float ROTATE_ANGLE = 90f;
    float dropTime;
    float dropTimer;
    #endregion

    #region UNITY EVENT FUNCTIONS
    void OnEnable()
    {
        PlayerInput.onMoveLeft += MoveLeft;
        PlayerInput.onMoveRight += MoveRight;
        PlayerInput.onDrop += Drop;
        PlayerInput.onCancelDrop += CancelDrop;
        PlayerInput.onRotate += Rotate;
    }

    void OnDisable()
    {
        PlayerInput.onMoveLeft -= MoveLeft;
        PlayerInput.onMoveRight -= MoveRight;
        PlayerInput.onDrop -= Drop;
        PlayerInput.onCancelDrop -= CancelDrop;
        PlayerInput.onRotate -= Rotate;
    }
    
    void Start()
    {
        dropTime = GameManager.Instance.AutoDropTime;
    }

    void FixedUpdate()
    {
        dropTimer += Time.fixedDeltaTime;

        if (dropTimer >= dropTime)
        {
            dropTimer = 0f;
            MoveDown();
        }

        if (PlayerInput.keepMoveLeft)
        {
            MoveLeft();
        }

        if (PlayerInput.keepMoveRight)
        {
            MoveRight();
        }
    }
    #endregion

    #region GENERIC
    bool Movable
    {
        get
        {
            foreach (Transform child in transform)
            {
                int x = Mathf.RoundToInt(child.position.x);
                int z = Mathf.RoundToInt(child.position.z);

                if (x < 0 || x >= WIDTH || z < 0 || z >= HEIGHT || grid[x, z] != null)
                {
                    return false;
                }
            }

            return true;
        }
    }

    void Land()
    {
        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.position.x);
            int z = Mathf.RoundToInt(child.position.z);
            grid[x, z] = child;
        }
    }
    #endregion

    #region  HORIZONTAL MOVE
    void MoveLeft()
    {
        transform.position += Vector3.left;
        if (!Movable) transform.position += Vector3.right;
    }

    void MoveRight()
    {
        transform.position += Vector3.right;
        if (!Movable) transform.position += Vector3.left;
    }
    #endregion

    #region  VERTICAL MOVE
    void MoveDown()
    {
        transform.position += Vector3.back;
        if (!Movable)
        {
            transform.position += Vector3.forward;
            Land();
            enabled = false;
            bool atTop = Mathf.RoundToInt(transform.position.z) >= HEIGHT - 2;
            bool atEnd = Vector3.Distance(transform.position, GameManager.Instance.endPoint.position) < 2f;
        
            if(atTop || atEnd)
            {
                GameManager.Instance.canSpawn = false;
            }
            GameManager.Instance.SpawnTetromino();
        }
    }

    void Drop()
    {
        dropTime = Time.fixedDeltaTime;
    }

    void CancelDrop()
    {
        dropTime = GameManager.Instance.AutoDropTime;
    }
    #endregion

    #region ROTATE
    void Rotate()
    {
        transform.Rotate(Vector3.up, ROTATE_ANGLE);
        if (!Movable) transform.Rotate(Vector3.up, -ROTATE_ANGLE);
    }
    #endregion
    
}