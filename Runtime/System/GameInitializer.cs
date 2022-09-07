using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace CollieMollie.System
{
    public class GameInitializer : MonoBehaviour
    {
        #region Variable Field
        [Header("Initializer")]
        [SerializeField] private ScenePreset _persistentScene = null;

        #endregion

        private void Start()
        {
            if (_persistentScene.SceneType == SceneType.Persistent)
            {
                _persistentScene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += UnloadInitializer;
            }
        }

        #region Subscribers
        private void UnloadInitializer(AsyncOperationHandle<SceneInstance> obj)
        {
            SceneManager.UnloadSceneAsync(0);
        }
        #endregion
    }
}
