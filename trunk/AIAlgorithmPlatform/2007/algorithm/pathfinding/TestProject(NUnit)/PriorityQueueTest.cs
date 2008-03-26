using System;
using System.Collections.Generic;
using System.Text;
using DataStructure;

/// 
/// Description:对优先队列的功能测试，对所有实现了的函数进行测试，使用了两个测试用例
/// 一个是基本数据类型double,一个是自定义的类People类型
/// @date 2007.05.05
/// @author Zhou
/// 
namespace TestProject
{
    public class PriorityQueueTest
    {
       protected void test()
        {
            TestComparer testComparator = new TestComparer();
            int count = 100;//测试数据的数量
            int range = (int)Math.Sqrt(count);//测试数据的取值范围~range-1
            Console.WriteLine("PriorityQueue");
            PriorityQueue<People> q = new PriorityQueue<People>(99,testComparator);
            PriorityQueue<People> tempq = new PriorityQueue<People>(1, testComparator);
            Console.WriteLine(count.ToString() + "\t" + range.ToString());
            Random r = new Random();
            List<People> all = new List<People>();
            List<People> elementInQueue = new List<People>();
            People p;
            for (int i = 0; i < count; i++)
            {
                p = new People("People" + i.ToString(), r.Next(range));
                all.Add(p);
                q.add(p);
            }
            for (int i = 0; (2 * i + 2) < q.getSize(); i++)
            {
                if (((People)q.getQueue()[i]).getAge() > ((People)q.getQueue()[2 * i + 1]).getAge())
                    Console.WriteLine("Error3");
                if (((People)q.getQueue()[i]).getAge() > ((People)q.getQueue()[2 * i + 2]).getAge())
                    Console.WriteLine("Error3");
            }
            Console.WriteLine("There are {0} elements in q", q.getSize());
                while (q.getSize() > 0)
                {
                    tempq.add(q.removeFirst());
                }
            Console.WriteLine("elementInQueue.Count:" + elementInQueue.Count);

            for (int i = 0; (2 * i + 2) < tempq.getSize(); i++)
            {
                if (((People)tempq.getQueue()[i]).getAge() > ((People)tempq.getQueue()[2 * i + 1]).getAge())
                    Console.WriteLine("Error4");
                if (((People)tempq.getQueue()[i]).getAge() > ((People)tempq.getQueue()[2 * i + 2]).getAge())
                    Console.WriteLine("Error4");
            }
            Console.WriteLine("There are {0} elements in q", q.getSize());
            Console.WriteLine("There are {0} elements in tempq", tempq.getSize());
        } 

        public void testPriorityQueue()
        {
            int count = 10000;//测试数据的数量
            int range = (int)Math.Sqrt(count);//测试数据的取值范围~range-1
            Console.WriteLine(count.ToString() + "\t" + range.ToString());
            Random r = new Random();
            TestComparer testComparator = new TestComparer();   //定义比较器
            List<People> all = new List<People>();
            List<People> elementInQueue = new List<People>();
            PriorityQueue<People> q = new PriorityQueue<People>(testComparator);   //优先队列
            People p;
            for (int i = 0; i < count; i++)
            {
                p = new People("People" + i.ToString(), r.Next(range));
                all.Add(p);
                q.add(p);
            }
     //       q.addAll(all);
            PriorityQueue<People> q2 = new PriorityQueue<People>(q);
            
            Console.WriteLine("There are {0} element in queue", q.getSize());
            Console.WriteLine("There are {0} element in queue2", q2.getSize());
            while (q2.getSize() > 0)
            {
                elementInQueue.Add(q2.removeFirst());
                if (q2.getSize() > 0)
                    q2.remove(all[r.Next(count)]);
            }
            Console.WriteLine("There are {0} element in queue2", q2.getSize());
            Console.WriteLine("elementInQueue.Count:" + elementInQueue.Count);
            for (int i = 1; i < elementInQueue.Count; i++)
            {
                if (elementInQueue[i - 1].getAge() > elementInQueue[i].getAge())
                    Console.WriteLine("Error");
            }
/*
            int count = 10000;//测试数据的数量
            int range = (int)Math.Sqrt(count);//测试数据的取值范围~range-1
            Console.WriteLine(count.ToString() + "\t" + range.ToString());
            Random r = new Random();
            List<int> all = new List<int>();
            List<int> elementInQueue = new List<int>();
            PriorityQueue<int> q = new PriorityQueue<int>();   //优先队列
            int p;

            for (int i = 0; i < count; i++)
            {
                p = r.Next(range);
                all.Add(p);
                q.add(p);
            }
            q.addAll(all);
            Console.WriteLine("There are {0} element in queue", q.getSize());
            while (q.getSize() > 0)
            {
                elementInQueue.Add(q.removeFirst());
                if (q.getSize() > 0)
                    q.remove(all[r.Next(all.Count)]);
            }
            Console.WriteLine("There are {0} element in queue", q.getSize());
            Console.WriteLine("elementInQueue.Count:" + elementInQueue.Count);
            for (int i = 1; i < elementInQueue.Count; i++)
            {
                if (elementInQueue[i - 1] > elementInQueue[i])
                    Console.WriteLine("Error");
            }*/
 
        } 

        public static void Main(string[] args)
        {

      //      new PriorityQueueTest().testPriorityQueue();

//*******************************************   Test case 1   *************************************************
 /*           
           CommonComparer<double> myComparator = new CommonComparer<double>();
        //    PriorityQueue<double> PQ1 = new PriorityQueue<double>();
        //    PriorityQueue<double> PQ1 = new PriorityQueue<double>(6);
        //    PriorityQueue<double> PQ1 = new PriorityQueue<double>(myComparator);
            PriorityQueue<double> PQ1 = new PriorityQueue<double>(6, myComparator);
            
            PQ1.add(2.2);
            PQ1.add(3.4);
            PQ1.add(1.7);
            PQ1.add(1.2);
            PQ1.add(4.5);
            PQ1.add(7.8);
            PQ1.add(3.9);
            for(int i =0; i<100; i++)
                PQ1.add(3.9);


            PriorityQueue<double> PQ2 = new PriorityQueue<double>(6, myComparator);
            PQ2.add(5.7777);
            PQ2.add(8.8888);
            PQ1.addAll(PQ2);

       //     double[] data = { 2.222, 3.3333, 4.44444, 5.55555, 6.666666, 7.77777, 6.66666, 6.66666};
       //     PQ1.addAll(data);


            //删除所有等于3.9的元素
            if (PQ1.remove(3.9))
                Console.WriteLine("Remove success");
            else
                Console.WriteLine("Remove failed,no this element");
            Console.WriteLine("Index :" + PQ1.indexOf(1111.111));

           Console.WriteLine("Fetch the first element : " + PQ1.getFirst() + "\n");
            Console.WriteLine("Fetch and remove the first element " + PQ1.removeFirst() + "\n");
            Console.WriteLine("Fetch and remove the first element " + PQ1.removeFirst() + "\n");
  //          Console.WriteLine("Fetch and remove the first element " + PQ1.remove() + "\n");
  //          Console.WriteLine("Fetch and remove the first element " + PQ1.remove() + "\n");
  //          Console.WriteLine("Fetch and remove the first element " + PQ1.remove() + "\n");
  //          Console.WriteLine("Fetch and remove the first element " + PQ1.remove() + "\n");
  //          Console.WriteLine("Fetch and remove the first element " + PQ1.remove() + "\n");

       //     PQ1.clear();

       //     PriorityQueue<double> PQ2 = new PriorityQueue<double>(PQ1);
       //     PQ2.add(5.7777);
       //     PQ2.add(8.8888);

            PQ1.output();
            
*/
//*********************************************    Test case 2    ***********************************************
 
            TestComparer testComparator = new TestComparer();
            People[] pdata = {new People("小张", 12), new People("小李", 8)};
            People p1 = new People("张三", 12);
            People p2 = new People("李四", 8);
            People p3 = new People("小明", 9);
            People p4 = new People("小东", 3);
            People p5 = new People("李白", 99);
            People p6 = new People("ken", 0);
            People p7 = new People("haha", 11);
            People p8 = new People("haha", 11);
            People p9 = new People("haha", 11);
            People p10 = new People("haha", 11);
            People p11 = new People("haha", 11);
            People p12 = new People("haha", 11);
            People p13 = new People("haha", 11);
            People p14 = new People("haha", 11);
    
      //      PriorityQueue<People> PQ1 = new PriorityQueue<People>();
      //      PriorityQueue<People> PQ1 = new PriorityQueue<People>(11);
      //      PriorityQueue<People> PQ1 = new PriorityQueue<People>(testComparator);
            PriorityQueue<People> PQ1 = new PriorityQueue<People>(3, testComparator);
            PQ1.add(p1);
            PQ1.add(p2);
            PQ1.add(p3);
            PQ1.add(p4);
      //      PQ1.addAll(pdata);
            PQ1.output();


      //      PriorityQueue<People> PQ2 = new PriorityQueue<People>(PQ1);
            PriorityQueue<People> PQ2 = new PriorityQueue<People>(3, testComparator);
            PQ2.add(p5);
            PQ2.add(p6);
            PQ2.add(p7);
            PQ2.add(p8);
            PQ2.add(p9);
            PQ2.add(p8);
            PQ2.add(p8);
            PQ2.add(p8);
            PQ2.addAll(PQ1);
            PQ2.output();
      //      PQ2.clear();

            Console.WriteLine("Fectch the first element : " + PQ2.getFirst() + "\n");
            int s = PQ2.getSize();
   //         for (int i = 0; i < s; i++ )
   //             Console.WriteLine("Fectch and remove the first element : " + PQ2.remove() + "\n");

            if (PQ2.remove(p8))
                Console.WriteLine("Remove success");
            else
                Console.WriteLine("Remove failed, no this element");

            if (PQ2.removeAll(p8))
                Console.WriteLine("Remove success");
            else
                Console.WriteLine("Remove failed, no this element");

            p6.setAge(13);
            PQ2.update(p6);

            PQ2.output();
            
            Console.ReadLine();
            
        }
  
    }
//***************************************     Use for test     *********************************************




    public class CommonComparer<T> : IComparer<T>
    {
        public int Compare(T o1, T o2)
        {
            double x = Convert.ToDouble(o1);
            double y = Convert.ToDouble(o2);
            if (x < y)
                return -1;
            if (x > y)
                return 1;
            return 0;
        }
    }

    public class People
    {
        private int age;
        private string name;

        public People(string name, int age)
        {
            this.age = age;
            this.name = name;
        }

        public int getAge()
        {
            return age;
        }

        public void setAge(int age)
        {
            this.age = age;
        }

        public override string ToString()
        {
            string str = name + " " + age;
            return str;
        }

    }

    public 
        class TestComparer : IComparer<People>
    {
        public int Compare(People o1, People o2)
        {
            if ( o1.getAge() < o2.getAge() )
                return -1;
            if ( o1.getAge() > o2.getAge() )
                return 1;
            return 0;
        }
    }
}
