using System;
using System.IO;
using System.Text;
using System.Xml;

namespace WixXmlGenerator.Services
{
    public static class OutputFileGenerator
    {
        public static void Generate(string xmlString, string targetFilePath)
        {
            try
            {
                string formattedXml;

                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);

                using (var ms = new MemoryStream())
                {
                    using (var xmlWriter = new XmlTextWriter(ms, Encoding.Unicode))
                    {
                        xmlWriter.Formatting = Formatting.Indented;
                        xmlDocument.WriteContentTo(xmlWriter);

                        xmlWriter.Flush();
                        ms.Flush();

                        ms.Position = 0;
                        using (var sr = new StreamReader(ms))
                        {
                            formattedXml = sr.ReadToEnd();
                        }
                    }
                }

                using (var sw = new StreamWriter(targetFilePath))
                {
                    sw.Write(formattedXml);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
