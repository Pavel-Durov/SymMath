using System.IO;
using System.Windows.Markup;
using System.Xml;

namespace SymMath
{
    internal static class Utils
   {
      public static T CloneWPFObject<T>(T o)
      {
         using (var xmlReader = XmlReader.Create(new StringReader(XamlWriter.Save(o))))
            return (T)XamlReader.Load(xmlReader);
      }
   }
}
