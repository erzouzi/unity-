using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : BaseManager<MusicMgr>
{
    // 背景音乐播放器
    private AudioSource bkMusic;
    // 背景音乐音量
    private float bkValue = 1;
    // 缓存当前正在播放的背景音乐名字，用于判断
    private string currentBkName;

    // 音效播放器列表
    private GameObject soundObj=null;
    private List<AudioSource> soundList = new List<AudioSource>();
    // 音效音量
    private float soundValue = 1;

    public MusicMgr()
    {
        MonoMgr.Instance().AddUpdateListener(Update);
    }

    public  void Update()
    {
        for(int i =soundList.Count - 1; i >= 0; i--)
        {
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);

            }
        }
    }

    public void PlayBkMusic(string name)
    {

        if(bkMusic!=null && currentBkName == name){
           bkMusic.Play();
            return;
        }

        if (bkMusic == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BkMusic";
            bkMusic = obj.AddComponent<AudioSource>();

        }
        currentBkName = name;
        //异步加载背景音乐资源
        ResourcesMgr.Instance().LoadAsync<AudioClip>("Music/BkMusic"+name, (clip) =>
        {
            bkMusic.clip = clip;
            bkMusic.loop = true;
            bkMusic.volume = bkValue;
            bkMusic.Play();
        });
        
    }

    public void ChangeBkValue(float value)
    {
        bkValue = value;
        if (bkMusic != null)
        {
            bkMusic.volume = bkValue;
        }
    }

    // 暂停背景音乐
    public void PauseBkMusic()
    {
        if (bkMusic != null)
        {
            bkMusic.Pause();
        }
    }

    // 停止背景音乐
    public void StopBkMusic()
    {
        if (bkMusic != null)
        {
            bkMusic.Stop();
        }
    }

    // 播放音效
    public void PlaySound(string name,bool isLoop = false, UnityAction<AudioSource> callBack = null)
    {
        if (soundObj == null)
        {
            soundObj = new GameObject();
            soundObj.name = "Sound";
        }

        //异步加载音效音乐资源,资源加载结束后再添加AudioSource组件并播放，避免了资源未加载完成就播放导致的错误
        ResourcesMgr.Instance().LoadAsync<AudioClip>("Music/Sound" + name, (clip) =>
        {
            AudioSource audioSource = soundObj.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.loop = isLoop;
            audioSource.volume = bkValue;
            audioSource.Play();
            soundList.Add(audioSource);
            if(callBack != null)
            callBack(audioSource);
        });
    }

    // 改变音效大小
    public void ChangeSoundValue(float value)
    {
        soundValue = value;
        foreach (var sound in soundList)
        {
            sound.volume = soundValue;
        }
    }

    // 暂停音效
    public void StopSound(AudioSource audioSource)
    {
        if (soundList.Contains(audioSource))
        {
            audioSource.Stop();
            soundList.Remove(audioSource);
            GameObject.Destroy(audioSource);
        }
    }

}
