using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager _isnstance;                   //单例模式的引用

    public GameObject numPrefab;                        //数字的预制体
    public Number[,] numbers = new Number[4, 4];        //保存方格中的数组


    public List<Number> isMovingNum = new List<Number>();  //正在移动中的Num
    public bool hasMove = false;                           //是否有数字发生了移动

    public GameObject exitMessage;


    void Awake()
    {
        _isnstance = this;
    }
    // Use this for initialization
    void Start()
    {
        //游戏开始生成两个数字
        Instantiate(numPrefab);
        Instantiate(numPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        //匹配手机上的返回键
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //若没有，就生成提示，若有则退出游戏（1s内点击2次）
            if (exitMessage == null)  
            {
                exitMessage = Instantiate(exitMessage) as GameObject;
                StartCoroutine("ResetQuitMessage");
            }
            else
            {
                Application.Quit();
            }
        }

        //触屏，，，
        //有触摸点，且滑动
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            int dieX = 0;
            int dieY = 0;
            //获取滑动的距离
            Vector2 touchDelPos = Input.GetTouch(0).deltaPosition;
            if(Mathf.Abs(touchDelPos.x)> Mathf.Abs(touchDelPos.y))
            {
                //滑动距离
                if (Mathf.Abs(touchDelPos.x) > 10)
                {
                    dieX = 1;
                }
                else if(Mathf.Abs(touchDelPos.x) < -10)
                {
                    dieX = -1;
                }
            }
            else
            {
                if (Mathf.Abs(touchDelPos.y) > 10)
                {
                    dieY = 1;
                }
                else if (Mathf.Abs(touchDelPos.y) < -10)
                {
                    dieY = -1;
                }
            }
            MoveNum(dieX, dieY);
        }


        //PC  端测试
        if (isMovingNum.Count == 0)
        {
            int dieX = 0;
            int dieY = 0;
            if (Input.GetKeyDown(KeyCode.A))
            {
                dieX = -1;
            }
            else
            if (Input.GetKeyDown(KeyCode.D))
            {
                dieX = 1;
            }
            else
            if (Input.GetKeyDown(KeyCode.W))
            {
                dieY = 1;
            }
            else
            if (Input.GetKeyDown(KeyCode.S))
            {
                dieY = -1;
            }
            MoveNum(dieX, dieY);
        }

        if (hasMove && isMovingNum.Count == 0)   //生成新的数字
        {
            Instantiate(numPrefab);
            hasMove = false;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (numbers[i, j] != null)
                    {
                        numbers[i, j].OneMove = false;
                    }
                }
            }
        }

    }

    public void MoveNum(int directionX, int directionY)
    {
        if (directionX == 1)  //向右移动  
        {
            //首先将空格填满   最右侧列不需做判断
            for (int j = 0; j < 4; j++)
            {
                for (int i = 2; i >= 0; i--)
                {
                    if (numbers[i, j] != null)  //格子中有物体（数字），，调用移动方法
                    {
                        numbers[i, j].Move(directionX, directionY);
                    }
                }
            }
        }
        else

        //===========向左移动==================
        if (directionX == -1)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int i = 1; i < 4; i++)
                {   //最左侧的一列 [0,0] [0,1] [0,2] [0,3]
                    if (numbers[i, j] != null)
                    {
                        numbers[i, j].Move(directionX, directionY);
                    }
                }
            }
        }
        else

        //===========向上移动==================
        if (directionY == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 2; j >= 0; j--)
                {
                    if (numbers[i, j] != null)
                    {
                        numbers[i, j].Move(directionX, directionY);
                    }
                }
            }
        }
        else

        //===========向下移动==================
        if (directionY == -1)
        {
            for (int i = 3; i >= 0; i--)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (numbers[i, j] != null)  //有物体（数字）就移动
                    {
                        numbers[i, j].Move(directionX, directionY);
                    }
                }
            }
        }
    }


    /// <summary>
    /// 判断是否是空格的方法
    /// </summary>
    /// <param name="x">数组索引X</param>
    /// <param name="y">数组索引Y</param>
    /// <returns></returns>
    public bool isEmpty(int x, int y)
    {
        if (x < 0 || x > 3 || y < 0 || y > 3)
        {
            return false;
        }
        else if (numbers[x, y] != null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void Restart()
    {
        //Application.LoadLevel("scene/HaoHua2048");
        SceneManager.LoadScene("scene/HaoHua2048");
    }
    /// <summary>
    /// 判断游戏是否结束
    /// </summary>
    /// <returns></returns>
    public bool isDead()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (numbers[i, j] == null)
                {
                    return false;
                }
            }
        }

        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                if (numbers[i, j].value == numbers[i + 1, j].value)
                {
                    return false;
                }
            }
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (numbers[i, j].value == numbers[i, j + 1].value)
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// 1秒之后，销毁提示语句
    /// </summary>
    /// <returns></returns>
    IEnumerable ResetQuitMessage()
    {
        yield return new WaitForSeconds(1.0f);
        if(exitMessage != null)
        {
            Destroy(exitMessage);
        }
    }
 
}
