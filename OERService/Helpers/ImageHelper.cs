using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace OERService.Helpers
{
    public class DataImage
    {
        private static readonly Regex DataUriPattern = new Regex(@"^data\:(?<type>image\/(png|tiff|jpg|gif));base64,(?<data>[A-Z0-9\+\/\=]+)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private DataImage(string mimeType, byte[] rawData)
        {
            MimeType = mimeType;
            RawData = rawData;
        }

        public string MimeType { get; }
        public byte[] RawData { get; }       

        public static DataImage TryParse(string dataUri)
        {
            if (string.IsNullOrWhiteSpace(dataUri)) return null;

            Match match = DataUriPattern.Match(dataUri);
            if (!match.Success) return null;

            string mimeType = match.Groups["type"].Value;
            string base64Data = match.Groups["data"].Value;

            try
            {
                byte[] rawData = Convert.FromBase64String(base64Data);
                return rawData.Length == 0 ? null : new DataImage(mimeType, rawData);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
