namespace api_library_cs.Models
{
    public class Book
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        private static readonly HashSet<string> AllowedGenres = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ficção",
            "romance",
            "mistério"
        };

        public bool IsTitleOrAuthorValid()
        {
            if (Title.Length > 120 || Author.Length > 120)
            {
                return false;
            }

            if (Title.Length < 2 || Author.Length < 2)
            {
                return false;
            }

            return true;
        }

        public bool IsGenreValid()
        {
            return AllowedGenres.Contains(Genre.Trim());
        }

        public bool IsPriceValid()
        {
            return Price >= 0;
        }

        public bool IsStockValid()
        {
            return Stock >= 0;
        }

        public void setUpdatedAt()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
