using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 输入控制模块
/// </summary>
public class InputMgr : BaseManager<InputMgr>
{

    private bool isStart = false;

    // 1. 可修改的按键配置（默认 WASD）
    private KeyCode MoveUp = KeyCode.W;
    private KeyCode MoveDown = KeyCode.S;
    private KeyCode MoveLeft = KeyCode.A;
    private KeyCode MoveRight = KeyCode.D;


    /// <summary>
    /// 构造函数中添加Updata监听
    /// </summary>
    public InputMgr()
    {
       MonoMgr.Instance().AddUpdateListener(Update);
    }

    // 2. 在Update中监听按键输入
    private void Update()
    {
        if (!isStart)
            return;
        CheckKeyCode(MoveUp);
        CheckKeyCode(MoveDown);
        CheckKeyCode(MoveLeft);
        CheckKeyCode(MoveRight);


    }

    // 3. 开启或关闭输入监听
    public void StartOrStopInput()
    {
        isStart = !isStart;
    }

    private void CheckKeyCode(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode))
        {
            EventCenter.Instance().EventTrigger("某键盘按下",keyCode);
        }

        if(Input.GetKeyUp(keyCode))
        {
            EventCenter.Instance().EventTrigger("某键盘抬起", keyCode);
        }

    }

    /// <summary>
    /// 修改移动向上按键
    /// </summary>
    public void ChangeMoveUpKey(KeyCode newKey)
    {
        MoveUp = newKey;
    }
    /// <summary>
    /// 修改移动向下按键
    /// </summary>
    public void ChangeMoveDownKey(KeyCode newKey)
    {
        MoveDown = newKey;
    }
    /// <summary>
    /// 修改移动向左按键
    /// </summary>
    public void ChangeMoveLeftKey(KeyCode newKey)
    {
        MoveLeft = newKey;
    }
    /// <summary>
    /// 修改移动向右按键
    /// </summary>
    public void ChangeMoveRightKey(KeyCode newKey)
    {
        MoveRight = newKey;
    }

}
