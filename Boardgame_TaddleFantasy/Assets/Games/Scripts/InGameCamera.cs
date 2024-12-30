using System.Collections;
using System.Collections.Generic;
using Taddle_Fantasy;
using UnityEngine;

public class InGameCamera : MonoBehaviour
{
    [SerializeField] Camera _camera;
    public Camera Camera => _camera;
    public static InGameCamera Instance;
    public static Camera GameCamera => Instance.Camera;
#if UNITY_EDITOR
    private void OnValidate()
    {
        _camera= GetComponent<Camera>();
    }
#endif
    private void Start()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        GridManager.onGridGeneratedComplete -= OnGridGeneratedComplete;
        GridManager.onGridGeneratedComplete += OnGridGeneratedComplete;
    }

    /// <summary>
    /// implement if need
    /// </summary>
    /// <returns></returns>
    float GetFOV() => _camera.fieldOfView; //40f;
    void OnGridGeneratedComplete()
    {
        _camera.fieldOfView = GetFOV();
        this.transform.position = new Vector3(((float)GridManager.Instance.Row / 2 - 0.5f)* GridManager.Instance.CellSize, ((float)GridManager.Instance.Col / 2 - 0.5f) * GridManager.Instance.CellSize, transform.position.z);
            
            //new Vector3(Mathf.FloorToInt(GridManager.Instance.GridWidth / 2), Mathf.FloorToInt(GridManager.Instance.GridHeight / 2), this.transform.position.z);
    }
}
