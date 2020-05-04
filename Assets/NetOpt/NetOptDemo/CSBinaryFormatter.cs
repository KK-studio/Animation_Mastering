using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace NetOpt.NetOptDemo
{
    public class BinaryFormatterSerializer : IDemoSerializer
    {
        private MemoryStream _stream;
    
        [System.Serializable]
        public struct Vector3S
        {
            public float x,y,z;
            public static explicit operator Vector3S(Vector3 other)
            {
                return new Vector3S(other);
            }

            public static implicit operator Vector3(Vector3S other)
            {
                return new Vector3(other.x, other.y, other.z);
            }

            public Vector3S(Vector3 other)
            {
                x = other.x;
                y = other.y;
                z = other.z;
            }
        }
    
        [System.Serializable]
        public struct QuaternionS
        {
            public float x,y,z,w;
            public static explicit operator QuaternionS(Quaternion other)
            {
                return new QuaternionS(other);
            }

            public static implicit operator Quaternion(QuaternionS other)
            {
                return new Quaternion(other.x, other.y, other.z, other.w);
            }

            public QuaternionS(Quaternion other)
            {
                x = other.x;
                y = other.y;
                z = other.z;
                w = other.w;
            }
        }
    
        public void Initialize()
        {
            _stream = new MemoryStream(16384);
        }

        public int Serialize(List<DemoEntity> entities)
        {
            _stream.Seek(0,SeekOrigin.Begin);
            BinaryFormatter bf = new BinaryFormatter();
        
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                bf.Serialize(_stream, (Vector3S)entity.logicalPosition);
                bf.Serialize(_stream,(QuaternionS)entity.logicalRotation);
                bf.Serialize(_stream,(Vector3S)entity.logicalScale);
            }

            return (int)_stream.Position;
        }
    
        public void Deserialize(List<DemoEntity> entities)
        {
            _stream.Seek(0,SeekOrigin.Begin);
            BinaryFormatter bf = new BinaryFormatter();
        
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                entity.serializedPosition = (Vector3S)bf.Deserialize(_stream);
                entity.serializedRotation = (QuaternionS)bf.Deserialize(_stream);
                entity.serializedScale = (Vector3S)bf.Deserialize(_stream);
            }
        }
    }
}
