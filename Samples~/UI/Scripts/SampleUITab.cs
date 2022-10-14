using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using CollieMollie.UI;
using UnityEngine;

public class SampleUITab : BaseUIPanel
{
    [SerializeField] private UIButton _triggerButton = null;


    private void Awake()
    {
        if (_triggerButton != null)
        {
            _triggerButton.OnSelected += (eventArgs) => Show(3f);
        }
    }

}
