using System;
using System.Collections.Generic;
using System.Numerics;

namespace URDFViewer
{
    public static class TransformUtils
    {
        // 由origin的xyz和rpy生成4x4变换矩阵
        public static Matrix4x4 FromOriginRPY(double x, double y, double z, double roll, double pitch, double yaw)
        {
            // RPY顺序为Z(roll)-Y(pitch)-X(yaw)
            var rx = Matrix4x4.CreateRotationX((float)roll);
            var ry = Matrix4x4.CreateRotationY((float)pitch);
            var rz = Matrix4x4.CreateRotationZ((float)yaw);
            var r = rx * ry * rz;
            var t = Matrix4x4.CreateTranslation((float)x, (float)y, (float)z);
            return r * t;
        }

        // 关节自身变换（考虑关节类型和JointValue）
        public static Matrix4x4 JointMotion(Joint joint)
        {
            if (joint.Axis == null) return Matrix4x4.Identity;
            float x = joint.Axis.X, y = joint.Axis.Y, z = joint.Axis.Z;
            float v = (float)joint.JointValue;
            if (joint.Type == JointType.revolute || joint.Type == JointType.continuous)
            {
                // 绕axis旋转v弧度
                var axis = Vector3.Normalize(new Vector3(x, y, z));
                return Matrix4x4.CreateFromAxisAngle(axis, v);
            }
            else if (joint.Type == JointType.prismatic)
            {
                // 沿axis平移v
                var axis = Vector3.Normalize(new Vector3(x, y, z));
                return Matrix4x4.CreateTranslation(axis * v);
            }
            return Matrix4x4.Identity;
        }

        // 递归计算每个link的世界变换
        public static Dictionary<string, Matrix4x4> ComputeLinkTransforms(Robot robot, string baseLink = "base_link")
        {
            var linkTransforms = new Dictionary<string, Matrix4x4>();
            linkTransforms[baseLink] = Matrix4x4.Identity;

            // child->joint映射
            var jointMap = new Dictionary<string, Joint>();
            if (robot.Joints != null)
            {
                foreach (var joint in robot.Joints)
                {
                    if (joint.Child?.Link != null)
                        jointMap[joint.Child.Link] = joint;
                }
            }

            foreach (var link in robot.Links ?? new List<Link>())
            {
                ComputeTransformRecursive(link.Name, jointMap, linkTransforms, baseLink);
            }
            return linkTransforms;
        }

        private static void ComputeTransformRecursive(string? linkName, Dictionary<string, Joint> jointMap, Dictionary<string, Matrix4x4> linkTransforms, string baseLink)
        {
            if (linkName == null || linkTransforms.ContainsKey(linkName)) return;
            if (!jointMap.TryGetValue(linkName, out var joint) || joint.Parent?.Link == null)
                return;

            // 递归先算父link
            ComputeTransformRecursive(joint.Parent.Link, jointMap, linkTransforms, baseLink);

            // joint的origin变换
            var o = joint.Origin;
            var m_origin = o != null ? FromOriginRPY(o.X, o.Y, o.Z, o.Roll, o.Pitch, o.Yaw) : Matrix4x4.Identity;
            // 关节自身变换（角度/位移）
            var m_joint = JointMotion(joint);

            // 父link的全局变换
            var parentT = linkTransforms[joint.Parent.Link];
            linkTransforms[linkName] = m_joint * m_origin * parentT;
        }
    }
}
