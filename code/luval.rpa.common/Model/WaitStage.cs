using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class WaitStage: Stage
    {
        public WaitStage(XElement el): base(el)
        {
            LoadChoices();
        }

        private void LoadChoices()
        {
            Choices = new List<Choice>();
            var choices = Xml.Elements().Where(i => i.Name.LocalName == "choices").ToList();
            foreach(var choice in choices)
            {
                Choices.Add(new Choice(choice));
            }
        }

        public List<Choice> Choices { get; set; }

        public string Timeout
        {
            get { return GetElementValue("timeout"); }
            set { TrySetElValue("timeout", value); }
        }

        public bool IsTimeoutHardCoded
        {
            get { return !string.IsNullOrWhiteSpace(Timeout) && !Timeout.StartsWith("["); }
        }

        public bool IsArbitraryWait
        {
            get { return !Choices.Any() && !string.IsNullOrWhiteSpace(Timeout); }
        }
    }
}
