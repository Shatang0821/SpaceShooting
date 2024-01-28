using Assets.Scripts.EventCenter;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// イベント処理中心
/// </summary>
public class EventCenter
{
    private static Dictionary<EventKey,Action> eventNoParamDictionary = new Dictionary<EventKey,Action>();
    
    private static Dictionary<EventKey, Action<object>> eventWithParamDictionary = new Dictionary<EventKey, Action<object>>();

    /// <summary>
    /// イベントをサブスクライブ
    /// </summary>
    /// <param name="eventKey">イベントキー</param>
    /// <param name="listener">イベント</param>
    public static void Subscribe(EventKey eventKey,Action listener)
    {
        if (eventNoParamDictionary.TryGetValue(eventKey, out var thisEvent))
        {
            // 既に同じリスナーが登録されている場合は追加しない
            if (thisEvent != null && !thisEvent.GetInvocationList().Contains(listener))
            {
                eventNoParamDictionary[eventKey] += listener;
            }
        }
        else
        {
            eventNoParamDictionary.Add(eventKey, listener);
        }
    }

    /// <summary>
    /// 引数持ちイベントをサブスクライブ
    /// </summary>
    /// <param name="eventKey">イベントキー</param>
    /// <param name="listener">イベント</param>
    public static void Subscribe(EventKey eventKey, Action<object> listener)
    {
        if(eventWithParamDictionary.TryGetValue(eventKey,out var thisEvent))
        {
            thisEvent += listener;
            eventWithParamDictionary[eventKey] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            eventWithParamDictionary.Add(eventKey, thisEvent);
        }
    }

    /// <summary>
    /// サブスクライブを解除
    /// </summary>
    /// <param name="eventKey">イベントキー</param>
    /// <param name="listener">イベント</param>
    public static void Unsubscribe(EventKey eventKey,Action listener)
    {
        if(eventNoParamDictionary.TryGetValue(eventKey, out var thisEvent))
        {
            thisEvent -= listener;
            eventNoParamDictionary[eventKey] = thisEvent;
        }
    }

    /// <summary>
    /// 引数持ちサブスクライブを解除
    /// </summary>
    /// <param name="eventKey">イベントキー</param>
    /// <param name="listener">イベント</param>
    public static void Unsubscribe(EventKey eventKey, Action<object> listener)
    {
        if(eventWithParamDictionary.TryGetValue(eventKey, out var thisEvent))
        {
            thisEvent -= listener;
            eventWithParamDictionary[eventKey] = thisEvent;
        }
    }

    /// <summary>
    /// イベントを実行する
    /// </summary>
    /// <param name="eventKey">イベントキー</param>
    public static void TriggerEvent(EventKey eventKey)
    {
        if(eventNoParamDictionary.TryGetValue(eventKey, out var thisEvent))
        {
            thisEvent?.Invoke();
        }
    }

    /// <summary>
    /// 引数持ちイベントを実行
    /// </summary>
    /// <param name="eventKey">イベントキー</param>
    /// <param name="eventParam">引数</param>
    public static void TriggerEvent(EventKey eventKey, object eventParam)
    {
        if (eventWithParamDictionary.TryGetValue(eventKey, out var thisEvent))
        {
            thisEvent?.Invoke(eventParam);
        }
    }
}
