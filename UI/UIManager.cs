using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

/// <summary>
/// UI层级枚举
/// </summary>
public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
    System
}

/// <summary>
/// UI管理器 
/// 1.管理所有显示的面板
/// 2.提供给外部 显示和隐藏等等接口
/// </summary>
public class UIManager : BaseManager<UIManager>
{

    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>(); 

    //为了方便管理UI界面，Canvas下创建了三个空物体，分别是Bot、Mid、Top、System，分别放置不同层级的UI界面
    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform System;
 
    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">显示在哪一层</param>
    /// <param name="callBack">面板创建完成后 你想做的事</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer = E_UI_Layer.Mid, UnityAction<T> callBack = null ) where T : BasePanel
    {
        if(panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowMe();
            //如果面板已经显示了 就直接返回
            if (callBack != null)
                callBack(panelDic[panelName] as T);
            return;
        }

        ResourcesMgr.Instance().LoadAsync<GameObject>("Prefabs/UI/" + panelName, (obj) =>
        {
            if (obj != null)
            {
                //把它作为Canvas的子对象
                //并且 要设置它的相对位置
                //找到父对象 你到底显示在哪一层
                Transform father = mid;
                switch(layer)
                {
                    case E_UI_Layer.Bot:
                        father = bot;
                        break;
                    case E_UI_Layer.Mid:
                        father = mid;
                        break;
                    case E_UI_Layer.Top:
                        father = top;
                        break;
                    case E_UI_Layer.System:
                        father = System;
                        break;
                }
                //设置父对象 设置相对位置和大小
                obj.transform.SetParent(father);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition= Vector3.zero;

                (obj.transform as RectTransform).offsetMax = Vector2.zero;
                (obj.transform as RectTransform).offsetMin = Vector2.zero;

                //得到面板脚本
                T panel = obj.GetComponent<T>();
                //处理面板创建完成后的逻辑
                if(callBack != null)
                    callBack(panel); 

                panel.ShowMe();

                panelDic.Add(panelName, panel);
            }
        });
    }

    //隐藏面板
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].HideMe();

            GameObject.Destroy(panelDic[panelName]);

            panelDic.Remove(panelName);
        }


    }

}
