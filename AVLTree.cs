using System;
namespace Application
{
    public class Node
    {
        public int value;
        public Node left, right;
        public int height;

        public Node(int x)
        {
            value = x;
            height = 1;
        }
    }

    public class AVLTree
    {
        public Node root;

        public void Insert(int x)
        {
            if (root == null)
            {
                root = new Node(x);
            }
            else
            {
                root = InsertNode(x, root);
            }
        }

        public Node InsertNode(int x, Node node)
        {
            if (node == null)
                return new Node(x);

            if (x > node.value)
            {
                node.right = InsertNode(x, node.right);
            }
            else
            {
                node.left = InsertNode(x, node.left);
            }

            if (GetBalance(node) > 1)
            {
                if (x > node.value)
                {
                    if (node.right.right != null && x >= node.right.right.value)
                    {
                        node = RightRightChildRotation(node);
                    }
                    else if (node.right.left != null && x <= node.right.left.value)
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
                    if (node.left.left != null && x <= node.left.left.value)
                    {
                        node = LeftLeftChildRotation(node);
                    }
                    else if (node.left.right != null && x >= node.left.right.value)
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
            return node;

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

        public void Delete(int value)
        {
            this.root = this.Delete(this.root, value);
        }

        public Node Delete(Node node, int value)
        {
            if (node.value == value)
            {
                if (node.right == null && node.left == null)
                {
                    return null;
                }
                else if (node.right == null)
                {
                    return node.left;
                }
                else if (node.left == null)
                {
                    return node.right;
                }
                else
                {
                    Node nio = null;
                    node.right = GetNextInorderToReplace(node.left, out nio);
                }

            }
            else if (value > node.value)
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


        public void PreOrder(Node node)
        {
            if (node != null)
            {
                Console.Write(node.value + " ");
                PreOrder(node.left);
                PreOrder(node.right);
            }
        }
    }
}
