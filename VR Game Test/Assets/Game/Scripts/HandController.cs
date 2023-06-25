using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Augbox{
public class HandController : MonoBehaviour
{
    ActionBasedController controller;
    public Hand hand;

    private void Start() {
        controller = GetComponent<ActionBasedController>();
    }

    private void Update() {
        
    }
}
}
