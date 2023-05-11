using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace wolf
{
    internal class Wolf
    {
        public Point3D Position { get; set; }
        private Dispatcher _dispatcher;

        public Wolf(Point3D position, Dispatcher dispatcher)
        {
            Position = position;
            _dispatcher = dispatcher;
        }


        public Rabbit FindNearestRabbit(List<Rabbit> rabbits)
        {
            Rabbit nearestRabbit = null;
            double minDistance = double.MaxValue;

            foreach (var rabbit in rabbits)
            {
                double distance = Point3D.Subtract(rabbit.Position, Position).Length;

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestRabbit = rabbit;
                }
            }

            return nearestRabbit;
        }

        public void RunWolf(List<Rabbit> rabbits)
        {
            double wolfSpeed = 2; // Set the wolf's speed

            while (true)
            {
                Rabbit nearestRabbit = FindNearestRabbit(rabbits);

                if (nearestRabbit != null)
                {
                    Debug.WriteLine("   RABBIT FOUNF " );
                    // Move the wolf towards the nearest rabbit
                    MoveTowards(nearestRabbit.Position, wolfSpeed);
                }

                Thread.Sleep(100); // Wait for 2 seconds before finding the nearest rabbit again
            }
        }
        public void MoveTowards(Point3D target, double speed)
        {
            while (Position != target)
            {
                Vector3D direction = Point3D.Subtract(target, Position);
                direction.Normalize();
                Vector3D movement = Vector3D.Multiply(direction, speed);
                Debug.WriteLine($"  WOLF MOV: {movement.X} {movement.Y} {movement.Z}");
                Position += movement;
            }
        }
    }
}
