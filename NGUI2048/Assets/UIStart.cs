using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStart : MonoBehaviour {

    public UISprite Help_UI;
    public UISprite TongJi_UI;

    // Use this for initialization
    void Start () {
        Help_UI.gameObject.SetActive(false);
        TongJi_UI.gameObject.SetActive(false);
    }

    void Update()
    {
        //匹配手机上的返回键
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //退出游戏
            Application.Quit();
           
        }
    }

    /// <summary>
    /// 进入游戏界面
    /// </summary>
    public void StartGame()
    {
        //Application.LoadLevel(0);
        SceneManager.LoadScene("Scene/HaoHua2048");
    }

    /// <summary>
    /// 帮助按钮
    /// </summary>
    public void Help_btn()
    {
        Help_UI.gameObject.SetActive(true);
    }

    /// <summary>
    /// 帮助返回按钮
    /// </summary>
    public void HelpBack_btn()
    {
        Help_UI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 统计函数
    /// </summary>
    public void TongJi()
    {
        TongJi_UI.gameObject.SetActive(true);
    }

    /// <summary>
    /// 关闭统计函数
    /// </summary>
    public void TongJi_back()
    {
        TongJi_UI.gameObject.SetActive(false);
    }
}
