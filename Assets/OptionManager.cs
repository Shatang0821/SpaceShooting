using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public GameObject optionPrefab;
    public int maxOptions = 2;
    public List<OptionFollower> options = new List<OptionFollower>();

    [SerializeField] private int SpawnCost = 100;

    private void OnEnable()
    {
        EventCenter.Subscribe(EventNames.AddOption, AddOption);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(EventNames.AddOption, AddOption);
    }

    public void AddOption()
    {
        if (!PlayerEnergy.Instance.IsEnough(SpawnCost) || options.Count >= maxOptions) return;

        PlayerEnergy.Instance.Use(SpawnCost);
        
        GameObject optionInstance = Instantiate(optionPrefab, transform);
        optionInstance.transform.parent = transform.parent;
        
        OptionFollower follower = optionInstance.GetComponent<OptionFollower>();
        Debug.Log(options.Count);
        
        follower.followDelayFrames = ((options.Count+1) * 20);  // 设置延迟帧数
        options.Add(follower);
    }
}
