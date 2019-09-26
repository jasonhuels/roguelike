using System;

namespace PlayerNS
{
    public class Player
    {
        private int[] _Position = {15, 35};
        public int Health { get; set; }

        public Player(int health)
        {
            Health = health;
        }

        public void DrawHealth()
        {
            Console.WriteLine((char)9786 + " Health: " + Health.ToString() + " Position: " + _Position[0] + "," + _Position[1]);
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