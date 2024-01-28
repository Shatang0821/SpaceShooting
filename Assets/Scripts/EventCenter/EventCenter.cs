using Assets.Scripts.EventCenter;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// �C�x���g�������S
/// </summary>
public class EventCenter
{
    private static Dictionary<EventKey,Action> eventNoParamDictionary = new Dictionary<EventKey,Action>();
    
    private static Dictionary<EventKey, Action<object>> eventWithParamDictionary = new Dictionary<EventKey, Action<object>>();

    /// <summary>
    /// �C�x���g���T�u�X�N���C�u
    /// </summary>
    /// <param name="eventKey">�C�x���g�L�[</param>
    /// <param name="listener">�C�x���g</param>
    public static void Subscribe(EventKey eventKey,Action listener)
    {
        if (eventNoParamDictionary.TryGetValue(eventKey, out var thisEvent))
        {
            // ���ɓ������X�i�[���o�^����Ă���ꍇ�͒ǉ����Ȃ�
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
    /// ���������C�x���g���T�u�X�N���C�u
    /// </summary>
    /// <param name="eventKey">�C�x���g�L�[</param>
    /// <param name="listener">�C�x���g</param>
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
    /// �T�u�X�N���C�u������
    /// </summary>
    /// <param name="eventKey">�C�x���g�L�[</param>
    /// <param name="listener">�C�x���g</param>
    public static void Unsubscribe(EventKey eventKey,Action listener)
    {
        if(eventNoParamDictionary.TryGetValue(eventKey, out var thisEvent))
        {
            thisEvent -= listener;
            eventNoParamDictionary[eventKey] = thisEvent;
        }
    }

    /// <summary>
    /// ���������T�u�X�N���C�u������
    /// </summary>
    /// <param name="eventKey">�C�x���g�L�[</param>
    /// <param name="listener">�C�x���g</param>
    public static void Unsubscribe(EventKey eventKey, Action<object> listener)
    {
        if(eventWithParamDictionary.TryGetValue(eventKey, out var thisEvent))
        {
            thisEvent -= listener;
            eventWithParamDictionary[eventKey] = thisEvent;
        }
    }

    /// <summary>
    /// �C�x���g�����s����
    /// </summary>
    /// <param name="eventKey">�C�x���g�L�[</param>
    public static void TriggerEvent(EventKey eventKey)
    {
        if(eventNoParamDictionary.TryGetValue(eventKey, out var thisEvent))
        {
            thisEvent?.Invoke();
        }
    }

    /// <summary>
    /// ���������C�x���g�����s
    /// </summary>
    /// <param name="eventKey">�C�x���g�L�[</param>
    /// <param name="eventParam">����</param>
    public static void TriggerEvent(EventKey eventKey, object eventParam)
    {
        if (eventWithParamDictionary.TryGetValue(eventKey, out var thisEvent))
        {
            thisEvent?.Invoke(eventParam);
        }
    }
}
