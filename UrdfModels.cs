using System.Collections.Generic;
using System.Xml.Serialization;

namespace URDFViewer
{
    [XmlRoot("robot")]
    public class Robot
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlElement("link")]
        public List<Link>? Links { get; set; }

        [XmlElement("joint")]
        public List<Joint>? Joints { get; set; }
        public override string ToString()
        {
            var linksStr = Links == null ? "" : string.Join("\n  ", Links);
            var jointsStr = Joints == null ? "" : string.Join("\n  ", Joints);
            return $"Robot: {Name}\nLinks ({Links?.Count ?? 0}):\n  {linksStr}\nJoints ({Joints?.Count ?? 0}):\n  {jointsStr}";
        }
    }

    public class Link
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlElement("inertial")]
        public Inertial? Inertial { get; set; }

        [XmlElement("visual")]
        public Visual? Visual { get; set; }

        [XmlElement("collision")]
        public Collision? Collision { get; set; }
        public override string ToString()
        {
            var inertialStr = Inertial?.Origin != null ? $", InertialOrigin=({Inertial.Origin})" : "";
            var visualStr = Visual?.Origin != null ? $", VisualOrigin=({Visual.Origin})" : "";
            var collisionStr = Collision?.Origin != null ? $", CollisionOrigin=({Collision.Origin})" : "";
            return $"Link: {Name}{inertialStr}{visualStr}{collisionStr}";
        }
    }

    public class Inertial
    {
        [XmlElement("origin")]
        public Origin? Origin { get; set; }

        [XmlElement("mass")]
        public Mass? Mass { get; set; }

        [XmlElement("inertia")]
        public Inertia? Inertia { get; set; }
        public override string ToString() => $"Inertial: Origin=({Origin}), Mass=({Mass}), Inertia=({Inertia})";
    }

    public class Mass
    {
        [XmlAttribute("value")]
        public double Value { get; set; }
        public override string ToString() => $"Mass: {Value}";
    }

    public class Inertia
    {
        [XmlAttribute("ixx")]
        public double Ixx { get; set; }
        [XmlAttribute("ixy")]
        public double Ixy { get; set; }
        [XmlAttribute("ixz")]
        public double Ixz { get; set; }
        [XmlAttribute("iyy")]
        public double Iyy { get; set; }
        [XmlAttribute("iyz")]
        public double Iyz { get; set; }
        [XmlAttribute("izz")]
        public double Izz { get; set; }
        public override string ToString() => $"Inertia: ixx={Ixx}, ixy={Ixy}, ixz={Ixz}, iyy={Iyy}, iyz={Iyz}, izz={Izz}";
    }

    public class Visual
    {
        [XmlElement("origin")]
        public Origin? Origin { get; set; }

        [XmlElement("geometry")]
        public Geometry? Geometry { get; set; }

        [XmlElement("material")]
        public Material? Material { get; set; }
        public override string ToString() => $"Visual: Origin=({Origin}), Geometry=({Geometry}), Material=({Material})";
    }

    public class Collision
    {
        [XmlElement("origin")]
        public Origin? Origin { get; set; }

        [XmlElement("geometry")]
        public Geometry? Geometry { get; set; }
        public override string ToString() => $"Collision: Origin=({Origin}), Geometry=({Geometry})";
    }

    public class Geometry
    {
        [XmlElement("mesh")]
        public Mesh? Mesh { get; set; }
        public override string ToString() => $"Geometry: Mesh=({Mesh})";
    }

    public class Mesh
    {
        [XmlAttribute("filename")]
        public string? Filename { get; set; }
        public override string ToString() => $"Mesh: {Filename}";
    }

    public class Material
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlElement("color")]
        public Color? Color { get; set; }
        public override string ToString() => $"Material: {Name}, Color=({Color})";
    }

    public class Color
    {
        [XmlIgnore]
        public float R { get; set; }
        [XmlIgnore]
        public float G { get; set; }
        [XmlIgnore]
        public float B { get; set; }
        [XmlIgnore]
        public float A { get; set; }

        [XmlAttribute("rgba")]
        public string? RgbaString
        {
            get => string.Join(" ", R, G, B, A);
            set
            {
                if (string.IsNullOrWhiteSpace(value)) { R = G = B = A = 0; return; }
                var arr = value.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                R = arr.Length > 0 ? float.Parse(arr[0]) : 0;
                G = arr.Length > 1 ? float.Parse(arr[1]) : 0;
                B = arr.Length > 2 ? float.Parse(arr[2]) : 0;
                A = arr.Length > 3 ? float.Parse(arr[3]) : 0;
            }
        }
        public override string ToString() => $"Color: R={R}, G={G}, B={B}, A={A}";
    }

    public class Joint
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("type")]
        public JointType Type { get; set; }

        [XmlElement("origin")]
        public Origin? Origin { get; set; }

        [XmlElement("parent")]
        public JointLinkName? Parent { get; set; }

        [XmlElement("child")]
        public JointLinkName? Child { get; set; }

        [XmlElement("axis")]
        public Axis? Axis { get; set; }

        [XmlElement("limit")]
        public Limit? Limit { get; set; }

        [XmlIgnore]
        public double JointValue { get; set; } = 0;

        public override string ToString()
        {
            var originStr = Origin != null ? $", Origin=({Origin})" : "";
            var valueStr = $", JointValue={JointValue}";
            return $"Joint: {Name}, Type={Type}, Parent=({Parent?.Link}), Child=({Child?.Link}), Axis=({Axis}){originStr}{valueStr}";
        }
    }

    public class JointLinkName
    {
        [XmlAttribute("link")]
        public string? Link { get; set; }
        public override string ToString() => $"Link: {Link}";
    }

    public enum JointType
    {
        revolute,
        continuous,
        prismatic,
        [XmlEnum("fixed")]
        Fixed,
        floating,
        planar
    }

    public class Axis
    {
        [XmlIgnore]
        public int X { get; set; }
        [XmlIgnore]
        public int Y { get; set; }
        [XmlIgnore]
        public int Z { get; set; }

        [XmlAttribute("xyz")]
        public string? XyzString
        {
            get => string.Join(" ", X, Y, Z);
            set
            {
                if (string.IsNullOrWhiteSpace(value)) { X = Y = Z = 0; return; }
                var arr = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                X = arr.Length > 0 ? int.Parse(arr[0]) : 0;
                Y = arr.Length > 1 ? int.Parse(arr[1]) : 0;
                Z = arr.Length > 2 ? int.Parse(arr[2]) : 0;
            }
        }
        public override string ToString() => $"Axis: X={X}, Y={Y}, Z={Z}";
    }

    public class Limit
    {
        [XmlAttribute("lower")]
        public double Lower { get; set; }
        [XmlAttribute("upper")]
        public double Upper { get; set; }
        [XmlAttribute("effort")]
        public double Effort { get; set; }
        [XmlAttribute("velocity")]
        public double Velocity { get; set; }
        public override string ToString() => $"Limit: lower={Lower}, upper={Upper}, effort={Effort}, velocity={Velocity}";
    }

    public class Origin
    {
        [XmlIgnore]
        public double X { get; set; }
        [XmlIgnore]
        public double Y { get; set; }
        [XmlIgnore]
        public double Z { get; set; }
        [XmlIgnore]
        public double Roll { get; set; }
        [XmlIgnore]
        public double Pitch { get; set; }
        [XmlIgnore]
        public double Yaw { get; set; }

        [XmlAttribute("xyz")]
        public string? XyzString
        {
            get => string.Join(" ", X, Y, Z);
            set
            {
                if (string.IsNullOrWhiteSpace(value)) { X = Y = Z = 0; return; }
                var arr = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                X = arr.Length > 0 ? double.Parse(arr[0]) : 0;
                Y = arr.Length > 1 ? double.Parse(arr[1]) : 0;
                Z = arr.Length > 2 ? double.Parse(arr[2]) : 0;
            }
        }

        [XmlAttribute("rpy")]
        public string? RpyString
        {
            get => string.Join(" ", Roll, Pitch, Yaw);
            set
            {
                if (string.IsNullOrWhiteSpace(value)) { Roll = Pitch = Yaw = 0; return; }
                var arr = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                Roll = arr.Length > 0 ? double.Parse(arr[0]) : 0;
                Pitch = arr.Length > 1 ? double.Parse(arr[1]) : 0;
                Yaw = arr.Length > 2 ? double.Parse(arr[2]) : 0;
            }
        }
        public override string ToString() => $"Origin: X={X}, Y={Y}, Z={Z}, Roll={Roll}, Pitch={Pitch}, Yaw={Yaw}";
    }
}
