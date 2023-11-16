namespace Domain.Events
{
    public class ShoppingListQuantityAdded : IEvent
    {
        public ShoppingListQuantityAdded(bool isVeg, string personId)
        {
            IsVeg = isVeg;
            PersonId = personId;
        }

        public string PersonId { get; set; }
        public bool IsVeg { get; set; }
        public long MeatQuantityAdded { get; set; }
        public long VegetablesQuantityAdded { get; set; }        
    }
}
