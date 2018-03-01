using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using saezuri;
using saezuri.NLP;

namespace chinosentence
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

        
    }
    public class chino
    {
        public static string matext()
        {
            string s = "青い羽を持った鳥は，私の心から飛び去った．";
            CMecab mecab = new CMecab();
            if (mecab.Analyze(s) == true)
            {
                CSentence sentence = new CSentence(s);
                sentence.ConvertTo(mecab.ResultText);

                return mecab.ResultText;
                
            }
            else
                return "エラー：" + mecab.Error;
        }
        public static void test()
        {
            string s2 = "青い羽を持った鳥は，私の心から飛び去った．";
            CCabocha cabocha = new CCabocha();
            cabocha.Analyze(s2);
            CSentence sentence2 = new CSentence(s2);
            sentence2.ConvertTo(cabocha.ResultText);
            new frmDepend(sentence2).ShowDialog();

        }

    }
}
