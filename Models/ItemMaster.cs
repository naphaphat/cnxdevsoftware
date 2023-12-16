namespace cnxdevsoftware.Models;

public partial class ItemMaster
{
    public string ItemCode { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public double ItemPrice { get; set; }

    public string? ItemDescription { get; set; }

    public byte[]? ItemImage { get; set; }
}
