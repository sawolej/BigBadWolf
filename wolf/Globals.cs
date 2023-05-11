using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace wolf
{
    internal class Globals
    {
        double maxUP = 20;
        double maxDOWN = 20;
        double maxL = 20;
        double minDOWN = 20;

        public static Dictionary<ModelVisual3D, Rabbit> rabbitModels = new Dictionary<ModelVisual3D, Rabbit>();

        public static Dictionary<string, (double speed, double x, double y, double z, Color color)> rabitsInfo = new Dictionary<string, (double, double, double, double, Color)>
        {
            { "1", (0.387, 0.206, 7.005, 3.28, Colors.Gray) },
            { "2", (0.723, 0.007, 3.395, 4.86, Colors.Orange) },
            { "3", (1, 0.017,0.0,  5.97, Colors.Blue) },
            { "4", (1.524, 0.093,  1.850, 6.39, Colors.Red) },
            { "5", (5.203, 0.048,  1.303, 1.898, Colors.BurlyWood) },
            { "6", (9.537, 0.054, 2.489,  5.683, Colors.Gold) },
            { "7", (19.191, 0.047,  0.772,  8.681, Colors.LightBlue) },
            { "8", (30.069, 0.009, 1.769,  1.024, Colors.BlueViolet) }
        };
    }
}
