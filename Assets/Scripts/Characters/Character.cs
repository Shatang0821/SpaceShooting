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
        public float MaxHealth { get; private set; }
        public float Health { get; private set; }
        public bool ShowOnHeadHealthBar {  get; private set; }

        public Character(float maxHealth,bool showOnHeadHealthBar)
        {
            MaxHealth = maxHealth;

            Health = maxHealth;

            ShowOnHeadHealthBar = showOnHeadHealthBar;
        }

        public Character() { }
    }
}
