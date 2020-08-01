using System;
using System.Collections.Generic;
using System.Text;

namespace test3
{

    class MainClass
    {
        static int n, r, c, sr, sc;
        static string dir;


        public static void Main(string[] args)
        {

            IntervalTree tree = new IntervalTree();
            tree.Insert(15, 20);
            tree.Insert(10, 30);
            tree.Insert(17, 19);
            tree.Insert(5, 20);
            tree.Insert(12, 15);
            tree.Insert(30, 40);

            tree.InOrder();
            Console.WriteLine("");

            var nd = tree.SearchOverlaps(6, 7);
            if (nd != null)
            {
                Console.WriteLine($"{nd.leftRange} , {nd.rightRange}");
            }

            // tree.Delete(10);

            //  tree.PreOrder(tree.root);



            ///////////////////////////////////////
            string input = Console.ReadLine();
            int tc = Convert.ToInt32(input);
            for (int t = 1; t <= tc; t++)
            {
                input = Console.ReadLine().Trim();
                string[] inputs = input.Split(' ');
                n = Convert.ToInt32(inputs[0]);
                r = Convert.ToInt32(inputs[1]);
                c = Convert.ToInt32(inputs[2]);
                sr = Convert.ToInt32(inputs[3]);
                sc = Convert.ToInt32(inputs[4]);

                dir = Console.ReadLine().Trim();

                Console.WriteLine("Case #" + t + ": " + 1234);
            }


        }

    }

    public class Node
    {
        public int leftRange, rightRange, max;
        public Node left, right;
        public int height;

        public Node(int lr, int rr)
        {
            leftRange = lr;
            rightRange = rr;
            max = rr;

            height = 1;
        }
    }

    public class IntervalTree
    {
        public Node root;

        public Node SearchOverlaps(int leftRange, int rightRange)
        {
            return SearchOverlaps(leftRange, rightRange, this.root);
        }

        public Node SearchOverlaps(int leftRange, int rightRange, Node node)
        {
            if (node == null)
            {
                return null;
            }

            if (node.leftRange <= leftRange && rightRange <= node.rightRange)
            {
                return node;
            }

            if (node.left != null && leftRange <= node.left.max)
            {
                return SearchOverlaps(leftRange, rightRange, node.left);
            }

            else
            {
                return SearchOverlaps(leftRange, rightRange, node.right);
            }
        }


        public void Insert(int leftRange, int rightRange)
        {
            if (root == null)
            {
                root = new Node(leftRange, rightRange);
            }
            else
            {
                root = InsertNode(leftRange, rightRange, root);
            }
        }

        public Node InsertNode(int leftRange, int rightRange, Node node)
        {
            if (node == null)
                return new Node(leftRange, rightRange);

            if (leftRange > node.leftRange)
            {
                node.right = InsertNode(leftRange, rightRange, node.right);
            }
            else
            {
                node.left = InsertNode(leftRange, rightRange, node.left);
            }

            if (GetBalance(node) > 1)
            {
                if (leftRange > node.leftRange)
                {
                    if (node.right.right != null && leftRange >= node.right.right.leftRange)
                    {
                        node = RightRightChildRotation(node);
                    }
                    else if (node.right.left != null && leftRange <= node.right.left.leftRange)
                    {
                        node.right = LeftChildRotation(node.right);
                        node = RightRightChildRotation(node);
                    }
                    else
                    {
                        throw new Exception("G. Something Logically wrong happended !");
                    }
                }
                else
                {
                    if (node.left.left != null && leftRange <= node.left.left.leftRange)
                    {
                        node = LeftLeftChildRotation(node);
                    }
                    else if (node.left.right != null && leftRange >= node.left.right.leftRange)
                    {
                        node.left = RightChildRotation(node.left);
                        node = LeftLeftChildRotation(node);
                    }
                    else
                    {
                        throw new Exception("H. Something Logically wrong happended !");
                    }
                }
            }

            AdjustHeight(node);
            AdjustMax(node);

            return node;

        }

        public void AdjustMax(Node node)
        {
            int left = 0, right = 0;

            if (node.left != null)
                left = node.left.max;
            if (node.right != null)
                right = node.right.max;

            node.max = Math.Max(node.max, Math.Max(left, right));

        }

        public int GetBalance(Node node)
        {
            if (node.right != null && node.left != null)
                return Math.Abs(node.left.height - node.right.height);

            if (node.right != null)
                return node.right.height;

            if (node.left != null)
                return node.left.height;

            return 0;
        }

        public void AdjustHeight(Node node)
        {
            if (node == null)
                return;

            if (node.right == null && node.left == null)
                node.height = 1;

            else if (node.right != null && node.left != null)
                node.height = Math.Max(node.left.height, node.right.height) + 1;

            else if (node.right != null)
                node.height = node.right.height + 1;

            else if (node.left != null)
                node.height = node.left.height + 1;

            else
            {
                throw new Exception("I. Something Logically wrong happended !");
            }
        }

        public Node LeftLeftChildRotation(Node node)
        {
            Node z = node;
            Node y = node.left;
            Node x = node.left.left;

            z.left = y.right;
            y.right = z;

            AdjustHeight(z);
            AdjustHeight(y);

            return y;
        }

        public Node RightRightChildRotation(Node node)
        {
            Node z = node;
            Node y = node.right;
            Node x = node.right.right;

            z.right = y.left;
            y.left = z;

            AdjustHeight(z);
            AdjustHeight(y);

            return y;
        }

        public Node RightChildRotation(Node node)
        {
            Node y = node;
            Node x = node.right;

            y.right = x.left;
            x.left = y;

            AdjustHeight(y);
            AdjustHeight(x);

            return x;
        }

        public Node LeftChildRotation(Node node)
        {
            Node y = node;
            Node x = node.left;

            y.left = x.right;
            x.right = y;

            AdjustHeight(y);
            AdjustHeight(x);

            return x;
        }

        /*  public void Delete(int value)
          {
              this.root = this.Delete(this.root, value);
          }


          public Node Delete(Node node, int value)
          {
              if (node.value == value)
              {
                  if(node.right==null && node.left==null)
                  {
                      return null;
                  }
                  else if(node.right == null)
                  {
                      return node.left;
                  }
                  else if(node.left== null)
                  {
                      return node.right;
                  }
                  else
                  {
                      Node nio = null;
                      node.right = GetNextInorderToReplace(node.left, out nio);
                  }

              }
              else if(value > node.value)
              {
                  node.right = Delete(node.right, value);
              }
              else
              {
                  node.left = Delete(node.left, value);
              }

              if (GetBalance(node) > 1)
              {
                  if (node.left == null || (node.right != null && (node.right.height > node.left.height)))
                  {
                      if (node.right.left == null || (node.right.right != null && (node.right.right.height > node.right.left.height)))
                      {
                          node = RightRightChildRotation(node);
                      }
                      else if (node.right.right == null || (node.right.left != null && (node.right.left.height <= node.right.right.height)))
                      {
                          node.right = LeftChildRotation(node.right);
                          node = RightRightChildRotation(node);
                      }
                      else
                      {
                          throw new Exception("A. Something Logically wrong happended !");
                      }
                  }
                  else if (node.right == null || (node.left != null && (node.left.height > node.right.height)))
                  {
                      if (node.left.right == null || (node.left.left != null && (node.left.left.height <= node.left.right.height)))
                      {
                          node = LeftLeftChildRotation(node);
                      }
                      else if (node.left.left == null || (node.left.right != null && (node.left.right.height > node.left.left.height)))
                      {
                          node.left = RightChildRotation(node.left);
                          node = LeftLeftChildRotation(node);
                      }
                      else
                      {
                          throw new Exception("B. Something Logically wrong happended !");
                      }
                  }
                  else
                  {
                      throw new Exception("C. Something Logically wrong happended !");
                  }

              }


              return node;
          }
          */

        private Node GetNextInorderToReplace(Node node, out Node nextION)
        {
            if (node.left == null)
            {
                nextION = node;
                node = null;
            }
            else
            {
                node.left = GetNextInorderToReplace(node.left, out nextION);
            }

            if (GetBalance(node) > 1)
            {
                if (node.left == null || (node.right != null && (node.right.height > node.left.height)))
                {
                    if (node.right.left == null || (node.right.right != null && (node.right.right.height > node.right.left.height)))
                    {
                        node = RightRightChildRotation(node);
                    }
                    else if (node.right.right == null || (node.right.left != null && (node.right.left.height > node.right.right.height)))
                    {
                        node.right = LeftChildRotation(node.right);
                        node = RightRightChildRotation(node);
                    }
                    else
                    {
                        throw new Exception("D. Something Logically wrong happended !");
                    }
                }
                else if (node.right == null || (node.left != null && (node.left.height > node.right.height)))
                {
                    if (node.left.right == null || (node.left.left != null && (node.left.left.height > node.left.right.height)))
                    {
                        node = RightRightChildRotation(node);
                    }
                    else if (node.left.left == null || (node.left.right != null && (node.left.right.height > node.left.left.height)))
                    {
                        node.right = LeftChildRotation(node.right);
                        node = RightRightChildRotation(node);
                    }
                    else
                    {
                        throw new Exception("E. Something Logically wrong happended !");
                    }
                }
                else
                {
                    throw new Exception("F. Something Logically wrong happended !");
                }

            }

            AdjustHeight(node);

            return node;
        }

        public void InOrder()
        {
            this.InOrder(this.root);
        }

        public void InOrder(Node node)
        {
            if (node != null)
            {
                InOrder(node.left);
                Console.WriteLine($"[{node.leftRange} , {node.rightRange}],Max= {node.max} ");
                InOrder(node.right);
            }
        }
    }
}



