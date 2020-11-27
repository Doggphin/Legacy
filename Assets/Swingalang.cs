using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // Start is called before the first frame update
    public class Swingalang : MonoBehaviour
    {
        public Vector3 ropePivot = new Vector3(5,2,7);
        float ropeLen;
        Vector3 velo;
        Vector3 forcesApplied;
        void Start()
        {
            ropeLen = (ropePivot - transform.position).magnitude;
        }

        void Update()
        {
            forcesApplied += Vector3.down * 9.8f * Time.deltaTime;
            velo += forcesApplied;

        //HOOKSTART

            Vector3 velocityOnRopeDir = Vector3.Project(velo, (ropePivot - transform.position));
            //Vector3 velocityOnRopeDirection = Vector3.Project(velocity, (pivotPoint - transform.position));
             velo -= velocityOnRopeDir;

        //HOOKEND

            Vector3 newPos = transform.position + velo;
            Vector3 dir = (newPos - ropePivot).normalized;
            newPos = dir * ropeLen;

        //HOOK

            transform.position = newPos; // + transform.position

            forcesApplied = Vector3.zero;
            velo *= 0.96f;
        }
    }
