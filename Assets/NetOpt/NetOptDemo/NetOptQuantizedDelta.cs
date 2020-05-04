using System.Collections.Generic;
using UnityEngine;

namespace NetOpt.NetOptDemo
{
    [System.Serializable]
    public class PackPantherQuantizedDiffSerializer : IDemoSerializer
    {

        protected PacketBuffer sourceBuffer;
        protected PacketBuffer targetBuffer;
        protected PacketDelta delta;

        public void Initialize()
        {
            sourceBuffer = new PacketBuffer(16384);
            targetBuffer = new PacketBuffer(16384);
        }
    
        public virtual int Serialize(List<DemoEntity> entities)
        {
            PacketWriter writer = new PacketWriter(targetBuffer);

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var position = entity.logicalPosition;
                writer.PackFloat(position.x, -1024f, 1023f, 0.01f);
                writer.PackFloat(position.y, -1024f, 1023f, 0.01f);
                writer.PackFloat(position.z, -1024f, 1023f, 0.01f);

                var rotation = entity.logicalRotation;
                writer.PackFloat(rotation.x, -1f, 1f, 0.01f);
                writer.PackFloat(rotation.y, -1f, 1f, 0.01f);
                writer.PackFloat(rotation.z, -1f, 1f, 0.01f);
                writer.PackFloat(rotation.w, -1f, 1f, 0.01f);

                var localScale = entity.logicalScale;
                writer.PackFloat(localScale.x, 0f, 15f, 0.01f);
                writer.PackFloat(localScale.y, 0f, 15f, 0.01f);
                writer.PackFloat(localScale.z, 0f, 15f, 0.01f);
            }
            writer.FlushFinalize();
        
            delta = DeltaCompressor.Encode(sourceBuffer, targetBuffer, DeltaCompressor.Algorithm.HDiffPatch);
        
            return delta.Length;
        }

        public virtual void Deserialize(List<DemoEntity> entities)
        {

            sourceBuffer = DeltaCompressor.Decode(sourceBuffer, delta, DeltaCompressor.Algorithm.HDiffPatch);
            PacketReader reader = new PacketReader(sourceBuffer);

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                Vector3 pos = new Vector3();
                reader.Unpack(out pos.x, -1024f, 1023f, 0.01f);
                reader.Unpack(out pos.y, -1024f, 1023f, 0.01f);
                reader.Unpack(out pos.z, -1024f, 1023f, 0.01f);

                Quaternion rot = new Quaternion();
                reader.Unpack(out rot.x, -1f, 1f, 0.01f);
                reader.Unpack(out rot.y, -1f, 1f, 0.01f);
                reader.Unpack(out rot.z, -1f, 1f, 0.01f);
                reader.Unpack(out rot.w, -1f, 1f, 0.01f);

                Vector3 scale = new Vector3();
                reader.Unpack(out scale.x, 0f, 15f, 0.01f);
                reader.Unpack(out scale.y, 0f, 15f, 0.01f);
                reader.Unpack(out scale.z, 0f, 15f, 0.01f);

                entity.serializedPosition = pos;
                entity.serializedRotation = rot;
                entity.serializedScale = scale;
            }
        }
    }
}