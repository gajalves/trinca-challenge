namespace Domain.Events
{
    public class ShoppingListHasBeenCreated : IEvent
    {
        public ShoppingListHasBeenCreated(string bbq, long meatQuantity, long vegetablesQuantity)
        {
            Bbq = bbq;
            MeatQuantity = meatQuantity;
            VegetablesQuantity = vegetablesQuantity;
        }

        public string Bbq { get; set;  }
        public long MeatQuantity { get; set; }
        public long VegetablesQuantity { get; set; }
    }
}
