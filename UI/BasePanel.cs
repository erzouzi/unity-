using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    // Start is called before the first frame update
    public Dictionary<string,List<UIBehaviour>> uiDic = new Dictionary<string, List<UIBehaviour>>();

    void Start()
    {
        FindChildrenControl<Button>();
        FindChildrenControl<TMP_Text>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<Slider>();
        FindChildrenControl<Scrollbar>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<TMP_InputField>();
        FindChildrenControl<InputField>();
        FindChildrenControl<TMP_Dropdown>();
        FindChildrenControl<RawImage>();
    }

    /// <summary>
    /// 得到对应名字的对应控件脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controlName"></param>
    /// <returns></returns>
    protected T GetControl<T>(string controlName)where T : UIBehaviour
    {
        if(uiDic.ContainsKey(controlName))
        {
            for(int i = 0; i < uiDic[controlName].Count; i++)
            {
                if(uiDic[controlName][i] is T)
                {
                    return uiDic[controlName][i] as T;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 找到子物体的对应控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>();
        string objName;
        for(int i = 0; i < controls.Length; i++)
        {
            objName = controls[i].gameObject.name;
            if(uiDic.ContainsKey(objName))
            {
                uiDic[objName].Add(controls[i]);
            }
            else
            {
                uiDic.Add(objName, new List<UIBehaviour>() { controls[i] });
            }

        }


    }

    public virtual void ShowMe()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void HideMe()
    {
        this.gameObject.SetActive(false);
    }
}
