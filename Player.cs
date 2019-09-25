using System;

namespace PlayerNS
{
    public class Player
    {
        private int[] _Position = {15, 35};
        private int _Health;

        public Player(int health)
        {
            _Health = health;
        }

        public void DrawHealth()
        {
            Console.WriteLine((char)9786 + " Health: " + _Health.ToString());
        }

        public int[] GetPosition()
        {
            return _Position;
        }

        public void SetPosition(int[] position)
        {
            _Position = position;
        }
    }
}