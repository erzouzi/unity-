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

    protected virtual void Awake()
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

    //让子类重写这个方法 来处理按钮点击事件
    protected virtual void OnClick(string btnName)
    {

    }
    //让子类重写这个方法 来处理Toggle值改变事件
    protected virtual void OnToggleValueChanged(string toggleName, bool value)
    {

    }

    /// <summary>
    /// 找到子物体的对应控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>();

        for (int i = 0; i < controls.Length; i++)
        {
            string objName = controls[i].gameObject.name;
            if (uiDic.ContainsKey(objName))
            {
                uiDic[objName].Add(controls[i]);
            }
            else
            {
                uiDic.Add(objName, new List<UIBehaviour>() { controls[i] });
            }

            //为按钮添加点击事件，为Toggle添加值改变事件 后续还有其他控件的事件可以在这里添加
            if (controls[i] is Button)
            {
                (controls[i] as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                });
            }

            if (controls[i] is Toggle)
            {
                (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnToggleValueChanged(objName, value);
                });
            }
        }

    }

    

    public virtual void ShowMe()
    {
        
    }

    public virtual void HideMe()
    {
        
    }
}
