namespace abibas_back.DTOs
{
    /// <summary>
    /// Catalog DTO
    /// </summary>
    public class CatalogResponseDTO
    {
        // to display all items on panel
        public int TotalCount { get; set; }
        // items list
        public List<ProductShortDTO> Items { get; set; } = new();
    }
}
