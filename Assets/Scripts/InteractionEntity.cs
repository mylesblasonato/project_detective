using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEntity : MonoBehaviour
{
    public Note _noteToUse;
    public Flowchart _entityFlowchart;

    [HideInInspector] public bool _hasInteracted = false;
}
