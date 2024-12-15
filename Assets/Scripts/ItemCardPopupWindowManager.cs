using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ItemCardPopupWindowManager : MonoBehaviour
{
    [SerializeField] private Button closePopupWindowButton;
    [SerializeField] private GameObject itemCardPopupWindow;
    [SerializeField] private Sprite misssingImage;
    [SerializeField] private Transform itemImageContainer;
    [SerializeField] private Transform dietaryInformation;
    [SerializeField] private Transform nutritionalDetails;
    [SerializeField] private Transform servingInformation;
    [SerializeField] private TextMeshProUGUI itemName;

    void Start()
    {
        ImageLoader.SetDefaultImage(misssingImage);

        if (itemCardPopupWindow != null)
        {
            itemCardPopupWindow.SetActive(false);
        }

        if (closePopupWindowButton != null)
        {
            closePopupWindowButton.onClick.AddListener(ClosePopupWindow);
        }
    }

    void ClosePopupWindow()
    {
        if (itemCardPopupWindow != null) 
        {
            itemCardPopupWindow.SetActive(false); 
        }
    }

    public void OpenPopupWindow(Item item)
    {
        if (itemCardPopupWindow == null)
        {
            return;
        }

        itemCardPopupWindow.SetActive(true);
        LoadItemData(item);
    }

    void LoadItemData(Item item)
    {
        if(item != null)
        {
            itemName.text = item.title;

            Image itemImage = itemImageContainer.Find("ItemImage").GetComponentInChildren<Image>();
            if (itemImage != null)
            {
                StartCoroutine(new ImageLoader().SetImageFromId(itemImage, item.image));
            }

            itemImageContainer.Find("ItemDescription").transform.Find("Description").GetComponent<TextMeshProUGUI>().text = item.description;

            Transform[] details = dietaryInformation.GetComponentsInChildren<Transform>(true).Where(t => t.name == "Detail").ToArray();

            if (details != null)
            {
                details[0].Find("Value").GetComponent<TextMeshProUGUI>().text = item.dietary_type;
                details[1].Find("Value").GetComponent<TextMeshProUGUI>().text = char.ToUpper(item.meal_type[0]) + item.meal_type.Substring(1);

                details = null;
            }

            details = nutritionalDetails.GetComponentsInChildren<Transform>(true).Where(t => t.name == "Detail").ToArray();

            if (details != null)
            {
                details[0].Find("Value").GetComponent<TextMeshProUGUI>().text = item.calories.ToString();
                details[1].Find("Value").GetComponent<TextMeshProUGUI>().text = item.protein.ToString();
                details[2].Find("Value").GetComponent<TextMeshProUGUI>().text = item.fat.ToString();
                details[3].Find("Value").GetComponent<TextMeshProUGUI>().text = item.carbohydrates.ToString();
                details[4].Find("Value").GetComponent<TextMeshProUGUI>().text = item.fiber.ToString();
                details[5].Find("Value").GetComponent<TextMeshProUGUI>().text = item.sugar.ToString();

                details = null;
            }

            details = servingInformation.GetComponentsInChildren<Transform>(true).Where(t => t.name == "Detail").ToArray();

            if (details != null)
            {
                details[0].Find("Value").GetComponent<TextMeshProUGUI>().text = item.standard_quantity;
                details[1].Find("Value").GetComponent<TextMeshProUGUI>().text = item.numeric_value.ToString() + item.measurement_unit;

                details = null;
            }
        }
    }

    void Update()
    {
        
    }
}
