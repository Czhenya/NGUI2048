using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour
{

    public int posX;                 //在二维数组中的位置X，Y，，，
    public int posY;
    public TweenPosition tpMove;     //Tween动画

    private bool isMoving = false;   //动画是否播放过的计数
    public int value;               //产生数字是几
    private bool toDestroy;          //判断数字是否销毁  
    public bool OneMove = false;     //标识数字是否合并过一次

 
    // Use this for initialization
    void Start()
    {

        //80%成2的概率，，，更改本身的Sprite名字，以更换图片
        value = Random.value > 0.2f ? 2 : 4;
        this.GetComponent<UISprite>().spriteName = value.ToString();
        do
        {
            posX = Random.Range(0, 4);
            posY = Random.Range(0, 4);
        } while (Manager._isnstance.numbers[posX, posY] != null);

        transform.localPosition = new Vector3(-290 + posX * 200, -280 + posY * 200, 0);
        Manager._isnstance.numbers[posX, posY] = this;   //存放数字本身到数组中，表示此位置有数字不能生成新的数字

        if (Manager._isnstance.isDead())
        {
            Debug.Log("TODO 游戏结束");
        }
        
        //生成时直接生成在目标位置
        tpMove.from = new Vector3(-290 + posX * 200, -280 + posY * 200, 0);
        tpMove.to = new Vector3(-290 + posX * 200, -280 + posY * 200, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //播放一次动画
        if (isMoving == false)
        {
            if (transform.localPosition != new Vector3(-290 + posX * 200, -280 + posY * 200, 0))
            {
                isMoving = true;
                tpMove.from = transform.localPosition;
                tpMove.to = new Vector3(-290 + posX * 200, -280 + posY * 200, 0);
                tpMove.ResetToBeginning();
                tpMove.PlayForward();

            }
        }
    }
    /// <summary>
    /// 核心，移动方法（有空格，有物体是否一样）
    /// </summary>
    public void Move(int directionX, int directionY)
    {
        //Debug.Log("测试");
        
        //==========向右移动==================  
        if (directionX == 1) 
        {
            int index = 1;  //空格标志
            while (Manager._isnstance.isEmpty(posX+index,posY))
            {
                index++;
            }
            //有空格的移动
            if (index>1)
            {
                
                if (!Manager._isnstance.isMovingNum.Contains(this))
                {   //保证不会重复添加物体（数字）到列表，
                    Manager._isnstance.isMovingNum.Add(this);
                }
                //移动一次，就生成两个数字的标志符
                Manager._isnstance.hasMove = true;
                //向空格位置移动
                Manager._isnstance.numbers[posX, posY] = null;
                posX = posX + index - 1;
                Manager._isnstance.numbers[posX, posY] = this;
            }
            //有相同数字的移动
            if (posX < 3 && value == Manager._isnstance.numbers[posX+1,posY].value &&
                !Manager._isnstance.numbers[posX+1,posY].OneMove)
            {
                //只和并一次的标志
                Manager._isnstance.numbers[posX + 1, posY].OneMove = true;
                //移动的标志，（生成新的物体（数字））
                Manager._isnstance.hasMove = true;
                //动画播放的限定（有数字在列表中就不会重复播放第二次动画）  
                if (!Manager._isnstance.isMovingNum.Contains(this))   //不会重复添加物体（数字）到列表，
                {
                    Manager._isnstance.isMovingNum.Add(this);
                }
                //碰到一样的数字，讲位置设为空 并销毁本身标识（true），再将其位置上的值变为2倍，（更换成新的数字）
                toDestroy = true;
                Manager._isnstance.numbers[posX, posY] = null;
                Manager._isnstance.numbers[posX + 1, posY].value *= 2;
                posX += 1;
            }
        }else

        //===========向左移动==================
        if (directionX == -1)
        {
            int index = 1;
            while (Manager._isnstance.isEmpty(posX - index, posY))
            {
                index++;
            }
            //有空格的移动
            if (index > 1)
            {
                Manager._isnstance.hasMove = true;
                if (!Manager._isnstance.isMovingNum.Contains(this))
                {
                    Manager._isnstance.isMovingNum.Add(this);
                }

                Manager._isnstance.numbers[posX, posY] = null;
                posX = posX - index + 1;
                Manager._isnstance.numbers[posX, posY] = this;
            }

            //碰到相同数字的移动
            if (posX > 0 && value == Manager._isnstance.numbers[posX - 1, posY].value && 
                !Manager._isnstance.numbers[posX - 1, posY].OneMove)
            {
                Manager._isnstance.numbers[posX - 1, posY].OneMove = true;
                Manager._isnstance.hasMove = true;
                if (!Manager._isnstance.isMovingNum.Contains(this))
                {
                    Manager._isnstance.isMovingNum.Add(this);
                }

                toDestroy = true;
                Manager._isnstance.numbers[posX, posY] = null;
                Manager._isnstance.numbers[posX - 1, posY].value *= 2;
                posX -= 1;
            }

        }else

        //===========向上移动==================
        if (directionY == 1)
        {
            int index = 1;   //空格标志
            while (Manager._isnstance.isEmpty(posX , posY + index))
            {
                index++;
            }
            //有空格的移动
            if (index > 1)
            {
                Manager._isnstance.hasMove = true;
                if (!Manager._isnstance.isMovingNum.Contains(this))
                {
                    Manager._isnstance.isMovingNum.Add(this);
                }

                Manager._isnstance.numbers[posX, posY] = null;
                posY = posY + index - 1;
                Manager._isnstance.numbers[posX, posY] = this;
            }
            //有相同位置的移动
            if (posY < 3 && value == Manager._isnstance.numbers[posX , posY + 1].value && !Manager._isnstance.numbers[posX, posY + 1].OneMove)
            {
                Manager._isnstance.numbers[posX , posY + 1].OneMove = true;
                Manager._isnstance.hasMove = true;
                if (!Manager._isnstance.isMovingNum.Contains(this))
                {
                    Manager._isnstance.isMovingNum.Add(this);
                }

                toDestroy = true;
                Manager._isnstance.numbers[posX, posY] = null;
                Manager._isnstance.numbers[posX , posY + 1].value *= 2;
                posY += 1;
            }

        }else

        //===========向下移动==================
        if (directionY == -1)
        {
            int index = 1;  //空格标志位
            while (Manager._isnstance.isEmpty(posX, posY - index))
            {
                index++;
            }
            //有空格的移动
            if (index > 1)
            {
                Manager._isnstance.hasMove = true;
                if (!Manager._isnstance.isMovingNum.Contains(this))
                {
                    Manager._isnstance.isMovingNum.Add(this);
                }

                Manager._isnstance.numbers[posX, posY] = null;
                posY = posY - index + 1;
                Manager._isnstance.numbers[posX, posY] = this;
            }
            //有相同数字的移动
            if (posY > 0 && value == Manager._isnstance.numbers[posX, posY - 1].value && !Manager._isnstance.numbers[posX, posY - 1].OneMove)
            {
                Manager._isnstance.numbers[posX, posY -1].OneMove = true;
                Manager._isnstance.hasMove = true;
                if (!Manager._isnstance.isMovingNum.Contains(this))
                {
                    Manager._isnstance.isMovingNum.Add(this);
                }

                toDestroy = true;
                Manager._isnstance.numbers[posX, posY] = null;
                Manager._isnstance.numbers[posX, posY - 1].value *= 2;
                posY -= 1;
                }
        }
    }

    /// <summary>
    /// 动画结束，标志改为false
    /// </summary>
    public void MoveOver()
    {
        isMoving = false;
        if (toDestroy)   //若碰到了相同的数字  销毁自己，和改变另一个图片（数字）
        {
            Destroy(this.gameObject);
            Manager._isnstance.numbers[posX, posY].GetComponent<UISprite>().spriteName =
                 Manager._isnstance.numbers[posX, posY].value.ToString();
            if (Manager._isnstance.numbers[posX, posY].value == 2048)
            {
                //TODO   达到2048  最高分
                Debug.Log("最高分");
            }
        }
        Manager._isnstance.isMovingNum.Remove(this);
        
    }

}
