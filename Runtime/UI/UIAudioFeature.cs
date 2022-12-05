using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CollieMollie.Audio;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UIAudioFeature : MonoBehaviour, IUIFeature
    {
        #region Variable Field
        [SerializeField] private AudioEventChannel _eventChannel = null;
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
                _featureOperation.Add(element.PlayAudio(state, _eventChannel));
                durations.Add(element.Preset.GetDuration(state));
            }

            _featureOperation.Start(this, durations.Count > 0 ? durations.Max() : 0, done);
        }

        #endregion

        [Serializable]
        public class Element
        {
            public bool IsEnabled = true;
            public UIAudioPreset Preset = null;

            public IEnumerator PlayAudio(string state, AudioEventChannel eventChannel)
            {
                UIAudioPreset.Setting setting = Array.Find(Preset.States, x => x.ExecutionState.ToString() == state);
                if (Preset.IsValid(setting.ExecutionState))
                {
                    if (!setting.IsEnabled) yield break;
                    eventChannel.RaisePlayAudioEvent(setting.AudioPreset);
                }
            }
        }
    }
}
