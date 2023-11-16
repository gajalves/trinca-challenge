namespace Application.Responses
{
    public class ShoppingListResponse
    {
        public string Churrasco { get; set; }
        public string Carnes { get; set; }
        public string Vegetais { get; set; }
        
        public ShoppingListResponse(string churrasco, long carnes, long vegetais)
        {
            Churrasco = churrasco;
            Carnes = $"{((double)carnes /1000)} KG";
            Vegetais = $"{((double)vegetais / 1000)} KG";
        }
    }
}
