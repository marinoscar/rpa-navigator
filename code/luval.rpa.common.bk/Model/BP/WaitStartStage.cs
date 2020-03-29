using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class WaitStartStage: Stage
    {
        public WaitStartStage(XElement el): base(el)
        {
            LoadChoices();
        }

        private void LoadChoices()
        {
            Choices = new List<Choice>();
            var choicesRoot = Xml.Elements().Where(i => i.Name.LocalName == "choices").FirstOrDefault();
            if (choicesRoot == null || !choicesRoot.HasElements) return;
            foreach(var choice in choicesRoot.Elements())
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
