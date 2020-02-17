using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OERService.Helpers
{
    public class MimeTypeLookup
    {
        private static readonly Dictionary<string, string> _mappings = new Dictionary<string, string>(2000, StringComparer.InvariantCultureIgnoreCase)
        {
            { "image/bmp",".bmp" },
            { "image/cgm",".cgm"},
            { "image/g3fax",".g3"},
            { "image/gif",".gif"},
            { "image/ief",".ief"},
            { "image/jpeg",".jpeg"},
            { "image/jpeg",".jpg"},
            { "image/jpeg",".jpe"},
            { "image/ktx",".ktx"},
            { "image/png",".png"},
            { "image/prs.btif",".btif"},
            {".sgi", "image/sgi"},
            {".svg", "image/svg+xml"},
            {".svgz", "image/svg+xml"},
            {".tiff", "image/tiff"},
            {".tif", "image/tiff"},
            {".psd", "image/vnd.adobe.photoshop"},
            {".uvi", "image/vnd.dece.graphic"},
            {".uvvi", "image/vnd.dece.graphic"},
            {".uvg", "image/vnd.dece.graphic"},
            {".uvvg", "image/vnd.dece.graphic"},
            {".sub", "text/vnd.dvb.subtitle"},
            {".djvu", "image/vnd.djvu"},
            {".djv", "image/vnd.djvu"},
            {".dwg", "image/vnd.dwg"},
            {".dxf", "image/vnd.dxf"},
            {".fbs", "image/vnd.fastbidsheet"},
            {".fpx", "image/vnd.fpx"},
            {".fst", "image/vnd.fst"},
            {".mmr", "image/vnd.fujixerox.edmics-mmr"},
            {".rlc", "image/vnd.fujixerox.edmics-rlc"},
            {".mdi", "image/vnd.ms-modi"},
            {".wdp", "image/vnd.ms-photo"},
            {".npx", "image/vnd.net-fpx"},
            {".wbmp", "image/vnd.wap.wbmp"},
            {".xif", "image/vnd.xiff"},
            {".webp", "image/webp"},
            {".3ds", "image/x-3ds"},
            {".ras", "image/x-cmu-raster"},
            {".cmx", "image/x-cmx"},
            {".fh", "image/x-freehand"},
            {".fhc", "image/x-freehand"},
            {".fh4", "image/x-freehand"},
            {".fh5", "image/x-freehand"},
            {".fh7", "image/x-freehand"},
            {".ico", "image/x-icon"},
            {".sid", "image/x-mrsid-image"},
            {".pcx", "image/x-pcx"},
            {".pic", "image/x-pict"},
            {".pct", "image/x-pict"},
            {".pnm", "image/x-portable-anymap"},
            {".pbm", "image/x-portable-bitmap"},
            {".pgm", "image/x-portable-graymap"},
            {".ppm", "image/x-portable-pixmap"},
            {".rgb", "image/x-rgb"},
            {".tga", "image/x-tga"},
            {".xbm", "image/x-xbitmap"},
            {".xpm", "image/x-xpixmap"},
            {".xwd", "image/x-xwindowdump"}
        };

        public string GetMimeType(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            string extension = Path.GetExtension(fileName);

            string mimeType;
            if (!String.IsNullOrEmpty(extension) && _mappings.TryGetValue(extension, out mimeType))
                return mimeType;

            return "application/octet-stream";
        }       
    }
}