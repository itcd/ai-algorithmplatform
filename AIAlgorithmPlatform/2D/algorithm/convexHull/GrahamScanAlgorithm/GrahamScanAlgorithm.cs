using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using ConvexHullEngine;
using Position_Implement;

namespace GrahamScanAlgorithm
{
   public class GrahamScan : IConvexHullEngine
    {
       /*
       public SimplePositionSet ConvexHull(SimplePositionSet ps)
       {
           int n = ps.GetCount();
           Point[] pts = new Point[n];
           for (int i = 0; i < n; i++)
           {
               SimplePosition pos = ps.GetNextPosition();
               pts[i] = new Point(pos.GetX(), pos.GetY());
           }
           Point[] resPts = _ConvexHull(pts);
           SimplePositionSet result = new SimplePositionSet();
           for (int i = 0; i < resPts.Length; i++)
               result.AddPosition(new SimplePosition(resPts[i].X, resPts[i].Y));
           return result;
       }
       */
       public IPositionSet ConvexHull(IPositionSet ps)
        {

            //转换成数组
            IPosition[] posArr = (IPosition[])ps.ToArray();
            int N = posArr.Length;
          
            //排序
            Polysort.Quicksort(posArr);
            IPosition left = posArr[0];
            IPosition right = posArr[N - 1];
            // Partition into lower hull and upper hull
            CDLL lower = new CDLL(left), upper = new CDLL(left);
            for (int i = 0; i < N; i++)
            {
                double det = Area2(left, right, posArr[i]);
                if (det > 0)
                    upper = upper.Append(new CDLL(posArr[i]));
                else if (det < 0)
                    lower = lower.Prepend(new CDLL(posArr[i]));
            }
            lower = lower.Prepend(new CDLL(right));
            upper = upper.Append(new CDLL(right)).Next;
            // Eliminate points not on the hull
            eliminate(lower);
            eliminate(upper);
            // Eliminate duplicate endpoints 消除重复点
           /*
            if (lower.Prev.val.Equals(upper.val))
                lower.Prev.Delete();
            if (upper.Prev.val.Equals(lower.val))
                upper.Prev.Delete();
            * */
            // Join the lower and upper hull
            IPosition[] res = new IPosition[lower.Size() + upper.Size()];
            lower.CopyInto(res, 0);
            upper.CopyInto(res, lower.Size());
            PositionSetEdit_ImplementByICollectionTemplate result = new PositionSetEdit_ImplementByICollectionTemplate();
            for (int i = 0; i < res.Length - 1; i++)
                result.AddPosition(res[i]);
            return result;

        }

       private double Area2(IPosition p0, IPosition p1, IPosition p2)
       {
           return p0.GetX() * (p1.GetY() - p2.GetY()) + p1.GetX() * (p2.GetY() - p0.GetY()) + p2.GetX() * (p0.GetY() - p1.GetY());
       }

        // Graham's scan
        private void eliminate(CDLL start)
        {
            CDLL v = start, w = start.Prev;
            bool fwd = false;
            while (v.Next != start || !fwd)
            {
                if (v.Next == w)
                    fwd = true;
                if (Area2(v.val, v.Next.val, v.Next.Next.val) < 0) // right turn
                    v = v.Next;
                else
                {                                       // left turn or straight
                    v.Next.Delete();
                    v = v.Prev;
                }
            }
        }

    }

    class CDLL
    {
        private CDLL prev, next;     // not null, except in deleted elements
        public IPosition val;

        // A new CDLL node is a one-element circular list
        public CDLL(IPosition val)
        {
            this.val = val; next = prev = this;
        }

        public CDLL Prev
        {
            get { return prev; }
        }

        public CDLL Next
        {
            get { return next; }
        }

        // Delete: adjust the remaining elements, make this one Position nowhere
        public void Delete()
        {
            next.prev = prev; prev.next = next;
            next = prev = null;
        }

        public CDLL Prepend(CDLL elt)
        {
            elt.next = this; elt.prev = prev; prev.next = elt; prev = elt;
            return elt;
        }

        public CDLL Append(CDLL elt)
        {
            elt.prev = this; elt.next = next; next.prev = elt; next = elt;
            return elt;
        }

        public int Size()
        {
            int count = 0;
            CDLL node = this;
            do
            {
                count++;
                node = node.next;
            } while (node != this);
            return count;
        }

        public void PrintFwd()
        {
            CDLL node = this;
            do
            {
                Console.WriteLine(node.val);
                node = node.next;
            } while (node != this);
            Console.WriteLine();
        }

        public void CopyInto(IPosition[] vals, int i)
        {
            CDLL node = this;
            do
            {
                vals[i++] = node.val;	// still, implicit checkcasts at runtime 
                node = node.next;
            } while (node != this);
        }
    }

    // ------------------------------------------------------------

    class Polysort
    {
        private static void swap(IPosition[] arr, int s, int t)
        {
            IPosition tmp = arr[s]; arr[s] = arr[t]; arr[t] = tmp;
        }
        /*
        private static void swap(Ordered[] arr, int s, int t)
        {
            Ordered tmp = arr[s]; arr[s] = arr[t]; arr[t] = tmp;
        }
        */

        // Typed OO-style quicksort a la Hoare/Wirth

        private static void qsort(IPosition[] arr, int a, int b)
        {
            // sort arr[a..b]
            if (a < b)
            {
                int i = a, j = b;
                IPosition x = arr[(i + j) / 2];
                do
                {
                    while (arr[i].GetX() < x.GetX()) i++;
                    while (x.GetX() < arr[j].GetX()) j--;
                    if (i <= j)
                    {
                        swap(arr, i, j);
                        i++; j--;
                    }
                } while (i <= j);
                qsort(arr, a, j);
                qsort(arr, i, b);
            }
        }

        public static void Quicksort(IPosition[] arr)
        {
            qsort(arr, 0, arr.Length - 1);
        }
    }

    /*
    public abstract class Ordered
    {
        public abstract bool Less(Ordered that);
    }
    */

}
