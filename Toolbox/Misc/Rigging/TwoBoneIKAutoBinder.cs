#if ANIMATION_RIGGING
using UnityEngine;

using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine.Animations.Rigging;
[RequireComponent(typeof(TwoBoneIKConstraint))]
public class TwoBoneIKAutoBinder : MonoBehaviour
{
    enum Limb
    {
        LeftArm,
        RightArm,
        LeftLeg,
        RightLeg
    }

    [SerializeField] Limb limb;
    public Transform searchFrom;

    public void Bind()
    {
        var rootName = new StringBuilder();
        var midName = new StringBuilder();
        var tipName = new StringBuilder();
        var names = new StringBuilder[] { rootName, midName, tipName };

        switch (limb)
        {
            case Limb.RightArm:
            case Limb.LeftArm:
                rootName.Append("mixamorig:RightArm");
                midName.Append("mixamorig:RightForeArm");
                tipName.Append("mixamorig:RightHand");
                if (limb == Limb.LeftArm) foreach (var name in names) name.Replace("Right", "Left");
                break;

            case Limb.LeftLeg:
            case Limb.RightLeg:
                foreach (var name in names) name.Clear();
                rootName.Append("mixamorig:RightUpLeg");
                midName.Append("mixamorig:RightLeg");
                tipName.Append("mixamorig:RightFoot");
                if (limb == Limb.LeftLeg) foreach (var name in names) name.Replace("Right", "Left");
                break;
        }

        var root = RecursiveFindChild(searchFrom, rootName.ToString());
        var mid = RecursiveFindChild(searchFrom, midName.ToString());
        var tip = RecursiveFindChild(searchFrom, tipName.ToString());

        var constraint = GetComponent<TwoBoneIKConstraint>();
        constraint.data.root = root;
        constraint.data.mid = mid;
        constraint.data.tip = tip;
        Debug.Log("TwoBoneIKConstraint auto binded");
    }

    public void BindAndRemove()
    {
        Bind();
        Remove();
    }

    public void Remove() => DestroyImmediate(this);

    private Transform RecursiveFindChild(Transform parent, string childName, bool caseInsensitive = false)
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
}

#if UNITY_EDITOR
[CustomEditor(typeof(TwoBoneIKAutoBinder))]
public class TwoBoneIKAutoBinderDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        TwoBoneIKAutoBinder binder = (target as TwoBoneIKAutoBinder);
        if (binder.searchFrom)
        {
            if (GUILayout.Button("Bind")) binder.Bind();
            if (GUILayout.Button("Bind & Remove")) binder.BindAndRemove();
        }
        else EditorGUILayout.HelpBox("Please assign a transform into 'searchFrom'", MessageType.Warning);
        if (GUILayout.Button("Remove")) binder.Remove();
    }
}
#endif
#endif