using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NetOpt.NetOptDemo
{
    public class BinaryWriterReaderSerializer : IDemoSerializer
    {
        private MemoryStream _stream;
    
        public void Initialize()
        {
            _stream = new MemoryStream(16384);
        }

        public int Serialize(List<DemoEntity> entities)
        {
            _stream.Seek(0,SeekOrigin.Begin);
            BinaryWriter writer= new BinaryWriter(_stream);

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var position = entity.logicalPosition;
                writer.Write(position.x);
                writer.Write(position.y);
                writer.Write(position.z);

                var rotation = entity.logicalRotation;
                writer.Write(rotation.x);
                writer.Write(rotation.y);
                writer.Write(rotation.z);
                writer.Write(rotation.w);

                var localScale = entity.logicalScale;
                writer.Write(localScale.x);
                writer.Write(localScale.y);
                writer.Write(localScale.z);
            }
            writer.Flush();
            return (int)_stream.Position;
        }

        public void Deserialize(List<DemoEntity> entities)
        {
            _stream.Seek(0,SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(_stream);
        
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                Vector3 pos = new Vector3();
                pos.x = reader.ReadSingle();
                pos.y = reader.ReadSingle();
                pos.z = reader.ReadSingle();

                Quaternion rot = new Quaternion();
                rot.x = reader.ReadSingle();
                rot.y = reader.ReadSingle();
                rot.z = reader.ReadSingle();
                rot.w = reader.ReadSingle();

                Vector3 scale = new Vector3();
                scale.x = reader.ReadSingle();
                scale.y = reader.ReadSingle();
                scale.z = reader.ReadSingle();

                entity.serializedPosition = pos;
                entity.serializedRotation = rot;
                entity.serializedScale = scale;
            }
        
        }
    }
}
