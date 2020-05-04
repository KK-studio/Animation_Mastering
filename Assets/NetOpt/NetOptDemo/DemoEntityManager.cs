using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace NetOpt.NetOptDemo
{
    public class DemoEntityManager : MonoBehaviour
    {
        public Camera cam;
        private Bounds bounds = new Bounds(Vector3.zero, new Vector3(19, 1, 19));

        public GameObject entityPrefab;
        public List<DemoEntity> entities;

        public enum DemoSerializer
        {
            BinaryFormatter, BinaryWriter,PACKR, PACKRQUANT,PACKRQUANTCOMPR,PACKRQUANTDELTACOMPR
        }
    
        public IDemoSerializer serializer;

        public Text dataStatText;
        public Text serializeStatText;
        public Text deserializeStatText;
        public Text entityStatText;
    
        private const float TickRate = 60;
        private float timer = 0f;


        public void OnDropdownChange(Dropdown change)
        {
            switch ((DemoSerializer)change.value)
            {
                case DemoSerializer.BinaryFormatter:
                    SetSerializer(new BinaryFormatterSerializer());
                    break;
                case DemoSerializer.BinaryWriter:
                    SetSerializer(new BinaryWriterReaderSerializer());
                    break;
                case DemoSerializer.PACKR:
                    SetSerializer(new PackPantherSerializer());
                    break;
                case DemoSerializer.PACKRQUANT:
                    SetSerializer(new PackPantherQuantizedSerializer());
                    break;
                case DemoSerializer.PACKRQUANTCOMPR:
                    SetSerializer(new PackPantherQuantizedCompressedSerializer());
                    break;
                case DemoSerializer.PACKRQUANTDELTACOMPR:
                    SetSerializer(new PackPantherQuantizedDiffSerializer());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    
        public void SetSerializer(IDemoSerializer serializer)
        {
            this.serializer = serializer;
            this.serializer.Initialize();
        }
    
        void Start()
        {
            SetSerializer(new BinaryFormatterSerializer());
            timer = Time.fixedUnscaledTime;
        }

        // Update is called once per frame
        void Update()
        {

            if (Time.fixedUnscaledTime > timer)
            {
                Stopwatch serializeStopwatch = Stopwatch.StartNew();
                int bytes = serializer.Serialize(entities);
                double serializeTime = serializeStopwatch.Elapsed.TotalMilliseconds;
                serializeStatText.text = string.Format("Time/serialize (ms): {0:N5}", serializeTime);
                dataStatText.text = string.Format("Kbit/sec (Kbps): {0:N0}", ((bytes * 8f) / (1000f)) * TickRate);
            
                Stopwatch deserializeStopwatch = Stopwatch.StartNew();
                serializer.Deserialize(entities);
                double deserializeTime = deserializeStopwatch.Elapsed.TotalMilliseconds;
                deserializeStatText.text = string.Format("Time/deserialize (ms): {0:N5}", deserializeTime);
            
                entityStatText.text = string.Format("Entities (transforms): {0:N0}", entities.Count);
            
                timer += 1f / TickRate;
                UserInput();
            }
        }

        void UserInput()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (bounds.Contains(hit.point))
                    {
                        GameObject instance = Instantiate(entityPrefab, hit.point, Quaternion.identity);
                        entities.Add(instance.GetComponent<DemoEntity>());
                    }
                }
            }
            else if (Input.GetMouseButton(1))
            {
                if (entities.Count > 0)
                {
                    Destroy(entities[entities.Count-1].gameObject);
                    entities.RemoveAt(entities.Count-1);
                }
            }
        }
    }
}