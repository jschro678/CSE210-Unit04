using System;

namespace Unit04.Game.Casting
{
    public class FallingObject : Actor
    {
        private int points;

        public FallingObject() { }

        public int getPoint()
        {
            return points;
        }

        public void setPoint(int Points)
        {
            points = Points;
        }
    }
}
