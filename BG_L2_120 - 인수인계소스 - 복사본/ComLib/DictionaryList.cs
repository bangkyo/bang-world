using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComLib
{
    public class DictionaryList
    {
        private string texts;
        private string values;
        private string values1;
        private string values2;
        private string values3;
        private string values4;


        public DictionaryList(string paramText, string paramValue)
        {
            this.texts = paramText;
            this.values = paramValue;
        }

        public DictionaryList(string paramText, string paramValue, string paramValue1)
        {
            this.texts = paramText;
            this.values = paramValue;
            this.values1 = paramValue1;
        }

        public DictionaryList(string paramText, string paramValue, string paramValue1, string paramValue2)
        {
            this.texts = paramText;
            this.values = paramValue;
            this.values1 = paramValue1;
            this.values2 = paramValue2;
        }

        public DictionaryList(string paramText, string paramValue, string paramValue1, string paramValue2, string paramValue3)
        {
            this.texts = paramText;
            this.values = paramValue;
            this.values1 = paramValue1;
            this.values2 = paramValue2;
            this.values3 = paramValue3;
        }

        public DictionaryList(string paramText, string paramValue, string paramValue1, string paramValue2, string paramValue3, string paramValue4)
        {
            this.texts = paramText;
            this.values = paramValue;
            this.values1 = paramValue1;
            this.values2 = paramValue2;
            this.values3 = paramValue3;
            this.values4 = paramValue4;
        }

        public string fnText
        {
            set { this.texts = value; }
            get { return this.texts; }
        }

        public string fnValue
        {
            set { this.values = value; }
            get { return this.values; }
        }

        public string fnValue1
        {
            set { this.values1 = value; }
            get { return this.values1; }
        }
        public string fnValue2
        {
            set { this.values2 = value; }
            get { return this.values2; }
        }
        public string fnValue3
        {
            set { this.values3 = value; }
            get { return this.values3; }
        }
        public string fnValue4
        {
            set { this.values4 = value; }
            get { return this.values4; }
        }
    }
}
