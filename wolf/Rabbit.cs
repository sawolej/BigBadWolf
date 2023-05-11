using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace wolf
{
    internal class Rabbit
    {

        public string Name { get; set; }
        public double Speed { get; set; }
        public System.Windows.Media.Color Color { get; set; }
        public System.Windows.Media.Media3D.Point3D Position { get; set; }
        public System.Windows.Media.Media3D.Point3D PrevPosition { get; set; }

        public static List<Rabbit> InitializeRabits()
        {
            //double theta = 0;
            List<Rabbit> rabits = new List<Rabbit>();

            foreach (var entry in Globals.rabitsInfo)
            {
                string rabbitName = entry.Key;
                var (speed, posX, posY, posZ, color) = entry.Value;

                rabits.Add(new Rabbit
                {
                    Name = rabbitName,
                    Speed = speed,
                    Color = color,
                    Position = new Point3D(posX, posY, posZ)
                });
            }
            return rabits;
        }

        public void Running(Rabbit rabbit)
        {
            double velocityX = GetSecureDouble();
            double velocityY = GetSecureDouble();
            double velocityZ = GetSecureDouble();

            rabbit.PrevPosition = rabbit.Position;
            rabbit.Position = new Point3D(
               rabbit.Position.X + velocityX,
               rabbit.Position.Y + velocityY,
               rabbit.Position.Z + velocityZ 



           );

            Debug.WriteLine($"change the pos of {rabbit.Name} with {velocityX} {velocityY} {velocityZ}.");
            Random r = new Random();
            int rInt = r.Next(100, 1000);
            Thread.Sleep(rInt);
        }

        public static double GetSecureDoubleWithinRange(double lowerBound, double upperBound)
        {
            var rDouble = GetSecureDouble();
            var rRangeDouble = (double)rDouble * (upperBound - lowerBound) + lowerBound;
            return rRangeDouble;
        }
        public static double GetSecureDouble()
        {
            Random random = new Random();

            double minValue = -1;
            double maxValue = 1;

            double randomDouble = minValue + random.NextDouble() * (maxValue - minValue);

            return randomDouble;
        }
    }
}
