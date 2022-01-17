using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HEX.BoardSystem
{
    //old script for board generation
    public class HexGrid : MonoBehaviour
    {
        [SerializeField]
        public GameObject _hexPrefab;
        [SerializeField]
        private int _radius;

        void Awake()
        {
            //to get the figure from website
            /*for (int q = -map.radius; q <= map_radius; q++)
            {
                int r1 = max(-map_radius, -q - map_radius);
                int r2 = min(map_radius, -q + map_radius);
                for (int r = r1; r <= r2; r++)
                {
                    grid.insert(Hex(q, r, -q - r));
                }
            }*/

            for (int q = -_radius; q <= _radius; q++)
            {
                int rMax = Mathf.Max(-_radius, -q - _radius);
                int rMin = Mathf.Min(_radius, -q + _radius);
                for (int r = rMax; r <= rMin; r++)
                {
                    //determine hex location through calculations
                    Vector3 hexPosition = CreateHex(q, r, 0);

                    //determine position
                    Vector3 position = new Vector3(hexPosition.x, 0, hexPosition.y);
                    int s = -q * 2 - r * 2;

                    //instantiate hex
                    GameObject hex = Instantiate(_hexPrefab, position, Quaternion.identity);
                    hex.name = $"Hex {q}, {r}, {s}";

                }
            }
        }

        //private Vector2 HexToPixel
        private Vector3 CreateHex(float q, float r, float s)
        {
            //Vector3 position = Position();
            var x = (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r);
            var y = (3f / 2f * r);

            return new Vector3(x, y, 0);
        }
    }
}
