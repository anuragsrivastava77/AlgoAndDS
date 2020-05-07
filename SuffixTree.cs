using System;
namespace Application
{
    // Suffix Tree using Ukkonen's algorithm.
   
   {
        SuffixNode root = null;

    int remSuffix;
    End globalEnd;
    ActivePoint active;

    public void BuildSuffixTree(string str)
    {
        str = str + '$';

        root = new SuffixNode(-1, null, -1, null);

        remSuffix = 0;
        globalEnd = new End(-1);
        active = new ActivePoint(root, -1, 0);

        for (int i = 0; i < str.Length; i++)
        {

            this.StartPhase(i, str);
        }
    }

    private void StartPhase(int index, string str)
    {
        ++remSuffix;
        ++globalEnd.endIndex;                                                                    //Handling Rule 1 extension.
        SuffixNode LastCreatedInternalNode = null;

        while (remSuffix > 0)
        {
            int ch = (int)str[index];                                                            // ascii of next character in input to compare.

            if (active.activeLength == 0)                                                        // No Need to traverse edge. Check directly on active node.
            {
                if (active.activeNode.child[ch] == null)                                         // If edge in current char direction doesn't exists.
                {
                    SuffixNode node = new SuffixNode(index, globalEnd, -1, root);                   // New node start index will be current index.

                    active.activeNode.child[ch] = node;                                          // Create and assign new Edge in current char direction.

                    --remSuffix;                                                                 // As a new leaf node is created, so one remaning suffix is handled.
                }
                else
                {
                    ++active.activeLength;
                    active.activeEdge = active.activeNode.child[ch].startIndex;                  // Set Active edge and increase active length.
                    break;
                }
            }
            else
            {
                char nextChInTree = nextCharInTree(str);

                SuffixNode activeEdgeNode = active.activeNode.child[str[active.activeEdge]];          // current edge in the direction of current char.                                  

                if (nextChInTree == str[index])                                    // If current char matches Next Char after active length. Rule 3 Extension
                {

                    // walk down and jump active node if required while increasing active length
                    int activeEdgeLength = activeEdgeNode.end.endIndex - activeEdgeNode.startIndex + 1;

                    if (active.activeLength >= activeEdgeLength)
                    {
                        active.activeNode = activeEdgeNode;
                        active.activeLength = 1;
                        active.activeEdge = active.activeNode.startIndex + activeEdgeLength;

                    }
                    else
                    {
                        ++active.activeLength;
                    }

                    break;                                                                  // Ends of phase as Rule 3 occurred.
                }
                else                                                                        // If current char does not match next char after active length. Rule 2 Extension.
                {
                    SuffixNode newInternalNode = new SuffixNode(                                  // Two new leaf nodes will be created and current node will become internal node.
                        activeEdgeNode.startIndex,
                        new End(activeEdgeNode.startIndex + active.activeLength - 1),
                        -1,
                        root);

                    SuffixNode newLeafNode = new SuffixNode(
                       index,
                       globalEnd,
                       -1,
                       root);

                    activeEdgeNode.startIndex = activeEdgeNode.startIndex + active.activeLength;

                    newInternalNode.child[str[activeEdgeNode.startIndex]] = activeEdgeNode;
                    newInternalNode.child[str[newLeafNode.startIndex]] = newLeafNode;

                    active.activeNode.child[str[newInternalNode.startIndex]] = newInternalNode;

                    --remSuffix;


                    if (active.activeNode != root)
                    {
                        active.activeNode = active.activeNode.suffixLink;
                    }
                    else
                    {
                        active.activeLength--;
                        active.activeEdge++;
                    }

                    if (LastCreatedInternalNode != null)
                    {
                        LastCreatedInternalNode.suffixLink = newInternalNode;               // Suffix link pointed to the internal node created in the same phase;
                    }

                    LastCreatedInternalNode = newInternalNode;

                }

            }
        }
    }

    private char nextCharInTree(string inputStr)
    {

        SuffixNode activeEdgeNode = active.activeNode.child[inputStr[active.activeEdge]];          // current edge in the direction of current char.                                  

        int activeEdgeLength = (activeEdgeNode.end.endIndex - activeEdgeNode.startIndex) + 1;
        if (activeEdgeLength > active.activeLength)
        {

            return inputStr[activeEdgeNode.startIndex + active.activeLength];
        }
        else
        {
            active.activeNode = activeEdgeNode;
            active.activeLength = active.activeLength - activeEdgeLength;
            active.activeEdge = active.activeEdge + activeEdgeLength;
            return nextCharInTree(inputStr);
        }
    }



    public List<SuffixNode> DFSToPopulateStartIndexAtLeaves()
    {
        List<SuffixNode> leaveNodes = new List<SuffixNode>();

        for (int i = 0; i < root.child.Length; i++)
        {
            if (root.child[i] != null)
            {
                DFSToPopulateStartIndexAtLeaves(root.child[i], 0, leaveNodes);
            }
        }

        return leaveNodes;
    }

    public void DFSToPopulateStartIndexAtLeaves(SuffixNode node, int sLength, List<SuffixNode> leaveNodes)
    {
        bool isLeaf = true;
        sLength += node.end.endIndex - node.startIndex + 1;
        for (int i = 0; i < node.child.Length; i++)
        {
            if (node.child[i] != null)
            {
                isLeaf = false;
                DFSToPopulateStartIndexAtLeaves(node.child[i], sLength, leaveNodes);
            }
        }
        if (isLeaf)
        {
            node.index = node.end.endIndex - sLength + 1;
            leaveNodes.Add(node);
        }
    }


    public class ActivePoint
    {
        public ActivePoint(SuffixNode an, int ae, int al)
        {
            activeNode = an;
            activeEdge = ae;
            activeLength = al;
        }

        public SuffixNode activeNode = null;

        public int activeEdge = -1;

        public int activeLength = -1;
    }

    public class SuffixNode
    {
        public SuffixNode(int si, End e, int ind, SuffixNode linkNode)
        {
            this.startIndex = si;
            this.end = e;
            this.index = ind;
            this.suffixLink = linkNode;
        }

        public SuffixNode[] child = new SuffixNode[256];

        public int startIndex;

        public End end;

        public int index;

        public SuffixNode suffixLink;
    }

    public class End
    {
        public End(int e)
        {
            this.endIndex = e;
        }

        public int endIndex;
    }

}
}
