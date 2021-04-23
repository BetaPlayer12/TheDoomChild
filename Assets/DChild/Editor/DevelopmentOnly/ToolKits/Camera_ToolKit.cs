using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using DChild.Gameplay.Cinematics.Cameras;
using DChild.Gameplay.Cinematics;

namespace DChildEditor.Gameplay.Cinematics
{
    public static class Camera_ToolKit
    {
        private const string CAMERAGROUP_NAME = "SceneCameraGroup";
        private const string DEFAULTCAMERAGROUP_NAME = "DefaultCameraGroup";
        private const string DEFAULTCAMERA_NAME = "DefaultCamera";
        private const string TRANSISTIONCAMERAGROUP_NAME = "TransistionCameraGroup";
        private const string TRANSISTIONCAMERA_NAME = "TransistionCamera";
        private const string LAYER_PLAYERONLY = "PlayerOnly";
        private const string LAYER_CAMERAONLY = "CameraOnly";
        private const string TAG_SENSOR = "Sensor";

        [MenuItem("Tools/Zone Building/Camera/Create Virtual Camera Group &c")]
        public static void CreateVirtualCameraGroup()
        {
            if (Selection.activeGameObject)
            {
                var selectedScene = Selection.activeGameObject.scene;

                var activeSceneName = selectedScene.name;
                var sceneCameraGroup = GameObject.Find($"{CAMERAGROUP_NAME}({activeSceneName})");
                if (sceneCameraGroup == null)
                {
                    var newSceneCameraGroup = new GameObject($"{CAMERAGROUP_NAME}({activeSceneName})");
                    SceneManager.MoveGameObjectToScene(newSceneCameraGroup, selectedScene);

                    //Default Camera
                    var defaultCameraGroupGO = new GameObject($"{DEFAULTCAMERAGROUP_NAME}({activeSceneName})");
                    defaultCameraGroupGO.transform.parent = newSceneCameraGroup.transform;
                    defaultCameraGroupGO.isStatic = true;
                    defaultCameraGroupGO.layer = LayerMask.NameToLayer(LAYER_PLAYERONLY);

                    var defaultCameraSensor = new GameObject($"{DEFAULTCAMERA_NAME}Sensor");
                    defaultCameraSensor.transform.parent = defaultCameraGroupGO.transform;
                    defaultCameraSensor.tag = TAG_SENSOR;
                    defaultCameraSensor.layer = LayerMask.NameToLayer(LAYER_PLAYERONLY);
                    defaultCameraSensor.isStatic = true;
                    defaultCameraSensor.transform.parent = defaultCameraGroupGO.transform;
                    //var setter = defaultCameraSensor.AddComponent<CameraDefaultSetter>();
                    var sensor = defaultCameraSensor.AddComponent<PolygonCollider2D>();
                    sensor.isTrigger = true;

                    var defaultCameraConfine = new GameObject($"{DEFAULTCAMERA_NAME}Confine");
                    defaultCameraConfine.transform.parent = defaultCameraGroupGO.transform;
                    defaultCameraConfine.layer = LayerMask.NameToLayer(LAYER_CAMERAONLY);
                    defaultCameraConfine.tag = TAG_SENSOR;
                    var collider = defaultCameraConfine.AddComponent<PolygonCollider2D>();
                    collider.isTrigger = true;

                    var defaultCameraGO = new GameObject($"{DEFAULTCAMERA_NAME}({activeSceneName})");
                    defaultCameraGO.transform.parent = defaultCameraGroupGO.transform;
                    defaultCameraGO.layer = LayerMask.NameToLayer(LAYER_PLAYERONLY);
                    var defaultCamera = defaultCameraGO.AddComponent<VirtualCamera>();
                    var confiner = defaultCameraGO.AddComponent<CinemachineConfiner>();
                    confiner.m_BoundingShape2D = collider;
                    //setter.Set(defaultCamera);

                    CreateCameraTransitionArea();
                }
            }
        }

        [MenuItem("Tools/Zone Building/Camera/Create Transistion Camera &v", true)]
        public static bool ValidateSceneCameraGroup()
        {
            if (Selection.activeGameObject)
            {
                var selectedScene = Selection.activeGameObject.scene;
                var activeSceneName = selectedScene.name;
                var sceneCameraGroup = GameObject.Find($"{CAMERAGROUP_NAME}({activeSceneName})");
                return sceneCameraGroup != null;
            }
            return false;
        }

        [MenuItem("Tools/Zone Building/Camera/Create Transistion Camera &v")]
        public static void CreateCameraTransitionArea()
        {
            var selectedScene = Selection.activeGameObject.scene;
            var activeSceneName = selectedScene.name;
            var sceneCameraGroup = GameObject.Find($"{CAMERAGROUP_NAME}({activeSceneName})");
            if (sceneCameraGroup != null)
            {
                var existingTransistionAreas = sceneCameraGroup.GetComponentsInChildren<CameraTransistionArea>();

                //Transistion Camera
                var transistionCameraGO = new GameObject($"{TRANSISTIONCAMERAGROUP_NAME}({activeSceneName}){existingTransistionAreas.Length}");
                transistionCameraGO.transform.parent = sceneCameraGroup.transform;
                transistionCameraGO.layer = LayerMask.NameToLayer(LAYER_PLAYERONLY);
                transistionCameraGO.isStatic = true;

                var transistionCameraSensor = new GameObject($"{TRANSISTIONCAMERA_NAME}Sensor");
                transistionCameraSensor.isStatic = true;
                transistionCameraSensor.transform.parent = transistionCameraGO.transform;
                transistionCameraSensor.layer = LayerMask.NameToLayer(LAYER_PLAYERONLY);
                transistionCameraSensor.tag = TAG_SENSOR;
                transistionCameraSensor.AddComponent<CameraTransistionArea>();
                var sensor = transistionCameraSensor.AddComponent<BoxCollider2D>();
                sensor.isTrigger = true;

                var transistionCamera = new GameObject($"{TRANSISTIONCAMERA_NAME}({activeSceneName})");
                transistionCamera.transform.parent = transistionCameraGO.transform;
                transistionCamera.layer = LayerMask.NameToLayer(LAYER_PLAYERONLY);
                var vCam = transistionCamera.AddComponent<VirtualCamera>();
                vCam.Deactivate();
            }
        }

        [MenuItem("Tools/Zone Building/Camera/Create Transistion Camera Confine &b", true)]
        public static bool ValidateTransistionCameraArea()
        {
            var activeObject = Selection.activeGameObject;
            if (activeObject)
            {
                return activeObject.name.StartsWith(TRANSISTIONCAMERAGROUP_NAME) &&
                    activeObject.transform.Find($"{TRANSISTIONCAMERA_NAME}Confine") == null &&
                    activeObject.GetComponentInChildren<CameraTransistionArea>() != null &&
                    activeObject.GetComponentInChildren<VirtualCamera>(true) != null;
            }
            return false;
        }

        [MenuItem("Tools/Zone Building/Camera/Create Transistion Camera Confine &b")]
        public static void CreateTranisitionCameraConfine()
        {
            var activeObject = Selection.activeGameObject;
            if (activeObject.GetComponent<CinemachineConfiner>() == null)
            {
                var transistionCameraConfine = new GameObject($"{TRANSISTIONCAMERA_NAME}Confine");
                transistionCameraConfine.transform.parent = activeObject.transform;
                transistionCameraConfine.transform.localPosition = Vector3.zero;
                transistionCameraConfine.layer = LayerMask.NameToLayer(LAYER_CAMERAONLY);
                transistionCameraConfine.tag = TAG_SENSOR;
                var collider = transistionCameraConfine.AddComponent<PolygonCollider2D>();
                collider.isTrigger = true;

                var transistionCamera = activeObject.GetComponentInChildren<VirtualCamera>(true);
                transistionCamera.Deactivate();
                var confine = transistionCamera.gameObject.AddComponent<CinemachineConfiner>();
                confine.m_BoundingShape2D = collider;
            }
        }
    }

}