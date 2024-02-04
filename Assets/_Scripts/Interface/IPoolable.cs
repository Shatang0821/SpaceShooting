using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Interface
{
    public interface IPoolable
    {
        void OnSpawn(); // インスタンスがプールから取り出された際に呼ばれる
        void OnDespawn(); // インスタンスがプールに戻される際に呼ばれる
    }
}
