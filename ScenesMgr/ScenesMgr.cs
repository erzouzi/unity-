using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScenesMgr : BaseManager<ScenesMgr>
{
    
    /// <summary>
    /// 同步加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadScene(string name, UnityAction fun)
    {
        // 同步加载场景，加载完成后执行回调函数
        SceneManager.LoadScene(name);
        fun();
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadSceneAsync(string name, UnityAction fun)
    {
            MonoMgr.Instance().StartCoroutine(LoadSceneAsyncCoroutine(name,fun));
    }

    IEnumerator LoadSceneAsyncCoroutine(string name, UnityAction fun)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
        // 异步加载场景，加载完成后执行回调函数
        while (!asyncOperation.isDone)
        {
            EventCenter.Instance().EventTrigger("LoadingProgress", asyncOperation.progress);
            //在这里更新UI进度条，或者其他需要在加载过程中执行的操作
            yield return null;
        }

        fun();


    }

}
