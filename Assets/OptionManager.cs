using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public GameObject optionPrefab;
    public int maxOptions = 2;
    public List<OptionFollower> options = new List<OptionFollower>();

    public void AddOption()
    {
        if(options.Count < maxOptions)
        {
            GameObject optionInstance = Instantiate(optionPrefab, transform);
            OptionFollower follower = optionInstance.GetComponent<OptionFollower>();
            follower.followDelayFrames = (options.Count * 60);  // 设置延迟帧数
            options.Add(follower);
        }
    }
}
