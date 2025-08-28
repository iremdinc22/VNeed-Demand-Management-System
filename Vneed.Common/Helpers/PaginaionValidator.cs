namespace Vneed.Common.Helpers;

public static class PaginationValidator
{
    public static void Validate(int pageNumber, int pageSize, int totalCount)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            throw new ArgumentException("Sayfa numarası ve sayfa boyutu 0 veya daha küçük olamaz.");

        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        if (pageNumber > totalPages && totalPages != 0)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), $"Toplam {totalPages} sayfa var. Sayfa numarası aşılmış.");
    }
}