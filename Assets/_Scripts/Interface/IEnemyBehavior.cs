using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Interface
{ 
    public interface IEnemyBehavior
    {
        void Initialize(GameObject gameObject);
        void Move();
        IEnumerator Attack();
    }
}
