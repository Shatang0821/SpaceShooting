using System.Collections.Generic;
using UnityEngine;

public class OptionFollower : MonoBehaviour
{
    public Queue<Vector3> positionsHistory = new Queue<Vector3>();
    public int followDelayFrames = 60;  // 跟随延迟（以帧为单位）

    public void UpdatePosition(Vector3 newPosition)
    {
        positionsHistory.Enqueue(newPosition);

        // 维护队列长度，确保其等于延迟帧数
        while (positionsHistory.Count > followDelayFrames)
        {
            positionsHistory.Dequeue();
        }

        // 更新 Option 的位置
        if (positionsHistory.Count == followDelayFrames)
        {
            transform.position = positionsHistory.Peek();
        }
    }
}
