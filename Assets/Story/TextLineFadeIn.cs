using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class TextLineFadeIn : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    public Button targetButton;
    public GameObject targetButtonObject;

    public float fadeInDurationPerLine = 0.5f; // 각 줄이 페이드 인 되는 시간
    public float delayBetweenLines = 0.2f;      // 각 줄의 페이드 인 시작 간 딜레이
    public float initialDelay = 1.5f; // 씬 로드 후 애니메이션 시작까지의 지연 시간

    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.ForceMeshUpdate();
    }

    void Start()
    {
        StartCoroutine(StartFadeInAfterDelay());
    }

    IEnumerator StartFadeInAfterDelay()
    {
        targetButton.interactable = false;
        targetButtonObject.SetActive(false);

        yield return new WaitForSeconds(initialDelay);
        yield return StartCoroutine(FadeInLinesSequentially());
        yield return new WaitForSeconds(1f);

        targetButton.interactable = true;
        targetButtonObject.SetActive(true);
    }

    IEnumerator FadeInLinesSequentially()
    {
        if (textComponent.textInfo == null || textComponent.textInfo.lineInfo == null)
        {
            yield break;
        }

        for (int i = 0; i < textComponent.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textComponent.textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            Color32[] newColors = textComponent.textInfo.meshInfo[materialIndex].colors32;
            newColors[vertexIndex + 0].a = 0;
            newColors[vertexIndex + 1].a = 0;
            newColors[vertexIndex + 2].a = 0;
            newColors[vertexIndex + 3].a = 0;
        }
        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        for (int lineIndex = 0; lineIndex < textComponent.textInfo.lineCount; lineIndex++)
        {
            TMP_LineInfo lineInfo = textComponent.textInfo.lineInfo[lineIndex];

            float timer = 0f;
            while (timer < fadeInDurationPerLine)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Clamp01(timer / fadeInDurationPerLine);

                for (int i = lineInfo.firstCharacterIndex; i <= lineInfo.lastCharacterIndex; i++)
                {
                    TMP_CharacterInfo charInfo = textComponent.textInfo.characterInfo[i];
                    if (!charInfo.isVisible) continue;

                    int materialIndex = charInfo.materialReferenceIndex;
                    int vertexIndex = charInfo.vertexIndex;

                    Color32[] newColors = textComponent.textInfo.meshInfo[materialIndex].colors32;
                    newColors[vertexIndex + 0].a = (byte)(alpha * 255);
                    newColors[vertexIndex + 1].a = (byte)(alpha * 255);
                    newColors[vertexIndex + 2].a = (byte)(alpha * 255);
                    newColors[vertexIndex + 3].a = (byte)(alpha * 255);
                }
                textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                yield return null;
            }

            if (lineIndex < textComponent.textInfo.lineCount - 1)
            {
                yield return new WaitForSeconds(delayBetweenLines);
            }
        }
    }
}