namespace SatGetsin2.Service.Extensions
{
    public static class UrlExtension
    {
        public static string GetFileName(this string url)
        {
            if (url != null)
            {
                if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
                {
                    throw new ArgumentException("Invalid URL format, ", nameof(url));
                }
                return Path.GetFileNameWithoutExtension(uri.AbsolutePath);
            }
            return string.Empty;
        }
    }
}
