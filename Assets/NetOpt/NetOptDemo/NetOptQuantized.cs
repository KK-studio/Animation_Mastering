using System.Collections.Generic;
using UnityEngine;

namespace NetOpt.NetOptDemo
{
    public class PackPantherQuantizedSerializer : IDemoSerializer
    {
        protected PacketBuffer buffer;

        public void Initialize()
        {
            buffer = new PacketBuffer(16384);
        }

        public virtual int Serialize(List<DemoEntity> entities)
        {
            PacketWriter writer= new PacketWriter(buffer);

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var position = entity.logicalPosition;
                writer.PackFloat(position.x, -32f, 31f, 0.01f);
                writer.PackFloat(position.y, -32f, 31f, 0.01f);
                writer.PackFloat(position.z, -32f, 31f, 0.01f);

                var rotation = entity.logicalRotation;
                writer.PackFloat(rotation.x, -1f, 1f, 0.01f);
                writer.PackFloat(rotation.y, -1f, 1f, 0.01f);
                writer.PackFloat(rotation.z, -1f, 1f, 0.01f);
                writer.PackFloat(rotation.w, -1f, 1f, 0.01f);

                var localScale = entity.logicalScale;
                writer.PackFloat(localScale.x, 0f, 7f, 0.01f);
                writer.PackFloat(localScale.y, 0f, 7f, 0.01f);
                writer.PackFloat(localScale.z, 0f, 7f, 0.01f);
            }

            return writer.FlushFinalize();
        }

        public virtual void Deserialize(List<DemoEntity> entities)
        {
            PacketReader reader = new PacketReader(buffer);
        
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                Vector3 pos = new Vector3();
                reader.Unpack(out pos.x, -32f, 31f, 0.01f);
                reader.Unpack(out pos.y, -32f, 31f, 0.01f);
                reader.Unpack(out pos.z, -32f, 31f, 0.01f);
            
                Quaternion rot = new Quaternion();
                reader.Unpack(out rot.x, -1f, 1f, 0.01f);
                reader.Unpack(out rot.y, -1f, 1f, 0.01f);
                reader.Unpack(out rot.z, -1f, 1f, 0.01f);
                reader.Unpack(out rot.w, -1f, 1f, 0.01f);

                Vector3 scale = new Vector3();
                reader.Unpack(out scale.x, 0f, 7f, 0.01f);
                reader.Unpack(out scale.y, 0f, 7f, 0.01f);
                reader.Unpack(out scale.z, 0f, 7f, 0.01f);

                entity.serializedPosition = pos;
                entity.serializedRotation = rot;
                entity.serializedScale = scale;
            }
        }
    }
}
