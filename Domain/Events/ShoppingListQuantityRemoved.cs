namespace Domain.Events
{
    public class ShoppingListQuantityRemoved : IEvent
    {
        public ShoppingListQuantityRemoved(bool isVeg, string personId)
        {
            IsVeg = isVeg; 
            PersonId = personId;            
        }

        public string PersonId { get; set; }
        public bool IsVeg { get; set; }
        public long MeatQuantityRemoved { get; set; }
        public long VegetablesQuantityRemoved { get; set; }
    }
}
