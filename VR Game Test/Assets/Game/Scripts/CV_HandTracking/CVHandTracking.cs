using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Augbox{
    public class CVHandTracking : MonoBehaviour
    {
        private const int BYTESKIP = 8;

        // Start is called before the first frame update
        public UDPReceive udpReceive;
        public GameObject handPoints1;
        public GameObject handPoints2;

        [SerializeField] private int zOffset = 10000;
        private int frameArea;
        void Start()
        {
        }
    
        // Update is called once per frame
        void Update()
        {
            string data1 = udpReceive.data;
            // string data2 = udpReceive.data2;
    
            data1 = data1.Remove(0, 1);
            data1 = data1.Remove(data1.Length-1, 1);
            string[] points1 = data1.Split(',');

            float x1 = 7-float.Parse(points1[0 * BYTESKIP + 1])/100;
            float y1 = float.Parse(points1[0 * BYTESKIP + 2]) / 100;
            float z1 = float.Parse(points1[0 * BYTESKIP + 3]) / 100;

            int w1 = int.Parse(points1[0 * BYTESKIP + 4]);
            int h1 = int.Parse(points1[0 * BYTESKIP + 5]);
            int bbx1 = int.Parse(points1[0 * BYTESKIP + 6]);
            int bby1 = int.Parse(points1[0 * BYTESKIP + 7]);

            frameArea = rectArea(w1, h1)/1000;
            int bbArea1 = rectArea(bbx1, bby1)/1000;

            if(points1[0].CompareTo("'Left'") == 0){
                handPoints1.transform.localPosition = new Vector3(x1, y1, z1 + ((frameArea-bbArea1)/zOffset) );
                if (points1.Length > BYTESKIP){
                    float x2 = 7-float.Parse(points1[1 * BYTESKIP + 1])/100;
                    float y2 = float.Parse(points1[1 * BYTESKIP + 2]) / 100;
                    float z2 = float.Parse(points1[1 * BYTESKIP + 3]) / 100;

                    int bbx2 = int.Parse(points1[1 * BYTESKIP + 6]);
                    int bby2 = int.Parse(points1[1 * BYTESKIP + 7]);

                    int bbArea2 = rectArea(bbx2, bby2)/1000;
                    handPoints2.transform.localPosition = new Vector3(x2, y2, z2 + ((frameArea-bbArea2)/zOffset) );
                }
            }
            else if(points1[0].CompareTo("'Right'") == 0){
                handPoints2.transform.localPosition = new Vector3(x1, y1, z1 + ((frameArea-bbArea1)/zOffset) );
                if (points1.Length > BYTESKIP){
                    float x2 = 7-float.Parse(points1[1 * BYTESKIP + 1])/100;
                    float y2 = float.Parse(points1[1 * BYTESKIP + 2]) / 100;
                    float z2 = float.Parse(points1[1 * BYTESKIP + 3]) / 100;

                    int bbx2 = int.Parse(points1[1 * BYTESKIP + 6]);
                    int bby2 = int.Parse(points1[1 * BYTESKIP + 7]);

                    int bbArea2 = rectArea(bbx2, bby2)/1000;
                    handPoints1.transform.localPosition = new Vector3(x2, y2, z2 + ((frameArea-bbArea2)/zOffset));
                }
            }
        }

        private int rectArea(int w, int h){
            return w*h;
        }
    }
}