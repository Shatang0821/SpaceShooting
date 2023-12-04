using System;
using System.Collections.Generic;

/// <summary>
/// イベント処理中心
/// </summary>
public class EventCenter
{
    private static Dictionary<string,Action> eventNoParamDictionary = new Dictionary<string,Action>();
    
    private static Dictionary<string,Action<object>> eventWithParamDictionary = new Dictionary<string,Action<object>>();

    /// <summary>
    /// イベントをサブスクライブ
    /// </summary>
    /// <param name="eventName">イベント名</param>
    /// <param name="listener">イベント</param>
    public static void Subscribe(string eventName,Action listener)
    {
        if(eventNoParamDictionary.TryGetValue(eventName,out var thisEvent))
        {
            thisEvent += listener;
            eventNoParamDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            eventNoParamDictionary.Add(eventName, thisEvent);
        }
    }

    /// <summary>
    /// 引数持ちイベントをサブスクライブ
    /// </summary>
    /// <param name="eventName">イベント名</param>
    /// <param name="listener">イベント</param>
    public static void Subscribe(string eventName, Action<object> listener)
    {
        if(eventWithParamDictionary.TryGetValue(eventName,out var thisEvent))
        {
            thisEvent += listener;
            eventWithParamDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            eventWithParamDictionary.Add(eventName, thisEvent);
        }
    }

    /// <summary>
    /// サブスクライブを解除
    /// </summary>
    /// <param name="eventName">イベント名</param>
    /// <param name="listener">イベント</param>
    public static void Unsubscribe(string eventName,Action listener)
    {
        if(eventNoParamDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent -= listener;
            eventNoParamDictionary[eventName] = thisEvent;
        }
    }

    /// <summary>
    /// 引数持ちサブスクライブを解除
    /// </summary>
    /// <param name="eventName">イベント名</param>
    /// <param name="listener">イベント</param>
    public static void Unsubscribe(string eventName,Action<object> listener)
    {
        if(eventWithParamDictionary.TryGetValue(eventName,out var thisEvent))
        {
            thisEvent -= listener;
            eventWithParamDictionary[eventName] = thisEvent;
        }
    }

    /// <summary>
    /// イベントを実行する
    /// </summary>
    /// <param name="eventName">イベント名</param>
    public static void TriggerEvent(string eventName)
    {
        if(eventNoParamDictionary.TryGetValue(eventName,out var thisEvent))
        {
            thisEvent?.Invoke();
        }
    }

    /// <summary>
    /// 引数持ちイベントを実行
    /// </summary>
    /// <param name="eventName">イベント名</param>
    /// <param name="eventParam">引数</param>
    public static void TriggerEvent(string eventName, object eventParam)
    {
        if (eventWithParamDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent?.Invoke(eventParam);
        }
    }
}
