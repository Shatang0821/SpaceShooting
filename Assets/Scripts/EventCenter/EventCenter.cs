using System;
using System.Collections.Generic;

public static class EventNames
{
    public const string Move = "Move";
    public const string StopMove = "StopMove";

    public const string Fire = "Fire";
    public const string StopFire = "StopFire";
    public const string LaunchMissile = "LaunchMissile";

    public const string Dodge = "Dodge";

    public const string InputOverDriveOn = "InputOverDriveOn";
    public const string PlayerOverDriveOn = "PlayerOverDriveOn";
    public const string OverDriveOff = "OverDriveOff";


}

/// <summary>
/// �C�x���g�������S
/// </summary>
public class EventCenter
{
    private static Dictionary<string,Action> eventNoParamDictionary = new Dictionary<string,Action>();
    
    private static Dictionary<string,Action<object>> eventWithParamDictionary = new Dictionary<string,Action<object>>();

    /// <summary>
    /// �C�x���g���T�u�X�N���C�u
    /// </summary>
    /// <param name="eventName">�C�x���g��</param>
    /// <param name="listener">�C�x���g</param>
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
    /// ���������C�x���g���T�u�X�N���C�u
    /// </summary>
    /// <param name="eventName">�C�x���g��</param>
    /// <param name="listener">�C�x���g</param>
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
    /// �T�u�X�N���C�u������
    /// </summary>
    /// <param name="eventName">�C�x���g��</param>
    /// <param name="listener">�C�x���g</param>
    public static void Unsubscribe(string eventName,Action listener)
    {
        if(eventNoParamDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent -= listener;
            eventNoParamDictionary[eventName] = thisEvent;
        }
    }

    /// <summary>
    /// ���������T�u�X�N���C�u������
    /// </summary>
    /// <param name="eventName">�C�x���g��</param>
    /// <param name="listener">�C�x���g</param>
    public static void Unsubscribe(string eventName,Action<object> listener)
    {
        if(eventWithParamDictionary.TryGetValue(eventName,out var thisEvent))
        {
            thisEvent -= listener;
            eventWithParamDictionary[eventName] = thisEvent;
        }
    }

    /// <summary>
    /// �C�x���g�����s����
    /// </summary>
    /// <param name="eventName">�C�x���g��</param>
    public static void TriggerEvent(string eventName)
    {
        if(eventNoParamDictionary.TryGetValue(eventName,out var thisEvent))
        {
            thisEvent?.Invoke();
        }
    }

    /// <summary>
    /// ���������C�x���g�����s
    /// </summary>
    /// <param name="eventName">�C�x���g��</param>
    /// <param name="eventParam">����</param>
    public static void TriggerEvent(string eventName, object eventParam)
    {
        if (eventWithParamDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent?.Invoke(eventParam);
        }
    }
}
