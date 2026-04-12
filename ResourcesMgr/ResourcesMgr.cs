using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesMgr : BaseManager<ResourcesMgr>
{

    //同步加载
    public T Load<T>(string path) where T : Object
    {
        T res =Resources.Load<T>(path);

        if (res is GameObject)
            return GameObject.Instantiate(res);
        else
            return res;

    }

    //异步加载
    public void LoadAsync<T>(string path,UnityAction<T> callBack) where T : Object
    {
        MonoMgr.Instance().StartCoroutine(LoadAsyncCoroutine<T>(path,callBack));
    }

    /// <summary>
    /// 这里是真正的异步加载，加载完成后执行回调函数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    private IEnumerator LoadAsyncCoroutine<T>(string path, UnityAction<T> callBack) where T : Object
    {
        ResourceRequest req = Resources.LoadAsync<T>(path);
        yield return req;

        if (req.asset is GameObject)
            callBack(GameObject.Instantiate(req.asset) as T);
        else
            callBack(req.asset as T);
    }

}
