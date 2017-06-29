using System;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using UnityEngine;

public class PrefabPlacer : MonoBehaviour, IInputClickHandler
{
    public Transform prefab;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        var prefabInstance = Instantiate(prefab);
        prefabInstance.gameObject.transform.position = GazeManager.Instance.GazeOrigin + GazeManager.Instance.GazeNormal * 1.5f; ;
        var tapToPlace = prefabInstance.gameObject.AddComponent<TapToPlace>();
        tapToPlace.SavedAnchorFriendlyName = Guid.NewGuid().ToString();
    }

    // Use this for initialization
    void Start()
    {
        InputManager.Instance.PushFallbackInputHandler(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
