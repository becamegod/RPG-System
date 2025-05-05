using UnityEditor;

// by ErnestSurys
internal static class RevertPrefabTransformAction
{
    private const string RevertPrefabTransformMenuName = "CONTEXT/RectTransform/Revert Prefab Transform";

    [MenuItem(RevertPrefabTransformMenuName)]
    private static void RevertPrefabTransform(MenuCommand command)
    {
        var serObj = new SerializedObject(command.context);
        var prop = serObj.GetIterator();
        while (prop.NextVisible(true))
        {
            PrefabUtility.RevertPropertyOverride(prop, InteractionMode.UserAction);
        }
    }

    [MenuItem(RevertPrefabTransformMenuName, validate = true)]
    private static bool RevertPrefabTransformValidate(MenuCommand command)
    {
        var obj = command.context;
        return PrefabUtility.IsPartOfPrefabInstance(obj);
    }
}
