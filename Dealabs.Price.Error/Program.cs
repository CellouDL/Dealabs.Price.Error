using System.Net;
using System.IO;

using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Xml;
using System.Windows;
using HtmlAgilityPack;
using System.Text;
using System.Xml.Linq;
using System.Data.SqlTypes;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography;
using File = System.IO.File;

var targetUrl = new Uri("https://www.dealabs.com/groupe/erreur-de-prix");
var webReq = (HttpWebRequest)WebRequest.Create(targetUrl);
webReq.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";

//Fonction permettant de comparer les fichiers
bool FileCompare(string file1, string file2)
{
    int file1byte;
    int file2byte;
    using var fs1 = File.OpenRead(file1);
    using var fs2 = File.OpenRead(file2);
    if (fs1.Length != fs2.Length)
        return false;
    using var br1 = new BinaryReader(fs1);
    using var br2 = new BinaryReader(fs2);

    while (fs1.Position < fs1.Length)
    {
        var b1 = br1.ReadByte();
        var b2 = br2.ReadByte();

        if (b1 != b2)
            return false;
    }

    return true;
}

using (WebResponse webRes = webReq.GetResponse())
{
    Stream stream = webRes.GetResponseStream();
    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
    var texte = readStream.ReadToEnd();

    texte = texte.Substring(texte.IndexOf("class=\"cept-tt thread-link linkPlain thread-title--list js-thread-title\"") + "class=\"cept-tt thread-link linkPlain thread-title--list js-thread-title\"".Length);
    texte = texte.Substring(texte.IndexOf(")\"") + ")\"".Length);
    string decoupe = texte.Substring(0, texte.IndexOf("</strong"));
    decoupe = decoupe.Substring(texte.IndexOf("data-t-click") + "data-t-click".Length);
    string final = decoupe.Substring(1, decoupe.IndexOf(" - "));
    Console.WriteLine(final);
    StreamWriter sourceCode1 = new StreamWriter("dealabs.txt", false);
    sourceCode1.Write(final);
    sourceCode1.Close();
    webRes.Close();
    readStream.Close();

    if (FileCompare("dealabs.txt", "dealabs1.txt") == true)
            Console.WriteLine("Files are equal.");
    else
        Console.WriteLine("Files are not equal.");

    System.IO.File.Delete("dealabs1.txt");
    System.IO.File.Copy("dealabs.txt", "dealabs1.txt");
}


//Récupère le code source de la page erreur de prix de dealabs et les écrit en txt


/*TODO : 
 * ajouter au bot twitter
 * faire fonctionner en 24/7
*/