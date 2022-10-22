using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   [SerializeField] 
   private Transform _player;
   private float _separation = 6.0f;
   private float _smoothTime = 0.3f;
   private Vector3 _velocity = Vector3.zero;

   void Update()
   {
      Vector3 targetPosition = _player.TransformPoint(new Vector3(_separation + 1, -_player.transform.position.y, -10));
      transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
   }

}
