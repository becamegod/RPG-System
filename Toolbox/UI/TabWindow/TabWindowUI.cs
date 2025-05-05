using System;
using System.Collections.Generic;
using UnityEngine;

public class TabWindowUI : MonoBehaviour
{
    [Serializable]
    public struct TabDefinition
    {
        public string label;
        public Sprite icon;
        public GameObject content;
    }

    public TabDefinition[] definitions;

    [Header("References")]
    [SerializeField] TabButton tabButtonPrefab;
    [SerializeField] Transform tabParent;
    [SerializeField] Transform contentParent;
    [SerializeField] UIAnimation contentWrapperPrefab;

    // events
    public event Action<Transform, int> OnContentChanged;
    public event Action<GameObject, int> OnContentCreated;

    private int currentIndex = 0;
    private List<Tab> tabs = new List<Tab>();

    public Tab CurrentTab => tabs[currentIndex];
    public Transform CurrentContent => CurrentTab.content.transform;
    public int CurrentIndex
    {
        get => currentIndex;
        set
        {
            var newValue = Mathf.Clamp(value, 0, tabs.Count - 1);
            if (currentIndex == newValue) return;
            tabs[currentIndex].Close();
            currentIndex = newValue;
            tabs[currentIndex].Open();
            OnContentChanged?.Invoke(CurrentContent, currentIndex);
        }
    }

    private void Awake()
    {
        Generate();
        tabs[currentIndex].Open();
    }

    private void Start()
    {
        OnContentChanged?.Invoke(CurrentContent, 0);
    }

    private void Generate()
    {
        // delete old items
        tabParent.DestroyChildren();
        contentParent.DestroyChildren();

        for (int i = 0; i < definitions.Length; i++)
        {
            var j = i;
            TabDefinition definition = definitions[i];
            var button = Instantiate(tabButtonPrefab, tabParent);
            button.Label = definition.label;
            button.OnClick.AddListener(() => CurrentIndex = j);

            var wrapper = Instantiate(contentWrapperPrefab, contentParent);
            var content = Instantiate(definition.content, wrapper.transform);
            Tab tab = new(button, wrapper);
            tab.Close();

            tabs.Add(tab);
            OnContentCreated?.Invoke(content, i);
        }
    }
}
