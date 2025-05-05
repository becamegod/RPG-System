using System.Collections.Generic;

using UnityEngine;

public class ButtonGeneratorByLabel : MonoBehaviour
{
    [SerializeField] UIInteraction itemPrefab;
    [SerializeField] Transform buttonParent;

    private void Reset() => buttonParent = GetComponent<Transform>();

    public void Generate(IEnumerable<string> labels)
    {
        buttonParent.DestroyChildren();
        foreach (var label in labels)
        {
            var item = Instantiate(itemPrefab, buttonParent);
            item.GetComponentInChildren<GenericText>().Text = label;
        }
    }
}
