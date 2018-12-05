using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BST_Solutions_Nov24
{
    class Program
    {
        static void Main(string[] args)
        {
            BinarySearchTree bst = new BinarySearchTree();
            bst.AddNode(10);
            bst.AddNode(5);
            bst.AddNode(20);
            bst.AddNode(22);
            bst.AddNode(21);
            bst.AddNode(11);
            bst.AddNode(2);
            bst.AddNode(7);
            bst.PrintAll(bst.root);
            Console.WriteLine();
            Console.WriteLine("After Remove : ");
            bst.RemoveUnderLevel(ref bst.root, 4);
            bst.PrintAll(bst.root);
        }
    }
    
    public class Node
    {
        public Node left;
        public Node right;
        public int value;
        public int hp;
        public int depth = 1;
        public int level = 1;  
    }
    public struct Path
    {
        public int sum;
    }
    public class BinarySearchTree
    {
        public Node root;
        public BinarySearchTree()
        {

        }
        public BinarySearchTree(int value)
        {
            root = new Node();
            root.value = value;
        }
        public void AddNode(int value)
        {
            AddNode(value, ref root);
        }
        public void AddNode(int value, ref Node node)
        {
            if (node == null)
            {
                Node newNode = new Node();
                newNode.value = value;
                node = newNode;
                return;
            }
            if (value < node.value)
            {
                AddNode(value, ref node.left);
                return;
            }
            if (value >= node.value)
            {
                AddNode(value, ref node.right);
                return;
            }
        }

        public Node RemoveUnderLevel(ref Node node, int k)
        { 
            if (node == null)
                return null;
            if (node.left != null)
            {
                node.left.level = node.level + 1;
                RemoveUnderLevel(ref node.left, k);
            }
            if (node.right != null)
            {
                node.right.level = node.level + 1;
                RemoveUnderLevel(ref node.right, k);
            }
            if (node.level < k && node.left == null && node.right == null)
            {
                node = null;
            }
            return node;
        }
        //public void Remove(Node node, int k)
        //{
        //    Node print;
        //    print = RemoveUnderLevel(node, k);
        //    Print(print);
        //}
        //public void Print(Node node)
        //{
        //    Print(node.left);
        //    Console.Write(node.value + " ");
        //    Print(node.right);
        //}

        public void PrintAll(Node node)
        {
            if (node == null)
            {
                return;
            }
            if (node.left != null)
            {
                PrintAll(node.left);
                Console.Write(node.value + " ");
            }
            else
            {
                Console.Write(node.value + " ");
            }
            if (node.right != null)
            {
                PrintAll(node.right);
            }

        }
        public void TopView(Node node)
        {
            Dictionary<int, int> d = new Dictionary<int, int>();
            node.hp = 0;
            TopView(node, d);
            foreach (var item in d)
            {
                Console.Write(item.Value + " ");
            }
        }
        public void TopView(Node node, Dictionary<int, int> d)
        {
            if (node == null)
                return;
            int hp = node.hp;
            if(!d.ContainsKey(node.hp))
            {
                d.Add(node.hp, node.value);
            }
            if(node.left!= null)
            {
                node.left.hp = hp - 1;
                TopView(node.left, d);
            }
            if(node.right != null)
            {
                node.right.hp = hp + 1;
                TopView(node.right, d);
            }
        }
        public Dictionary<int, int> ShowBottom(Node node)
        {
            Dictionary<int, int> pairs = new Dictionary<int, int>();
            if (node == null)
            {
                return pairs;
            }
            Queue<Node> q = new Queue<Node>();
            node.hp = 0;
            q.Enqueue(node);
            while (q.Count > 0)
            {
                node = q.Dequeue();
                int hp = node.hp;
                pairs[hp] = node.value;

                if (node.left != null)
                {
                    node.left.hp = hp - 1;
                    q.Enqueue(node.left);
                }
                if (node.right != null)
                {
                    node.right.hp = hp + 1;
                    q.Enqueue(node.right);
                }
            }
            return pairs;
        }
        public class Holder
        {
            public int dep;
        }
        public int MinDepRC(Node node)
        {
            Holder h = new Holder();
            h.dep = int.MaxValue;
            MinDepRC(node, h);
            return h.dep;
        }
        public void MinDepRC(Node node, Holder holder)
        {
            if (node == null)
            {
                return;
            }
            int depth = node.depth;
            if(node.left == null && node.right == null)
            {
                holder.dep = node.depth < holder.dep ? node.depth : holder.dep;
            }
            if(node.left != null)
            {
                node.left.depth = depth + 1;
                MinDepRC(node.left, holder);
            }
            if(node.right != null)
            {
                node.right.depth = depth + 1;
                MinDepRC(node.right, holder);
            }
        }
        public int FindDepthMin(Node node)
        {
            if(node == null)
            {
                return 0;
            }
            Queue<Node> q = new Queue<Node>();
            node.depth = 1;
            q.Enqueue(node);
            int depth = 0;
            while (q.Count > 0)
            {
                node = q.Dequeue();
                depth = node.depth;

                if(node.left == null && node.right == null)
                {
                    return depth;
                }
                if(node.left!= null)
                {
                    node.left.depth = depth + 1;
                    q.Enqueue(node.left);
                }
                if(node.right!= null)
                {
                    node.right.depth = depth + 1;
                    q.Enqueue(node.right);
                }
                
            }
            return depth;
        }
        public int PathSum(Node node)
        {
            Path path = new Path();
            path.sum = int.MinValue;
            PathSum(node, ref path.sum);
            return path.sum;   
        }
        public int PathSum(Node node, ref int path)
        {
            //acts as base case, restoring function from null nodes
            if(node == null)
            {
                return 0;
            }

            //traces the path of the tree as the left control
            int l = PathSum(node.left, ref path);

            //traces the path of the tree as the right control
            int r = PathSum(node.right, ref path);

            //identifies the path with the highest value trace on the side
            int singlePath = Math.Max(Math.Max(l, r) + node.value, node.value);

            //finds the max path, building from subtree to larger subtree until it is at root
            int maxPath = node.value + l + r;

            //looks at the and identifies with the full tree is of larger value than just one side(in case of negative values)
            path = Math.Max(maxPath, singlePath);

            //continues to return the value of the path with highest value, 
            //ultimately presenting the max path for left and right of the root
            return singlePath;
        }
    }
}
