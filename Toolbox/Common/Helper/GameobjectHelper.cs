using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public static class GameobjectHelper
{
    // transform
    public static Transform RecursiveFindChild(this Transform parent, string childName, bool caseInsensitive = false)
    {
        foreach (Transform child in parent)
        {
            bool found;
            if (caseInsensitive) found = child.name.ToLower() == childName.ToLower();
            else found = child.name == childName;

            if (found) return child;
            else
            {
                Transform target = RecursiveFindChild(child, childName, caseInsensitive);
                if (target != null) return target;
            }
        }
        return null;
    }

    public static List<T> GetComponentsInCloseChildren<T>(this Transform transform) where T : Component
    {
        var list = new List<T>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.TryGetComponent<T>(out var t)) list.Add(t);
        }
        return list;
    }

    public static IEnumerable<T> GetComponentsInChildrenOnly<T>(this Transform transform) where T : Component
    {
        return transform.GetComponentsInChildren<T>().Skip(1);
    }

    public static void SetPosX(this Transform t, float x) => t.position = new Vector3(x, t.position.y, t.position.z);
    public static void SetPosY(this Transform t, float y) => t.position = new Vector3(t.position.x, y, t.position.z);
    public static void SetPosZ(this Transform t, float z) => t.position = new Vector3(t.position.x, t.position.y, z);

    public static void Reset(this Transform transform)
    {
        transform.position = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public static void CopyFrom(this Transform transform, Transform other)
    {
        transform.SetPositionAndRotation(other.position, other.rotation);
        Vector3 scale = other.lossyScale;
        if (transform.parent != null) scale = transform.parent.InverseTransformVector(scale);
        transform.localScale = scale;
    }
    public static List<GameObject> GetCloseChildrenGameObjects(this Transform transform)
    {
        List<GameObject> children = new();
        for (int i = 0; i < transform.childCount; i++) children.Add(transform.GetChild(i).gameObject);
        return children;
    }

    public static void DestroyChildren(this Transform transform)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.TryGetComponent<DontMassDestroyThisChildren>(out var _)) continue;
            UnityEngine.Object.DestroyImmediate(child);
        }
    }

    public static void SetLayer(this GameObject gameObject, int layer, bool setChildren = false)
    {
        gameObject.layer = layer;
        if (!setChildren) return;
        for (int i = 0; i < gameObject.transform.childCount; i++)
            gameObject.transform.GetChild(i).gameObject.SetLayer(layer, setChildren);
    }
    public static Material FindMaterialByName(this Renderer renderer, string name)
    {
        foreach (var material in renderer.materials)
        {
            string matName = material.name;
            var i = matName.LastIndexOf("(Instance)");
            if (i != -1) matName = matName.Remove(i - 1);
            if (matName == name) return material;
        }
        return null;
    }

    public static void UpdateLayout(this MonoBehaviour behaviour)
    {
        var contentSizeFitters = behaviour.GetComponentsInChildren<ContentSizeFitter>();
        var layoutGroups = behaviour.GetComponentsInChildren<LayoutGroup>();
        foreach (var contentSizeFitter in contentSizeFitters)
        {
            contentSizeFitter.SetLayoutVertical();
            contentSizeFitter.SetLayoutHorizontal();
        }
        foreach (var layoutGroup in layoutGroups)
        {
            layoutGroup.SetLayoutVertical();
            layoutGroup.SetLayoutHorizontal();
        }
    }

    public static void SetActive(this CanvasGroup canvasGroup, bool active)
    {
        canvasGroup.alpha = (active) ? 1 : 0;
        canvasGroup.interactable = active;
        canvasGroup.blocksRaycasts = active;
    }
}
