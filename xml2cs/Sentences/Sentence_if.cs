using System.Collections.Generic;
using System.Xml;
using xml2cs.Resulters;


namespace xml2cs.Sentences
{
    internal class Sentence_if : ISentence
    {
        public string GasStr { get; set; }
        (IResulter express, List<ISentence> run) then;
        List<(IResulter express, List<ISentence> run)> elif = new List<(IResulter express, List<ISentence> run)>();
        List<ISentence> _else = new List<ISentence>();
        public void LoadFromXml(XmlElement element)
        {
            GasStr = element.GetAttribute("str");
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.Name == "then")
                    then = GetIF(node as XmlElement);
                else if (node.Name == "elif")
                    elif.Add(GetIF(node as XmlElement));
                else if (node.Name == "else")
                    foreach (var i in node.FirstChild.ChildNodes)
                        _else.Add(Sentence.LoadSentencesFromXml(i as XmlElement));

            }

        }

        public string ToCsharp(string enviname)
        {
            return "";
        }

        private static (IResulter express, List<ISentence> run) GetIF(XmlElement element)
        {
            var run = new List<ISentence>();
            foreach (var i in element.ChildNodes[1].ChildNodes)
            {
                run.Add(Sentence.LoadSentencesFromXml(i as XmlElement));
            }
            return
                (Resulter.LoadResulterFromXml(element.FirstChild.FirstChild as XmlElement), run);
        }

    }


}
