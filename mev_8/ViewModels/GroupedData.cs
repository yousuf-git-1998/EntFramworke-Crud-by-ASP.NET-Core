namespace mev_8.ViewModels
{
    public class GroupedData<T>
    {
        public string Key { get; set; } = default!;
        public IEnumerable<T> Items { get; set; } = [];
    }
}
