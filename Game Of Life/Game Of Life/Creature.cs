
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
