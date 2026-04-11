using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 用里氏替换原则 避免装箱拆箱
/// </summary>
public interface IEventInfo
{

}
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;
    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

/// <summary>
/// 事件中心模块
/// </summary>
public class EventCenter : BaseManager<EventCenter>
{
   public Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();
    public void AddEventListener<T>(string eventName, UnityAction<T> action)
    {
        if(eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions += action;

        }
        else
        {
            eventDic.Add(eventName, new EventInfo<T>(action));
        }
    }
    public void RemoveEventListener<T>(string eventName, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions -= action;
        }
    }
    public void EventTrigger<T>(string eventName, T info)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions?.Invoke(info);
        }
    }

    public void Clear()
    {
        eventDic.Clear();
    }

}
