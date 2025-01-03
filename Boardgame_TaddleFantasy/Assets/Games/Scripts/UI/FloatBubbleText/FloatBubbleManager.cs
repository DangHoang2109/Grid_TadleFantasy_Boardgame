using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FloatBubbleManager : MonoBehaviour
{
    public static FloatBubbleManager Instance;
    void Awake() => Instance = this;

    [SerializeField] FloatBubbleText bublePrefab;
    [SerializeField] Transform tfPool;
    private ObjectPool<FloatBubbleText> poolBuble;
    public void Init()
    {
        PreparePool();
    }
    void PreparePool()
    {
        poolBuble = new ObjectPool<FloatBubbleText>(
                createFunc: OnCreate,
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                defaultCapacity: 5,
                maxSize: 100
                );
    }
    public FloatBubbleText SpawnBubble(string text, Color color, Vector3 position)
    {
        FloatBubbleText t = poolBuble.Get();
        t.Initialize( text, color, position, OnCompleteBubleAnim);
        return t;
    }
    void OnCompleteBubleAnim(FloatBubbleText caller)
    {
        poolBuble.Release(caller);
    }
    #region Pool
    FloatBubbleText OnCreate()
    {
        // Create a new instance of the base enemy prefab
        var newItem = Instantiate(this.bublePrefab, this.tfPool);
        newItem.gameObject.SetActive(false); // Initially inactive
        return newItem as FloatBubbleText;
    }
    void OnGet(FloatBubbleText newItem)
    {
        newItem.gameObject.SetActive(true);
    }
    void OnRelease(FloatBubbleText newItem)
    {
        newItem.gameObject.SetActive(false); // Initially inactive
    }
    #endregion
}
