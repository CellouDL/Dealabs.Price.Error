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



var targetUrl = new Uri("https://www.dealabs.com/groupe/erreur-de-prix");
var webReq = (HttpWebRequest)WebRequest.Create(targetUrl);
webReq.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";

//Fonction permettant de comparer les fichiers
bool FileCompare(string file1, string file2)
{
    int file1byte;
    int file2byte;
    FileStream fs1;
    FileStream fs2;

    fs1 = new FileStream(file1, FileMode.Open);
    fs2 = new FileStream(file2, FileMode.Open);
    
    if (fs1.Length != fs2.Length)
    {
        fs1.Close();
        fs2.Close();

        return false;
    }

    do
    {
        file1byte = fs1.ReadByte();
        file2byte = fs2.ReadByte();
        Console.WriteLine(file1byte + "\n" + file2byte);
    }
    while ((file1byte == file2byte) && (file1byte != -1));

    fs1.Close();
    fs2.Close();

    return ((file1byte - file2byte) == 0);
}




//Récupère le code source de la page erreur de prix de dealabs et les écrit en html
using (WebResponse webRes = webReq.GetResponse())
{
    Stream stream = webRes.GetResponseStream();
    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
    var texte = readStream.ReadToEnd();
    StreamWriter sourceCode1 = new StreamWriter("dealabs.html", false);
    sourceCode1.Write(texte);
    sourceCode1.Close();
    webRes.Close();
    readStream.Close();


    if (FileCompare("dealabs.html", "dealabs1.html"))
    {
        Console.WriteLine("Files are equal.");
    }
    else
    {
        Console.WriteLine("Files are not equal.");
        StreamWriter sourceCode2Write = new StreamWriter("dealabs1.html", false);
        sourceCode2Write.Write(texte);
        sourceCode2Write.Close();
    }
}

/*
    //Vérifie si le fichier à comparer existe
    if (new FileInfo("dealabs1.html").Length == 0)
    {
        StreamWriter sourceCode2 = new StreamWriter("dealabs1.html");
        sourceCode2.Write(texte);
        sourceCode2.Close();

    }
    else
    {
        Console.WriteLine("NOT EMPTY !\n\n----------------------------------------------------------------------------------------");
    }
    StreamReader sourceCode2Read = new StreamReader("dealabs1.html");
    StreamReader sourceCode1Read = new StreamReader("dealabs.html");
    var compare2 = sourceCode2Read.ReadToEnd();
    var compare1 = sourceCode1Read.ReadToEnd();
    sourceCode1Read.Close();
    sourceCode2Read.Close();



    //Comparaison des ffichiers

    fs1 = new FileStream("dealabs.html", FileMode.Open);
    fs2 = new FileStream("dealabs1.html", FileMode.Open);

    // Check the file sizes. If they are not the same, the files
    // are not the same.
    if (fs1.Length != fs2.Length)
    {
        // Close the file
        fs1.Close();
        fs2.Close();

        // Return false to indicate files are different
        Console.WriteLine("C'est pas les mêmes fichiers mon pote");
    }

    // Read and compare a byte from each file until either a
    // non-matching set of bytes is found or until the end of
    // file1 is reached.
    do
    {
        // Read one byte from each file.
        file1byte = fs1.ReadByte();
        file2byte = fs2.ReadByte();
    }
    while ((file1byte == file2byte) && (file1byte != -1));

    // Close the files.
    fs1.Close();
    fs2.Close();

    // Return the success of the comparison. "file1byte" is
    // equal to "file2byte" at this point only if the files are
    // the same.
    Console.WriteLine("Tout est ok ! C'est pareil");
}


/*TODO : 
 * filtrer sur dernier deal
 * si différents : nouveau deal en place
 * verifier si faux positifs
 * peut-être réduire la quantité de données a récup : moins sensibles aux modifs
 * hebeerger
*/