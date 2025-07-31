using System;
using System.IO;
using System.Xml.Serialization;

namespace URDFImporter
{
    public static class UrdfParser
    {
        /// <summary>
        /// 解析URDF文件，返回Robot对象。
        /// </summary>
        /// <param name="filePath">URDF文件路径</param>
        /// <returns>Robot对象</returns>
        public static Robot? Parse(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                throw new FileNotFoundException($"URDF文件未找到: {filePath}");

            var serializer = new XmlSerializer(typeof(Robot));
            using var stream = File.OpenRead(filePath);
            return serializer.Deserialize(stream) as Robot;
        }
    }
}
