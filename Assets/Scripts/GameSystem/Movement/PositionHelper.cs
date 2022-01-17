using HEX.Additional;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HEX.GameSystem
{
    [CreateAssetMenu(fileName = "PositionHelper", menuName = "HEX/PositionHelper")]
    public class PositionHelper: ScriptableObject
    {
        // Start is called before the first frame update
        private void OnValidate()
        {
            if (_tileDimension <= 0)
                _tileDimension = 1;
        }

        [SerializeField]
        private float _tileDimension = 1.8f;

        public (int x, int y, int z) ToGridPostion(Transform parent, Vector3 worldPosition)
        {
            var q = ((Mathf.Sqrt(3f) / 3f) * worldPosition.x - 1f / 3f * worldPosition.z) ;
            var r = (2f / 3f * worldPosition.z) ;

            var x = Mathf.RoundToInt(q);
            var y = Mathf.RoundToInt(r);
            var s = -x - y;

            Vector3 wordPosition2 = ToWorldPosition(x, y);

            Debug.DrawRay(new Vector3(wordPosition2.x, 0, wordPosition2.z), Vector3.down, Color.green, 10f);

            return (y, x, s);
        }

        //public Vector3 ToHexPosition(Vector2 position)
        //{
        //    var q = Mathf.Sqrt(1f) / 3f * position.x - 2f / 3f * position.y;
        //    var r = 2f / 3f * position.y;
        //    var s = -q * 1.8f - r * 1.8f;

        //    return new Vector3(q, r, s);
        //}

        public Vector3 ToWorldPosition(int x, int y)
        {
            //logic website
            #region logic
            /*function flat_hex_to_pixel(hex):
            var x = size * (     3./2 * hex.q                    )
            var y = size * (sqrt(3)/2 * hex.q  +  sqrt(3) * hex.r)
            return Point(x, y)*/
            #endregion

            //calculations
            var q = 3f / 2f * x; /*Mathf.Sqrt(3f) * x - Mathf.Sqrt(3f) / 2f * y;*/
            var r = Mathf.Sqrt(3f) / 2f * x + Mathf.Sqrt(3f) * y;

            //relative position
            var position = new Vector3(q, 0, r);
            //position = position * 1.8f;

            return position;
        }
    }
}
