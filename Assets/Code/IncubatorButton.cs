using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncubatorButton : MonoBehaviour {

   public Incubator Parent;
   public GameManager.HumanType Type;


   private void OnMouseUp() {
      Parent.GrowButtonEvent.Invoke(Type);
   }
}