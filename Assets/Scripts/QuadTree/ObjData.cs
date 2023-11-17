using UnityEngine;

namespace QuadTree
{
    public class ObjData
    {
        public string uid; //独一无二的id，通过guid创建
        public Vector3 pos; //位置

        public ObjData(string uid, Vector3 pos)
        {
            this.uid = uid;
            this.pos = pos;
        }
    }
}