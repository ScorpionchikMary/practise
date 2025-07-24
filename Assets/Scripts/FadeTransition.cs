using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeTransition : MonoBehaviour
{
    public static FadeTransition Instance;

    [Header("Настройки")]
    public float fadeSpeed = 1.5f;
    public AnimationCurve fadeCurve;

    private Image fadeImage;
    private Canvas fadeCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFadeSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeFadeSystem()
    {
        // Создаём Canvas
        fadeCanvas = new GameObject("FadeCanvas").AddComponent<Canvas>();
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.sortingOrder = 999;
        var scaler = fadeCanvas.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        DontDestroyOnLoad(fadeCanvas.gameObject);

        // Создаём Image с гарантированным растягиванием
        fadeImage = new GameObject("FadeImage").AddComponent<Image>();
        fadeImage.transform.SetParent(fadeCanvas.transform);
        ForceFullscreenStretch(fadeImage);
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    // Метод для принудительного растягивания на весь экран
    void ForceFullscreenStretch(Image image)
    {
        RectTransform rt = image.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
    }

    void Update()
    {
        // Дополнительная защита (если другие скрипты меняют размер)
        if (fadeImage != null)
        {
            ForceFullscreenStretch(fadeImage);
        }
    }

    public IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Затемнение
        yield return StartCoroutine(Fade(0, 1));

        // Загрузка сцены
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // Осветление
        yield return StartCoroutine(Fade(1, 0));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float t = 0;
        fadeImage.color = new Color(0, 0, 0, startAlpha);

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            float curveValue = fadeCurve.Evaluate(t);
            float alpha = Mathf.Lerp(startAlpha, endAlpha, curveValue);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}