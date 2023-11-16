using Domain.Events;
using System;

namespace Domain.Entities
{
    public class ShoppingList : AggregateRoot
    {
        public ShoppingList() { }
                        
        public long MeatQuantity { get; set; }
        public long VegetablesQuantity { get; set; }


        public void When(ShoppingListHasBeenCreated @event)
        {
            Id = @event.Bbq;
            MeatQuantity = @event.MeatQuantity;
            VegetablesQuantity = @event.VegetablesQuantity;
        }

        public void When(ShoppingListQuantityAdded @event)
        {
            if (@event.IsVeg)
            {
                @event.MeatQuantityAdded = 0;
                @event.VegetablesQuantityAdded = 600;
            }
            else
            {
                @event.MeatQuantityAdded = 300;
                @event.VegetablesQuantityAdded = 300;
            }

            MeatQuantity += @event.MeatQuantityAdded;
            VegetablesQuantity += @event.VegetablesQuantityAdded;
        }

        public void When(ShoppingListQuantityRemoved @event)
        {
            if (@event.IsVeg)
            {
                @event.VegetablesQuantityRemoved = 600;
                @event.MeatQuantityRemoved = 0;
            }
            else
            {
                @event.MeatQuantityRemoved = 300;
                @event.VegetablesQuantityRemoved = 300;
            }

            MeatQuantity -= @event.MeatQuantityRemoved;
            VegetablesQuantity -= @event.VegetablesQuantityRemoved;
        }

        public object TakeSnapshot()
        {
            return new
            {
                Id,
                MeatQuantity,
                VegetablesQuantity                
            };
        }
    }
}
