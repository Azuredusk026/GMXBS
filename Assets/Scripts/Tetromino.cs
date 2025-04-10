using UnityEngine;

public class Tetromino : MonoBehaviour
{
    #region FIELDS
    static Transform[,] grid = new Transform[WIDTH, HEIGHT];
    
    const int WIDTH = 9;
    const int HEIGHT = 13;
    const float ROTATE_ANGLE = 90f;
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

    void FixedUpdate()
    {
        if (PlayerInput.keepMoveLeft) MoveLeft();
        if (PlayerInput.keepMoveRight) MoveRight();
        if (PlayerInput.keepDrop) MoveDown();
    }
    #endregion

    #region MOVEMENT LOGIC
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
            
            if (x >= 0 && x < WIDTH && z >= 0 && z < HEIGHT)
            {
                grid[x, z] = child;
            }
            else
            {
                Debug.LogWarning($"Block landed out of bounds at ({x}, {z})");
            }
        }
    }
    #endregion

    #region MOVEMENT METHODS
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

    void MoveDown()
    {
        transform.position += Vector3.back;
        if (!Movable)
        {
            transform.position += Vector3.forward;
            Land();
            enabled = false;
            
            CheckGamePhaseTransition();
        }
    }

    void Drop() => MoveDown();
    void CancelDrop() { } // 保留接口，暂不实现
    #endregion

    #region ROTATION
    void Rotate()
    {
        transform.Rotate(Vector3.up, ROTATE_ANGLE);
        if (!Movable) transform.Rotate(Vector3.up, -ROTATE_ANGLE);
    }
    #endregion

    #region GAME PHASE TRANSITION
    void CheckGamePhaseTransition()
    {
        bool atTop = Mathf.RoundToInt(transform.position.z) >= HEIGHT - 2;
        bool atEnd = Vector3.Distance(transform.position, 
                    TetrominoSpawner.Instance.endPoint.position) < 2f;
        
        if(atTop || atEnd)
        {
            TetrominoSpawner.Instance.CanSpawn = false;
        }
        
        // 通知生成器生成下一个方块
        TetrominoSpawner.Instance.SpawnNext();
    }
    #endregion
}