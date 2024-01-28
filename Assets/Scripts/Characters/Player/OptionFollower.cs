using System.Collections.Generic;
using UnityEngine;

public class OptionFollower : MonoBehaviour
{
    public Queue<Vector3> positionsHistory = new Queue<Vector3>();
    public int followDelayFrames = 60;  // フレーム間隔

    public void UpdatePosition(Vector3 newPosition)
    {
        positionsHistory.Enqueue(newPosition);

        // キュー
        while (positionsHistory.Count > followDelayFrames)
        {
            positionsHistory.Dequeue();
        }

        // option位置更新
        if (positionsHistory.Count == followDelayFrames)
        {
            transform.position = positionsHistory.Peek();
        }
    }
}
