namespace RTCV.CorruptCore
{
    /// <summary>
    /// A generic object for combobox purposes.
    /// Has a name and a value of type T for storing any object.
    /// </summary>
    /// <typeparam name="T">The type of object you want the comboxbox value to be</typeparam>
    public class ComboBoxItem<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }

        public ComboBoxItem(string name, T value)
        {
            Name = name;
            Value = value;
        }

        public ComboBoxItem()
        {
        }
    }
}
