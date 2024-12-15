[System.Serializable]
public class Item
{
    public string id;
    public string created_on;
    public string updated_on;
    public string title;
    public string dietary_type;
    public string meal_type;
    public string standard_quantity;
    public int numeric_value;
    public string measurement_unit;
    public int calories;
    public int protein;
    public int fat;
    public int carbohydrates;
    public int fiber;
    public int sugar;
    public bool allow_partial_quantity;
    public bool enabled;
    public string description;
    public string image;
    public int standard_quantity_value;
}

[System.Serializable]
public class ItemList
{
    public Item[] data;
    public int Count => data?.Length ?? 0;

    public Item GetItemById(string id)
    {
        if (data == null || string.IsNullOrEmpty(id))
        {
            return null;
        }

        foreach (var item in data)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }
}
