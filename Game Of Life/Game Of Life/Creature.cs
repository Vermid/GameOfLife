using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Game_Of_Life
{
    class Creature
    {
        public Creature()
        {
            isAlive = true;
        }

        private bool isAlive;

        public bool IsAlive {
            get { return isAlive; }
            set { isAlive = value; }
        }
    }
}
