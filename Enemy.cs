using System.Threading;

namespace EnemyNS
{
    public class Enemy
    {
        private int _Health;
        private bool _Active = false;
        private int[] _Position = {0, 0};
        public Enemy(int health, int[] position) 
        {
            _Health = health;
        }

        private void Pursue(int[] playerPosition)
        {
            int horz = playerPosition[1] - _Position[1];
            int vert = playerPosition[0] - _Position[0];
            if(horz == 0) {
                // move vertical
            } 
            else if(vert == 0) 
            {
                // move horizontal
            } 
            else
            {
                // move random
            }
        }
    }
}