namespace ArtworkSharing.Core.ViewModels.Paypals
{
    public class PaypalOrderModel
    {
        public string intent { get; set; }
        public List<PurchaseUnit> purchase_units { get; set; }
        public ApplicationContext application_context { get; set; }
    }

    public class PurchaseUnit
    {
        public Item items { get; set; }
        public Amount amount { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public UnitAmount unit_amount { get; set; }

    }

    public class UnitAmount
    {
        public string currency_code { get; set; }
        public double value { get; set; }
    }

    public class Amount
    {
        public string currency_code { get; set; }
        public double value { get; set; }
        public Breakdown breakdown { get; set; }

    }

    public class Breakdown
    {
        public ItemTotal item_total { get; set; }
    }

    public class ItemTotal
    {
        public string currency_code { get; set; }
        public double value { get; set; }
    }

    public class ApplicationContext
    {
        public string return_url { get; set; }
        public string cancel_url { get; set; }
    }
}
