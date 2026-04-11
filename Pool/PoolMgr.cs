using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 缓存池里的抽屉类，包含一个父物体和一个抽屉列表
/// </summary>
public class PoolData
{
    public GameObject fatherObj;
    public List<GameObject> poolList;
    public PoolData(GameObject obj,GameObject poolObj)
    {
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;
        poolList = new List<GameObject>();
        PushObj(obj);
    }

    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);
        poolList.Add(obj);
        obj.transform.parent = fatherObj.transform;
    }

    public GameObject GetObj()
    {
        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0);
        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }

}

/// <summary>
/// 缓存池模块
/// </summary>
public class PoolMgr : BaseManager<PoolMgr>
{
    public Dictionary<string ,PoolData> poolDic = new Dictionary<string ,PoolData>();

    //池父物体
    private GameObject poolObj;

    public GameObject GetObj(string name)
    {
        GameObject obj = null;
        //有抽屉并且抽屉里有东西
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            obj = poolDic[name].GetObj();
        }
        //没有抽屉或者抽屉里没有东西
        else
        {
            obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            obj.name= name;
        }

        return obj;

    }

    public void PushObj(string name,GameObject obj)
    {
        //池父物体不存在就创建一个
        if (poolObj == null)
            poolObj = new GameObject("Pool");

        //有抽屉就放进去，没有抽屉就创建一个抽屉再放进去
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);

        }
        else
        {
            poolDic.Add(name, new PoolData(obj, poolObj));
        }
        
    }

    //清空池子
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }


}
