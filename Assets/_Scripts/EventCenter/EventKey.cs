using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EventCenter
{
    public class EventKey
    {
        private readonly string id;
        private readonly int hash;

        /// <summary>
        /// 文字列をハッシュ
        /// </summary>
        /// <param name="id"></param>
        public EventKey(string id)
        {
            this.id = id;
            this.hash = id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is EventKey key && id == key.id;
        }

        /// <summary>
        /// コンストラクターで計算した値を返す
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return hash;
        }

        public static bool operator ==(EventKey key1, EventKey key2)
        {
            return key1.Equals(key2);
        }

        public static bool operator !=(EventKey key1, EventKey key2)
        {
            return !(key1 == key2);
        }
    }
}
