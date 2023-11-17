using System;
using UnityEngine;

namespace QuadTree
{
    public class Tree : INode
    {
        public Bounds bound { get; set; }
        private Node root;
        public int maxDepth { get; }

        public Tree(Bounds bound,int maxDepth)
        {
            this.bound = bound;
            this.maxDepth = maxDepth;
            root = new Node(bound, 0, this);
        }

        public void InsertObj(ObjData obj)
        {
            root.InsertObj(obj);
        }

        public void TriggerMove(Camera camera, Action<ObjData> callback)
        {
            root.TriggerMove(camera, callback);
        }

        public void DrawBound()
        {
            root.DrawBound();
        }
    }
}