using DotLiquid;
using DotLiquid.NamingConventions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace TransformFunctionApp
{
    internal class TransformHelper
    {
        public static object ApplyJSONToJSONLiquidTranform(string blobstring, string requestBody)
        {
            try
            {
                Template.NamingConvention = new CSharpNamingConvention();

                Template template = Template.Parse(blobstring);
                var inputToTransform = new Dictionary<string, object>();
                var reqDictionary = JsonConvert.DeserializeObject<IDictionary<string, object>>(requestBody, new JsonSerializerSettings());
                inputToTransform.Add("content", reqDictionary);


                Hash hash = Hash.FromDictionary(inputToTransform);
                var renderedOutput = template.Render(hash);
                var renderedJson = JsonConvert.DeserializeObject(renderedOutput);
                return renderedJson;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        public static object ApplyXMLToJSONLiquidTranform(string blobstring, string requestBody)
        {
            try
            {
                Template.NamingConvention = new CSharpNamingConvention();

                Template template = Template.Parse(blobstring);
                var inputToTransform = new Dictionary<string, object>();
                var xDoc = XDocument.Parse(requestBody);
                var json = JsonConvert.SerializeXNode(xDoc);
                
                var reqDictionary = JsonConvert.DeserializeObject<IDictionary<string, object>>(json,new DictionaryConverter());

                inputToTransform.Add("content", reqDictionary);

                Hash hash = Hash.FromDictionary(inputToTransform);
                var renderedOutput = template.Render(hash);
                var renderedJson = JsonConvert.DeserializeObject(renderedOutput);
                
                return renderedJson;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static MemoryStream ApplyXSLTransform(string requestBody, string blobstring)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(requestBody);
            XmlDocument xmlExtesion = new XmlDocument();
            MemoryStream xmlOutStream = new MemoryStream();
            try
            {
                if (xmlDoc != null && blobstring != String.Empty)
                {
                    //xmlExtesion.Load(_ExtensionFilePath);
                    XmlReader xmlReader = XmlReader.Create(new StringReader(blobstring));
                    XsltSettings xSettings = new XsltSettings(false, true);
                    xslt.Load(xmlReader, xSettings, null);
                    xslt.Transform(xmlDoc, new XsltArgumentList(), xmlOutStream);
                    return xmlOutStream;
                }
                
                else
                {
                    throw new System.ArgumentNullException("xsltString", "Could not load XSLT template from the blob");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private static Dictionary<string, object> XMLStringToDicConverter(string inputXML)
        //{
        //    XDocument xDoc = XDocument.Parse(inputXML);
        //    var json = JsonConvert.SerializeXNode(xDoc);
        //    Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

        //    foreach (XElement element in xDoc.Descendants().Where(p => p.HasElements == false))
        //    {
        //        int keyInt = 0;
        //        string keyName = element.Name.LocalName;

        //        var parent = element.Parent;
        //        while (parent != null)
        //        {
        //            keyName = parent.Name.LocalName + "." + keyName;

        //            parent = parent.Parent;
        //        }

        //        while (dataDictionary.ContainsKey(keyName))
        //        {
        //            keyName = keyName + "_" + keyInt++;
        //        }

        //        dataDictionary.Add(keyName, element.Value);
        //    }
        //    return dataDictionary;
        //}
    }
}