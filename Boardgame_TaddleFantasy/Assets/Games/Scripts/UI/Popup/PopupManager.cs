using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    void Awake() => Instance = this;
    private List<BasePopup> activePopup = new List<BasePopup>();

    private Dictionary<string, BasePopup> popupDictionary = new Dictionary<string, BasePopup>();
    public BasePopup CreatePopup(string path)
    {
        BasePopup prefab = Resources.Load<BasePopup>(path);
        if (prefab != null)
        {
            var popup = Instantiate(prefab, this.transform);
            if (popup != null)
            {
                popup.gameObject.name = popup.name.Replace("(Clone)", "");
                popup.gameObject.SetActive(false);
                return popup;
            }
        }
        return null;
    }

    public T ShowPopup<T>(string path, System.Action callback = null) where T : BasePopup
    {
        if(!popupDictionary.TryGetValue(path, out BasePopup popup))
        {
            popup = CreatePopup(path);
            popupDictionary.Add(path, popup);
        }

        if (popup.gameObject.activeSelf)
            return null;

        popup.gameObject.SetActive(true);
        popup.transform.localScale = Vector3.one;
        popup.transform.localPosition = Vector3.zero;
        popup.transform.SetAsLastSibling();
        popup.OnShow(callback);
        if (!this.activePopup.Contains(popup))
        {
            this.activePopup.Add(popup);
        }
        return (T)popup;
    }
    public void CloseActivePopup<T>() where T : BasePopup
    {
        BasePopup p = activePopup.Find(popup => popup is T);
        p.ClickClose();
    }
    public void OnAPopupHide(BasePopup popup)
    {
        if (this.activePopup.Contains(popup))
        {
            this.activePopup.Remove(popup);
        }
    }
    public bool IsAnyPopupShowing() => activePopup.Any();
}
