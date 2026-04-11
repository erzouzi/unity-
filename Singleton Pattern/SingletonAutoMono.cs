using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 继承MonoBehaviour的单例类，适用于一些需要挂载在物体上的管理类
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance()
    {
        if( instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = typeof(T).Name;
            DontDestroyOnLoad(obj);
            instance = obj.AddComponent<T>();
        }
        return instance;
    }

}
