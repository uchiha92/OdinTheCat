using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   
   private const float Separation = 6.0f;
   private const float SmoothTime = 0.3f;
   
   private Vector3 _velocity = Vector3.zero;
   
   [SerializeField] 
   private Transform player;
   
   void Update()
   {
      Vector3 targetPosition = player.TransformPoint(new Vector3(Separation + 1, -player.transform.position.y, -10));
      transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, SmoothTime);
   }

}
