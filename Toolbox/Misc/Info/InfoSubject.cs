using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Info Subject")]
public class InfoSubject : ScriptableObject
{
    [SerializeReference] List<Info> infos;
    public IReadOnlyCollection<Info> Info => infos;
}
