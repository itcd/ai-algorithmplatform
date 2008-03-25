using System;
using System.Collections.Generic;
using System.Text;

namespace BFS_AStarforGrid
{
    //AStar和BFS用到的节点
    public class Node
    {
        public int x;
        public int y;
        public int f;
        public int g;
        //public int h;
        public bool closed;
        public Node parent;
    }
    //----------------------------------------------------------------

    //----------------------------------------------------------------
    //搜索算法的接口
    public interface SearchAlgorithm
    {
        List<MyPoint> getPath();
        bool search(Map map);
    }
    //----------------------------------------------------------------

    //----------------------------------------------------------------
    public interface IQueue<T>
    {
        void enqueue(T item);
        T dequeue();
        int getCount();
        void clear();
        int getSize();
    }
    //----------------------------------------------------------------

    //----------------------------------------------------------------
    public class ArrayQueue<T> : IQueue<T>
    {
        T[] queue;
        int count = 0;
        int head = 0;
        int tail = -1;
        int size;

        public ArrayQueue(int size)
        {
            queue = new T[size];
            this.size = size;
        }

        public void enqueue(T item)
        {
            if (count < size)
            {
                tail = (tail + 1) % size;
                queue[tail] = item;
                count++;
            }
            else
            {
                size *= 2;
                T[] temp = new T[size];
                for (int i = 0; i < queue.GetLength(0); i++)
                {
                    temp[i] = queue[i];
                }
                queue = temp;
                enqueue(item);
            }
        }

        public T dequeue()
        {
            if (count > 0)
            {
                int i = head;
                head = (head + 1) % size;
                count--;
                return queue[i];
            }
            else
                return default(T);

        }

        public int getCount()
        {
            return count;
        }

        public void clear()
        {
            count = 0;
            tail = -1;
            head = 0;
        }

        public int getSize()
        {
            return size;
        }

        public void show()
        {
            int index = head;
            for (int i = 0; i < count; i++)
            {
                Console.Write(queue[index]);
                Console.Write("\t");
                index++;
            }
            Console.WriteLine();
        }
    }
    //----------------------------------------------------------------

    //----------------------------------------------------------------
    //广度优先搜索
    public class BFS : SearchAlgorithm
    {
        const int D = 4;//direction amount
        protected int[] dx = { 1, -1, 0, 0 };
        protected int[] dy = { 0, 0, 1, -1 };
        Node currentNode = null;
        IQueue<Node> queue = null;
        Node[,] nodeMap = null;
        int elementCount;
        int height;
        int width;

        public void init(Map map)
        {
            if (nodeMap != null)
            {
                clear();
                assignMap(map);
            }
            else
            {
                assignSpace(map);
                assignMap(map);
            }
        }

        //地图大小改变之后要重新为算法内部的地图申请空间
        public void assignSpace(Map map)
        {
            elementCount = map.getElementCount();
            height = map.getHeight();
            width = map.getWidth();
            nodeMap = new Node[height, width];
            queue = new ArrayQueue<Node>(elementCount);
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    nodeMap[i, j] = new Node();
                }
        }

        public void assignMap(Map map)
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    if (map.isEmpty(j, i))
                        nodeMap[i, j].g = 0;
                    else
                        nodeMap[i, j].g = -1;
                }
        }

        public void clear()
        {
            queue.clear();
        }

        public List<MyPoint> getPath()
        {
            List<MyPoint> pathList = new List<MyPoint>();
            Node tempNode;

            if (currentNode != null)
            {
                pathList.Insert(0, new MyPoint(currentNode.x, currentNode.y));
                tempNode = currentNode.parent;
                while (tempNode != null)
                {
                    pathList.Insert(0, new MyPoint(tempNode.x, tempNode.y));
                    tempNode = tempNode.parent;
                }
            }
            return pathList;
        }

        public bool search(Map map)
        {
            //init(map);

            Node nextNode;
            int x, y;
            int startX = map.getStart().x;
            int startY = map.getStart().y;
            int endX = map.getEnd().x;
            int endY = map.getEnd().y;

            currentNode = nodeMap[startY, startX];
            //currentNode = new Node();
            currentNode.x = startX;
            currentNode.y = startY;
            currentNode.g = 1;
            currentNode.parent = null;
            //map.setValue(currentNode.x, currentNode.y, currentNode.g);

            while (currentNode.x != endX || currentNode.y != endY)
            {
                for (int k = 0; k < D; k++)
                {
                    x = currentNode.x + dx[k];
                    y = currentNode.y + dy[k];
                    if (x >= 0 && x < width && y >= 0 && y < height)
                    {
                        if (nodeMap[y, x].g == 0)
                        {
                            nextNode = nodeMap[y, x];
                            //nextNode = new Node();
                            nextNode.x = x;
                            nextNode.y = y;
                            nextNode.g = currentNode.g + 1;
                            nextNode.parent = currentNode;
                            //map.setValue(nextNode.x, nextNode.y, nextNode.g);
                            queue.enqueue(nextNode);
                        }
                    }
                }
                if (queue.getCount() > 0)
                    currentNode = queue.dequeue();
                else
                    return false;
            }
            return true;
        }

        public MapMark getMapMark()
        {
            if (nodeMap == null)
                return null;

            MapMark mark = new MapMark(width, height);
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    mark.setValue(j, i, nodeMap[i, j].g);
                }
            return mark;
        }
    }
    //----------------------------------------------------------------

    //----------------------------------------------------------------
    //比较两个元素的delegate
    public delegate int Compare<T>(T obj1, T obj2);
    //----------------------------------------------------------------

    //----------------------------------------------------------------
    //AStar的open表用到的数据结构
    public interface LinkedList<T>
    {
        void sortedInsert(T item, Compare<T> c);
        void removeFirst();
        T getFirst();
        int getCount();
        void clear();
        T find(Predicate<T> match);
        void addFirst(T item);
    }
    //----------------------------------------------------------------

    //----------------------------------------------------------------
    //链表实现的LinkedList
    public class SortedLinkedList<T> : LinkedList<T>
    {
        class LinkedNode
        {
            public T value;
            public LinkedNode next;
        }

        LinkedNode head = null;
        int count = 0;

        public SortedLinkedList()
        {
            head = new LinkedNode();
            head.value = default(T);
            head.next = null;
        }

        public void sortedInsert(T item, Compare<T> c)
        {
            LinkedNode p = head;
            LinkedNode temp = new LinkedNode();
            temp.value = item;
            temp.next = null;
            count++;

            while (p.next != null)
            {
                if (c(item, p.next.value) > 0)
                {
                    p = p.next;
                }
                else
                {
                    temp.next = p.next;
                    p.next = temp;
                    return;
                }
            }
            p.next = temp;
        }

        public void addFirst(T item)
        {
            LinkedNode temp = new LinkedNode();
            temp.value = item;
            temp.next = head.next;
            count++;
            head.next = temp;
        }

        public void removeFirst()
        {
            if (head.next != null)
            {
                head.next = head.next.next;
                count--;
            }
        }

        public T getFirst()
        {
            return head.next.value;
        }

        public int getCount()
        {
            return count;
        }

        public void clear()
        {
            head.next = null;
            count = 0;
        }

        public T find(Predicate<T> match)
        {
            LinkedNode p = head;
            while (p.next != null)
            {
                if (match(p.next.value))
                    return p.next.value;
                p = p.next;
            }
            return default(T);
        }

        public void show()
        {
            LinkedNode p = head;
            while (p.next != null)
            {
                Console.Write(p.next.value);
                Console.Write("\t");
                p = p.next;
            }
            Console.WriteLine();
        }
    }
    //----------------------------------------------------------------

    //----------------------------------------------------------------
    //hash表
    public interface HashMap<T>
    {
        void clear();
        void resize(int mapWidth, int mapHeight);
        void put(int x, int y, T item);
        T get(int x, int y);
        void remove(int x, int y);
        T find(int x, int y, Predicate<T> match);
        int getCount(int x, int y);
    }
    //----------------------------------------------------------------

    //----------------------------------------------------------------
    //用LinkedList<T>[,]实现的hash表
    public class LinkedHashMap<T> : HashMap<T>
    {
        LinkedList<T>[,] map;
        int width;
        int height;

        public LinkedHashMap(int mapWidth, int mapHeight)
        {
            init(mapWidth, mapHeight);
        }

        protected void init(int mapWidth, int mapHeight)
        {
            width = mapWidth;
            height = mapHeight;
            map = new SortedLinkedList<T>[mapHeight, mapWidth];
            int i, j;
            for (i = 0; i < height; i++)
                for (j = 0; j < width; j++)
                    map[i, j] = new SortedLinkedList<T>();
        }

        public void clear()
        {
            int i, j;
            for (i = 0; i < height; i++)
                for (j = 0; j < width; j++)
                    map[i, j].clear();
        }

        public void resize(int mapWidth, int mapHeight)
        {
            if (width < mapWidth || height < mapHeight)
            {
                init(mapWidth, mapHeight);
            }
            else
            {
                clear();
            }
        }

        public void put(int x, int y, T item)
        {
            map[y, x].addFirst(item);
        }

        public T get(int x, int y)
        {
            if (map[y, x].getCount() > 0)
                return map[y, x].getFirst();
            else
                return default(T);
        }

        public int getCount(int x, int y)
        {
            return map[y, x].getCount();
        }

        public void remove(int x, int y)
        {
            if (map[y, x].getCount() > 0)
                map[y, x].removeFirst();
        }

        public T find(int x, int y, Predicate<T> match)
        {
            if (map[y, x].getCount() > 0)
            {
                return map[y, x].find(match);
            }
            else
            {
                return default(T);
            }
        }
    }
    //----------------------------------------------------------------

    //----------------------------------------------------------------
    //AStar搜索
    public class AStar : SearchAlgorithm
    {
        const int D = 4;//direction amount
        protected int[] dx = { 0, 0, -1, 1 };
        protected int[] dy = { -1, 1, 0, 0 };
        Node currentNode = null;
        //HashMap<Node> closed = null;
        LinkedList<Node> open = null;
        int x, y;
        Node[,] nodeMap = null;
        int elementCount;
        int height;
        int width;

        public void init(Map map)
        {
            if (nodeMap != null)
            {
                clear();
                assignMap(map);
            }
            else
            {
                assignSpace(map);
                assignMap(map);
            }
        }

        //地图大小改变之后要重新为算法内部的地图申请空间
        public void assignSpace(Map map)
        {
            elementCount = map.getElementCount();
            height = map.getHeight();
            width = map.getWidth();
            nodeMap = new Node[height, width];
            open = new SortedLinkedList<Node>();
            //if (closed != null)
            //    closed.resize(width, height);
            //else
            //closed = new LinkedHashMap<Node>(width, height);      
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    nodeMap[i, j] = new Node();
                }
        }

        public void assignMap(Map map)
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    nodeMap[i, j].f = 0;
                    nodeMap[i, j].closed = false;
                    if (map.isEmpty(j, i))
                        nodeMap[i, j].g = 0;
                    else
                        nodeMap[i, j].g = -1;
                }
        }

        public void clear()
        {
            open.clear();
            //closed.clear();
        }

        public List<MyPoint> getPath()
        {
            List<MyPoint> pathList = new List<MyPoint>();
            Node tempNode;

            if (currentNode != null)
            {
                pathList.Insert(0, new MyPoint(currentNode.x, currentNode.y));
                tempNode = currentNode.parent;
                while (tempNode != null)
                {
                    pathList.Insert(0, new MyPoint(tempNode.x, tempNode.y));
                    tempNode = tempNode.parent;
                }
            }
            return pathList;
        }

        public int compareF(Node obj1, Node obj2)
        {
            if (obj1.f < obj2.f)
                return -1;
            if (obj1.f > obj2.f)
                return 1;
            return 0;
        }

        public bool compareXY(Node node)
        {
            if (node.x == x && node.y == y)
                return true;
            else
                return false;
        }

        public bool search(Map map)
        {
            //init(map);

            Node nextNode;

            int startX = map.getStart().x;
            int startY = map.getStart().y;
            int endX = map.getEnd().x;
            int endY = map.getEnd().y;
            int f;

            currentNode = nodeMap[startY, startX];
            currentNode.x = startX;
            currentNode.y = startY;
            currentNode.g = 1;
            //currentNode.h = Math.Abs(endX - currentNode.x) + Math.Abs(endY - currentNode.y);
            currentNode.f = currentNode.g + Math.Abs(endX - currentNode.x) + Math.Abs(endY - currentNode.y);
            currentNode.parent = null;
            //map.setValue(currentNode.x, currentNode.y, currentNode.g);

            while (currentNode.x != endX || currentNode.y != endY)
            {
                //closed.put(currentNode.x, currentNode.y, currentNode);
                currentNode.closed = true;
                for (int k = 0; k < D; k++)
                {
                    x = currentNode.x + dx[k];
                    y = currentNode.y + dy[k];
                    if (x >= 0 && x < width && y >= 0 && y < height)
                    {
                        //if (nodeMap[y, x].g > -1 && closed.find(x, y, compareXY) == null)
                        if (nodeMap[y, x].g == 0)
                        {
                            nextNode = nodeMap[y, x];
                            nextNode.x = x;
                            nextNode.y = y;
                            nextNode.g = currentNode.g + 1;
                            //nextNode.h = Math.Abs(endX - nextNode.x) + Math.Abs(endY - nextNode.y);
                            nextNode.f = nextNode.g + Math.Abs(endX - nextNode.x) + Math.Abs(endY - nextNode.y);
                            nextNode.parent = currentNode;
                            //map.setValue(nextNode.x, nextNode.y, nextNode.g);
                            //在open表中查找是否已经存在当前位置的节点
                            //tempNode = open.find(x, y);
                            //if (tempNode != null && nextNode.f < tempNode.f)
                            //{
                            //    open.remove(tempNode);
                            //}
                            open.sortedInsert(nextNode, compareF);
                        }

                        if (!nodeMap[y, x].closed && nodeMap[y, x].g > 0)
                        {
                            f = currentNode.g + 1 + Math.Abs(endX - x) + Math.Abs(endY - y);
                            if (f < nodeMap[y, x].f)
                            {
                                nextNode = nodeMap[y, x];
                                nextNode.g = currentNode.g + 1;
                                nextNode.f = f;
                                nextNode.parent = currentNode;
                                open.sortedInsert(nextNode, compareF);
                            }
                        }
                    }
                }
                if (open.getCount() > 0)
                {
                    currentNode = open.getFirst();
                    open.removeFirst();
                }
                else
                    return false;
            }
            return true;
        }

        public MapMark getMapMark()
        {
            if (nodeMap == null)
                return null;

            MapMark mark = new MapMark(width, height);
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    mark.setValue(j, i, nodeMap[i, j].g);
                }
            return mark;
        }
    }
}
