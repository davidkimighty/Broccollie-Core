using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CollieMollie.Core;
using CollieMollie.Helper;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CollieMollie.UI
{
    public class UIColorFeature : MonoBehaviour, IUIFeature
    {
        #region Variable Field
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private List<Element> _elements = null;

        private Operation _featureOperation = new Operation();

        #endregion

        #region Public Functions
        public void Execute(string state, PointerEventData eventData = null, Action done = null)
        {
            if (!_isEnabled) return;

            _featureOperation.Stop(this);

            List<float> durations = new List<float>();
            foreach (Element element in _elements)
            {
                if (!element.IsEnabled) continue;
                _featureOperation.Add(element.ChangeColor(state));
                durations.Add(element.Preset.GetDuration(state));
            }

            _featureOperation.Start(this, durations.Count > 0 ? durations.Max() : 0, done);
        }

        #endregion

        [Serializable]
        public class Element
        {
            public bool IsEnabled = true;
            public MaskableGraphic Graphic = null;
            public UIColorPreset Preset = null;

            public IEnumerator ChangeColor(string state)
            {
                UIColorPreset.Setting setting = Array.Find(Preset.States, x => x.ExecutionState.ToString() == state);
                if (Preset.IsValid(setting.ExecutionState) && setting.IsEnabled)
                {
                    yield return Graphic.ChangeColorGradually(setting.TargetColor, setting.Duration, setting.Curve);
                }
            }

            public void ChangeColorInstant(string state)
            {
                UIColorPreset.Setting setting = Array.Find(Preset.States, x => x.ExecutionState.ToString() == state);
                if (Preset.IsValid(setting.ExecutionState) && setting.IsEnabled)
                {
                    Graphic.color = setting.TargetColor;
                }
            }
        }
    }
}
