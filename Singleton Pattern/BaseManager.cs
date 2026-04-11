using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 不继承MonoBehaviour的单例类，适用于一些不需要挂载在物体上的管理类
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseManager<T> where T : new()
{
    private static T instance;
    public static T Instance()
    {
        if (instance == null)
        {
            instance = new T();
        }
        return instance;
    }
}
