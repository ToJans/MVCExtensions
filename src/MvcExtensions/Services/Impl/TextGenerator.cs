using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace MvcExtensions.Services.Impl
{
    public static class RandomGenerator
    {
        static Random _r = new Random(DateTime.Now.Millisecond);

        public static int Get(int max)
        {
            return Get(0, max);
        }

        public static int Get(int min, int max)
        {
            return _r.Next(min, max);
        }


    }
    
    public class TextGenerator
    {
        List<string> words;

        static TextGenerator _Instance;
        public static TextGenerator Instance
        {
            get { return _Instance = _Instance ?? new TextGenerator(); }
        }

        public TextGenerator()
        {
            Type t = this.GetType();
            string x="";
            using (StreamReader sr = new StreamReader(t.Assembly.GetManifestResourceStream(t, "LoremIpsum.txt")))
            {
                x = sr.ReadToEnd().Replace("\n","").Replace("\r","");
            }
            words = new List<string>(x.Split(" ".ToCharArray(),StringSplitOptions.RemoveEmptyEntries));
        }


        public string Words(int minamount,int maxamount)
        {
            int count = RandomGenerator.Get(minamount, maxamount);
            string s = "";
            while (count-- > 0)
                s += words[RandomGenerator.Get(words.Count-1)]+ " ";
            return s.Trim();
        }
    }
}
