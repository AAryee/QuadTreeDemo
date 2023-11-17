using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuadTree
{
    public class Node : INode
    {
        public Bounds bound { get; set; }

        private int depth;
        private Tree belongTree;
        private Node[] childList;
        private List<ObjData> objList;

        public Node(Bounds bound, int depth, Tree belongTree)
        {
            this.belongTree = belongTree;
            this.bound = bound;
            this.depth = depth;
            objList = new List<ObjData>();
        }

        public void InsertObj(ObjData obj)
        {
            Node node = null;
            bool bChild = false;

            if (depth < belongTree.maxDepth && childList == null)
            {
                //如果还没到叶子节点，可以拥有儿子且儿子未创建，则创建儿子
                CerateChild();
            }

            if (childList != null)
            {
                for (int i = 0; i < childList.Length; ++i)
                {
                    Node item = childList[i];
                    if (item == null)
                    {
                        break;
                    }

                    if (item.bound.Contains(obj.pos))
                    {
                        if (node != null)
                        {
                            bChild = false;
                            break;
                        }

                        node = item;
                        bChild = true;
                    }
                }
            }

            if (bChild)
            {
                //只有一个儿子可以包含该物体，则该物体
                node.InsertObj(obj);
            }
            else
            {
                objList.Add(obj);
            }
        }


        public void TriggerMove(Camera camera, Action<ObjData> callback)
        {
            //刷新当前节点
            for (int i = 0; i < objList.Count; i++)
            {
                callback?.Invoke(objList[i]);
            }

            //刷新子节点
            if (childList != null)
            {
                for (int i = 0; i < childList.Length; i++)
                {
                    var child = childList[i];
                    if (child.bound.CheckBoundIsInCamera(camera))
                    {
                        child.TriggerMove(camera, callback);
                    }
                }
            }
        }

        private void CerateChild()
        {
            childList = new Node[4];
            int index = 0;
            for (int i = -1; i <= 1; i += 2)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    Vector3 centerOffset = new Vector3(bound.size.x / 4 * i, 0, bound.size.z / 4 * j);
                    Vector3 cSize = new Vector3(bound.size.x / 2, bound.size.y, bound.size.z / 2);
                    Bounds cBound = new Bounds(bound.center + centerOffset, cSize);
                    childList[index++] = new Node(cBound, depth + 1, belongTree);
                }
            }
        }

        public void DrawBound()
        {
            if (objList.Count != 0)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(bound.center, bound.size);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(bound.center, bound.size);
            }

            if (childList != null)
            {
                for (int i = 0; i < childList.Length; ++i)
                {
                    childList[i].DrawBound();
                }
            }
        }
    }
}