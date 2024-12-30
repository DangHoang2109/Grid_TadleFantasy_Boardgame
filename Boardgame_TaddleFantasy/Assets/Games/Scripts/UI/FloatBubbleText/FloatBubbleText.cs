using System.Collections;
using UnityEngine;

public class FloatBubbleText : MonoBehaviour
{
    [Header("Text Properties")]
    public TMPro.TextMeshProUGUI bubbleText; // Reference to the Text component.

    [Header("Animation Properties")]
    public float floatSpeed = 2f; // Speed of floating movement.
    public float fadeDuration = 1f; // Time it takes to fade out.
    public Vector3 floatDirection = new Vector3(0, 1, 0); // Direction of the floating motion.

    private Color initialColor;
    private System.Action<FloatBubbleText> _onCompleteAnim;
    void Start()
    {
        if (bubbleText == null)
        {
            Debug.LogError("BubbleText: Text component not assigned!");
        }

        initialColor = bubbleText.color;
    }

    public void Initialize(string text, Color color, Vector3 position, System.Action<FloatBubbleText> onCompleteAnim)
    {
        _onCompleteAnim = onCompleteAnim;
        // Set the text content, color, and position dynamically.
        bubbleText.text = text;
        bubbleText.color = color;
        transform.position = position;

        // Start the floating animation.
        StartCoroutine(FloatAndFade());
    }

    private IEnumerator FloatAndFade()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < fadeDuration)
        {
            // Move the bubble in the float direction.
            transform.position = startPosition + floatDirection * (elapsedTime / fadeDuration) * floatSpeed;

            // Gradually fade out the text.
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            bubbleText.color = new Color(bubbleText.color.r, bubbleText.color.g, bubbleText.color.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the text is completely invisible at the end.
        bubbleText.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0);

        _onCompleteAnim?.Invoke(this);
    }
}
