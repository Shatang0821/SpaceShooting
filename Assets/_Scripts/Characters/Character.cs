using Assets.Scripts.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Characters
{
    public class Character
    {
        //プロパティ
        public float MaxHealth { get; protected set; }
        public float Health { get; protected set; }
        public bool ShowOnHeadHealthBar {  get; protected set; }

        public bool IsActive { get; protected set; }

        public Character(bool isActive) 
        {
            this.IsActive = isActive;
        }

        public virtual void Die()
        {
            IsActive = false;
        }
    }
}
