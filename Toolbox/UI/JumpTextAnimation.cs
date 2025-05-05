using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Battle
{
    public class JumpTextAnimation : MonoBehaviour
    {
        class CharacterData
        {
            public float yOffset = 0;
            public byte alpha = 0;
            public float timeOffset = 0;
            public CharacterData(float timeOffset) => this.timeOffset = timeOffset;
        }

        [SerializeField] TMP_Text text;
        [SerializeField] int height = 20;
        [SerializeField] float speed = .025f;
        [SerializeField] float liveTime = 1;
        [SerializeField] float outroDuration = .2f;

        private CharacterData[] datas;

        private void Reset() => text = GetComponent<TMP_Text>();

        [ContextMenu("PlayAnimation")]
        public void Play() => Play(null);

        public void Play(Action onCompleted = null) => StartCoroutine(PlayCR(onCompleted));

        public IEnumerator PlayCR(Action onCompleted = null)
        {
            transform.localScale = Vector3.one;
            text.ForceMeshUpdate();
            datas = new CharacterData[text.textInfo.characterCount];
            for (int i = 0; i < datas.Length; i++) datas[i] = new((float)i / datas.Length);

            var t = 0f;
            while (t < 2)
            {
                foreach (var data in datas)
                {
                    if (t < data.timeOffset) continue;
                    float p = Mathf.Min(1, t - data.timeOffset);
                    var alpha = (byte)Mathf.Lerp(0, 255, p);
                    var yOffset = height * Mathf.Sin(Mathf.Lerp(0, Mathf.PI, p));
                    data.alpha = alpha;
                    data.yOffset = yOffset;
                }

                UpdateTMPMeshData();
                t += speed * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(liveTime);
            transform.DOScale(0, outroDuration).OnComplete(() => onCompleted?.Invoke());
        }

        private void UpdateTMPMeshData()
        {
            text.ForceMeshUpdate();
            var textInfo = text.textInfo;
            var meshInfos = textInfo.meshInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                var meshInfo = meshInfos[charInfo.materialReferenceIndex];
                var vertices = meshInfo.vertices;

                for (int j = 0; j < 4; j++)
                {
                    var vertexIndex = charInfo.vertexIndex + j;
                    meshInfo.colors32[vertexIndex].a = datas[i].alpha;
                    vertices[vertexIndex].y += datas[i].yOffset;
                }
            }

            for (int i = 0; i < meshInfos.Length; i++)
            {
                var meshInfo = meshInfos[i];
                meshInfo.mesh.colors32 = meshInfo.colors32;
                meshInfo.mesh.vertices = meshInfo.vertices;
                text.UpdateGeometry(meshInfo.mesh, i);
            }
        }
    }
}
