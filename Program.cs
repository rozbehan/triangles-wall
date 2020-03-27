using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
1. I have tried on a binary tree instead of triangle-mesh, based on duality. 
2. Every segment is a node in binary tree and two new vertices of the triangle are the children of the node. 
3. The binary tree has a root, the base segment with two vertices of (0,0), (length,0). 
4. A recursive function has tried to make all the possible trees. 
5. The next triangle will be made only through one of the last two segments. The binary tree will only grow through one of the last two nodes. 
6. After selecting two new segments among the rest segments, we have 8 options to make a new triangle with them. 
(2 base segments) * (swapping 2 new segments) * (2 mirroring). The same is for the binary tree. 
7. We skip the non-triangle set. 
8. We calculate the coordinate of the new vertex by having the three(or two) edges and two vertices. 
9. We keep the maximum of the y-dimension among the coordinates of the new vertices. 
10. We skip the triangles with negative y-dimension. 
*/

namespace ConsoleApp8
{
    public struct Vertex
    {
        public double x;
        public double y;
    }
    class Program
    {
        static List<int> seg = new List<int>();
        static Dictionary<int, int> seg2 = new Dictionary<int, int>();
        static double max = 0;
        static long n = 0;
        static Stack<int> v = new Stack<int>();
        static void Main(string[] args)
        {
            List<string> l = new List<string>(Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            if (l.Count < 4) return;

            for (int i = 0; i < int.Parse(l[0]); i++)
            {
                seg.Add(int.Parse(l[i + 1]));
                seg2.Add(seg[i], (int)Math.Pow(seg[i], 2));
            }

            for (int k = 0; k < seg.Count; k++)
            {
                List<int> s1 = new List<int>(seg);
                s1.RemoveAt(k);
                MakeTree(s1, new Vertex { x = 0, y = 0 }, new Vertex { x = seg[k], y = 0 }, seg[k]);
            }
            Console.WriteLine(max);
            //Console.ReadLine();
        }
        // 9 42 40 32 30 25 18 15 19 21
        static void MakeTree(List<int> s, Vertex A1, Vertex A2, int Al)
        {
            for (int i = 0; i < s.Count; i++)
            {
                for (int j = i + 1; j < s.Count; j++)
                {
                    if (s[i] > s[j] ? (Math.Abs(Al - s[i]) >= s[j]) : (Math.Abs(Al - s[j]) >= s[i])) continue;

                    Vertex[] third1 = new Vertex[2];
                    Vertex[] third2 = new Vertex[2];
                    third1 = ThirdVertex(A1, A2, Al, s[i], s[j]);
                    third2 = ThirdVertex(A1, A2, Al, s[j], s[i]);
                    for (int k = 0; k < 2; k++)
                    {
                        max = max < third1[k].y ? third1[k].y : max;
                        max = max < third2[k].y ? third2[k].y : max;
                    }
                    
                    if (s.Count < 4) continue;

                    List<int> s1 = new List<int>(s);
                    s1.RemoveAt(j);
                    s1.RemoveAt(i);

                    if (third1[0].y > 0)
                    {
                        n++;
                        MakeTree(s1, A1, third1[0], s[i]);
                        MakeTree(s1, A2, third1[0], s[j]);
                    }
                    if (third1[1].y > 0)
                    {
                        n++;
                        MakeTree(s1, A1, third1[1], s[i]);
                        MakeTree(s1, A2, third1[1], s[j]);
                    }
                    if (third2[0].y > 0)
                    {
                        n++;
                        MakeTree(s1, A1, third2[0], s[j]);
                        MakeTree(s1, A2, third2[0], s[i]);
                    }
                    if (third2[1].y > 0)
                    {
                        n++;
                        MakeTree(s1, A1, third2[1], s[j]);
                        MakeTree(s1, A2, third2[1], s[i]);
                    }
                }
            }
        }
        static Vertex[] ThirdVertex(Vertex A1, Vertex A2, int Al, int Bl, int Cl)
        {
            double x1 = (Math.Pow(Al, 2) + Math.Pow(Bl, 2) - Math.Pow(Cl, 2)) / 2 / Al;
            double y1 = Math.Sqrt((Al + Bl + Cl) * (Al + Bl - Cl) * (Al - Bl + Cl) * (-Al + Bl + Cl))/2/Al;
            double teta = Math.Atan2(A2.y - A1.y, A2.x - A1.x);
            double s = Math.Sin(teta);
            double c = Math.Cos(teta);
            Vertex v1 = new Vertex { x = A1.x + x1 * c - y1 * s, y = A1.y + x1 * s + y1 * c };
            Vertex v2 = new Vertex { x = A1.x + x1 * c + y1 * s, y = A1.y + x1 * s - y1 * c };

            //double res1 = Math.Pow(A1.x, 2) - Math.Pow(A2.x, 2) + Math.Pow(A1.y, 2) - Math.Pow(A2.y, 2) - Math.Pow(Bl, 2) + Math.Pow(Cl, 2)
            //+ 2 * (v1.x * (A1.x - A2.x) + v1.y * (A1.y - A2.y));
            //if (res1 != 0) v1.y = -1;
            //double res2 = Math.Pow(A1.x, 2) - Math.Pow(A2.x, 2) + Math.Pow(A1.y, 2) - Math.Pow(A2.y, 2) - Math.Pow(Bl, 2) + Math.Pow(Cl, 2)
            //+ 2 * (v2.x * (A1.x - A2.x) + v2.y * (A1.y - A2.y));
            //if (res2 != 0) v2.y = -1;

            return new Vertex[] { v1, v2 };
        }
        static Vertex[] ThirdVertexOld(Vertex A1, Vertex A2, int Al, int Bl, int Cl)
        {
            double phi1 = Math.Atan2(A1.y - A2.y, A1.x - A2.x);
            double phi2 = Math.Acos((seg2[Bl] + seg2[Al] - seg2[Cl]) / (2 * Bl * Al));
            Vertex v1 = new Vertex { x = A2.x + Bl * Math.Cos(phi1 + phi2), y = A2.y + Bl * Math.Sin(phi1 + phi2) };
            Vertex v2 = new Vertex { x = A2.x + Bl * Math.Cos(phi1 - phi2), y = A2.y + Bl * Math.Sin(phi1 - phi2) };
            return new Vertex[] { v1, v2 };
            //double phi2 = Math.Acos((seg2[Bl] + seg2[Al] - seg2[Cl]) / (2 * Bl * Al));
            //return new Vertex { x = A1.x + Bl * Math.Cos(phi1 + phi2), y = A1.y + Bl * Math.Sin(phi1 + phi2) };
        }
    }
}
