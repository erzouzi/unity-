using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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

    //记录我们UI的Canvas 方便以后外部可能会使用它
    public RectTransform canvas;

    public UIManager()
    {
        //创建Canvas让其在场景切换时不被销毁
        GameObject obj = ResourcesMgr.Instance().Load<GameObject>("Prefabs/UI/Canvas");
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(canvas);

        //找到各层级
        bot = canvas.transform.Find("Bot");
        mid = canvas.transform.Find("Mid");
        top = canvas.transform.Find("Top");
        System = canvas.transform.Find("System");

        //创建EventSystem让其在场景切换时不被销毁
        obj = ResourcesMgr.Instance().Load<GameObject>("Prefabs/UI/EventSystem");
        GameObject.DontDestroyOnLoad(obj);
    }

    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bot:
                return bot;
            case E_UI_Layer.Mid:
                return mid;
            case E_UI_Layer.Top:
                return top;
            case E_UI_Layer.System:
                return System;
        }
        return null;
    }

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

    /// <summary>
    /// 得到某一个已经显示的面板方便外部使用
    /// </summary>
    /// <param name="panelName"></param>
    /// <param name="callBack"></param>
    public T GetPanel<T>(string panelName) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return null;
    }

    /// <summary>
    /// 给空间添加一个自定义事件监听器 方便外部使用
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">事件的响应函数</param>
    public static void AddCustomEventTriggerListener(UIBehaviour control, EventTriggerType type ,UnityAction<BaseEventData> callBack)
    {

        EventTrigger trigger = control.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = control.gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        trigger.triggers.Add(entry);

    }

}
