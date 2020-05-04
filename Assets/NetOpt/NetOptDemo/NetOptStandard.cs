using System.Collections.Generic;
using UnityEngine;

namespace NetOpt.NetOptDemo
{
    public class PackPantherSerializer : IDemoSerializer
    {
        PacketBuffer buffer;

        public void Initialize()
        {
            buffer = new PacketBuffer(16384);
        }

        public int Serialize(List<DemoEntity> entities)
        {
            PacketWriter writer= new PacketWriter(buffer);

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var position = entity.logicalPosition;
                writer.PackFloat(position.x);
                writer.PackFloat(position.y);
                writer.PackFloat(position.z);

                var rotation = entity.logicalRotation;
                writer.PackFloat(rotation.x);
                writer.PackFloat(rotation.y);
                writer.PackFloat(rotation.z);
                writer.PackFloat(rotation.w);

                var localScale = entity.logicalScale;
                writer.PackFloat(localScale.x);
                writer.PackFloat(localScale.y);
                writer.PackFloat(localScale.z);
            }

            return writer.FlushFinalize();
        }

        public void Deserialize(List<DemoEntity> entities)
        {
            PacketReader reader = new PacketReader(buffer);
        
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                Vector3 pos = new Vector3();
                reader.Unpack(out pos.x);
                reader.Unpack(out pos.y);
                reader.Unpack(out pos.z);
            
                Quaternion rot = new Quaternion();
                reader.Unpack(out rot.x);
                reader.Unpack(out rot.y);
                reader.Unpack(out rot.z);
                reader.Unpack(out rot.w);

                Vector3 scale = new Vector3();
                reader.Unpack(out scale.x);
                reader.Unpack(out scale.y);
                reader.Unpack(out scale.z);

                entity.serializedPosition = pos;
                entity.serializedRotation = rot;
                entity.serializedScale = scale;
            }
        }
    }
}
