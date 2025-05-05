using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BoundGenerator : MonoBehaviour
{
    [SerializeField] float thickness = 1;
    [SerializeField] bool ignoreRaycast = true;

    [Header("Sides")]
    [SerializeField] bool leftSide = true;
    [SerializeField] bool rightSide = true;
    [SerializeField] bool backSide = true;
    [SerializeField] bool frontSide = true;
    [SerializeField] bool bottomSide = true;
    [SerializeField] bool topSide = true;

    [Header("Thickness override")]
    [ShowWhen("bottomSide")][SerializeField] bool overrideBottomThickness;
    [ShowWhen("overrideBottomThickness")][SerializeField] float bottomThickness = 1;

    [ShowWhen("topSide")][SerializeField] bool overrideTopThickness;
    [ShowWhen("overrideTopThickness")][SerializeField] float topThickness = 1;

    [ShowWhen("leftSide")][SerializeField] bool overrideLeftThickness;
    [ShowWhen("overrideLeftThickness")][SerializeField] float leftThickness = 1;

    [ShowWhen("rightSide")][SerializeField] bool overrideRightThickness;
    [ShowWhen("overrideRightThickness")][SerializeField] float rightThickness = 1;

    [ShowWhen("frontSide")][SerializeField] bool overrideFrontThickness;
    [ShowWhen("overrideFrontThickness")][SerializeField] float frontThickness = 1;

    [ShowWhen("backSide")][SerializeField] bool overrideBackThickness;
    [ShowWhen("overrideBackThickness")][SerializeField] float backThickness = 1;

    private void Awake()
    {
        var box = GetComponent<BoxCollider>();
        var scale = Vector3.zero;
        var pos = Vector3.zero;
        var layer = (ignoreRaycast) ? LayerMask.NameToLayer("Ignore Raycast") : 0;

        if (leftSide)
        {
            #region left side
            var leftWall = new GameObject("LeftSide");
            leftWall.layer = layer;
            leftWall.AddComponent<BoxCollider>();
            var t = (overrideLeftThickness) ? leftThickness : thickness;

            scale = transform.lossyScale;
            scale.x = t;
            leftWall.transform.localScale = scale;

            pos = box.center;
            pos.x -= box.size.x / 2;
            pos = transform.TransformPoint(pos);
            pos.x -= t / 2;
            leftWall.transform.position = pos;

            leftWall.transform.SetParent(transform, true);
            #endregion
        }

        if (rightSide)
        {
            #region right side
            var rightWall = new GameObject("RightSide");
            rightWall.layer = layer;
            rightWall.AddComponent<BoxCollider>();
            var t = (overrideRightThickness) ? rightThickness : thickness;

            scale = transform.lossyScale;
            scale.x = t;
            rightWall.transform.localScale = scale;

            pos = box.center;
            pos.x += box.size.x / 2;
            pos = transform.TransformPoint(pos);
            pos.x += t / 2;
            rightWall.transform.position = pos;

            rightWall.transform.SetParent(transform, true);
            #endregion
        }

        if (frontSide)
        {
            #region front side
            var frontWall = new GameObject("FrontSide");
            frontWall.layer = layer;
            frontWall.AddComponent<BoxCollider>();
            var t = (overrideFrontThickness) ? frontThickness : thickness;

            scale = transform.lossyScale;
            scale.z = t;
            frontWall.transform.localScale = scale;

            pos = box.center;
            pos.z -= box.size.z / 2;
            pos = transform.TransformPoint(pos);
            pos.z -= t / 2;
            frontWall.transform.position = pos;

            frontWall.transform.SetParent(transform, true);
            #endregion
        }

        if (backSide)
        {
            #region back side
            var backWall = new GameObject("BackSide");
            backWall.layer = layer;
            backWall.AddComponent<BoxCollider>();
            var t = (overrideBackThickness) ? backThickness : thickness;

            scale = transform.lossyScale;
            scale.z = t;
            backWall.transform.localScale = scale;

            pos = box.center;
            pos.z += box.size.z / 2;
            pos = transform.TransformPoint(pos);
            pos.z += t / 2;
            backWall.transform.position = pos;

            backWall.transform.SetParent(transform, true);
            #endregion
        }

        if (bottomSide)
        {
            #region bottom side
            var bottomWall = new GameObject("BottomSide");
            bottomWall.layer = layer;
            bottomWall.AddComponent<BoxCollider>();
            var t = (overrideBottomThickness) ? bottomThickness : thickness;

            scale = transform.lossyScale;
            scale.y = t;
            bottomWall.transform.localScale = scale;

            pos = box.center;
            pos.y -= box.size.y / 2;
            pos = transform.TransformPoint(pos);
            pos.y -= t / 2;
            bottomWall.transform.position = pos;

            bottomWall.transform.SetParent(transform, true);
            #endregion
        }

        if (topSide)
        {
            #region top side
            var topWall = new GameObject("TopSide");
            topWall.layer = layer;
            topWall.AddComponent<BoxCollider>();
            var t = (overrideTopThickness) ? topThickness : thickness;

            scale = transform.lossyScale;
            scale.y = t;
            topWall.transform.localScale = scale;

            pos = box.center;
            pos.y += box.size.y / 2;
            pos = transform.TransformPoint(pos);
            pos.y += t / 2;
            topWall.transform.position = pos;

            topWall.transform.SetParent(transform, true);
            #endregion
        }
    }
}
