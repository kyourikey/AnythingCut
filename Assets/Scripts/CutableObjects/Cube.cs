using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnythingCut
{
    public class Cube : CutObject, ICutable
    {
        public override void Cut()
        {
            if (!CanCut())
                return;

            NowCutLevel++;

            // Instantiate
            var obj = Instantiate(gameObject);
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.GetComponent<CutObject>().NowCutLevel = NowCutLevel;

            // ReName
            obj.name = gameObject.name;

            //ReScale
            gameObject.transform.localScale = gameObject.transform.localScale / 1.2f;
            obj.transform.localScale = obj.transform.localScale / 1.2f;
            // RePosition
            var leftPos = gameObject.transform.TransformPoint(-Vector3.right);
            var rightPos = gameObject.transform.TransformPoint(Vector3.right);
            gameObject.transform.position = rightPos;
            obj.transform.position = leftPos;
        }

        public void TakeCut()
        {
            Cut();
        }
    }
}
